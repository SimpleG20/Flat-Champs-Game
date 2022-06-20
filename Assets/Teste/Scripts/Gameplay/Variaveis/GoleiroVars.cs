using UnityEngine;

public static class GoleiroVars 
{
    [Header("Chutar")]
    public static float m_maxForca = 45;     //MovimentacaoGoleiro, GoleiroMetodos
    public static float m_forcaGoleiro; //MovimentacaoGoleiro, GoleiroMetodos
    public static bool m_medirChute;    //MovimentacaoGoleiro
    public static bool m_aplicarChute;  //MovimentacaoGoleiro

    [Header("Rotacionar")]
    public static float m_sensibilidade = 45;        //MovimentacaoGoleiro
    public static float m_sensibilidadeChute = 10;   //MovimentacaoGoleiro

    [Header("Movimentacao")]
    public static bool m_movimentar;    //MovimentacaoGoleiro, InputManager
    public static float m_speed = 5;        //MovimentacaoGoleiro
    internal static bool chutou;
}
