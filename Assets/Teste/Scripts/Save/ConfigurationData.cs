using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConfigurationData
{
    public int m_corMenuCustom;
    public int m_corJogadorCustom;
    public int m_corTimeCustom;
    public int m_corConfigCustom;
    public int m_corGameplayCustom;

    public int m_camSensibilidade;
    public float m_camPosY;
    public float m_camAngulo;
    public float m_velocidadeBarraChute;

    public int m_somTorcida;
    public int m_somInterface;
    public int m_somMusica;
    public int m_somEfeito;

    public int m_resolucao;

    public int m_language;
    

    public ConfigurationData (Configuration config)
    {
        m_corConfigCustom = config.m_corConfigCustom;
        m_corJogadorCustom = config.m_corJogadorCustom;
        m_corMenuCustom = config.m_corMenuCustom;
        m_corTimeCustom = config.m_corTimeCustom;
        m_corGameplayCustom = config.m_corGameplayCustom;

        m_camSensibilidade = config.m_camSensibilidade;
        m_camPosY = config.m_camPosY;
        m_camAngulo = config.m_camAngulo;

        m_velocidadeBarraChute = config.m_velocidadeBarraChute;

        m_somTorcida = config.m_somTorcida;
        m_somInterface = config.m_somInterface;
        m_somMusica = config.m_somMusica;
        m_somEfeito = config.m_somEfeito;

        m_resolucao = config.m_resolucao;

        m_language = config.m_language;
    }
}
