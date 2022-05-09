using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public static class LogisticaVars
{
    [Header("Goleiro")]
    #region Goleiro Variaveis

    #region Boolean
    public static bool goleiroT1, goleiroT2;    //GameplayOff, DirecionalBola, FisicaBola, CamerasSettings, InputManager, GoleiroMetodos,
                                                //MovimentacaoJogador, RotinasGameplay, SelecaoMetodos, UIMetodosGameplay
    #endregion

    #region Goleiro GameObject
    public static GameObject m_goleiroGameObject;         //GameplayOff, MovimentacaoGoleiro, GoleiroMetodos, RotinasGameplay
    #endregion

    #endregion

    [Header("Gameplay")]
    #region Gameplay Variaveis

    #region GameObjects
    public static GameObject m_jogadorEscolhido_Atual;              //Fisicajogador, FisicaBola, GameplayOff, JogadorMetodos, MovimentacaoJogador
                                                                    //SelecaoMetodos, LinkarBotao, RotinasGameplay
    public static GameObject m_jogadorPlayer;
    public static GameObject m_jogadorAi;
    public static List<GameObject> jogadoresT1, jogadoresT2;  //Abertura, SelecaoMetodos, RotinasGameplay
    #endregion

    #region JogadorEscolhido
    public static Rigidbody m_rbJogadorEscolhido;                   //FisicaBola, RotinasGameplay, SelecaoMetodos
    public static CinemachineVirtualCamera cameraJogador;           //FisicaBola, RotinasGameplay, SelecaoMetodos
    #endregion

    #region Tempo
    public static int minutosCorridos;              //GameplayOff, PlacarManager, TempoCorridoPartida
    public static int segundosCorridos;             //GameplayOff, PlacarManager, TempoCorridoPartida
    public static int tempoMaxJogada;               //Abertura, GameplayOff
    public static int tempoMaxEscolhaJogador = 8;   //RotinasGameplay
    public static float tempoCorrido;               //GameplayOff
    public static float tempoJogada;                //GameplayOff, SelecaoMetodos, TempoJogada
    public static float tempoEscolherJogador;       //GameplayOff, TempoEscolha
    public static float tempoPartida;               //Abertura, GameplayOff
    public static bool contarTempoJogada;
    public static bool contarTempoSelecao;
    #endregion

    #region Boolean
    public static bool jogoComecou;                                 //GameplayOff, MovimentacaoJogador
    public static bool primeiraJogada;                              //Abertura, GameplayOff
    public static bool vezJ1, vezJ2;                                //Abertura, GameplayOff, SelecaoMetodos, RotinasGameplay, FisicaBola, FisicaJogador
                                                                    //CamerasSettings, MovimentacaoJogadores, MiraEspecial, UIMetodosGameplay
    public static bool jogadorSelecionado;                          //Abertura, FisicaJogador, MovimentacaoJogador, SelecaoMetodos, LinkarBotao
                                                                    //DirecionalBola
    public static bool especialT1Disponivel, especialT2Disponivel;  //GameplayOff

    #region Relacionadas ao Gol
    public static bool gol;                                         //GameplayOff, FisicaBola
    public static bool golT1, golT2;                                //GameplayOff, RotinasGameplay, FisicaBola
    #endregion

    #region Bola
    public static bool bolaRasteiraT1, bolaRasteiraT2;              //Abertura, SelecaoMetodos, MovimentacaoJogadores, RotinasGameplay, UIMetodosGameplay
                                                                    //FisicaBola
    public static bool redirecionamentoAutomatico;                  //GameplayOff, MovimentacaoJogador
    public static bool mostrarDirecaoBola;                          //MovimentacaoJogador
    public static bool podeRedirecionar;                            //MovimentacaoJogador
    #endregion

    #region Situacao
    public static bool continuaSendoFora;                           //GameplayOff, JogadorMetodos, RotinasGameplay, SelecaoMetodos, MovimentacaoJogador
    public static bool foraLateralD, foraLateralE;                  //FisicaBola, RotinasGameplay
    public static bool lateral;                                     //GameplayOff, FisicaBola, RotinasGameplay, UIMetodosGameplay
    public static bool foraFundo;                                   //GameplayOff, FisicaBola, RotinasGameplay
    public static bool tiroDeMeta;                                  //GameplayOff, FisicaBola, RotinasGameplay, SelecaoMetodos, MovimentacaoGoleiro
    public static bool fundo1, fundo2;                              //GameplayOff, FisicaBola, RotinasGameplay
    public static bool bolaPermaneceNaPequenaArea;                  //GameplayOff, FisicaBola, RotinasGameplay
    public static bool bolaEntrouPequenaArea;                       //GameplayOff, FisicaBola
    public static bool defenderGoleiro;                             //RotinasGameplay, GoleiroMetodos
    public static bool escolherOutroJogador;                        //MovimentacaoJogador, SelecaoMetodos, LinkarBotao
    public static bool auxChuteAoGol;                               //FameplayOff, JogadorMetodos, FIsicaBola, InputManager, GoleiroMetodos, RotinasG
    public static bool especial;                                    //GameplayOff, MovimentacaoJogador, InputManager, UIMetodosGameplay, PrevisaoChute
    public static bool jogoParado;                                  //GameplayOff, JogadorMetodos, MovimentacaoJogador
    public static bool aplicouPrimeiroToque;                        //Abertura, GameplayOff, MovimentacaoJogador, RotinasGameplay, SelecaoMetodos
    public static bool aplicouEspecial;                             //GameplayOff, MovimentacaoJogador, RotinasGameplay, PrevisaoChute
    public static bool trocarVez;                                   //RotinasGameplay, SelecaoMetodos
    public static bool escolheu;                                    //SelecaoMetodos, RotinasGameplay,LinkarBotao
    public static bool desabilitouDadosJogador;                     //RotinasGameplay, SelecaoMetodos
    public static bool j1Ganhou, j2Ganhou;                          
    public static bool empate;                                      
    #endregion

    #endregion

    #region Spawns
    public static float[,] esquemaT1;     //Abertura, RotinasGameplay
    public static float[,] esquemaT2;       //Abertura, RotinasGameplay
    #endregion

    #region Outros
    public static float m_especialAtualT1;      //GameplayOff, FisicaJogador
    public static float m_especialAtualT2;      //GameplayOff, FisicaJogador
    public static float m_maxEspecial = 500;    //GameplayOff, RotinasGameplay, MiraEspecial
    public static int placarT1;                 //GameplayOff, GolComponentes, PlacarManager
    public static int placarT2;                 //GameplayOff, GolComponentes, PlacarManager
    public static int jogadas;                  //GameplayOff, JogadorMetodos, RotinasGameplay, SelecaoMetodos, NumeroJogadas
    public static int ultimoToque;              //FisicaBola, GameplayOff, RotinasGameplay, MovimentacaoJogador, GoleiroMetodos

    public static Animator m_tempoSelecaoAnimator;    //Abertura, LinkarBotao, SelecaoMetodos
    #endregion

    #endregion

}
