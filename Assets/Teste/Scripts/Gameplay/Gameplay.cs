using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour
{
    public enum Situacoes { NONE, COMECAR, JOGO_NORMAL, FORA, PAUSADO, ESCOLHER, TROCAR_VEZ, CHUTE_AO_GOL, PEQUENA_AREA, GOL, ESPECIAL}

    public bool quitou;

    public Vector3 posGol1, posGol2;
    public Quaternion rotacaoAnt;
    public Transform rotacaoCamera;
    public GameObject canvas, mira;

    public Situacoes _atual;
    Situacoes aux;
    Situacao _situacaoAtual;

    public Abertura abertura;
    public FisicaBola _bola;
    VariaveisUIsGameplay ui;
    EventsManager events;

    public Partida.Tipo tipoPartida;
    public Partida.Modo modoPartida;
    public Partida.Conexao conexaoPartida;

    public static Gameplay _current;
    private void Awake()
    {
        _current = this;
    }

    void Start()
    {
        abertura = GetComponent<Abertura>();
        events = EventsManager.current;
        ui = VariaveisUIsGameplay._current;
        tipoPartida = GameManager.Instance.m_partida.getTipo();
        modoPartida = GameManager.Instance.m_partida.getModo();
        conexaoPartida = GameManager.Instance.m_partida.getConexao();
        _bola = GameObject.FindGameObjectWithTag("Bola").GetComponent<FisicaBola>();

        canvas = GameObject.Find("Canvas");
        rotacaoCamera = GameObject.Find("RotacaoCamera").transform;
        posGol1 = GameObject.FindGameObjectWithTag("Gol1").transform.position;
        posGol2 = GameObject.FindGameObjectWithTag("Gol2").transform.position;

        LogisticaVars.m_especialAtualT1 = LogisticaVars.m_especialAtualT2 = 0;
        BarraEspecial(0, LogisticaVars.m_maxEspecial);
        EstadoJogo.JogoParado();
        SetSituacao("comecar");
    }

    // Update is called once per frame
    void Update()
    {
        events.OnAtualizarNumeros();
        if (LogisticaVars.jogoComecou && !LogisticaVars.jogoParado) LogisticaVars.tempoCorrido += Time.deltaTime;
        TempoJogo();

        if (LogisticaVars.especialT1Disponivel && LogisticaVars.vezJ1 || LogisticaVars.especialT2Disponivel && LogisticaVars.vezJ2) ui.especialBt.interactable = true;
        else ui.especialBt.interactable = false;

        if (LogisticaVars.contarTempoSelecao)
        {
            LogisticaVars.tempoEscolherJogador += Time.deltaTime;
        }

        if (LogisticaVars.contarTempoJogada)
        {
            if(LogisticaVars.jogadas != 3) LogisticaVars.tempoJogada += Time.deltaTime;

            if (LogisticaVars.vezJ1)
            {
                if (!LogisticaVars.especial) LogisticaVars.m_especialAtualT1 += Time.deltaTime * 20;

                if (LogisticaVars.m_especialAtualT1 >= LogisticaVars.m_maxEspecial && !LogisticaVars.especialT1Disponivel)
                { LogisticaVars.m_especialAtualT1 = LogisticaVars.m_maxEspecial; LogisticaVars.especialT1Disponivel = true; }
                BarraEspecial(LogisticaVars.m_especialAtualT1, LogisticaVars.m_maxEspecial);
            }
            else
            {
                if (!LogisticaVars.especial) LogisticaVars.m_especialAtualT2 += Time.deltaTime * 0f; //mudar para 0.5f

                if (LogisticaVars.m_especialAtualT2 >= LogisticaVars.m_maxEspecial && !LogisticaVars.especialT2Disponivel)
                { LogisticaVars.m_especialAtualT2 = LogisticaVars.m_maxEspecial; LogisticaVars.especialT2Disponivel = true; }
                BarraEspecial(LogisticaVars.m_especialAtualT2, LogisticaVars.m_maxEspecial);
            }
        }

        if (LogisticaVars.tempoJogada > 20 && !LogisticaVars.trocarVez) { LogisticaVars.tempoJogada = 20; LogisticaVars.trocarVez = true; SetSituacao("trocar vez"); }
    }

    void TempoJogo()
    {
        LogisticaVars.segundosCorridos = Mathf.RoundToInt(LogisticaVars.tempoCorrido - (60 * LogisticaVars.minutosCorridos));

        if (LogisticaVars.tempoCorrido - (60 * LogisticaVars.minutosCorridos) >= 60) LogisticaVars.minutosCorridos++;
    }
    public void RotacaoAuto()
    {
        if (ui.rotacaoAutoBt.isOn)
        {
            ui.rotacaoAutoBt.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            LogisticaVars.redirecionamentoAutomatico = true;
        }
        else
        {
            ui.rotacaoAutoBt.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            LogisticaVars.redirecionamentoAutomatico = false;
        }
    }

    #region Situacoes
    public void SetSituacao(string situacao)
    {
        print("");
        print("* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *");
        print("* * * * * ANTERIOR: " + _situacaoAtual);
        print("Numero de controle: " + LogisticaVars.numControle);
        if (LogisticaVars.escolherOutroJogador) { StartCoroutine(EsperarParaMudarSituacao(situacao)); return; }

        switch (situacao)
        {
            case "pausar jogo":
                aux = _atual;
                _atual = Situacoes.PAUSADO;
                EstadoJogo.PausarJogo();
                return;
            case "despausar jogo":
                _atual = aux;
                EstadoJogo.DespausarJogo();
                return;
            case "quit jogo":
                _atual = Situacoes.NONE;
                EstadoJogo.QuitarJogo();
                return;
            case "comecar":
                _atual = Situacoes.COMECAR;
                _situacaoAtual = new Comecar(_current, VariaveisUIsGameplay._current, CamerasSettings._current);
                break;
            case "escolher outro":
                _atual = Situacoes.ESCOLHER;
                _situacaoAtual = new EscolherJogador(_current, VariaveisUIsGameplay._current, CamerasSettings._current);
                break;
            case "trocar vez":
                if (_atual == Situacoes.GOL) return;
                _atual = Situacoes.TROCAR_VEZ;
                _situacaoAtual = new TrocarVez(_current, VariaveisUIsGameplay._current, CamerasSettings._current);
                break;
            case "lateral":
                if (_atual == Situacoes.GOL) return;
                _atual = Situacoes.FORA;
                _situacaoAtual = new Lateral(_current, VariaveisUIsGameplay._current, CamerasSettings._current);
                break;
            case "escanteio":
                if (_atual == Situacoes.GOL) return;
                _atual = Situacoes.FORA;
                _situacaoAtual = new Escanteio(_current, VariaveisUIsGameplay._current, CamerasSettings._current);
                break;
            case "tiro de meta":
                if (_atual == Situacoes.GOL) return;
                _atual = Situacoes.FORA;
                _situacaoAtual = new Tiro_de_Meta(_current, VariaveisUIsGameplay._current, CamerasSettings._current);
                break;
            case "chute ao gol":
                _atual = Situacoes.CHUTE_AO_GOL;
                _situacaoAtual = new ChuteAoGol(_current, VariaveisUIsGameplay._current, CamerasSettings._current);
                break;
            case "pequena area":
                _atual = Situacoes.PEQUENA_AREA;
                _situacaoAtual = new GoleiroPequenaArea(_current, VariaveisUIsGameplay._current, CamerasSettings._current);
                break;
            case "gol":
                _atual = Situacoes.GOL;
                _situacaoAtual = new Gol(_current, VariaveisUIsGameplay._current, CamerasSettings._current);
                break;
            case "especial":
                _atual = Situacoes.ESPECIAL;
                _situacaoAtual = new Especial(_current, VariaveisUIsGameplay._current, CamerasSettings._current);
                break;
            case "fim":
                _atual = Situacoes.NONE;
                _situacaoAtual = new Fim(_current, VariaveisUIsGameplay._current, CamerasSettings._current);
                break;
        }
        LogisticaVars.numControle++;
        print("* * * * * " + _atual);
        LogisticaVars.fimSituacao = false;
        StartCoroutine(_situacaoAtual.Inicio());
    }
    IEnumerator EsperarParaMudarSituacao(string s)
    {
        Fim();
        yield return new WaitUntil(() => LogisticaVars.fimSituacao);
        print("Terminou espera");
        SetSituacao(s);
    }
    public Situacao GetSituacao()
    {
        return _situacaoAtual;
    }
    public void Fim()
    {
        print("* * * * * FIM: " + _situacaoAtual);
        StartCoroutine(_situacaoAtual.Fim());
    }

    #region Comecar
    public void AjeitarBarraChute()
    {
        FollowWorld barra = FindObjectOfType<FollowWorld>();
        barra.lookAt = LogisticaVars.m_jogadorEscolhido_Atual.transform;
        barra.PosicionarBarra();
    }
    #endregion

    #region Chute ao Gol
    public void GoleiroPosicionado()
    {
        StartCoroutine(_situacaoAtual.Meio());
    }
    #endregion

    #region Selecionar Outro
    public void CriarIconesSelecao(List<GameObject> jogadores)
    {
        Vector3 jogadorRef = LogisticaVars.m_jogadorEscolhido_Atual.transform.position;
        float distancia;
        foreach (GameObject jogador in jogadores)
        {
            if (jogador.CompareTag("Player"))
            {
                GameObject icone = new GameObject();
                distancia = (jogadorRef - jogador.transform.position).magnitude;
                if (distancia <= 15) icone = VariaveisUIsGameplay._current.iconePequeno;
                else if (distancia > 15 && distancia <= 40) icone = VariaveisUIsGameplay._current.iconeMedio;
                else icone = VariaveisUIsGameplay._current.iconeGrande;

                icone.GetComponent<LinkarBotaoComIcone>().cam = FindObjectOfType<Camera>();
                icone.GetComponent<LinkarBotaoComIcone>().jogadorReferenciado = jogador;

                Instantiate(icone, GameObject.Find("Canvas").transform.GetChild(1));
            }
        }
    }
    public bool VerificarIconesSelecao()
    {
        if (GameObject.FindGameObjectWithTag("Icone Selecao") != null) return true;
        else return false;
    }
    public List<GameObject> ListaIconesSelecao()
    {
        List<GameObject> icones = new List<GameObject>();
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("Icone Selecao")) icones.Add(i);

        return icones;
    }
    public void DestruirIconesSelecao()
    {
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("Icone Selecao")) Destroy(i.gameObject);
    }
    public void OnJogadorSelecionado()
    {
        StartCoroutine(_situacaoAtual.Fim());
    }
    public void RotacionarJogadorPerto(GameObject jogadorPerto)
    {
        StartCoroutine(_situacaoAtual.RotacionarParaJogadorMaisPerto(jogadorPerto));
    }
    public void Situacao_BolaRasteira()
    {
        if (LogisticaVars.vezJ1)
        {
            if (LogisticaVars.bolaRasteiraT1) ui.direcaoBolaBt.isOn = true;
            else ui.direcaoBolaBt.isOn = false;
        }
        else
        {
            if (LogisticaVars.bolaRasteiraT2) ui.direcaoBolaBt.isOn = true;
            else ui.direcaoBolaBt.isOn = false;
        }
        ui.UI_BolaRasteira();
    }
    #endregion

    #region Trocar vez

    #endregion

    #region Gol

    #endregion

    #region Especial
    public void BarraEspecial(float atual, float max)
    {
        ui.barraEspecial.GetComponent<Image>().fillAmount = atual / max;
    }
    public void InstanciarMira()
    {
        var _mira = ui.miraEspecial;
        Instantiate(_mira, FindObjectOfType<Camera>().WorldToScreenPoint(GameObject.FindGameObjectWithTag("Direcao Especial").transform.position,
            Camera.MonoOrStereoscopicEye.Mono), Quaternion.identity, canvas.transform.GetChild(2));
        mira = _mira;
    }
    public void TravarMira()
    {
        FindObjectOfType<MiraEspecial>().TravarMira();
        ui.travarMiraBt.gameObject.SetActive(false);
        ui.chuteEspecialBt.gameObject.SetActive(true);
    }
    public void DestruirObjetosEspecial()
    {
        Destroy(GameObject.FindGameObjectWithTag("Mira Especial"));
        Destroy(GameObject.FindGameObjectWithTag("Trajetoria Especial"));
    }
    #endregion

    #region Fora
    public void Spawnar(string situacao)
    {
        switch (situacao)
        {
            case "lateral":
                if (_bola.m_pos.x < 0) StartCoroutine(_situacaoAtual.Spawnar("lateral esquerda"));
                else StartCoroutine(_situacaoAtual.Spawnar("lateral direita"));
                break;
            case "escanteio":
                if (_bola.m_pos.z > 0) StartCoroutine(_situacaoAtual.Spawnar("fundo 2"));
                else StartCoroutine(_situacaoAtual.Spawnar("fundo 1"));
                break;
            case "tiro de meta":
                if (_bola.m_pos.z > 0) StartCoroutine(_situacaoAtual.Spawnar("fundo 2"));
                else StartCoroutine(_situacaoAtual.Spawnar("fundo 1"));
                break;
        }
    }
    #endregion

    #endregion
}
