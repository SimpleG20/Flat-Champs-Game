using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SelecaoMetodos : MonoBehaviour
{
    static FisicaBola bola;
    static EventsManager events;

    private void Start()
    {
        bola = FindObjectOfType<FisicaBola>();
        events = EventsManager.current;

        events.onEscolherJogador += EscolherJogador;
        events.onSelecaoAutomatica += SelecaoAutomatica;
    }


    #region Automatico
    void EscolherJogador()
    {
        //if (LogisticaVars.vezAI) LogisticaVars.m_jogadorPlayer = LogisticaVars.m_jogadorEscolhido_Atual;

        if (LogisticaVars.m_jogadorEscolhido_Atual != null && !LogisticaVars.vezAI)
            LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>().m_Priority = 0;

        if (LogisticaVars.vezJ1) LogisticaVars.m_jogadorEscolhido_Atual = QuemEstaMaisPerto(bola.m_pos, LogisticaVars.jogadoresT1);
        else LogisticaVars.m_jogadorEscolhido_Atual = QuemEstaMaisPerto(bola.m_pos, LogisticaVars.jogadoresT2);

        if (!LogisticaVars.vezAI)
        {
            LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>().m_Priority = 99;
        }
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
        //SituacaoBolaRasteira();
    }
    void SelecaoAutomatica()
    {
        LogisticaVars.escolherOutroJogador = false;
        Debug.Log("SELECAO: Escolha Automática");
        DesabilitarDadosJogadorAtual();
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
            if (LogisticaVars.vezAI)
            {
                if (distancia <= distanciaMenor && distancia != 0)
                {
                    distanciaMenor = distancia;
                    jogador = Jg;
                }
                else continue;
            }
            else
            {
                if (distancia <= distanciaMenor && distancia != 0 && Jg != LogisticaVars.m_jogadorEscolhido_Atual)
                {
                    distanciaMenor = distancia;
                    jogador = Jg;
                }
                else continue;
            }
        }
        return jogador;
    } //Tudo certo
    #endregion

    #region Dados
    public static void DadosJogador()
    {
        LogisticaVars.m_jogadorEscolhido_Atual.tag = "Player Selecionado";
        if (LogisticaVars.vezAI)
        {
            LogisticaVars.m_jogadorAi = Gameplay._current.GetAISystem().ai_player = LogisticaVars.m_jogadorEscolhido_Atual;
            //VariaveisUIsGameplay._current.UI_Espera();
            LogisticaVars.jogadorSelecionado = false;
        }
        else
        {
            LogisticaVars.m_jogadorPlayer = LogisticaVars.m_jogadorEscolhido_Atual;
            CameraJogadorSelecionado();
            LogisticaVars.jogadorSelecionado = true;
        }

        //LogisticaVars.m_jogadorEscolhido.GetComponentInChildren<AudioListener>().enabled = true;
        LogisticaVars.m_jogadorEscolhido_Atual.GetComponent<FisicaJogador>().enabled = true;
        JogadorVars.m_fisica = LogisticaVars.m_jogadorPlayer == null ? null : LogisticaVars.m_jogadorPlayer.GetComponent<FisicaJogador>();

        //LogisticaVars.m_colJogadorEscolhido = LogisticaVars.m_jogadorEscolhido.transform.GetChild(2).GetComponent<MeshCollider>();
        LogisticaVars.m_rbJogadorEscolhido = LogisticaVars.m_jogadorEscolhido_Atual.GetComponent<Rigidbody>();

        //Ativar o indicador de jogador selecionado
        LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(3).gameObject.SetActive(true);
    } //Tudo certo
    private static void CameraJogadorSelecionado()
    {
        LogisticaVars.cameraJogador = LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>();
        LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>().m_Priority = 99;
        if (CamerasSettings._current.getCameraEspera())
        {
            CamerasSettings._current.MudarBlendCamera(CinemachineBlendDefinition.Style.Cut);
            CamerasSettings._current.SituacoesCameras("desabilitar camera espera");
        }
    }

    public static void DesabilitarDadosJogadorAtual()
    {
        //Debug.Log("Desabilitando dados do Jogador");
        LogisticaVars.m_jogadorEscolhido_Atual.tag = "Player";
        LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(3).gameObject.SetActive(false);

        LogisticaVars.desabilitouDadosJogador = true;
    } //Tudo Certo
    public static void DesabilitarDadosPlayer()
    {
        if(LogisticaVars.m_jogadorPlayer != null) LogisticaVars.m_jogadorPlayer.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>().m_Priority = 0;
    }
    public static void DesabilitarComponentesDosNaoSelecionados()
    {
        if (GameObject.FindGameObjectsWithTag("Player") != null)
        {
            foreach (GameObject j in GameObject.FindGameObjectsWithTag("Player"))
            {
                if(LogisticaVars.m_jogadorPlayer != null)
                {
                    if(j != LogisticaVars.m_jogadorPlayer) 
                        j.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>().m_Priority = 0;
                }
                //j.GetComponentInChildren<AudioListener>().enabled = false;
            }
        }
    } //Tudo certo
    #endregion
}
