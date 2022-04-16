using UnityEngine;

public class JogadorVars
{
    public static FisicaJogador m_fisica;

    [Header("Logica")]
    public static bool m_chuteAoGol;        //JogadorMetodos
    public static bool m_correndo;          //MovimentacaoJogador

    [Header("Chutar")]
    public static float m_maxForca;         //MovimentacaoJogador, JogadorMetodos, GoleiroMetodos
    public static float m_forca;            //JogadorMetodos, GoleiroMetodos, MovimentacaoJogador, RotinasGameplay
    public static float m_forcaMin;         //JogadorMetodos, MovimentacaoJogador
    public static bool m_medirChute;        //JogadorMetodos, MovimentacaoJogador
    public static bool m_aplicarChute;      //JogadorMetodos, MovimentacaoJogador, GameplayOff
    public static bool m_esperandoContato;  //FisicaBola, JogadorMetodos

    [Header("Rotacionar")]
    public static float m_sensibilidade, sensibilidadeEscolha;  //MovimentacaoJogador
    public static float m_sensibilidadeChute;                   //MovimentacaoJogador
    public static bool m_rotacionar;                            //InputManager, MovimentacaoJogador, SelecaoMetodos
}
