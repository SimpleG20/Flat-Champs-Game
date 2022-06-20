using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    #region Scrpits
    [Header("Scripts")]
    [SerializeField] public MenuScenesManager m_sceneManager;
    [SerializeField] public LogoManager m_logoManager;
    [SerializeField] public StatsManager m_statsManager;
    [SerializeField] public PlayerEditionManager m_playerEditionManager;
    [SerializeField] public TeamEditionManager m_teamEditionManager;
    [SerializeField] public ConfigurationManager m_configurationManager;
    [SerializeField] public LevelManager m_levelManager;
    #endregion

    #region Player, Config, Partida, TimeOff
    [Header("Player Data")]
    [SerializeField] public Player m_usuario;

    [Header("Configuration Data")]
    [SerializeField] public Configuration m_config;

    [Header("Modo de Jogos")]
    public Partida m_partida;

    [Header("Times Off")]
    [SerializeField] Teams m_timeOffEscolhido;
    [SerializeField] List<Teams> m_timesOff;
    #endregion

    [SerializeField] bool aumentar_XP;
    [SerializeField] float qnt_XP;

    public bool m_transicaoCena;
    
    //bool m_colocouPos, m_adicionouPos, m_criouDadosDosJogadores;

    /*[Header("Player GameObjects Menu")]
    [SerializeField] GameObject m_playerButtonMenu;
    [SerializeField] GameObject m_playerGoleiroMenu;*/

    #region Singleton
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
    #endregion

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    #region Metodos Get e Set
    public void SetarScripts()
    {
        m_playerEditionManager = FindObjectOfType<PlayerEditionManager>();
        m_sceneManager = FindObjectOfType<MenuScenesManager>();
        m_statsManager = FindObjectOfType<StatsManager>();
        m_logoManager = FindObjectOfType<LogoManager>();
        m_teamEditionManager = FindObjectOfType<TeamEditionManager>();
        m_configurationManager = FindObjectOfType<ConfigurationManager>();
        m_levelManager = FindObjectOfType<LevelManager>();
    } //Verificar se todos os scripts nao estao null

    #region Gameplay Inicio
    public Teams GetTimeOff()
    {
        if (m_timeOffEscolhido != null) return m_timeOffEscolhido;
        else m_timeOffEscolhido = m_timesOff[Random.Range(0, m_timesOff.Count)];
        return m_timeOffEscolhido;
    }
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

    #region Gameplay Fim
    public bool getOnAumentarXP()
    {
        return aumentar_XP;
    }
    public float getXP_ParaAumentar()
    {
        return qnt_XP;
    }
    public void setAumentarXP(bool b)
    {
        aumentar_XP = b;
        m_usuario.m_onPendendente_XP = b;
    }
    public void setQntXP(float qnt)
    {
        //Verificar se o jogador tem bonus para o XP como 2x, 3x
        qnt_XP = qnt;
        m_usuario.m_xpPendente_qnt = qnt_XP;
    }
    #endregion

    #endregion

    #region Modos de Jogo
    public void JogoOff_JogadorAi(int i)
    {
        m_partida = ScriptableObject.CreateInstance<Partida>();
        m_partida.setConexao(Partida.Conexao.OFFLINE);
        m_partida.setModo(Partida.Modo.JOGADOR_VERSUS_AI);
        switch (i)
        {
            case 1:
                m_partida.setTipo(Partida.Tipo.CLASSICO);
                break;
            case 2:
                m_partida.setTipo(Partida.Tipo.QUICK);
                break;
            case 3:
                m_partida.setTipo(Partida.Tipo.VERSUS3_TIME);
                break;
            case 4:
                m_partida.setTipo(Partida.Tipo.VERSUS1);
                break;
        }
        m_partida.setXPEsperado();

        //print(m_partida.getConexao() + " - " + m_partida.getModo()+ " - " + m_partida.getTipo());

        m_timeOffEscolhido = m_timesOff[Random.Range(0, m_timesOff.Count)];
        m_transicaoCena = true;
    }
    public void JogoOff_JogadorJogador(int i)
    {
        m_partida = ScriptableObject.CreateInstance<Partida>();
        m_partida.setConexao(Partida.Conexao.OFFLINE);
        m_partida.setModo(Partida.Modo.JOGADO_VERSUS_JOGADOR);
        switch (i)
        {
            case 1:
                m_partida.setTipo(Partida.Tipo.CLASSICO);
                break;
            case 2:
                m_partida.setTipo(Partida.Tipo.QUICK);
                break;
            case 3:
                m_partida.setTipo(Partida.Tipo.VERSUS3_TIME);
                break;
            case 4:
                m_partida.setTipo(Partida.Tipo.VERSUS1);
                break;
        }
        m_partida.setXPEsperado();

        //print(m_partida.getConexao() + " - " + m_partida.getModo() + " - " + m_partida.getTipo());

        m_timeOffEscolhido = m_timesOff[Random.Range(0, m_timesOff.Count)];
        m_transicaoCena = true;
    }
    public void JogoOn_JogadorJogador(int i)
    {
        if (InternetConnectivity())
        {
            m_partida = ScriptableObject.CreateInstance<Partida>();
            m_partida.setConexao(Partida.Conexao.ONLINE);
            m_partida.setModo(Partida.Modo.JOGADO_VERSUS_JOGADOR);
            switch (i)
            {
                case 1:
                    m_partida.setTipo(Partida.Tipo.CLASSICO);
                    break;
                case 2:
                    m_partida.setTipo(Partida.Tipo.QUICK);
                    break;
                case 3:
                    m_partida.setTipo(Partida.Tipo.VERSUS3_TIME);
                    break;
                case 4:
                    m_partida.setTipo(Partida.Tipo.VERSUS3_INDIVIDUAL);
                    break;
                case 5:
                    m_partida.setTipo(Partida.Tipo.VERSUS1);
                    break;
            }
            m_partida.setXPEsperado();
        }
    }
    #endregion

    public void Menu()
    {
        //SaveSystem.CarregarMenu();
        m_sceneManager.cenaAtual = "Menu";
        //m_playerButtonMenu = GameObject.Find("Player Botao");
        //m_playerGoleiroMenu = GameObject.Find("Player Goleiro");
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
}
