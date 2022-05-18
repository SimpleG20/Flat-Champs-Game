using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Abertura : MonoBehaviour
{
    [SerializeField] int jogo;
    [SerializeField] bool connected;

    [SerializeField] GameObject bola;

    [SerializeField] List<GameObject> estadios_11, estadios_6, estadios_3;
    [SerializeField] GameObject aberturaOff, aberturaOn;

    float[,] esquemaT1, esquemaT2;
    int qntJogadoresPorTime;

    Vector3 posGoleiro1, posGoleiro2;

    Partida.Tipo tipoJogo;
    Partida.Conexao conexaoJogo;

    void Awake()
    {
        print("*--------------------* INICIO JOGO *--------------------*");
        LogisticaVars.abertura = true;
        InstanciarEstadio_e_Abertura();
        
        //InstanciarEstadio_e_Abertura(jogo, connected);
        //Setar Dia/Noite, iluminacao
    }

    private void Start()
    {
        StartCoroutine(IniciarAnimacaoAbertura());
    }

    //Instancia Estadio e Abertura GO e seta os componentes da Abertura;
    void InstanciarEstadio_e_Abertura()
    {
        tipoJogo = GameManager.Instance.m_partida.getTipo();
        conexaoJogo = GameManager.Instance.m_partida.getConexao();

        //print("Instanciar Estadio e Abertura");
        int randomEstadio;

        switch (tipoJogo)
        {
            case Partida.Tipo.CLASSICO:
                randomEstadio = Random.Range(0, estadios_11.Count);
                Instantiate(estadios_11[randomEstadio]);
                break;
            case Partida.Tipo.QUICK:
                randomEstadio = Random.Range(0, estadios_6.Count);
                Instantiate(estadios_6[randomEstadio]);
                break;
            default:
                randomEstadio = Random.Range(0, estadios_3.Count);
                Instantiate(estadios_3[randomEstadio]);
                break;
        }


        if (conexaoJogo == Partida.Conexao.ONLINE)
        {
            if (tipoJogo != Partida.Tipo.CLASSICO || tipoJogo != Partida.Tipo.QUICK || tipoJogo != Partida.Tipo.VERSUS3_TIME)
            {
                Instantiate(aberturaOn, GameObject.Find("Canvas").transform);
                FindObjectOfType<AberturaComponentes>().SetarComponentesOn();
            }
            else
            {

            }
        }
        else
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
        //print("esperando comecar Abertura");
        yield return new WaitUntil(() => LoadManager.Instance.m_loadingScreen.activeSelf == false);

        SceneManager.MoveGameObjectToScene(GameObject.FindGameObjectWithTag("Bola"), SceneManager.GetActiveScene());
        SceneManager.MoveGameObjectToScene(GameObject.FindGameObjectWithTag("Estadio"), SceneManager.GetActiveScene());

        //print("Animacao Abertura");
        QualTimeComeca();
        SetarVariaveisGameplay();
        VariaveisUIsGameplay._current.EstadoTodosOsBotoes(false);
        GameObject.FindGameObjectWithTag("Abertura").GetComponent<Animator>().SetBool("Abertura", true);

        //SetarLocalBotoes(connected);
        SetarLocalBotoes(conexaoJogo);

        yield return new WaitForSeconds(4.5f);
        GameObject.FindGameObjectWithTag("Abertura").GetComponent<Animator>().SetBool("Abertura", false);
        GameObject.FindGameObjectWithTag("Abertura").SetActive(false);
        FindObjectOfType<RandomPlacares>().InstanciarPlacar();
        VariaveisUIsGameplay._current.m_placar = GameObject.FindGameObjectWithTag("Placar");

        if (conexaoJogo == Partida.Conexao.OFFLINE) VariaveisUIsGameplay._current.m_placar.GetComponent<PlacarManager>().SetarPlacarConfigOff();
        else VariaveisUIsGameplay._current.m_placar.GetComponent<PlacarManager>().SetarPlacarOn(false);

        VariaveisUIsGameplay._current.m_placar.SetActive(false);
        //if (!connected) ui.m_placar.GetComponent<PlacarManager>().SetarPlacarConfigOff();

        yield return new WaitForSeconds(0.5f);
        GetComponent<Gameplay>().enabled = true;
    }

    void SetarLocalBotoes(Partida.Conexao conexao)
    {
        //print("Setar Local Botoes");
        float tamanhoCampoX, tamanhoCampoZ;
        tamanhoCampoX = FindObjectOfType<DimensaoCampo>().TamanhoCampo().x / 2;
        tamanhoCampoZ = FindObjectOfType<DimensaoCampo>().TamanhoCampo().y / 2;


        if (conexao == Partida.Conexao.OFFLINE)
        {
            GameObject t1, t2;

            if(tipoJogo == Partida.Tipo.VERSUS1)
            {
                qntJogadoresPorTime = 1;
            }
            else if(tipoJogo == Partida.Tipo.VERSUS3_TIME)
            {
                qntJogadoresPorTime = 3;
                t2 = Resources.Load<GameObject>("Esquemas Taticos/" + tipoJogo.ToString() + "/" + GameManager.Instance.GetTimeOff().esquema);
                t2.GetComponent<ListaDeEsquemas>().SetarInfoBotoes(out esquemaT2, tamanhoCampoX, tamanhoCampoZ);

                if (GameManager.Instance.m_usuario.m_esquema3v3 != 4)
                {
                    if (GameManager.Instance.m_usuario.m_esquemaRapido == 0) GameManager.Instance.m_usuario.m_esquema3v3 = 1;
                    t1 = Resources.Load<GameObject>("Esquemas Taticos/" + tipoJogo.ToString() + "/" + GameManager.Instance.m_usuario.m_esquema3v3);
                    t1.GetComponent<ListaDeEsquemas>().SetarInfoBotoes(out esquemaT1, tamanhoCampoX, tamanhoCampoZ);
                }
                else
                {
                    esquemaT1 = GameManager.Instance.m_usuario.posicoesCustom3v3;
                }
            } // Esquema T1 e T2 off no 3v3 time contra time
            else if(tipoJogo == Partida.Tipo.QUICK)
            {
                qntJogadoresPorTime = 6;
                t2 = Resources.Load<GameObject>("Esquemas Taticos/" + tipoJogo.ToString() + "/" + GameManager.Instance.GetTimeOff().esquema);
                t2.GetComponent<ListaDeEsquemas>().SetarInfoBotoes(out esquemaT2, tamanhoCampoX, tamanhoCampoZ);

                if (GameManager.Instance.m_usuario.m_esquemaRapido != 4)
                {
                    if (GameManager.Instance.m_usuario.m_esquemaRapido == 0) GameManager.Instance.m_usuario.m_esquemaRapido = 1;
                    t1 = Resources.Load<GameObject>("Esquemas Taticos/" + tipoJogo.ToString() + "/" + GameManager.Instance.m_usuario.m_esquemaRapido);
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
                t2 = Resources.Load<GameObject>("Esquemas Taticos/" + tipoJogo.ToString() + "/" + GameManager.Instance.GetTimeOff().esquema);
                t2.GetComponent<ListaDeEsquemas>().SetarInfoBotoes(out esquemaT2, tamanhoCampoX, tamanhoCampoZ);

                if (GameManager.Instance.m_usuario.m_esquemaClassico != 4)
                {
                    if (GameManager.Instance.m_usuario.m_esquemaRapido == 0) GameManager.Instance.m_usuario.m_esquemaClassico = 1;
                    t1 = Resources.Load<GameObject>("Esquemas Taticos/" + tipoJogo.ToString() + "/" + GameManager.Instance.m_usuario.m_esquemaClassico);
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

        //print("Instanciar Botoes");
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
        SetarBotoes(conexaoJogo);
    }

    void SetarBotoes(Partida.Conexao conexao)
    {
        //print("Setar Botoes");
        if (conexao == Partida.Conexao.OFFLINE)
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
                LogisticaVars.jogadoresT1[i].name = "P1 " + i;
            }

            for (int i = 0; i < LogisticaVars.jogadoresT2.Count; i++)
            {
                GameManager.Instance.SetarBotaoAdversarioOff(LogisticaVars.jogadoresT2[i], GameManager.Instance.GetTimeOff());
                RedirecionarBotaoParaBola(LogisticaVars.jogadoresT2[i], GameManager.Instance.GetTimeOff().m_tipoJogador);
                LogisticaVars.jogadoresT2[i].GetComponent<Rigidbody>().isKinematic = false;
                LogisticaVars.jogadoresT2[i].name = "P2 " + i;
            }

            if (tipoJogo == Partida.Tipo.CLASSICO || tipoJogo ==  Partida.Tipo.QUICK || tipoJogo == Partida.Tipo.VERSUS3_TIME)
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

        LogisticaVars.abertura = false;
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
        //print("Determinar qual time comeca");
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
        LogisticaVars.numControle = 1;
        
        //print("Setar Variaveis da Gameplay");
        switch (tipoJogo)
        {
            case Partida.Tipo.CLASSICO:
                LogisticaVars.tempoPartida = 600;
                LogisticaVars.tempoMaxJogada = 24;
                break;
            case Partida.Tipo.QUICK:
                LogisticaVars.tempoPartida = 480;
                LogisticaVars.tempoMaxJogada = 20;
                break;
            case Partida.Tipo.VERSUS3_TIME:
                LogisticaVars.tempoPartida = 420;
                LogisticaVars.tempoMaxJogada = 20;
                break;
            case Partida.Tipo.VERSUS1:
                LogisticaVars.tempoPartida = 330;
                LogisticaVars.tempoMaxJogada = 15;
                break;
            default: //online 3v3 e 2v2
                LogisticaVars.tempoPartida = 450;
                LogisticaVars.tempoMaxJogada = 20;
                break;
        }

        LogisticaVars.jogadorSelecionado = false;
        LogisticaVars.aplicouPrimeiroToque = false;

        LogisticaVars.m_tempoSelecaoAnimator = VariaveisUIsGameplay._current.tempoEscolhaGO.GetComponent<Animator>();
        LogisticaVars.bolaRasteiraT1 = LogisticaVars.bolaRasteiraT2 = false;

        MovimentacaoDoJogador mJ = FindObjectOfType<MovimentacaoDoJogador>();
        mJ.SetBola(FindObjectOfType<FisicaBola>());
        mJ.SetInput(InputManager.current);
        mJ.SetDirecional(GameObject.FindGameObjectWithTag("Direcional Chute"), false);
        MovimentacaoDoGoleiro mG = FindObjectOfType<MovimentacaoDoGoleiro>();
        mG.SetBola(FindObjectOfType<FisicaBola>());
        mG.SetInput(InputManager.current);
        mG.SetDirecional(GameObject.FindGameObjectWithTag("Direcional Chute"), true);
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
