using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SelecaoMetodos : MonoBehaviour
{
    static int numTrocaAtual, numTrocaAnterior;

    Quaternion rotacaoAnt;
    static FisicaBola bola;
    static UIMetodosGameplay ui;
    static EventsManager events;

    private void Start()
    {
        bola = FindObjectOfType<FisicaBola>();
        ui = FindObjectOfType<UIMetodosGameplay>();

        events = EventsManager.current;
        events.onAplicarMetodosUiComBotao += MetodosBotoesUI;
        events.onTrocarVez += FimDaVez;
        events.onEscolherJogador += EscolherJogador;
        events.onSelecaoAutomatica += SelecaoAutomatica;
        events.onAjeitarCamera += AjustarCameraParaSelecao;
    }

    private void MetodosBotoesUI(string s)
    {
        switch (s)
        {
            case "selecionar outro":
                SelecionarOutroJogador();
                break;
            case "desistir selecao":
                DesistirDeSelecionarOutroJogador();
                break;
                
        }
    }

    #region Selecionar Outro jogador
    void AjustarCameraParaSelecao(float y)
    {
        if(y == 1)
        {
            LogisticaVars.m_jogadorEscolhido.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>().m_Priority = 0;
            LogisticaVars.m_jogadorEscolhido.transform.GetChild(1).GetChild(1).GetComponent<CinemachineVirtualCamera>().m_Priority = 99;
            LogisticaVars.cameraJogador = LogisticaVars.m_jogadorEscolhido.transform.GetChild(1).GetChild(1).GetComponent<CinemachineVirtualCamera>();
        }
        else
        {
            LogisticaVars.m_jogadorEscolhido.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>().m_Priority = 99;
            LogisticaVars.m_jogadorEscolhido.transform.GetChild(1).GetChild(1).GetComponent<CinemachineVirtualCamera>().m_Priority = 0;
            LogisticaVars.cameraJogador = LogisticaVars.m_jogadorEscolhido.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>();
        }
    }
    void ColocarIconeParaSelecao(List<GameObject> jogadores)
    {
        Vector3 jogadorRef = LogisticaVars.m_jogadorEscolhido.transform.position;
        float distancia;
        foreach (GameObject jogador in jogadores)
        {
            if (jogador.CompareTag("Player"))
            {
                GameObject icone = new GameObject();
                distancia = (jogadorRef - jogador.transform.position).magnitude;
                if (distancia <= 15) icone = ui.iconePequeno;
                else if (distancia > 15 && distancia <= 40) icone = ui.iconeMedio;
                else icone = ui.iconeGrande;

                icone.GetComponent<LinkarBotaoComIcone>().cam = FindObjectOfType<Camera>();
                icone.GetComponent<LinkarBotaoComIcone>().jogadorReferenciado = jogador;

                Instantiate(icone, GameObject.Find("Canvas").transform.GetChild(1));
            }
        }
    }
    IEnumerator EsperarTransicaoCameras(bool jogador, string situacao)
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !FindObjectOfType<CinemachineBrain>().IsBlending);

        GameObject jogadorPerto = null;

        switch (situacao) 
        {
            case "selecionar outro":
                if (LogisticaVars.vezJ1) {
                    jogadorPerto = QuemEstaMaisPerto(LogisticaVars.m_jogadorEscolhido.transform.position, LogisticaVars.jogadoresT1);
                    ColocarIconeParaSelecao(LogisticaVars.jogadoresT1);
                }
                else {
                    jogadorPerto = QuemEstaMaisPerto(LogisticaVars.m_jogadorEscolhido.transform.position, LogisticaVars.jogadoresT2);
                    ColocarIconeParaSelecao(LogisticaVars.jogadoresT2);
                }
                break;
            case "ui normal":
                if (!LogisticaVars.continuaSendoFora)
                {
                    JogadorMetodos.ResetarValoresChute();
                    events.OnAplicarMetodosUiSemBotao("estados dos botoes", "normal");
                    FindObjectOfType<CinemachineBrain>().m_DefaultBlend.m_Time = 1f;
                }
                break;

        }


        if (jogador && !JogadorVars.m_rotacionar) StartCoroutine(RotacionarJogadorMaisPerto(jogadorPerto));
    }

    IEnumerator RotacionarJogadorMaisPerto(GameObject jogadorPerto)
    {
        float step = 0;

        yield return new WaitForSeconds(0.01f);
        step += 0.05f;
        GameObject.Find("RotacaoCamera").transform.position =
            Vector3.MoveTowards((LogisticaVars.m_jogadorEscolhido.transform.position - LogisticaVars.m_jogadorEscolhido.transform.up), jogadorPerto.transform.position, step);

        Vector3 direction = GameObject.Find("RotacaoCamera").transform.position - LogisticaVars.m_jogadorEscolhido.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        LogisticaVars.m_jogadorEscolhido.transform.rotation = rotation;
        LogisticaVars.m_jogadorEscolhido.transform.eulerAngles = new Vector3(-90, LogisticaVars.m_jogadorEscolhido.transform.eulerAngles.y, LogisticaVars.m_jogadorEscolhido.transform.eulerAngles.z);

        if (rotacaoAnt != rotation && !JogadorVars.m_rotacionar && !LogisticaVars.escolheu) { rotacaoAnt = rotation; StartCoroutine(RotacionarJogadorMaisPerto(jogadorPerto)); }
        else rotacaoAnt = Quaternion.identity;
    }

    void SelecionarOutroJogador()
    {
        LogisticaVars.escolheu = false;
        FindObjectOfType<CinemachineBrain>().m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseOut;
        FindObjectOfType<CinemachineBrain>().m_DefaultBlend.m_Time = 0.5f;

        AjustarCameraParaSelecao(1);
        StartCoroutine(EsperarTransicaoCameras(true, "selecionar outro"));
        events.OnAplicarMetodosUiSemBotao("estados dos botoes", "selecao");
        events.OnAplicarRotinas("rotina tempo selecao");

        LogisticaVars.jogadorSelecionado = false;
        LogisticaVars.escolherOutroJogador = true;
        LogisticaVars.m_tempoSelecaoAnimator.SetBool("SelecionarJogador", true);
        LogisticaVars.m_tempoSelecaoAnimator.SetBool("SairSelecionarJogador", false);
    } //Tudo certo
    void DesistirDeSelecionarOutroJogador()
    {
        print("Desistiu Troca, Anterior: " + numTrocaAnterior + " e Atual: " + numTrocaAtual);
        LogisticaVars.escolheu = true;
        foreach (LinkarBotaoComIcone l in FindObjectsOfType<LinkarBotaoComIcone>()) Destroy(l.gameObject);

        AjustarCameraParaSelecao(-1);
        LogisticaVars.escolherOutroJogador = false;
        LogisticaVars.m_tempoSelecaoAnimator.SetBool("SelecionarJogador", false);
        LogisticaVars.m_tempoSelecaoAnimator.SetBool("SairSelecionarJogador", true);
        ui.sairSelecaoBt.gameObject.SetActive(false);

        if (LogisticaVars.trocarVez) TrocarVez();
        else
        {
            events.OnAplicarMetodosUiComBotao("jogador selecionado");
            LogisticaVars.jogadorSelecionado = true;

            if (!LogisticaVars.continuaSendoFora && !LogisticaVars.tiroDeMeta) StartCoroutine(EsperarTransicaoCameras(false, "ui normal"));
        }

        JogadorMetodos.ResetarValoresChute();
    }
    #endregion

    #region Automatico
    private void EscolherJogador()
    {
        if (LogisticaVars.m_jogadorEscolhido != null)
            LogisticaVars.m_jogadorEscolhido.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>().m_Priority = 0;

        if (LogisticaVars.vezJ1) LogisticaVars.m_jogadorEscolhido = QuemEstaMaisPerto(GameObject.FindGameObjectWithTag("Bola").transform.position, LogisticaVars.jogadoresT1);
        else LogisticaVars.m_jogadorEscolhido = QuemEstaMaisPerto(GameObject.FindGameObjectWithTag("Bola").transform.position, LogisticaVars.jogadoresT2);

        LogisticaVars.m_jogadorEscolhido.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>().m_Priority = 99;
        
        
        if(LogisticaVars.aplicouPrimeiroToque && !LogisticaVars.continuaSendoFora) StartCoroutine(EsperarTransicaoCameras(false, "ui normal"));
        SituacaoBolaRasteira();
    }
    public static void EscolherGoleiro()
    {
        if (LogisticaVars.vezJ1)
        {
            LogisticaVars.goleiroT2 = true;
            LogisticaVars.m_goleiroGameObject = GameObject.FindGameObjectWithTag("Goleiro2");
        }
        else if (LogisticaVars.vezJ2)
        {
            LogisticaVars.goleiroT1 = true;
            LogisticaVars.m_goleiroGameObject = GameObject.FindGameObjectWithTag("Goleiro1");
        }
        SituacaoBolaRasteira();
    }
    void SelecaoAutomatica()
    {
        LogisticaVars.escolherOutroJogador = false;
        ui.sairSelecaoBt.gameObject.SetActive(false);
        Debug.Log("Escolha Automática");
        DesabilitarDadosJogador();
        EscolherJogador();
        DadosJogador();
    }
    GameObject QuemEstaMaisPerto(Vector3 referencia, List<GameObject> jogadores)
    {
        GameObject jogador = null;
        float distancia, distanciaMenor;
        distanciaMenor = 1000;

        foreach (GameObject Jg in jogadores)
        {
            distancia = (referencia - Jg.transform.position).magnitude;
            if (distancia <= distanciaMenor && distancia != 0)
            {
                distanciaMenor = distancia;
                jogador = Jg;
            }
            else continue;
        }
        return jogador;
    } //Tudo certo
    #endregion

    #region Dados
    public static void DadosJogador()
    {
        LogisticaVars.m_jogadorEscolhido.tag = "Player Selecionado";
        LogisticaVars.cameraJogador = LogisticaVars.m_jogadorEscolhido.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>();
        LogisticaVars.m_jogadorEscolhido.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>().m_Priority = 99;
        
        //LogisticaVars.m_jogadorEscolhido.GetComponentInChildren<AudioListener>().enabled = true;
        LogisticaVars.m_jogadorEscolhido.GetComponent<FisicaJogador>().enabled = true;
        JogadorVars.m_fisica = LogisticaVars.m_jogadorEscolhido.GetComponent<FisicaJogador>();

        //LogisticaVars.m_colJogadorEscolhido = LogisticaVars.m_jogadorEscolhido.transform.GetChild(2).GetComponent<MeshCollider>();
        LogisticaVars.m_rbJogadorEscolhido = LogisticaVars.m_jogadorEscolhido.GetComponent<Rigidbody>();

        //Ativar o indicador de jogador selecionado
        LogisticaVars.m_jogadorEscolhido.transform.GetChild(3).gameObject.SetActive(true);

        LogisticaVars.jogadorSelecionado = true;
    } //Tudo certo
    public static void DesabilitarDadosJogador()
    {
        //Debug.Log("Desabilitando dados do Jogador");
        LogisticaVars.m_jogadorEscolhido.tag = "Player";
        LogisticaVars.m_jogadorEscolhido.transform.GetChild(3).gameObject.SetActive(false);

        LogisticaVars.desabilitouDadosJogador = true;
    } //Tudo Certo
    public static void DesabilitarComponentesDosNaoSelecionados()
    {
        if (GameObject.FindGameObjectsWithTag("Player") != null)
        {
            foreach (GameObject j in GameObject.FindGameObjectsWithTag("Player"))
            {
                j.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>().m_Priority = 0;
                //j.GetComponentInChildren<AudioListener>().enabled = false;
            }
        }
    } //Tudo certo
    #endregion


    void TrocarVez()
    {
        events.SituacaoGameplay("jogo parado");
        bool aux = LogisticaVars.vezJ1;
        LogisticaVars.vezJ1 = LogisticaVars.vezJ2;
        LogisticaVars.vezJ2 = aux;

        LogisticaVars.tempoJogada = 0;
        LogisticaVars.jogadas = 0;

        LogisticaVars.escolherOutroJogador = false;
        LogisticaVars.desabilitouDadosJogador = false;

        SituacaoBolaRasteira();
        StartCoroutine(RotinasGameplay.DesabilitarApos3Jogadas(bola));
    } //Tudo Certo
    void FimDaVez()
    {
        LogisticaVars.trocarVez = true;
        if (LogisticaVars.escolherOutroJogador) DesistirDeSelecionarOutroJogador();
        else TrocarVez();
    } //Tudo Certo
    static void SituacaoBolaRasteira()
    {
        if (LogisticaVars.vezJ1 && LogisticaVars.bolaRasteiraT1 || LogisticaVars.vezJ2 && LogisticaVars.bolaRasteiraT2) { ui.direcaoBolaBt.isOn = true; events.OnAplicarMetodosUiComBotao("bola rasteira"); }
        else if (LogisticaVars.vezJ1 && !LogisticaVars.bolaRasteiraT1 || LogisticaVars.vezJ2 && !LogisticaVars.bolaRasteiraT2) { ui.direcaoBolaBt.isOn = false; events.OnAplicarMetodosUiComBotao("bola rasteira"); }
    }
}
