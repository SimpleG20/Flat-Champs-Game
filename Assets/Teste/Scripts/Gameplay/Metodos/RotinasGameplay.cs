using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RotinasGameplay : MonoBehaviour
{
    int tempo = LogisticaVars.tempoMaxEscolhaJogador;

    static FisicaBola bola;
    static UIMetodosGameplay ui;
    static EventsManager events;

    private void Start()
    {
        ui = FindObjectOfType<UIMetodosGameplay>();
        bola = FindObjectOfType<FisicaBola>();

        events = EventsManager.current;
        events.onAplicarRotinas += RotinasPosSituacoes;
    }

    private void RotinasPosSituacoes(string s)
    {
        switch (s)
        {
            case "rotina 3 jogadas":
                StartCoroutine(TrocarVezPos3Jogadas());
                break;

            case "rotina chute inicial":
                StartCoroutine(ChuteInicial());
                break;
            case "rotina animacao gol":
                StartCoroutine(AnimacaoTorcida());
                break;
            case "rotina chute ao gol":
                StartCoroutine(GoleiroDefenderChuteAoGol());
                break;
            case "rotina sair lateral":
                StartCoroutine(SairLateral(bola));
                break;
            case "rotina sair escanteio":
                StartCoroutine(SairEscanteio(bola));
                break;


            case "rotina pos chute goleiro":
                StartCoroutine(RotinaAposChuteGoleiro());
                break;


            #region Tempo Para Chute Auto
            case "rotina tempo chute ao gol":
                StartCoroutine(TempoParaChuteAoGol());
                break;
            case "rotina tempo lateral":
                StartCoroutine(TempoParaLateral());
                break;
            case "rotina tempo escanteio":
                StartCoroutine(TempoParaEscanteio());
                break;
            case "rotina tempo tiro de meta":
                StartCoroutine(TempoParaTiroDeMeta());
                break;
            case "rotina tempo pequena area":
                StartCoroutine(TempoParaTirarBolaDaPequenaArea());
                break;
            case "rotina tempo especial":
                StartCoroutine(TempoParaEspecial());
                break;
            #endregion

            case "rotina tempo selecao":
                StartCoroutine(TempoParaSelecao());
                break;
        }
    }

    IEnumerator TrocarVezPos3Jogadas()
    {
        print("Rotina trocar pos 3 jogadas");
        yield return new WaitForSeconds(0.01f);
        if (!bola.m_bolaCorrendo)
        {
            if (!LogisticaVars.continuaSendoFora && !LogisticaVars.tiroDeMeta) events.OnTrocarVez();
        }
        else StartCoroutine(TrocarVezPos3Jogadas());
    }


    #region Inicio
    IEnumerator ChuteInicial()
    {
        JogadorMetodos.ResetarValoresChute();
        Debug.Log("7 segundos para dar o chute Inicial");
        yield return new WaitForSeconds(7f);
        if (!LogisticaVars.aplicouPrimeiroToque)
        {
            events.SituacaoGameplay("aplicar toque inicial");
            events.SituacaoGameplay("jogo normal");
        }
    }
    #endregion

    #region Chutes Automaticos
    IEnumerator TempoParaTirarBolaDaPequenaArea()
    {

        yield return new WaitForSeconds(9);
        if (LogisticaVars.bolaPermaneceNaPequenaArea)
        {
            if (LogisticaVars.bolaRasteiraT1 && LogisticaVars.vezJ1 || LogisticaVars.bolaRasteiraT2 && LogisticaVars.vezJ2) GoleiroMetodos.ChuteAutomatico(true);
            else GoleiroMetodos.ChuteAutomatico(false);
            LogisticaVars.bolaPermaneceNaPequenaArea = false;
        }
    }
    IEnumerator TempoParaTiroDeMeta()
    {
        yield return new WaitForSeconds(11);
        if (LogisticaVars.tiroDeMeta)
        {
            if (LogisticaVars.bolaRasteiraT1 && LogisticaVars.vezJ1 || LogisticaVars.bolaRasteiraT2 && LogisticaVars.vezJ2) GoleiroMetodos.ChuteAutomatico(true);
            else GoleiroMetodos.ChuteAutomatico(false);
        }
    }
    IEnumerator TempoParaLateral()
    {
        yield return new WaitForSeconds(9);
        if (LogisticaVars.lateral)
        {
            Debug.Log("Lateral Automatico");
            JogadorMetodos.AutoChuteLateral();
        }
    }
    IEnumerator TempoParaEscanteio()
    {
        yield return new WaitForSeconds(12);
        if (LogisticaVars.continuaSendoFora)
        {
            Debug.Log("Escanteio Automatico");
            JogadorVars.m_forca = 15;
            JogadorMetodos.AutoChuteEscanteio();
        }
    }
    IEnumerator TempoParaChuteAoGol()
    {
        yield return new WaitForSeconds(7);
        if (LogisticaVars.auxChuteAoGol)
        {
            Debug.Log("TempoParaChuteAoGol(): Jogadas = 3");
            LogisticaVars.jogadas = 3;
        }
        LogisticaVars.auxChuteAoGol = false;
    }
    IEnumerator TempoParaEspecial()
    {
        EventsManager.current.OnAplicarMetodosUiSemBotao("especial");
        //UIMetodosGameplay.Especial();

        yield return new WaitForSeconds(30); //15
        if (!LogisticaVars.aplicouEspecial)
        {
            Debug.Log("Tempo para o Especial acabou :(");
            Destroy(GameObject.FindGameObjectWithTag("Mira Especial"));
            events.OnAplicarMetodosUiSemBotao("fim especial");

            Physics.gravity = new Vector3(0, -9.81f, 0);
            LogisticaVars.aplicouEspecial = true;
        }
    }
    #endregion

    #region Gol
    IEnumerator AnimacaoTorcida()
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !FindObjectOfType<CinemachineBrain>().IsBlending);
        Debug.Log("Começando a animacao da torcida");

        Player jogador1 = GameManager.Instance.m_usuario;

        if(LogisticaVars.golT1)
        {
            ui.golMarcadoGO.GetComponent<GolComponentes>().SpriteTimeMarcou(jogador1.m_baseLogo,
                jogador1.m_fundoLogo, jogador1.m_simboloLogo, jogador1.m_corPrimaria, jogador1.m_corSecundaria, jogador1.m_corTerciaria);
        } 
        else
        {
            if (GameManager.Instance.m_offline)
            {
                Teams jogador2 = GameManager.Instance.GetTimeOff();
                ui.golMarcadoGO.GetComponent<GolComponentes>().SpriteTimeMarcou(jogador2.m_base,
                    jogador2.m_fundo, jogador2.m_simbolo, jogador2.m_corPrimaria, jogador2.m_corSecundaria, jogador2.m_corTerciaria);
            }
        }
        ui.golMarcadoGO.SetActive(true);
        ui.golMarcadoGO.GetComponent<Animator>().SetBool("Gol", true);

        yield return new WaitForSeconds(0.5f);

        ui.golMarcadoGO.GetComponent<Animator>().SetBool("Gol", false);
        for (int i = 0; i < FindObjectOfType<Abertura>().QuantiaJogadores(); i++)
        {
            LogisticaVars.jogadoresT1[i].transform.position = new Vector3(LogisticaVars.esquemaT1[i, 0], LogisticaVars.jogadoresT1[i].transform.position.y, LogisticaVars.esquemaT1[i, 1]);
            LogisticaVars.jogadoresT2[i].transform.position = new Vector3(-LogisticaVars.esquemaT2[i, 0], LogisticaVars.jogadoresT2[i].transform.position.y, -LogisticaVars.esquemaT2[i, 1]);
        }
        GameObject.FindGameObjectWithTag("Goleiro1").transform.position = FindObjectOfType<Abertura>().PosicaoGoleiro(1);
        GameObject.FindGameObjectWithTag("Goleiro2").transform.position = FindObjectOfType<Abertura>().PosicaoGoleiro(2);

        yield return new WaitForSeconds(4f);

        events.OnAplicarMetodosUiSemBotao("estado jogador e goleiro", "", false);

        events.SituacaoGameplay("reiniciar pos gol");
        ui.golMarcadoGO.SetActive(false);

        events.SituacaoGameplay("toque pos gol");
        GameObject.Find("Canvas").transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 1;
    }
    #endregion

    #region Selecao
    public static IEnumerator DesabilitarApos3Jogadas(FisicaBola bola)
    {
        events.OnAplicarMetodosUiSemBotao("estado jogador e goleiro", "", false);
        
        Debug.Log("Trocando a vez");

        yield return new WaitForSeconds(1f);
        if (!LogisticaVars.desabilitouDadosJogador)
        {
            //JogadorMetodos.ResetarValoresChute();

            bola.RedirecionarJogadores(true);

            SelecaoMetodos.DesabilitarDadosJogador();
            events.EscolherJogador();
            SelecaoMetodos.DadosJogador();
            

            SelecaoMetodos.DesabilitarComponentesDosNaoSelecionados();
            //events.OnAplicarMetodosUiSemBotao("estados dos botoes", "normal");
            events.OnAplicarMetodosUiComBotao("bola rasteira");
        }
        LogisticaVars.esperandoTrocas = false;
        LogisticaVars.trocarVez = false;

        yield return new WaitForSeconds(1);
        events.SituacaoGameplay("jogo normal");
    }
    IEnumerator TempoParaSelecao()
    {
        yield return new WaitForSeconds(0.95f);
        tempo -= 1;
        if (!LogisticaVars.escolheu)
        {
            if (tempo == 0) { events.OnAplicarMetodosUiComBotao("desistir selecao"); tempo = LogisticaVars.tempoMaxEscolhaJogador; }
            else StartCoroutine(TempoParaSelecao());
        }
        else tempo = LogisticaVars.tempoMaxEscolhaJogador;
    }
    #endregion

    #region Goleiro
    IEnumerator GoleiroDefenderChuteAoGol()
    {
        events.SituacaoGameplay("jogo parado");
        Debug.Log("Posicione o goleiro");
        events.OnAplicarMetodosUiSemBotao("estados dos botoes", "defender chute ao gol");

        SelecaoMetodos.EscolherGoleiro();

        GoleiroMetodos.ComponentesParaGoleiro(true);
        ui.barraChuteJogador.SetActive(false);
        ui.barraChuteGoleiro.SetActive(false);

        LogisticaVars.defenderGoleiro = true;
        LogisticaVars.acionouChuteAoGol = false;

        yield return new WaitForSeconds(8);
        if (LogisticaVars.goleiroT1 && LogisticaVars.defenderGoleiro || LogisticaVars.goleiroT2 && LogisticaVars.defenderGoleiro)
        {
            print("Goleiro Posicionado Auto");
            events.OnAplicarMetodosUiComBotao("goleiro posicionado");
        }
    }
    IEnumerator RotinaAposChuteGoleiro()
    {
        LogisticaVars.bolaPermaneceNaPequenaArea = false;

        LogisticaVars.podeRedirecionar = true;

        LogisticaVars.fundo1 = LogisticaVars.fundo2 = LogisticaVars.tiroDeMeta = false;
        //LogisticaVars.continuaSendoFora = false;

        Debug.Log("Camera tiro de meta");
        GoleiroMetodos.ComponentesParaGoleiro(false);

        events.SituacaoGameplay("habilitar camera tiro de meta");
        events.OnAplicarMetodosUiSemBotao("estados dos botoes", "camera tiro de meta");

        yield return new WaitForSeconds(1);

        StartCoroutine(PosChuteGoleiro(bola));
        yield return new WaitForSeconds(0.5f);

        if (LogisticaVars.goleiroT2) LogisticaVars.m_goleiroGameObject.transform.position = new Vector3(0, LogisticaVars.m_goleiroGameObject.transform.position.y, FindObjectOfType<Abertura>().PosicaoGoleiro(2).z);
        else LogisticaVars.m_goleiroGameObject.transform.position = new Vector3(0, LogisticaVars.m_goleiroGameObject.transform.position.y, FindObjectOfType<Abertura>().PosicaoGoleiro(1).z);

        LogisticaVars.goleiroT2 = LogisticaVars.goleiroT1 = LogisticaVars.m_goleiroEscolhido = false;
        LogisticaVars.m_goleiroGameObject = null;
    }
    #endregion

    #region Fora
    public static IEnumerator SpawnarLat(string lateral, FisicaBola bola)
    {
        Debug.Log("Spawnar Lateral");

        yield return new WaitForSeconds(1.5f);

        LogisticaVars.esperandoTrocas = false;

        if (lateral == "lateral direita")
        {
            LogisticaVars.m_jogadorEscolhido.transform.position = new Vector3(bola.m_posLateral.x + 3f, LogisticaVars.m_jogadorEscolhido.transform.position.y, bola.m_posLateral.z);

            LogisticaVars.m_rbJogadorEscolhido.velocity = Vector3.zero;
            LogisticaVars.foraLateralD = false;
        }
        if (lateral == "lateral esquerda")
        {
            LogisticaVars.m_jogadorEscolhido.transform.position = new Vector3(bola.m_posLateral.x - 3f, LogisticaVars.m_jogadorEscolhido.transform.position.y, bola.m_posLateral.z);

            LogisticaVars.m_rbJogadorEscolhido.velocity = Vector3.zero;
            LogisticaVars.foraLateralE = false;
        }

        yield return new WaitForSeconds(0.5f);
        events.SituacaoGameplay("desabilitar camera lateral");

        LogisticaVars.m_jogadorEscolhido.transform.GetChild(3).gameObject.SetActive(true);

        bola.RedirecionarJogadorEscolhido(bola.transform);
        LogisticaVars.podeRedirecionar = true;
    }
    public static IEnumerator SpawnarEscanteio(string fundo, FisicaBola bola)
    {
        Debug.Log("Spawnar Escanteio");

        yield return new WaitForSeconds(1.5f);

        Vector3 novaPos = new Vector3(bola.m_posicaoFundo.x, LogisticaVars.m_jogadorEscolhido.transform.position.y, bola.m_posicaoFundo.z);

        if (fundo == "fundo 1")
        {
            if (bola.transform.position.x < 0) LogisticaVars.m_jogadorEscolhido.transform.position = novaPos + new Vector3(-2f, 0, -1.5f);
            else LogisticaVars.m_jogadorEscolhido.transform.position = novaPos + new Vector3(+2f, 0, -1.5f);

            LogisticaVars.fundo1 = false;
        }
        else if (fundo == "fundo 2")
        {
            if (bola.transform.position.x < 0) LogisticaVars.m_jogadorEscolhido.transform.position = novaPos + new Vector3(-2f, 0, +1.5f);
            else LogisticaVars.m_jogadorEscolhido.transform.position = novaPos + new Vector3(+2f, 0, +1.5f);

            LogisticaVars.fundo2 = false;
        }

        yield return new WaitForSeconds(0.5f);

        LogisticaVars.esperandoTrocas = false;

        events.SituacaoGameplay("desabilitar camera lateral");

        bola.RedirecionarJogadorEscolhido(bola.transform);
        LogisticaVars.podeRedirecionar = true;
    }
    IEnumerator SairLateral(FisicaBola bola)
    {
        ui.botaoMeio.SetActive(false);
        ui.lateralBt.gameObject.SetActive(false);
        events.SituacaoGameplay("jogo normal");

        LogisticaVars.lateral = false;

        yield return new WaitForSeconds(1f);
        Transform jogador = LogisticaVars.m_jogadorEscolhido.transform;

        Debug.Log("Deslocando Jogador");
        Vector3 target;
        if (jogador.position.x < 0) target = new Vector3(bola.m_posLateral.x + 2f, jogador.position.y, bola.m_posLateral.z);
        else target = new Vector3(bola.m_posLateral.x - 2f, jogador.position.y, bola.m_posLateral.z);
        StartCoroutine(MovimentoSairFora(target));

        LogisticaVars.continuaSendoFora = false;
    }
    IEnumerator SairEscanteio(FisicaBola bola)
    {
        ui.botaoMeio.SetActive(false);
        ui.escanteioBt.gameObject.SetActive(false);
        events.SituacaoGameplay("jogo normal");

        LogisticaVars.foraFundo = false;

        yield return new WaitForSeconds(1);
        Transform jogador = LogisticaVars.m_jogadorEscolhido.transform;

        Vector3 target = new Vector3(bola.m_posicaoFundo.x, jogador.position.y, bola.m_posicaoFundo.z);

        if(jogador.position.z < 0)
        {
            if (jogador.position.x < 0) target += Vector3.forward * 1.5f + Vector3.right * 1.5f;
            else target += Vector3.forward * 1.5f + Vector3.left * 1.5f;
        }
        else
        {
            if (jogador.position.x < 0) target += Vector3.back * 1.5f + Vector3.right * 1.5f;
            else target += Vector3.back * 1.5f + Vector3.left * 1.5f;
        }

        StartCoroutine(MovimentoSairFora(target));

        if (LogisticaVars.vezJ1) LogisticaVars.ultimoToque = 1;
        else LogisticaVars.ultimoToque = 2;

        LogisticaVars.continuaSendoFora = false;
    }
    IEnumerator MovimentoSairFora(Vector3 target)
    {
        float fator = 0;
        LogisticaVars.m_rbJogadorEscolhido.isKinematic = true;

        yield return new WaitForSeconds(0.01f);
        fator += 0.3f;
        LogisticaVars.m_jogadorEscolhido.transform.position = Vector3.MoveTowards(LogisticaVars.m_jogadorEscolhido.transform.position, target, fator);

        if ((LogisticaVars.m_jogadorEscolhido.transform.position - target).magnitude > 0.1f) StartCoroutine(MovimentoSairFora(target));
        else
        {
            JogadorMetodos.ResetarValoresChute();
            GameObject.Find("Bandeira Dir").transform.GetChild(0).gameObject.SetActive(true);
            GameObject.Find("Bandeira Esq").transform.GetChild(0).gameObject.SetActive(true);
            GameObject.Find("Bandeira Dir 2").transform.GetChild(0).gameObject.SetActive(true);
            GameObject.Find("Bandeira Esq 2").transform.GetChild(0).gameObject.SetActive(true);
            LogisticaVars.m_rbJogadorEscolhido.isKinematic = false;
            events.OnAplicarMetodosUiSemBotao("estados dos botoes", "normal");
        }
    }

    public static IEnumerator SpawnarTiroDeMeta(string fundo, FisicaBola bola)
    {
        //LogisticaVars.esperandoTrocas = true;

        yield return new WaitForSeconds(1f);

        if (fundo == "fundo 2")
        {
            Debug.Log("Fundo 2");
            LogisticaVars.tiroDeMeta = true;
            LogisticaVars.goleiroT2 = true;
            LogisticaVars.m_goleiroGameObject = GameObject.FindGameObjectWithTag("Goleiro2");

            LogisticaVars.m_goleiroGameObject.transform.position = new Vector3(bola.m_posicaoFundo.x, LogisticaVars.m_goleiroGameObject.transform.position.y, bola.m_posicaoFundo.z + 3);
            LogisticaVars.fundo2 = false;
        }
        if (fundo == "fundo 1")
        {
            Debug.Log("Fundo 1");
            LogisticaVars.tiroDeMeta = true;
            LogisticaVars.goleiroT1 = true;
            LogisticaVars.m_goleiroGameObject = GameObject.FindGameObjectWithTag("Goleiro1");

            LogisticaVars.m_goleiroGameObject.transform.position = new Vector3(bola.m_posicaoFundo.x, LogisticaVars.m_goleiroGameObject.transform.position.y, bola.m_posicaoFundo.z - 3);

            LogisticaVars.fundo1 = false;
        }

        yield return new WaitForSeconds(1f);

        LogisticaVars.esperandoTrocas = false;
        bola.m_toqueChao = false;
        
        events.SituacaoGameplay("desabilitar camera lateral");
        GoleiroMetodos.ComponentesParaGoleiro(true);
        bola.RedirecionarGoleiros();
    }
    IEnumerator PosChuteGoleiro(FisicaBola bola)
    {
        if (!LogisticaVars.bolaRasteiraT1 && !LogisticaVars.bolaRasteiraT2) yield return new WaitUntil(() => bola.m_toqueChao);
        else yield return new WaitUntil(() => !bola.m_bolaCorrendo);

        Debug.Log("Bola relou no chao");
        events.SituacaoGameplay("jogo normal");
        LogisticaVars.continuaSendoFora = false;
        LogisticaVars.tiroDeMeta = false;
        LogisticaVars.esperandoTrocas = false;
        JogadorMetodos.ResetarValoresChute();

        bola.RedirecionarJogadores(true);
        events.SelecaoAutomatica();

        events.SituacaoGameplay("desabilitar camera tiro de meta");
    }
    #endregion
}