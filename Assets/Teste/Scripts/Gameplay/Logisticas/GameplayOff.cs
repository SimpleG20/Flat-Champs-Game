using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameplayOff : MonoBehaviour
{
    /* Abreviacoes:
        MJ: movimentacaoDoJogador
        MG: movimentacaoDoGoleiro
        UI: UIMetodosGameplay
     */
    bool comecarContagemJogada, comecarContagemSelecao;

    GameObject canvas, direcionalChute;
    Vector3 posGol1, posGol2;

    CinemachineBrain camPrincipal;
    FisicaBola bola;
    EventsManager events;
    UIMetodosGameplay ui;

    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<UIMetodosGameplay>();
        bola = FindObjectOfType<FisicaBola>();
        camPrincipal = FindObjectOfType<CinemachineBrain>();

        canvas = GameObject.Find("Canvas");
        posGol1 = GameObject.FindGameObjectWithTag("Gol1").transform.position;
        posGol2 = GameObject.FindGameObjectWithTag("Gol2").transform.position;
        direcionalChute = GameObject.FindGameObjectWithTag("Direcional Chute");

        events = EventsManager.current;

        LogisticaVars.m_especialAtualT1 = LogisticaVars.m_especialAtualT2 = 0;
        //BarraEspecial(0, LogisticaVars.m_maxEspecial);
        LogisticaVars.jogoParado = false;
    }


    void UiMetodos(string metodo)
    {
        switch (metodo)
        {
            case "direcionar jogador":
                DirecionamentoAuto();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        events.OnAtualizarNumeros();
        if(LogisticaVars.jogoComecou && !LogisticaVars.jogoParado) LogisticaVars.tempoCorrido += Time.deltaTime;

        TempoJogo();

        if (LogisticaVars.especialT1Disponivel && LogisticaVars.vezJ1 || LogisticaVars.especialT2Disponivel && LogisticaVars.vezJ2) ui.especialBt.interactable = true;
        else ui.especialBt.interactable = false;

        if (comecarContagemSelecao)
        {
            LogisticaVars.tempoEscolherJogador += Time.deltaTime;
        }


        if (comecarContagemJogada)
        {
            LogisticaVars.tempoJogada += Time.deltaTime;

            if (LogisticaVars.vezJ1)
            {
                if (!LogisticaVars.especial) LogisticaVars.m_especialAtualT1 += Time.deltaTime * 5;

                if (LogisticaVars.m_especialAtualT1 >= LogisticaVars.m_maxEspecial && !LogisticaVars.especialT1Disponivel) 
                { LogisticaVars.m_especialAtualT1 = LogisticaVars.m_maxEspecial;  LogisticaVars.especialT1Disponivel = true; }
                //BarraEspecial(LogisticaVars.m_especialAtualT1, LogisticaVars.m_maxEspecial);
            }
            else
            {
                if (!LogisticaVars.especial) LogisticaVars.m_especialAtualT2 += Time.deltaTime * 0f; //mudar para 0.5f
                
                if(LogisticaVars.m_especialAtualT2 >= LogisticaVars.m_maxEspecial && !LogisticaVars.especialT2Disponivel)
                { LogisticaVars.m_especialAtualT2 = LogisticaVars.m_maxEspecial; LogisticaVars.especialT2Disponivel = true; }
                //BarraEspecial(LogisticaVars.m_especialAtualT2, LogisticaVars.m_maxEspecial);
            }
        }

        //if (LogisticaVars.tempoJogada >= LogisticaVars.tempoMaxJogada) events.OnTrocarVez();

        
    }

    void TempoJogo()
    {
        LogisticaVars.segundosCorridos = Mathf.RoundToInt(LogisticaVars.tempoCorrido - (60 * LogisticaVars.minutosCorridos));
        if (LogisticaVars.tempoCorrido - (60 * LogisticaVars.minutosCorridos) >= 60)
        {
            LogisticaVars.minutosCorridos++;
        }
    }
    

    void DirecionamentoAuto()
    {
        if (ui.rotacaoAutoBt.isOn)
        {
            ui.rotacaoAutoBt.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            LogisticaVars.redirecionamentoAutomatico = true;
            //bola.RedirecionarJogadorEscolhido(bola.transform);
        }
        else
        {
            ui.rotacaoAutoBt.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            LogisticaVars.redirecionamentoAutomatico = false;
        }
    }

    public static void BolaNaPequenaArea(int i) //Rever
    {
        Debug.Log("Bola permanece na pequena Area");
        //EventsManager.current.OnAplicarMetodosUiSemBotao("estado jogador e goleiro", "", false);
        //EventsManager.current.OnAplicarMetodosUiSemBotao("estados dos botoes", "bola na pequena area"); //.EstadoBotoesJogador(false);
        if (i == 1 && !LogisticaVars.vezJ1)
        {
            if (LogisticaVars.ultimoToque == 2)
            {
                LogisticaVars.vezJ2 = false;
                LogisticaVars.vezJ1 = true;
                LogisticaVars.tempoJogada = 0;
                LogisticaVars.jogadas = 0;
            }

            LogisticaVars.goleiroT1 = true;
            LogisticaVars.m_goleiroGameObject = GameObject.FindGameObjectWithTag("Goleiro1");
        }
        else if (i == 1 && LogisticaVars.vezJ1)
        {
            LogisticaVars.goleiroT1 = true;
            LogisticaVars.m_goleiroGameObject = GameObject.FindGameObjectWithTag("Goleiro1");
        }
        else if (i == 2 && !LogisticaVars.vezJ2)
        {
            if (LogisticaVars.ultimoToque == 1)
            {
                LogisticaVars.vezJ1 = false;
                LogisticaVars.vezJ2 = true;
                LogisticaVars.tempoJogada = 0;
                LogisticaVars.jogadas = 0;
            }

            LogisticaVars.goleiroT2 = true;
            LogisticaVars.m_goleiroGameObject = GameObject.FindGameObjectWithTag("Goleiro2");
        }
        else if (i == 2 && LogisticaVars.vezJ2)
        {
            LogisticaVars.goleiroT2 = true;
            LogisticaVars.m_goleiroGameObject = GameObject.FindGameObjectWithTag("Goleiro2");
        }

        GoleiroMetodos.ComponentesParaGoleiro(true);
        SelecaoMetodos.DesabilitarDadosJogador();
        //EventsManager.current.OnAplicarMetodosUiSemBotao("estados dos botoes", "bola pequena area");
        //EstadosDosBotoesEmCertasSituacoes("bola pequena area");
    }



    public static float Modulo(float numero)
    {
        float modulo = 0;

        modulo = Mathf.Sqrt(Mathf.Pow(numero, 2));
        return modulo;
    }
}
