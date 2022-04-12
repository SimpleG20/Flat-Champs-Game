using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayGeral : MonoBehaviour
{
    EventsManager events;
    FisicaBola bola;

    // Start is called before the first frame update
    void Start()
    {
        events = EventsManager.current;
        bola = GameObject.FindGameObjectWithTag("Bola").GetComponent<FisicaBola>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Segue_O_Jogo()
    {
        //comecarContagemJogada = true;
        LogisticaVars.tempoEscolherJogador = 0;
    }

    #region Selecao
    void SelecionarOutroJogador()
    {

    }
    void SairSelecao()
    {

    }
    #endregion

    #region Gol
    void GolMarcado()
    {
        LogisticaVars.tempoJogada = LogisticaVars.tempoEscolherJogador = 0;
        LogisticaVars.jogadas = 0;
        LogisticaVars.jogoParado = true;
        LogisticaVars.primeiraJogada = true;

        events.OnAplicarRotinas("rotina animacao gol");
        if (LogisticaVars.golT1)
        {
            LogisticaVars.placarT1 += 1;
            LogisticaVars.vezJ1 = false;
            LogisticaVars.vezJ2 = true;
        }
        if (LogisticaVars.golT2)
        {
            LogisticaVars.placarT2 += 1;
            LogisticaVars.vezJ1 = true;
            LogisticaVars.vezJ2 = false;
        }

        bola.RedirecionarJogadores(true);
        ReiniciarPosGol();
    } //Ativar Rotina da animacao do gol, computa os gols e posiciona os jogadores
    void ReiniciarPosGol()
    {
        bola.PosicionarAposGol();
        LogisticaVars.bolaPermaneceNaPequenaArea = LogisticaVars.auxChuteAoGol = LogisticaVars.acionouChuteAoGol = false;
        LogisticaVars.lateral = LogisticaVars.foraFundo = false;

        LogisticaVars.primeiraJogada = true;
        LogisticaVars.aplicouPrimeiroToque = false;
        LogisticaVars.jogoComecou = false;

        LogisticaVars.bolaEntrouPequenaArea = false;
        LogisticaVars.gol = false;
        LogisticaVars.golT2 = false;
        LogisticaVars.golT1 = false;
        LogisticaVars.posGol = false;
        LogisticaVars.jogadaDepoisGol = true;
        LogisticaVars.aplicouPrimeiroToque = false;
    }
    #endregion

    public static void BolaNaPequenaArea(int i)
    {
        Debug.Log("Bola permanece na pequena Area");
        EventsManager.current.OnAplicarMetodosUiSemBotao("estado jogador", "", false); //.EstadoBotoesJogador(false);
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
        EventsManager.current.OnAplicarMetodosUiSemBotao("estado dos botoes", "bola pequena area");
        //EstadosDosBotoesEmCertasSituacoes("bola pequena area");
    }

    #region Fora
    void ForaLateral()
    {
        events.OnAplicarRotinas("rotina tempo lateral");
    }
    void ForaEscanteio()
    {
        if (LogisticaVars.fundo1 && LogisticaVars.ultimoToque == 1)
        {
            Debug.Log("Escanteio");
            /*if (LogisticaVars.m_goleiroGameObject != null)
            {
                LogisticaVars.goleiroT1 = LogisticaVars.goleiroT2 = false;
                GoleiroMetodos.ComponentesParaGoleiro(false);
                LogisticaVars.m_goleiroGameObject = null;
            }*/
        }
        if (LogisticaVars.fundo2 && LogisticaVars.ultimoToque == 2) //Fundo2
        {
            Debug.Log("Escanteio");
            /*if (LogisticaVars.m_goleiroGameObject != null)
            {
                LogisticaVars.goleiroT1 = LogisticaVars.goleiroT2 = false;
                GoleiroMetodos.ComponentesParaGoleiro(false);
                LogisticaVars.m_goleiroGameObject = null;
            }*/
        }
        LogisticaVars.foraFundo = false;
        events.OnAplicarRotinas("tempo chute escanteio");
    }
    void ForaTiroDeMeta()
    {
        if (LogisticaVars.fundo1 && LogisticaVars.ultimoToque != 1)
        {
            LogisticaVars.vezJ1 = true;
            LogisticaVars.vezJ2 = false;
        }

        if (LogisticaVars.fundo2 && LogisticaVars.ultimoToque != 2)
        {
            LogisticaVars.vezJ2 = true;
            LogisticaVars.vezJ1 = false;
        }

        events.OnAplicarRotinas("rotina tempo tiro de meta");
    }
    void ForaGeral()
    {
        LogisticaVars.jogoParado = true;

        if (LogisticaVars.ultimoToque == 1 && LogisticaVars.vezJ1 || LogisticaVars.ultimoToque == 2 && LogisticaVars.vezJ2) { LogisticaVars.tempoJogada = 0; LogisticaVars.jogadas = 0; }

        if (LogisticaVars.tempoJogada > 15) LogisticaVars.tempoJogada = 14;
        if (LogisticaVars.jogadas != 0 && LogisticaVars.jogadas != 1) LogisticaVars.jogadas--;

        LogisticaVars.vezJ1 = LogisticaVars.vezJ2 = false;
        if (LogisticaVars.ultimoToque == 1) LogisticaVars.vezJ2 = true;
        else LogisticaVars.vezJ1 = true;

        SelecaoMetodos.DesabilitarDadosJogador();
        EventsManager.current.SelecaoAutomatica();

        LogisticaVars.m_jogadorEscolhido.GetComponentInChildren<Camera>().enabled = false;
        LogisticaVars.m_jogadorEscolhido.GetComponentInChildren<AudioListener>().enabled = false;
        //LogisticaVars.esperandoTrocas = true;
        LogisticaVars.continuaSendoFora = true;
    }
    #endregion
}
