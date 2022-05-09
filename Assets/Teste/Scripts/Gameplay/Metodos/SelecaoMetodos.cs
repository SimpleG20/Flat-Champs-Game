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

        events.onTrocarVez += FimDaVez;
        events.onEscolherJogador += EscolherJogador;
        events.onSelecaoAutomatica += SelecaoAutomatica;
        events.onAjeitarCamera += AjustarCameraParaSelecao;
    }


    #region Selecionar Outro jogador
    void AjustarCameraParaSelecao(float y)
    {
        if(y == 1)
        {
            LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>().m_Priority = 0;
            LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(1).GetChild(1).GetComponent<CinemachineVirtualCamera>().m_Priority = 99;
            LogisticaVars.cameraJogador = LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(1).GetChild(1).GetComponent<CinemachineVirtualCamera>();
        }
        else
        {
            LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>().m_Priority = 99;
            LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(1).GetChild(1).GetComponent<CinemachineVirtualCamera>().m_Priority = 0;
            LogisticaVars.cameraJogador = LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>();
        }
    }
    public static void ColocarIconeParaSelecao(List<GameObject> jogadores)
    {
        Vector3 jogadorRef = LogisticaVars.m_jogadorEscolhido_Atual.transform.position;
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
    #endregion

    #region Automatico
    private void EscolherJogador()
    {
        if (LogisticaVars.m_jogadorEscolhido_Atual != null)
            LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>().m_Priority = 0;

        if (LogisticaVars.vezJ1) LogisticaVars.m_jogadorEscolhido_Atual = QuemEstaMaisPerto(GameObject.FindGameObjectWithTag("Bola").transform.position, LogisticaVars.jogadoresT1);
        else LogisticaVars.m_jogadorEscolhido_Atual = QuemEstaMaisPerto(GameObject.FindGameObjectWithTag("Bola").transform.position, LogisticaVars.jogadoresT2);

        LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>().m_Priority = 99;
        
        
        //if(LogisticaVars.aplicouPrimeiroToque && !LogisticaVars.continuaSendoFora) StartCoroutine(EsperarTransicaoCameras(false, "ui normal"));
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
    public static GameObject QuemEstaMaisPerto(Vector3 referencia, List<GameObject> jogadores)
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
        LogisticaVars.m_jogadorEscolhido_Atual.tag = "Player Selecionado";
        LogisticaVars.cameraJogador = LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>();
        LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>().m_Priority = 99;
        
        //LogisticaVars.m_jogadorEscolhido.GetComponentInChildren<AudioListener>().enabled = true;
        LogisticaVars.m_jogadorEscolhido_Atual.GetComponent<FisicaJogador>().enabled = true;
        JogadorVars.m_fisica = LogisticaVars.m_jogadorEscolhido_Atual.GetComponent<FisicaJogador>();

        //LogisticaVars.m_colJogadorEscolhido = LogisticaVars.m_jogadorEscolhido.transform.GetChild(2).GetComponent<MeshCollider>();
        LogisticaVars.m_rbJogadorEscolhido = LogisticaVars.m_jogadorEscolhido_Atual.GetComponent<Rigidbody>();

        //Ativar o indicador de jogador selecionado
        LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(3).gameObject.SetActive(true);

        LogisticaVars.jogadorSelecionado = true;
    } //Tudo certo
    public static void DesabilitarDadosJogador()
    {
        //Debug.Log("Desabilitando dados do Jogador");
        LogisticaVars.m_jogadorEscolhido_Atual.tag = "Player";
        LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(3).gameObject.SetActive(false);

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
        EstadoJogo.TempoJogada(false);
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
        TrocarVez();
        //if (LogisticaVars.escolherOutroJogador) DesistirDeSelecionarOutroJogador();
        //else TrocarVez();
    } //Tudo Certo
    static void SituacaoBolaRasteira()
    {
        if (LogisticaVars.vezJ1 && LogisticaVars.bolaRasteiraT1 || LogisticaVars.vezJ2 && LogisticaVars.bolaRasteiraT2) 
        { ui.direcaoBolaBt.isOn = true; VariaveisUIsGameplay._current.UI_BolaRasteira(); }
        else if (LogisticaVars.vezJ1 && !LogisticaVars.bolaRasteiraT1 || LogisticaVars.vezJ2 && !LogisticaVars.bolaRasteiraT2) 
        { ui.direcaoBolaBt.isOn = false; VariaveisUIsGameplay._current.UI_BolaRasteira(); }
    }
}
