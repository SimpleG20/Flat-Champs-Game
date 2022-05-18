using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Nomes
    public string m_username;
    public string m_timeNome;
    public string m_abreviacao;
    #endregion

    #region Stats
    public int m_level;
    
    public int m_vitorias;
    public int m_empates;
    public int m_derrotas;

    public float m_xp;
    public float m_xpReferenciaAnterior;
    public float m_xpReferencia;
    public float m_xpPendente_qnt;
    public bool m_onPendendente_XP;

    public List<int> m_recompensasLevel;
    #endregion

    #region Logo
    public Sprite m_baseLogo;
    public Sprite m_fundoLogo;
    public Sprite m_simboloLogo;
    public Sprite m_bordaLogo;
    public Sprite m_transparente;
    public Sprite m_estrela;

    public int m_tipoBaseLogo;
    public int m_tipoFundoLogo;
    public int m_tipoSimboloLogo;
    public int m_tipoBorda;

    public bool m_usarLogo;
    public bool m_usarEstrelas;
    #endregion

    #region Adesivo
    public bool m_podeAdesivo;
    public int m_opcaoCorAdesivo;
    public int m_codigoAdesivo;
    public Color m_corAdesivo;
    #endregion

    #region GameObjects
    public GameObject m_playerBotao;
    public int m_tipoPlayerBotaoOnline;
    public int m_codigoBotao;
    public GameObject m_goleiroBotao;
    public int m_tipoGoleiroBotao;
    public int m_codigoGoleiro;
    #endregion

    #region Cores
    /*public Material m_materialC1;
    public Material m_materialC2;
    public Material m_materialC3;*/

    public Color m_corPrimaria = Color.white;
    public int m_codigoCorPrimaria;
    public Color m_corSecundaria = Color.grey;
    public int m_codigoCorSecundaria;
    public Color m_corTerciaria = Color.black;
    public int m_codigoCorTerciaria;

    public int m_codTextura;
    #endregion

    #region Esquema Time
    public int m_esquemaClassico;
    public int m_esquemaRapido;
    public int m_esquema3v3;

    public float[,] posicoesCustomClassico;
    public float[,] posicoesCustomRapido;
    public float[,] posicoesCustom3v3;

    public int m_tipoBotaoAtaque;
    public int m_tipoBotaoMeio;
    public int m_tipoBotaoDefesa;
    #endregion

    public void AtualizarData(PlayerData data)
    {
        m_username = data.m_username;
        m_timeNome = data.m_timeNome;
        m_abreviacao = data.m_abreviacao;

        #region Setup Status
        m_level = data.m_level;
        m_xp = data.m_xp;
        m_xpReferencia = data.m_xpReferencia;
        m_xpReferenciaAnterior = data.m_xpReferenciaAnterior;
        m_xpPendente_qnt = data.m_xpPendente_qnt;
        m_onPendendente_XP = data.m_onPendendente_XP;

        m_vitorias = data.m_vitorias;
        m_derrotas = data.m_derrotas;
        m_empates = data.m_empates;

        if (data.m_recompensasLevel == null)
        {
            data.m_recompensasLevel = new int[75];
            data.m_recompensasLevel[0] = 1;
            for (int i = 0; i < 74; i++) data.m_recompensasLevel[i+1] = 0;
        }

        for (int i = 0; i < 75; i++)
        {
            m_recompensasLevel[i] = data.m_recompensasLevel[i];
        }
        #endregion

        #region Setup Logo
        m_tipoBaseLogo = data.m_baseLogo;
        m_tipoFundoLogo = data.m_fundoLogo;
        m_tipoSimboloLogo = data.m_simboloLogo;
        m_tipoBorda = data.m_tipoBorda;

        m_usarLogo = data.m_usarLogo;
        m_usarEstrelas = data.m_usarEstrelas;

        m_baseLogo = Resources.Load<Sprite>("Testes/Textures/Logos/Bases/" + m_tipoBaseLogo.ToString());

        Logos fundo = Resources.Load("Testes/Scriptable Objects/Logos Fundos/" + m_tipoFundoLogo.ToString()) as Logos;
        Logos simbolo = Resources.Load("Testes/Scriptable Objects/Logos Simbolos/" + m_tipoSimboloLogo.ToString()) as Logos;
        Logos bordaImg = Resources.Load("Testes/Scriptable Objects/Logos Bordas/" + m_tipoBorda.ToString()) as Logos;
        m_bordaLogo = bordaImg.m_tipo1;
        Sprite estrelaImg = Resources.Load("Teste/Textures/Logo/Bordas/Estrela") as Sprite;
        m_estrela = estrelaImg;

        switch (m_tipoBaseLogo)
        {
            case 1:
                m_fundoLogo = fundo.m_tipo1;

                if (simbolo.m_tipo1 != null) m_simboloLogo = simbolo.m_tipo1;
                else m_simboloLogo = m_transparente;
                break;
            case 2:
                m_fundoLogo = fundo.m_tipo2;

                if(simbolo.m_tipo2 != null) m_simboloLogo = simbolo.m_tipo2;
                else m_simboloLogo = m_transparente;
                break;
            case 3:
                m_fundoLogo = fundo.m_tipo3;

                if(simbolo.m_tipo3 != null) m_simboloLogo = simbolo.m_tipo3;
                else m_simboloLogo = m_transparente;
                break;
            case 4:
                m_fundoLogo = fundo.m_tipo4;

                if(simbolo.m_tipo4 != null) m_simboloLogo = simbolo.m_tipo4;
                else m_simboloLogo = m_transparente;
                break;
        }
        #endregion

        #region Setup Botoes
        m_tipoPlayerBotaoOnline = data.m_tipoPlayerOnline;
        m_codigoBotao = data.m_codigoBotaoOnline;
        m_tipoGoleiroBotao = data.m_tipoGoleiro;
        m_codigoGoleiro = data.m_codigoGoleiro;

        if (m_tipoPlayerBotaoOnline == 0) m_tipoPlayerBotaoOnline = 1;
        if (m_tipoGoleiroBotao == 0) m_tipoGoleiroBotao = 1;

        if (m_codigoBotao == 0) m_codigoBotao = 1;
        if (m_codigoGoleiro == 0) m_codigoGoleiro = 1;

        GameObject j = Resources.Load("Testes/Prefabs/Referencia para os Formatos dos Botao/Botao Tipo " + m_tipoPlayerBotaoOnline.ToString()) as GameObject;
        GameObject g = Resources.Load<GameObject>("Testes/Prefabs/Goalkeeper/Goleiro Tipo " + m_tipoGoleiroBotao.ToString());

        m_playerBotao.transform.GetChild(2).GetComponent<MeshFilter>().mesh =  j.GetComponent<MeshFilter>().sharedMesh;
        m_playerBotao.transform.GetChild(2).GetComponent<MeshRenderer>().materials = j.GetComponent<MeshRenderer>().sharedMaterials;

        m_goleiroBotao.GetComponent<MeshFilter>().mesh = g.GetComponent<MeshFilter>().sharedMesh;
        m_goleiroBotao.GetComponent<MeshRenderer>().materials = g.GetComponent<MeshRenderer>().sharedMaterials;


        m_corPrimaria.r = data.m_corPrimaria[0];
        m_corPrimaria.g = data.m_corPrimaria[1];
        m_corPrimaria.b = data.m_corPrimaria[2];
        m_codigoCorPrimaria = data.m_codigoCorPrimaria;

        m_corSecundaria.r = data.m_corSecundaria[0];
        m_corSecundaria.g = data.m_corSecundaria[1];
        m_corSecundaria.b = data.m_corSecundaria[2];
        m_codigoCorSecundaria = data.m_codigoCorSecundaria;

        m_corTerciaria.r = data.m_corTerciaria[0];
        m_corTerciaria.g = data.m_corTerciaria[1];
        m_corTerciaria.b = data.m_corTerciaria[2];
        m_codigoCorTerciaria = data.m_codigoCorTerciaria;

        m_codTextura = data.m_codTextura;
        #endregion

        #region Setup Adesivo
        m_podeAdesivo = data.m_podeAdesivo;
        m_opcaoCorAdesivo = data.m_opcaoCorAdesivo;
        m_codigoAdesivo = data.m_tipoAdesivo;
        m_corAdesivo.r = data.m_corAdesivo[0];
        m_corAdesivo.g = data.m_corAdesivo[1];
        m_corAdesivo.b = data.m_corAdesivo[2];
        m_corAdesivo.a = data.m_corAdesivo[3];
        #endregion

        #region Setup Esquema
        m_esquemaClassico = data.m_esquemaClassico;
        m_esquemaRapido = data.m_esquemaRapido;
        m_esquema3v3 = data.m_esquema3v3;

        if(m_esquemaClassico == 4)
        {
            for (int i = 0; i < 10; i++)
            {
                posicoesCustomClassico[i, 0] = data.m_posicoesCustomClassico[i, 0];
                posicoesCustomClassico[i, 1] = data.m_posicoesCustomClassico[i, 1];
                posicoesCustomClassico[i, 2] = data.m_posicoesCustomClassico[i, 2];
            }
        }
        
        if(m_esquemaRapido == 4)
        {
            for (int i = 0; i < 6; i++)
            {
                posicoesCustomRapido[i, 0] = data.m_posicoesCustomRapido[i, 0];
                posicoesCustomRapido[i, 1] = data.m_posicoesCustomRapido[i, 1];
                posicoesCustomRapido[i, 2] = data.m_posicoesCustomRapido[i, 2];
            }
        }

        if(m_esquema3v3 == 4)
        {
            for (int i = 0; i < 3; i++)
            {
                posicoesCustom3v3[i, 0] = data.m_posicoesCustom3v3[i, 0];
                posicoesCustom3v3[i, 1] = data.m_posicoesCustom3v3[i, 1];
                posicoesCustom3v3[i, 2] = data.m_posicoesCustom3v3[i, 2];
            }
        }

        m_tipoBotaoAtaque = data.m_tipoBotaoAtaque;
        m_tipoBotaoMeio = data.m_tipoBotaoMeio;
        m_tipoBotaoDefesa = data.m_tipoBotaoDefesa;

        if (m_esquemaClassico == 0) m_esquemaClassico = 1;
        if (m_esquemaRapido == 0) m_esquemaRapido = 1;
        if (m_esquema3v3 == 0) m_esquema3v3 = 1;
        if (m_tipoBotaoAtaque == 0) m_tipoBotaoAtaque = 1;
        if (m_tipoBotaoMeio == 0) m_tipoBotaoMeio = 1;
        if (m_tipoBotaoDefesa == 0) m_tipoBotaoDefesa = 1;
        #endregion
    }
}
