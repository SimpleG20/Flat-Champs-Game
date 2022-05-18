using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    //Nomes
    public string m_username;
    public string m_timeNome;
    public string m_abreviacao;

    //Numeros level, vitoria etc...
    public int m_level;

    public float m_xp;
    public float m_xpReferencia;
    public float m_xpReferenciaAnterior;
    public float m_xpPendente_qnt;
    public bool m_onPendendente_XP;

    public int m_vitorias;
    public int m_empates;
    public int m_derrotas;
    public int[] m_recompensasLevel;

    //Parte do Brasao
    public int m_baseLogo;
    public int m_fundoLogo;
    public int m_simboloLogo;
    public int m_tipoBorda;

    public bool m_usarLogo;
    public bool m_usarEstrelas;

    //Parte do Adesivo
    public int m_opcaoCorAdesivo;
    public int m_tipoAdesivo;
    public float[] m_corAdesivo;
    public bool m_podeAdesivo;

    //Cor do time
    public float[] m_corPrimaria;
    public int m_codigoCorPrimaria;
    public float[] m_corSecundaria;
    public int m_codigoCorSecundaria;
    public float[] m_corTerciaria;
    public int m_codigoCorTerciaria;

    //Textura
    public int m_codTextura;

    //Tipo dos Botoes
    public int m_tipoPlayerOnline;
    public int m_codigoBotaoOnline;
    public int m_tipoGoleiro;
    public int m_codigoGoleiro;

    //Parte Esquema Tatico
    public int m_esquemaClassico;
    public int m_esquemaRapido;
    public int m_esquema3v3;

    public float[,] m_posicoesCustomClassico;
    public float[,] m_posicoesCustomRapido;
    public float[,] m_posicoesCustom3v3;

    public int m_tipoBotaoAtaque;
    public int m_tipoBotaoMeio;
    public int m_tipoBotaoDefesa;

    public PlayerData (Player player)
    {
        m_username = player.m_username;
        m_timeNome = player.m_timeNome;
        m_abreviacao = player.m_abreviacao;

        m_level = player.m_level;
        m_xp = player.m_xp;
        m_xpReferencia = player.m_xpReferencia;
        m_xpReferenciaAnterior = player.m_xpReferenciaAnterior;
        m_xpPendente_qnt = player.m_xpPendente_qnt;
        m_onPendendente_XP = player.m_onPendendente_XP;

        m_vitorias = player.m_vitorias;
        m_derrotas = player.m_derrotas;
        m_empates = player.m_empates;

        #region Recompensas Pegas
        if (player.m_recompensasLevel == null)
        {
            player.m_recompensasLevel = new List<int>();
            player.m_recompensasLevel.Add(1);
            for (int i = 0; i < 74; i++) player.m_recompensasLevel.Add(0);
        }

        m_recompensasLevel = new int[75];
        for (int i = 0; i < 75; i++) m_recompensasLevel[i] = player.m_recompensasLevel[i];
        #endregion

        m_baseLogo = player.m_tipoBaseLogo;
        m_fundoLogo = player.m_tipoFundoLogo;
        m_simboloLogo = player.m_tipoSimboloLogo;
        m_tipoBorda = player.m_tipoBorda;
        m_usarLogo = player.m_usarLogo;
        m_usarEstrelas = player.m_usarEstrelas;

        m_podeAdesivo = player.m_podeAdesivo;
        m_tipoAdesivo = player.m_codigoAdesivo;
        m_corAdesivo = new float[4];
        m_corAdesivo[0] = player.m_corAdesivo.r;
        m_corAdesivo[1] = player.m_corAdesivo.g;
        m_corAdesivo[2] = player.m_corAdesivo.b;
        m_corAdesivo[3] = player.m_corAdesivo.a;

        m_corPrimaria = new float[3];
        m_corPrimaria[0] = player.m_corPrimaria.r;
        m_corPrimaria[1] = player.m_corPrimaria.g;
        m_corPrimaria[2] = player.m_corPrimaria.b;
        m_codigoCorPrimaria = player.m_codigoCorPrimaria;

        m_corSecundaria = new float[3];
        m_corSecundaria[0] = player.m_corSecundaria.r;
        m_corSecundaria[1] = player.m_corSecundaria.g;
        m_corSecundaria[2] = player.m_corSecundaria.b;
        m_codigoCorSecundaria = player.m_codigoCorSecundaria;

        m_corTerciaria = new float[3];
        m_corTerciaria[0] = player.m_corTerciaria.r;
        m_corTerciaria[1] = player.m_corTerciaria.g;
        m_corTerciaria[2] = player.m_corTerciaria.b;
        m_codigoCorTerciaria = player.m_codigoCorTerciaria;

        m_codTextura = player.m_codTextura;

        m_tipoGoleiro = player.m_tipoGoleiroBotao;
        m_codigoGoleiro = player.m_codigoGoleiro;
        m_tipoPlayerOnline = player.m_tipoPlayerBotaoOnline;
        m_codigoBotaoOnline = player.m_codigoBotao;

        m_esquemaClassico = player.m_esquemaClassico;
        m_esquemaRapido = player.m_esquemaRapido;
        m_esquema3v3 = player.m_esquema3v3;

        m_tipoBotaoAtaque = player.m_tipoBotaoAtaque;
        m_tipoBotaoMeio = player.m_tipoBotaoMeio;
        m_tipoBotaoDefesa = player.m_tipoBotaoDefesa;

        if (m_esquemaClassico == 4)
        {
            m_posicoesCustomClassico = new float[10, 3];
            for (int i = 0; i < 10; i++)
            {
                m_posicoesCustomClassico[i, 0] = player.posicoesCustomClassico[i, 0];
                m_posicoesCustomClassico[i, 1] = player.posicoesCustomClassico[i, 1];
                m_posicoesCustomClassico[i, 2] = player.posicoesCustomClassico[i, 2];
            }
        }

        if (m_esquemaRapido == 4)
        {
            m_posicoesCustomRapido = new float[6, 3];
            for (int i = 0; i < 6; i++)
            {
                m_posicoesCustomRapido[i, 0] = player.posicoesCustomRapido[i, 0];
                m_posicoesCustomRapido[i, 1] = player.posicoesCustomRapido[i, 1];
                m_posicoesCustomRapido[i, 2] = player.posicoesCustomRapido[i, 2];
            }
        }

        if (m_esquema3v3 == 4)
        {
            m_posicoesCustom3v3 = new float[3, 3];
            for (int i = 0; i < 3; i++)
            {
                m_posicoesCustom3v3[i, 0] = player.posicoesCustom3v3[i, 0];
                m_posicoesCustom3v3[i, 1] = player.posicoesCustom3v3[i, 1];
                m_posicoesCustom3v3[i, 2] = player.posicoesCustom3v3[i, 2];
            }
        }

    }

}
