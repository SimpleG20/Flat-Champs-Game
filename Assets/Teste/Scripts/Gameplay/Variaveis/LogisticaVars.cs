using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public static class LogisticaVars
{
    [Header("Goleiro")]
    #region Goleiro Variaveis

    #region Boolean
    public static bool goleiroT1;
    public static bool goleiroT2, m_goleiroEscolhido;
    #endregion

    #region Goleiro GameObject
    public static GameObject m_goleiroGameObject;
    #endregion

    #endregion

    [Header("Bola")]
    #region Bola Variaveis

    #endregion

    [Header("Gameplay")]
    #region Gameplay Variaveis

    #region GameObjects
    public static GameObject m_jogadorEscolhido;
    public static GameObject m_jogadorAi;
    public static List<GameObject> jogadoresT1, jogadoresT2;
    public static GameObject m_especialPrefab;
    #endregion

    #region JogadorEscolhido
    public static MeshCollider m_colJogadorEscolhido;
    public static Rigidbody m_rbJogadorEscolhido;
    public static CinemachineVirtualCamera cameraJogador;
    #endregion

    #region Tempo
    public static int minutosCorridos;
    public static int segundosCorridos;
    public static int tempoMaxJogada = 20, tempoMaxEscolhaJogador = 8;
    public static float tempoCorrido;
    public static float tempoJogada , tempoEscolherJogador, tempoPartida;
    #endregion

    #region Boolean
    public static bool abertura, jogoComecou;
    public static bool primeiraJogada, vezJ1, vezJ2;
    public static bool jogadorSelecionado, especialT1Disponivel, especialT2Disponivel;

    public static bool jogadaDepoisGol, acionouChuteAoGol, posChuteAoGol, gol, posGol, golT1, golT2;
    public static bool bolaRasteiraT1, bolaRasteiraT2, redirecionamentoAutomatico, bolaEntrouPequenaArea, mostrarDirecaoBola;

    #region Situacao
    public static bool continuaSendoFora, foraLateralD, foraLateralE, lateral, foraFundo, tiroDeMeta, fundo1, fundo2;
    public static bool bolaPermaneceNaPequenaArea, sairEscanteio, sairLateral, esperandoTrocas;
    public static bool defenderGoleiro, escolherOutroJogador, auxChuteAoGol, especial, normal;
    #endregion
    public static bool jogoParado, aplicouPrimeiroToque, podeRedirecionar,aplicouEspecial;

    public static bool trocarVez, m_trocouVez, prontoParaEscolher, escolheu, desabilitouDadosJogador;

    public static bool j1Ganhou, j2Ganhou, empate;
    #endregion

    #region Spawns
    public static float[,] esquemaT1;
    public static float[,] esquemaT2;
    #endregion

    #region Outros
    public static float m_especialAtualT1, m_especialAtualT2, m_maxEspecial = 500;
    public static int placarT1, placarT2, jogadas, ultimoToque;
    public static int moeda, ultimaPosse;

    public static Animator m_tempoSelecaoAnimator;
    public static Animator m_especialAnimator;
    #endregion

    #endregion

}
