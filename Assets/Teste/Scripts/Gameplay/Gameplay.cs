using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour
{
    public enum Situacoes { JOGO_NORMAL, FORA, PAUSADO}

    public Vector3 posGol1, posGol2;
    public Quaternion rotacaoAnt;
    public GameObject canvas, mira;

    public Situacoes _atual;
    Situacao _situacaoAtual;

    public Abertura abertura;
    public FisicaBola _bola;
    VariaveisUIsGameplay ui;

    public static Gameplay _current;
    private void Awake()
    {
        _current = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        ui = VariaveisUIsGameplay._current;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSituacao(Situacao situacao)
    {
        _situacaoAtual = situacao;
        _situacaoAtual.Inicio();
    }
    public Situacao GetSituacao()
    {
        return _situacaoAtual;
    }
    public void Fim()
    {
        StartCoroutine(_situacaoAtual.Fim());
    }

    #region Comecar
    public void AjeitarBarraChute()
    {
        FindObjectOfType<FollowWorld>().lookAt = LogisticaVars.m_jogadorEscolhido_Atual.transform;
    }
    #endregion

    #region Chute ao Gol
    public void OnChuteAoGol() //Botao Chute ao Gol
    {
        SetSituacao(new ChuteAoGol(_current, VariaveisUIsGameplay._current, CamerasSettings._current));
    }
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
    public void OnSelecionarOutro()//Botao
    {
        SetSituacao(new EscolherJogador(_current, VariaveisUIsGameplay._current, CamerasSettings._current));
    }
    public void OnJogadorSelecionado()
    {
        _situacaoAtual.Fim();
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
        Instantiate(ui.miraEspecial, FindObjectOfType<Camera>().WorldToScreenPoint(GameObject.FindGameObjectWithTag("Direcao Especial").transform.position,
            Camera.MonoOrStereoscopicEye.Mono), Quaternion.identity, canvas.transform.GetChild(2));
    }
    public void TravarMira()
    {
        mira.GetComponent<MiraEspecial>().TravarMira();
        ui.travarMiraBt.gameObject.SetActive(false);
        ui.chuteEspecialBt.gameObject.SetActive(true);
    }
    public void DestruirObjetosEspecial()
    {
        Destroy(GameObject.FindGameObjectWithTag("Mira Especial"));
        Destroy(GameObject.FindGameObjectWithTag("Trajetoria Especial"));
    }
    #endregion

    public void Spawnar(string situacao)
    {
        switch (situacao)
        {
            case "lateral":
                if (_bola.m_pos.x < 0) StartCoroutine(_situacaoAtual.Spawnar("lateral esquerda"));
                else StartCoroutine(_situacaoAtual.Spawnar("lateral direita"));
                break;
            case "escanteio":
                if (_bola.m_pos.z < 0) StartCoroutine(_situacaoAtual.Spawnar("fundo 2"));
                else StartCoroutine(_situacaoAtual.Spawnar("fundo 1"));
                break;
            case "tiro de meta":
                if (_bola.m_pos.z < 0) StartCoroutine(_situacaoAtual.Spawnar("fundo 2"));
                else StartCoroutine(_situacaoAtual.Spawnar("fundo 1"));
                break;
        }
    }
}
