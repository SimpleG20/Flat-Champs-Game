using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Configuration : MonoBehaviour
{
    public int m_corMenuCustom;
    public int m_corJogadorCustom;
    public int m_corTimeCustom;
    public int m_corConfigCustom;
    public int m_corGameplayCustom;

    public int m_camSensibilidade = 50;
    public float m_camPosY = 0;
    public float m_camAngulo = 0;
    public float m_velocidadeBarraChute = 1;

    public int m_somTorcida = 50;
    public int m_somInterface = 50;
    public int m_somMusica = 50;
    public int m_somEfeito = 50;

    public int m_resolucao = 2;

    public int m_language = 1;

    public void AtualizarConfigs(ConfigurationData configData)
    {
        m_corConfigCustom = configData.m_corConfigCustom;
        m_corJogadorCustom = configData.m_corJogadorCustom;
        m_corMenuCustom = configData.m_corMenuCustom;
        m_corTimeCustom = configData.m_corTimeCustom;
        m_corGameplayCustom = configData.m_corGameplayCustom;

        m_camSensibilidade = configData.m_camSensibilidade;
        m_camPosY = configData.m_camPosY;
        m_camAngulo = configData.m_camAngulo;

        m_velocidadeBarraChute = configData.m_velocidadeBarraChute;

        m_somTorcida = configData.m_somTorcida;
        m_somInterface = configData.m_somInterface;
        m_somMusica = configData.m_somMusica;
        m_somEfeito = configData.m_somEfeito;

        m_resolucao = configData.m_resolucao;

        m_language = configData.m_language;
    }
}
