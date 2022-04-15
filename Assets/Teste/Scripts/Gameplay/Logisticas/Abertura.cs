using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Abertura : MonoBehaviour
{
    public int jogo;
    public bool connected;

    [SerializeField] GameObject bola;

    [SerializeField] List<GameObject> estadios_11, estadios_6, estadios_3;
    [SerializeField] GameObject aberturaOff, aberturaOn;

    [SerializeField] UIMetodosGameplay ui;

    float[,] esquemaT1, esquemaT2;
    int modoJogo, qntJogadoresPorTime;

    Vector3 posGoleiro1, posGoleiro2;

    void Awake()
    {
        InstanciarEstadio_e_Abertura(GameManager.Instance.modoDeJogo, GameManager.Instance.m_online);
        
        //InstanciarEstadio_e_Abertura(jogo, connected);
        //Setar Dia/Noite, iluminacao
    }

    private void Start()
    {
        StartCoroutine(IniciarAnimacaoAbertura());
    }

    //Instancia Estadio e Abertura GO e seta os componentes da Abertura;
    void InstanciarEstadio_e_Abertura(int modo, bool conexao)
    {
        print("Instanciar Estadio e Abertura");
        bool online = false;
        int randomEstadio;

        switch (modo)
        {
            case 11:
                randomEstadio = Random.Range(0, estadios_11.Count);
                Instantiate(estadios_11[randomEstadio]);
                break;
            case 6:
                randomEstadio = Random.Range(0, estadios_6.Count);
                Instantiate(estadios_6[randomEstadio]);
                break;
            default:
                randomEstadio = Random.Range(0, estadios_3.Count);
                Instantiate(estadios_3[randomEstadio]);
                break;
        }

        modoJogo = modo;

        if (conexao)
        {
            if (GameManager.Instance.modoDeJogo != 11 || GameManager.Instance.modoDeJogo != 6 || GameManager.Instance.modoDeJogo != 30)
            {
                Instantiate(aberturaOn, GameObject.Find("Canvas").transform);
                FindObjectOfType<AberturaComponentes>().SetarComponentesOn();
                online = true;
            }
        }
        
        if(!online)
        {
            Instantiate(aberturaOff, GameObject.Find("Canvas").transform);
            FindObjectOfType<AberturaComponentes>().SetarComponentesOff();
        }

        posGoleiro1 = GameObject.Find("Posicao Goleiro 1").transform.position;
        posGoleiro2 = GameObject.Find("Posicao Goleiro 2").transform.position;

        Instantiate(bola);
    } 

    IEnumerator IniciarAnimacaoAbertura()
    {
        print("esperando comecar Abertura");
        yield return new WaitUntil(() => LoadManager.Instance.m_loadingScreen.activeSelf == false);

        SceneManager.MoveGameObjectToScene(GameObject.FindGameObjectWithTag("Bola"), SceneManager.GetActiveScene());
        SceneManager.MoveGameObjectToScene(GameObject.FindGameObjectWithTag("Estadio"), SceneManager.GetActiveScene());

        print("Animacao Abertura");
        QualTimeComeca();
        SetarVariaveisGameplay();
        EventsManager.current.OnAplicarMetodosUiSemBotao("estados todos botoes", "" ,false);
        GameObject.FindGameObjectWithTag("Abertura").GetComponent<Animator>().SetBool("Abertura", true);

        //SetarLocalBotoes(connected);
        SetarLocalBotoes(GameManager.Instance.m_online);

        yield return new WaitForSeconds(4.5f);
        GameObject.FindGameObjectWithTag("Abertura").GetComponent<Animator>().SetBool("Abertura", false);
        GameObject.FindGameObjectWithTag("Abertura").SetActive(false);
        FindObjectOfType<RandomPlacares>().InstanciarPlacar();
        ui.m_placar = GameObject.FindGameObjectWithTag("Placar");

        if (!GameManager.Instance.m_online) ui.m_placar.GetComponent<PlacarManager>().SetarPlacarConfigOff();
        else ui.m_placar.GetComponent<PlacarManager>().SetarPlacarOn(false);

        ui.m_placar.SetActive(false);
        //if (!connected) ui.m_placar.GetComponent<PlacarManager>().SetarPlacarConfigOff();

        yield return new WaitForSeconds(0.5f);
        if(GameManager.Instance.m_online) gameObject.AddComponent<GameplayOn>();
        else gameObject.AddComponent<GameplayOff>();
    }

    void SetarLocalBotoes(bool conexao)
    {
        print("Setar Local Botoes");
        float tamanhoCampoX, tamanhoCampoZ;
        tamanhoCampoX = FindObjectOfType<DimensaoCampo>().TamanhoCampo().x / 2;
        tamanhoCampoZ = FindObjectOfType<DimensaoCampo>().TamanhoCampo().y / 2;


        if (conexao == false)
        {
            GameObject t1, t2;

            if(modoJogo == 1)
            {
                qntJogadoresPorTime = 1;
            }
            else if(modoJogo == 30)
            {
                qntJogadoresPorTime = 3;
                t2 = Resources.Load<GameObject>("Esquemas Taticos/" + modoJogo.ToString() + "/" + GameManager.Instance.GetTimeOff().esquema);
                t2.GetComponent<ListaDeEsquemas>().SetarInfoBotoes(out esquemaT2, tamanhoCampoX, tamanhoCampoZ);

                if (GameManager.Instance.m_usuario.m_esquema3v3 != 4)
                {
                    if (GameManager.Instance.m_usuario.m_esquemaRapido == 0) GameManager.Instance.m_usuario.m_esquema3v3 = 1;
                    t1 = Resources.Load<GameObject>("Esquemas Taticos/" + modoJogo.ToString() + "/" + GameManager.Instance.m_usuario.m_esquema3v3);
                    t1.GetComponent<ListaDeEsquemas>().SetarInfoBotoes(out esquemaT1, tamanhoCampoX, tamanhoCampoZ);
                }
                else
                {
                    esquemaT1 = GameManager.Instance.m_usuario.posicoesCustom3v3;
                }
            } // Esquema T1 e T2 off no 3v3 time contra time
            else if(modoJogo == 6)
            {
                qntJogadoresPorTime = 6;
                t2 = Resources.Load<GameObject>("Esquemas Taticos/" + modoJogo.ToString() + "/" + GameManager.Instance.GetTimeOff().esquema);
                t2.GetComponent<ListaDeEsquemas>().SetarInfoBotoes(out esquemaT2, tamanhoCampoX, tamanhoCampoZ);

                if (GameManager.Instance.m_usuario.m_esquemaRapido != 4)
                {
                    if (GameManager.Instance.m_usuario.m_esquemaRapido == 0) GameManager.Instance.m_usuario.m_esquemaRapido = 1;
                    t1 = Resources.Load<GameObject>("Esquemas Taticos/" + modoJogo.ToString() + "/" + GameManager.Instance.m_usuario.m_esquemaRapido);
                    t1.GetComponent<ListaDeEsquemas>().SetarInfoBotoes(out esquemaT1, tamanhoCampoX, tamanhoCampoZ);
                }
                else
                {
                    esquemaT1 = GameManager.Instance.m_usuario.posicoesCustomRapido;
                }
            } // Esquema T1 e T2 off no 6v6
            else
            {
                qntJogadoresPorTime = 10;
                t2 = Resources.Load<GameObject>("Esquemas Taticos/" + modoJogo.ToString() + "/" + GameManager.Instance.GetTimeOff().esquema);
                t2.GetComponent<ListaDeEsquemas>().SetarInfoBotoes(out esquemaT2, tamanhoCampoX, tamanhoCampoZ);

                if (GameManager.Instance.m_usuario.m_esquemaClassico != 4)
                {
                    if (GameManager.Instance.m_usuario.m_esquemaRapido == 0) GameManager.Instance.m_usuario.m_esquemaClassico = 1;
                    t1 = Resources.Load<GameObject>("Esquemas Taticos/" + modoJogo.ToString() + "/" + GameManager.Instance.m_usuario.m_esquemaClassico);
                    t1.GetComponent<ListaDeEsquemas>().SetarInfoBotoes(out esquemaT1, tamanhoCampoX, tamanhoCampoZ);
                }
                else
                {
                    esquemaT1 = GameManager.Instance.m_usuario.posicoesCustomClassico;
                }
            } //Esquema T1 e T2 off no 11v11

        }
        else
        {

        }

        LogisticaVars.esquemaT1 = new float[qntJogadoresPorTime, 3];
        LogisticaVars.esquemaT2 = new float[qntJogadoresPorTime, 3];

        LogisticaVars.esquemaT1 = esquemaT1;
        LogisticaVars.esquemaT2 = esquemaT2;

        InstanciarBotoes();
    }

    void InstanciarBotoes()
    {
        LogisticaVars.jogadoresT1 = new List<GameObject>();
        LogisticaVars.jogadoresT2 = new List<GameObject>();

        print("Instanciar Botoes");
        for (int i = 0; i < qntJogadoresPorTime; i++)
        {
            var b1 = GameManager.Instance.m_usuario.m_playerBotao;
            Instantiate(b1, new Vector3(esquemaT1[i, 0], 4, esquemaT1[i, 1]), Quaternion.identity);
        }

        for (int i = 0; i < qntJogadoresPorTime; i++)
        {
            var b2 = GameManager.Instance.GetTimeOff().m_playerBotao;
            Instantiate(b2, new Vector3(-esquemaT2[i, 0], 4, -esquemaT2[i, 1]), Quaternion.identity);
        }

        foreach (GameObject botao in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (botao.layer == 8) LogisticaVars.jogadoresT1.Add(botao);
            else if(botao.layer == 9) LogisticaVars.jogadoresT2.Add(botao);
            botao.transform.eulerAngles = new Vector3(-90, 0, 0);
            botao.GetComponent<Rigidbody>().isKinematic = true;
        }

        Instantiate(GameManager.Instance.m_usuario.m_goleiroBotao, 
            new Vector3(posGoleiro1.x, GameManager.Instance.m_usuario.m_goleiroBotao.transform.position.y, posGoleiro1.z), Quaternion.identity);
        Instantiate(GameManager.Instance.GetTimeOff().m_playerGoleiro, 
            new Vector3(posGoleiro2.x, GameManager.Instance.GetTimeOff().m_playerGoleiro.transform.position.y, posGoleiro2.z), Quaternion.identity);

        //SetarBotoes(connected);
        SetarBotoes(GameManager.Instance.m_online);
    }

    void SetarBotoes(bool conexao)
    {
        print("Setar Botoes");
        if (conexao == false)
        {
            for (int i = 0; i < LogisticaVars.jogadoresT1.Count; i++)
            {
                int posBotao = (int)esquemaT1[i, 2];
                int tipoBotao;
                if (posBotao == 1) tipoBotao = GameManager.Instance.m_usuario.m_tipoBotaoAtaque;
                else if (posBotao == 2) tipoBotao = GameManager.Instance.m_usuario.m_tipoBotaoMeio;
                else tipoBotao = GameManager.Instance.m_usuario.m_tipoBotaoDefesa;

                if (tipoBotao == 0) tipoBotao = 1;

                GameManager.Instance.SetarBotaoPlayerParaJogo(LogisticaVars.jogadoresT1[i], tipoBotao);
                RedirecionarBotaoParaBola(LogisticaVars.jogadoresT1[i], tipoBotao);
                LogisticaVars.jogadoresT1[i].GetComponent<Rigidbody>().isKinematic = false;
            }

            for (int i = 0; i < LogisticaVars.jogadoresT2.Count; i++)
            {
                GameManager.Instance.SetarBotaoAdversarioOff(LogisticaVars.jogadoresT2[i], GameManager.Instance.GetTimeOff());
                RedirecionarBotaoParaBola(LogisticaVars.jogadoresT2[i], GameManager.Instance.GetTimeOff().m_tipoJogador);
                LogisticaVars.jogadoresT2[i].GetComponent<Rigidbody>().isKinematic = false;
            }

            if (modoJogo == 11 || modoJogo == 6 || modoJogo == 30)
            {
                GameManager.Instance.SetarGoleiroPlayerParaJogo(GameObject.FindGameObjectWithTag("Goleiro1"));
                GameManager.Instance.SetarGoleiroAdversarioOff(GameObject.FindGameObjectWithTag("Goleiro2"), GameManager.Instance.GetTimeOff());

                GameObject.FindGameObjectWithTag("Goleiro1").transform.eulerAngles = new Vector3(-90, 0, 0);
                GameObject.FindGameObjectWithTag("Goleiro2").transform.eulerAngles = new Vector3(-90, 0, 180);
            }
            else print("Sem Goleiros");
        }
        else
        {

        }


    }

    void RedirecionarBotaoParaBola(GameObject g, int tipo)
    {
        PosicionarCameraLogoMeshJogador(g, tipo);
        g.transform.LookAt(GameObject.FindGameObjectWithTag("Bola").transform.transform);
        g.transform.eulerAngles = new Vector3(-90, g.transform.eulerAngles.y, g.transform.eulerAngles.z);
    }

    void PosicionarCameraLogoMeshJogador(GameObject g, int i)
    {
        float angulo = 0, camera = 90, logo = 230;
        switch (i)
        {
            case 1:
                angulo = 50;
                break;
            case 2:
                angulo = 275;
                break;
            case 3:
                angulo = 320;
                break;
            case 4:
                angulo = 357;
                break;
            case 5:
                angulo = 150;
                break;
            case 6:
                angulo = 288;
                break;
            case 7:
                angulo = 265;
                break;
            case 8:
                angulo = 175;
                break;
            case 9:
                angulo = 278;
                break;
        }

        g.transform.GetChild(0).localEulerAngles = new Vector3(0, 0, logo);
        g.transform.GetChild(1).localEulerAngles = new Vector3(camera, 90, 90);
        g.transform.GetChild(2).localEulerAngles = new Vector3(0, 0, angulo + 180);
    }

    void QualTimeComeca()
    {
        print("Determinar qual time comeca");
        int moeda = Random.Range(0, 2);
        if (moeda == 0)
        {
            LogisticaVars.vezJ1 = false;
            LogisticaVars.vezJ2 = true;
        }
        else
        {
            LogisticaVars.vezJ1 = true;
            LogisticaVars.vezJ2 = false;
        }

        LogisticaVars.primeiraJogada = true;
    }

    void SetarVariaveisGameplay()
    {
        LogisticaVars.tempoMaxJogada = 20;
        print("Setar Variaveis da Gameplay");
        switch (modoJogo)
        {
            case 11:
                LogisticaVars.tempoPartida = 600;
                break;
            case 6:
                LogisticaVars.tempoPartida = 480;
                break;
            case 30:
                LogisticaVars.tempoPartida = 420;
                break;
            case 1:
                LogisticaVars.tempoPartida = 300;
                break;
            default: //online 3v3 e 2v2
                LogisticaVars.tempoPartida = 450;
                break;
        }

        LogisticaVars.jogadorSelecionado = false;
        LogisticaVars.aplicouPrimeiroToque = false;

        LogisticaVars.m_tempoSelecaoAnimator = ui.tempoEscolhaGO.GetComponent<Animator>();
        //LogisticaVars.m_especialAnimator = ui.especialBt.gameObject.GetComponent<Animator>();
        LogisticaVars.bolaRasteiraT1 = LogisticaVars.bolaRasteiraT2 = false;
    }

    public Vector3 PosicaoGoleiro(int i)
    {
        if (i == 1) return posGoleiro1;
        else return posGoleiro2;
    }
    public int QuantiaJogadores()
    {
        return qntJogadoresPorTime;
    }
}
