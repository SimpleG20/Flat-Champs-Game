using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GoleiroVars 
{
    [Header("Angulo")]
    public static float m_anguloGoleiro;
    public static float m_senoGoleiro, m_cosGoleiro;

    [Header("Chutar")]
    public static float m_maxForca;
    public static float m_forcaGoleiro;
    public static bool m_medirChute, m_aplicarChute;

    [Header("Rotacionar")]
    public static float m_sensibilidade;
    public static float m_sensibilidadeChute;
    public static bool m_rotacionar;

    [Header("Movimentacao")]
    public static bool m_movimentar;
    public static float m_speed;
}
