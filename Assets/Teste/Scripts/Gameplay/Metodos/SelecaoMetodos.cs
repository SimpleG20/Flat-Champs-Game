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
        if (LogisticaVars.m_jogadorEscolhido_Atual != null)
            LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>().m_Priority = 0;

        if (LogisticaVars.vezJ1) LogisticaVars.m_jogadorEscolhido_Atual = QuemEstaMaisPerto(bola.m_pos, LogisticaVars.jogadoresT1);
        else LogisticaVars.m_jogadorEscolhido_Atual = QuemEstaMaisPerto(bola.m_pos, LogisticaVars.jogadoresT2);

        LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>().m_Priority = 99;
        //SituacaoBolaRasteira();
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
        //ui.sairSelecaoBt.gameObject.SetActive(false);
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
        if (GameManager.Instance.m_jogadorAi)
        {
            if (LogisticaVars.vezJ2) LogisticaVars.m_jogadorAi = LogisticaVars.m_jogadorEscolhido_Atual;
        }
        else LogisticaVars.m_jogadorPlayer = LogisticaVars.m_jogadorEscolhido_Atual;

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
}
