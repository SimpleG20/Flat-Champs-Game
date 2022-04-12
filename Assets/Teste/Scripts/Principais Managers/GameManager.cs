﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] public MenuScenesManager m_sceneManager;
    [SerializeField] public LogoManager m_logoManager;
    [SerializeField] public StatsManager m_statsManager;
    [SerializeField] public PlayerEditionManager m_playerEditionManager;
    [SerializeField] public TeamEditionManager m_teamEditionManager;
    [SerializeField] public ConfigurationManager m_configurationManager;
    [SerializeField] public LevelManager m_levelManager;

    [Header("Player Data")]
    [SerializeField] public Player m_usuario;

    [Header("Configuration Data")]
    [SerializeField] public Configuration m_config;

    [Header("Modo de Jogos")]
    public int modoDeJogo;
    public bool m_menu, m_jogoClassico;
    public bool m_jogo6v6, m_jogo1v1, m_online, m_offline;
    public bool m_jogadorAi, m_jogadorJogador;

    [Header("Tempo da Partida")]
    [SerializeField] public int m_tempoPartida;

    bool m_colocouPos, m_adicionouPos, m_criouDadosDosJogadores;

    [Header("Player GameObjects Menu")]
    [SerializeField] GameObject m_playerButtonMenu;
    [SerializeField] GameObject m_playerGoleiroMenu;

    [Header("Times Off")]
    [SerializeField] Teams m_timeOffEscolhido;
    [SerializeField] List<Teams> m_timesOff;

    public bool m_transicaoCena;

    private static GameManager m_Instance;
    public static GameManager Instance
    {
        get
        {
            if(m_Instance == null)
            {
                if(FindObjectOfType<GameManager>() == null)
                {
                    GameObject gameManager = Instantiate(Resources.Load<GameObject>("GameManager"));
                    m_Instance = gameManager.GetComponent<GameManager>();
                }
                else
                {
                    m_Instance = FindObjectOfType<GameManager>();
                }
            }
            return m_Instance;
        }
    }


    void Start()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        #region Jogo 6 contra 6
        /*if (m_jogo6v6 && SceneManager.GetActiveScene().isLoaded)
        {
            if (m_transicaoCena)
            {
                m_transicaoCena = false;
            }
             
            
        }*/
        #endregion

        #region Jogo Classico
        if (m_jogoClassico && SceneManager.GetActiveScene().isLoaded)
        {
            if (m_transicaoCena) { print("Jogo Clássico 11 x 11 \n Aproveite!!!"); m_transicaoCena = false; }
        }
        #endregion

    }

    #region Metodos Get e Set
    public Teams GetTimeOff()
    {
        if (m_timeOffEscolhido != null) return m_timeOffEscolhido;
        else m_timeOffEscolhido = m_timesOff[Random.Range(0, m_timesOff.Count)];
        return m_timeOffEscolhido;
    }

    public void SetarScripts()
    {
        if (m_playerEditionManager == null) m_playerEditionManager = FindObjectOfType<PlayerEditionManager>();
        if (m_sceneManager == null) m_sceneManager = FindObjectOfType<MenuScenesManager>();
        if (m_statsManager == null) m_statsManager = FindObjectOfType<StatsManager>();
        if (m_logoManager == null) m_logoManager = FindObjectOfType<LogoManager>();
        if (m_teamEditionManager == null) m_teamEditionManager = FindObjectOfType<TeamEditionManager>();
        if (m_configurationManager == null) m_configurationManager = FindObjectOfType<ConfigurationManager>();
        if (m_levelManager == null) m_levelManager = FindObjectOfType<LevelManager>();
    } //Verificar se todos os scripts nao estao null
    public void SetarBotaoPlayerParaJogo(GameObject g, int tipoBotao)
    {
        GameObject botao = Resources.Load<GameObject>("Testes/Prefabs/Referencia para os Formatos dos Botao/Botao Tipo " + tipoBotao.ToString());

        #region Usar Logo
        if (m_usuario.m_usarLogo)
        {
            for (int i = 0; i < 3; i++)
                g.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);

            g.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = m_usuario.m_baseLogo;
            g.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sprite = m_usuario.m_fundoLogo;
            g.transform.GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().sprite = m_usuario.m_simboloLogo;

            g.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = m_usuario.m_corSecundaria;
            g.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().color = m_usuario.m_corPrimaria;
            g.transform.GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().color = m_usuario.m_corTerciaria;
            
            g.transform.GetChild(0).localPosition = new Vector3(0, 0, botao.transform.GetChild(0).transform.localPosition.z);
        }
        else
        {
            for (int i = 0; i < 3; i++)
                g.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
        }
        #endregion

        #region Mesh e Textures
        MeshRenderer renderer = g.transform.GetChild(2).GetComponent<MeshRenderer>();
        Mesh referencia = Resources.Load<GameObject>("Testes/Prefabs/Referencia para os Formatos dos Botao/Botao Tipo " + tipoBotao.ToString()).GetComponent<MeshFilter>().sharedMesh;

        renderer.materials = botao.GetComponent<MeshRenderer>().sharedMaterials;
        
        g.transform.GetChild(2).GetComponent<MeshFilter>().mesh = referencia;
        g.transform.GetChild(2).GetComponent<MeshCollider>().sharedMesh = referencia;

        /*Texture c1 = Resources.Load<Texture>("Testes/Textures/UVs/C1/C1 T" + tipoBotao.ToString());
        Texture c2 = Resources.Load<Texture>("Testes/Textures/UVs/C2/C2 T" + tipoBotao.ToString());
        Texture c3 = Resources.Load<Texture>("Testes/Textures/UVs/C3/C3 T" + tipoBotao.ToString());

        renderer.sharedMaterials[0].SetTexture("_MainTex", c1);
        renderer.sharedMaterials[1].SetTexture("_MainTex", c2);
        renderer.sharedMaterials[2].SetTexture("_MainTex", c3);*/

        #endregion

        #region Cores e Adesivo
        renderer.sharedMaterials[0].color = m_usuario.m_corPrimaria;
        renderer.sharedMaterials[1].color = m_usuario.m_corSecundaria;
        renderer.sharedMaterials[2].color = m_usuario.m_corTerciaria;

        Adesivos adesivo = Resources.Load("Testes/Scriptable Objects/Adesivos/Adesivo " + m_usuario.m_codigoAdesivo.ToString()) as Adesivos;
        renderer.sharedMaterials[renderer.sharedMaterials.Length - 1].SetTexture("_MainTex", adesivo.m_imagem.texture);
        Color b = renderer.sharedMaterials[renderer.sharedMaterials.Length - 1].color;
        b = m_usuario.m_corAdesivo;
        b = renderer.sharedMaterials[renderer.sharedMaterials.Length - 1].color = b;

        g.transform.GetChild(3).GetComponent<SpriteRenderer>().color = m_usuario.m_corPrimaria;
        g.transform.GetChild(3).gameObject.SetActive(false);
        #endregion

        #region Camera
        g.transform.GetChild(1).localPosition += Vector3.up * m_config.m_camPosY;
        g.transform.GetChild(1).localEulerAngles = new Vector3(-40, 90, 90) + Vector3.right * m_config.m_camAngulo;
        #endregion
    }
    public void SetarGoleiroPlayerParaJogo(GameObject g)
    {
        MeshRenderer renderer = g.GetComponent<MeshRenderer>();
        GameObject referencia = Resources.Load<GameObject>("Testes/Prefabs/Goalkeeper/Goleiro Tipo " + m_usuario.m_tipoGoleiroBotao.ToString());

        g.GetComponent<MeshFilter>().mesh = referencia.GetComponent<MeshFilter>().sharedMesh;
        g.GetComponent<MeshCollider>().sharedMesh = referencia.GetComponent<MeshFilter>().sharedMesh;
        renderer.sharedMaterials[0].color = m_usuario.m_corPrimaria;
        renderer.sharedMaterials[1].color = m_usuario.m_corTerciaria;

        g.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = m_usuario.m_baseLogo;
        g.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sprite = m_usuario.m_fundoLogo;
        g.transform.GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().sprite = m_usuario.m_simboloLogo;

        g.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = m_usuario.m_corSecundaria;
        g.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().color = m_usuario.m_corPrimaria;
        g.transform.GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().color = m_usuario.m_corTerciaria;

    }
    public void SetarBotaoAdversarioOff(GameObject g, Teams adversario)
    {
        GameObject botao = Resources.Load<GameObject>("Testes/Prefabs/Referencia para os Formatos dos Botao/Botao Tipo " + adversario.m_tipoJogador.ToString());
        Mesh referencia = Resources.Load<GameObject>("Testes/Prefabs/Referencia para os Formatos dos Botao/Botao Tipo " + adversario.m_tipoJogador.ToString()).GetComponent<MeshFilter>().sharedMesh;
        
        Renderer r = g.transform.GetChild(2).GetComponent<MeshRenderer>();

        r.materials = botao.GetComponent<MeshRenderer>().sharedMaterials;

        g.transform.GetChild(2).GetComponent<MeshFilter>().mesh = referencia;
        g.transform.GetChild(2).GetComponent<MeshCollider>().sharedMesh = referencia;
        g.transform.GetChild(3).GetComponent<SpriteRenderer>().color = adversario.m_corPrimaria;
        g.transform.GetChild(3).gameObject.SetActive(false);

        #region Texture e Cor dos Materiais
        r.materials[0].color = adversario.m_corPrimaria;
        r.materials[1].color = adversario.m_corSecundaria;
        r.materials[2].color = adversario.m_corTerciaria;

        g.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = adversario.m_base;
        g.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sprite = adversario.m_fundo;
        g.transform.GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().sprite = adversario.m_simbolo;

        g.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = adversario.m_corSecundaria;
        g.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().color = adversario.m_corPrimaria;
        g.transform.GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().color = adversario.m_corTerciaria;

        g.transform.GetChild(0).localPosition = new Vector3(0, 0, botao.transform.GetChild(0).localPosition.z);
        #endregion
    }
    public void SetarGoleiroAdversarioOff(GameObject g, Teams adversario)
    {
        GameObject referencia = Resources.Load<GameObject>("Testes/Prefabs/Goalkeeper/Goleiro Tipo " + adversario.m_tipoGoleiro.ToString());

        g.GetComponent<MeshFilter>().mesh = referencia.GetComponent<MeshFilter>().sharedMesh;
        g.GetComponent<MeshCollider>().sharedMesh = referencia.GetComponent<MeshFilter>().sharedMesh;
        MeshRenderer renderer = g.GetComponent<MeshRenderer>();
        renderer.sharedMaterials[0].color = adversario.m_corPrimaria;
        renderer.sharedMaterials[1].color = adversario.m_corTerciaria;

        g.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = adversario.m_base;
        g.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sprite = adversario.m_fundo;
        g.transform.GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().sprite = adversario.m_simbolo;

        g.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = adversario.m_corSecundaria;
        g.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().color = adversario.m_corPrimaria;
        g.transform.GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().color = adversario.m_corTerciaria;

    }
    #endregion

    public void Menu()
    {
        SaveSystem.CarregarData();
        m_playerButtonMenu = GameObject.Find("Player Botao");
        m_playerGoleiroMenu = GameObject.Find("Player Goleiro");
        //CarregarData();
        //CarregarConfiguration();
    }

    public bool InternetConnectivity()
    {
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.LogWarning("No internet connection");
            return false;
        }
        else print("Conectado");
        return true;
    } //Verificar Conexao

    public void SimularPrimeiraEntrada()
    {
        PlayerPrefs.SetInt("FIRSTTIMEOPENING", 1);
    }

    #region Modos de Jogo
    public void ModosJogoOff(int i)
    {
        switch (i)
        {
            case 1:
                modoDeJogo = 11;
                break;
            case 2:
                modoDeJogo = 6;
                break;
            case 3:
                modoDeJogo = 30;
                break;
            case 4:
                modoDeJogo = 1;
                m_jogo1v1 = true;
                m_tempoPartida = 300;
                break;
        }
        m_online = false;
        m_timeOffEscolhido = m_timesOff[Random.Range(0, m_timesOff.Count)];
        m_menu = false;
        m_transicaoCena = true;
    }
    public void ModosJogoOn(int i)
    {
        if (InternetConnectivity())
        {
            m_online = true;
            switch (i)
            {
                case 1:
                    m_jogoClassico = true;
                    break;
                case 2:
                    m_jogo6v6 = true;
                    break;
                case 3:
                    m_jogo1v1 = true;
                    break;
            }
        }
        
    }
    #endregion

}
