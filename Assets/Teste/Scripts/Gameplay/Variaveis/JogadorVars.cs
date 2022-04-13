using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JogadorVars
{
    [Header("Valores")]
    public static float m_forcaEscanteio;
    public static float m_forcaGoleiro;
    public static float m_aceleracaoJogador, m_energiaCineticaJogador, m_distanciaPercorridaJogador, m_velocidadeImpacto;
    public static float m_quantidadeMovimento;
    public static float m_velocidadeJogador, m_anguloJogadorEscolhido;

    [Header("Logica")]
    public static bool m_toqueBola;
    public static bool toqueJogador, m_chuteAoGol, m_auxChuteGol;
    public static bool jCorrendo, rotacionarGoleiro;
    private static bool m_printGoleiro, print_deslocarJg;

    [Header("Dados Jogador")]
    public static Vector3 direcaoChute, direcaoEspecial;
    public static float ajusteDirecao;
    public static float m_senoJogadorEscolhido, m_cosJogadorEscolhido;

    [Header("Chutar")]
    public static float m_maxForca;
    public static float m_forca, m_forcaMin, m_chuteAltura, m_alturaMax;
    public static bool m_medirChute, m_aplicarChute;

    [Header("Rotacionar")]
    public static float m_sensibilidade, sensibilidadeEscolha;
    public static float m_sensibilidadeChute;
    public static bool m_rotacionar;
}