using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenuAttribute(fileName = "NovoTime", menuName = "ScriptableObjects/Times Off")]
public class Teams : ScriptableObject
{ 
    public string m_nomeTime;
    public string m_abreviacao;

    public Sprite m_base;
    public Sprite m_fundo;
    public Sprite m_simbolo;

    public Color m_corPrimaria;
    public Color m_corSecundaria;
    public Color m_corTerciaria;

    public Color m_Casa;
    public Color m_Fora;

    public GameObject m_playerBotao;
    public GameObject m_playerGoleiro;
    public int m_tipoJogador;
    public int m_tipoGoleiro;

    public int esquema;
}
