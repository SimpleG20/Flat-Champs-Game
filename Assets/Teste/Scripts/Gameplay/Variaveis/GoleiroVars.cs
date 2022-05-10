using UnityEngine;

public static class GoleiroVars 
{
    [Header("Chutar")]
    public static float m_maxForca;     //MovimentacaoGoleiro, GoleiroMetodos
    public static float m_forcaGoleiro; //MovimentacaoGoleiro, GoleiroMetodos
    public static bool m_medirChute;    //MovimentacaoGoleiro
    public static bool m_aplicarChute;  //MovimentacaoGoleiro

    [Header("Rotacionar")]
    public static float m_sensibilidade;        //MovimentacaoGoleiro
    public static float m_sensibilidadeChute;   //MovimentacaoGoleiro

    [Header("Movimentacao")]
    public static bool m_movimentar;    //MovimentacaoGoleiro, InputManager
    public static float m_speed;        //MovimentacaoGoleiro
    internal static bool chutou;
}
