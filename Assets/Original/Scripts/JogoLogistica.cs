using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JogoLogistica : MonoBehaviour
{
    #region Váriaveis
    public int placarT1, placarT2, qntJogadores, jogadas, ultimoToque;

    #region Tempo
    [Header("Tempos")]
    private int minutosCorridos, segundosCorridos;
    private int tempoMaxJogada, tempoMaxEscolhaJogador;
    private float tempoCorrido;
    public float tempoJogada, tempoEscolherJogador, tempoPartida;
    #endregion

    private int moeda;
    [Header("Atributos Fisicos")]
    public float gravidade;
    public float coefAtritoEs, coefAtritoDi, fatorAtrito, fatorChute, forcaEscanteio, forcaChute, forcaGoleiro;

    #region Logica
    private bool trocouVez, pulouAbertura, primeiraJogada;
    [Header("Logica")]
    public bool vezJ1;
    public bool vezJ2, jogadorSelecionado, chuteGol, auxChuteGol, gol, golT1, golT2;
    public bool foraLateralD, foraLateralE, lateral, foraFundo, tiroDeMeta, fundo1, fundo2, bolaPermaneceNaPequenaArea, esperandoTrocas;
    public bool jogoParado, primeiroToque;
    private bool abertura, animacaoAbertura, jogoComecou, jogadaDepoisGol;
    private bool j1Ganhou, j2Ganhou, empate;
    #endregion

    #region Prints
    private bool print_chuteAutomatico, print_escolhaJg, print_fora, print_vezGoleiro, print_DesabilitarJg;
    #endregion

    [Header("Layers")]
    [SerializeField] LayerMask Player1;
    [SerializeField] LayerMask Player2;

    [Header("GameObjects")]
    public GameObject jogador;
    public GameObject cameraOrto, cameraTiroDeMeta, cameraTorcida, cameraAbertura;
    [SerializeField] List<GameObject> jogadoresT1, jogadoresT2;

    [Header("Player Physics")]
    public CapsuleCollider _col;
    public Rigidbody rb;

    #region UI
    [Header("UI's")]
    [SerializeField] public GameObject placar;
    [SerializeField] public GameObject botaoSelecionarOutroJogador, botaoChuteAoGol, botaoBolaRasteira, botaoParaRotacionarGoleiro, botaoPararJogo, visualizarTempos;
    private TextMeshProUGUI tempoTexto;
    #endregion

    public Animator aberturaAnimator;

    //private BallPhysics sB;
    //private PlayerPhysics sJ;
    private PlacarManager placarManager;
    #endregion

    private void Awake()
    {
        tempoMaxJogada = 28;
        tempoMaxEscolhaJogador = 8;
        forcaGoleiro = 0;
        forcaChute = 120;
        gravidade = -9.81f;
        coefAtritoDi = 0.7f;
        coefAtritoEs = 0.81f;
    }

    // Start is called before the first frame update
    void Start()
    {
        //sJ = GameObject.Find("FisicaJogador").GetComponent<PlayerPhysics>();
        //sB = GameObject.Find("Bola").GetComponent<BallPhysics>();
        placarManager = FindObjectOfType<PlacarManager>();
        placar = placarManager.gameObject;

        trocouVez = false;
        jogadorSelecionado = false;
        abertura = true;
        animacaoAbertura = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (abertura)
        { 
            if (animacaoAbertura)
            {
                aberturaAnimator.SetBool("Abertura", true);
                StartCoroutine(TempoParaAbertura());
            }
            if (Input.GetKeyDown(KeyCode.X)) PularAbertura();//Pular Abertura
            /*Animação dos Jogadores entrando no estádio
            * Apos um certo tempo aparece uma tela mostrando um jogador de cada time com o juiz no centro e este joga uma moeda, quando clicado um botão(no jogo off),
            * apos a escolha da paridade da moeda voltamos para o campo e os jogadores estão posicionados*/
        } 

        if (primeiraJogada && !primeiroToque)
        { 
            if(!jogadorSelecionado)
            {
                QuemEstaMaisPerto();
                DadosJogador();
                DesabilitarComponentesDosNaoSelecionados();
                StartCoroutine(ChuteInicial());
            }
            //FindObjectOfType<FollowWorld>().cam = jogador.GetComponentInChildren<Camera>();
            FindObjectOfType<FollowWorld>().lookAt = jogador.transform;
            primeiraJogada = false;
        }

        if (primeiroToque) { jogoComecou = true;  StopCoroutine(ChuteInicial()); botaoPararJogo.SetActive(true); }

        if (jogoComecou)
        {
            #region Tempo da Partida
            if (!jogoParado) tempoCorrido += Time.deltaTime;
            else tempoCorrido += 0;

            segundosCorridos = Mathf.RoundToInt(tempoCorrido % 59.5f);
            minutosCorridos = Mathf.FloorToInt(tempoCorrido / 59.5f);

            if (tempoCorrido > tempoPartida) { tempoCorrido = tempoPartida; TempoAcabou(); }

            tempoTexto.text = minutosCorridos.ToString() + ":" + segundosCorridos.ToString();
            #endregion //Tudo Certo

            if (lateral || foraFundo) Fora();

            if (!jogadorSelecionado && !lateral || !jogadorSelecionado && !foraFundo) SelecionarJogador();

            if (gol) { StartCoroutine(AnimacaoTorcida()); }

            if (jogadaDepoisGol)
            {
                if (!jogadorSelecionado)
                {
                    QuemEstaMaisPerto();
                    DadosJogador();
                    DesabilitarComponentesDosNaoSelecionados();
                    StartCoroutine(ChuteInicial());
                }
            }

            if (bolaPermaneceNaPequenaArea) StartCoroutine(TempoParaTirarBolaDaPequenaArea());

            if (tiroDeMeta) StartCoroutine(TempoParaTiroDeMeta());

            #region Caso o Jogador foi Selecionado ou Não
            if (jogadorSelecionado)
            {
                print_escolhaJg = false;
                tempoEscolherJogador = 0;
                if (!lateral && !foraFundo && !auxChuteGol && !bolaPermaneceNaPequenaArea && !jogoParado) { tempoJogada += Time.deltaTime; }
                Jogadas();
                if (tempoJogada >= tempoMaxJogada) FimDaVez();
            }
            else
            {
                BotoesOFF();
                if (!print_escolhaJg) { print("Escolha um jogador"); print_escolhaJg = true; }
                tempoJogada += Time.deltaTime;
                tempoEscolherJogador += Time.deltaTime;
                cameraOrto.SetActive(true);
                SelecionarJogador();
                if (tempoJogada >= tempoMaxJogada) FimDaVez();
                if (tempoEscolherJogador >= tempoMaxEscolhaJogador)
                {
                    print("Escolha Automática");
                    QuemEstaMaisPerto();
                    DadosJogador();
                    cameraOrto.SetActive(false);
                    tempoEscolherJogador = 0;
                }
            }
            #endregion

        }
    }

    #region Inicio do Jogo e Fim
    public void Abertura()
    {
        Moeda();
        PrimeiroSapawn();
        AdicionarJogadoresNosTimes();
        primeiraJogada = true;
        abertura = false;
    } //Tudo Certo
    public void PularAbertura()
    {
        pulouAbertura = true;
        aberturaAnimator.SetBool("Abertura", false);
        print("Pulou Abertura");
        cameraAbertura.SetActive(false);
        cameraOrto.SetActive(true);
        Abertura();
        placar.SetActive(true);
        tempoTexto = GameObject.Find("Contagem").GetComponent<TextMeshProUGUI>();
        animacaoAbertura = false;
        GameObject.Find("Pular Abertura Botao").SetActive(false);
    } //Tudo Certo
    public void Moeda()
    {
        moeda = Random.Range(0, 9);
    } //Tudo certo
    public void PrimeiroSapawn()
    {
        if (!jogoComecou)
        {
            if (moeda <= 4)
            {
                vezJ1 = true;
                vezJ2 = false;
                for (int i = 0; i < 6; i++)
                {
                    GameObject t1, t2;
                    t1 = Resources.Load("J1", typeof(GameObject)) as GameObject;
                    t2 = Resources.Load("J2", typeof(GameObject)) as GameObject;

                    t1.name = "T1 jogador " + i;
                    //Instantiate(t1, GameManager.Instance.AtaE[i].position, Quaternion.identity);
                    t2.name = "T2 jogador " + i;
                    //Instantiate(t2, GameManager.Instance.DefD[i].position, Quaternion.identity);
                }
            }
            else
            {
                vezJ2 = true;
                vezJ1 = false;
                for (int i = 0; i < 6; i++)
                {
                    GameObject t1, t2;
                    t1 = Resources.Load("Prefabs/J1", typeof(GameObject)) as GameObject;
                    t2 = Resources.Load("Prefabs/J2", typeof(GameObject)) as GameObject;

                    t2.name = "T2 jogador " + i;
                    //Instantiate(t2, GameManager.Instance.AtaD[i].transform.position, Quaternion.identity);
                    t1.name = "T1 jogador " + i;
                    //Instantiate(t1, GameManager.Instance.DefE[i].transform.position, Quaternion.identity);
                }
            }
        }
        else return;

    } //Tudo certo
    public void AposPrimeiroToque()
    {
        botaoSelecionarOutroJogador.SetActive(true);
        botaoBolaRasteira.SetActive(true);
        botaoChuteAoGol.SetActive(true);
        botaoPararJogo.SetActive(true);
        //sB.rbBola.AddForce(Vector3.right * 140);
        primeiroToque = true;
        ultimoToque = vezJ1 ? 1 : 2; jogadas++;
    }
    public void Fechamento()
    {

    }
    #endregion

    #region Estado dos Dados dos Jogadores
    public void DadosJogador()
    {
        jogador.tag = "Player Selecionado";
        jogador.GetComponentInChildren<Camera>().enabled = true;
        jogador.GetComponentInChildren<AudioListener>().enabled = true;
        //jogador.GetComponent<ForcaAtrito>().enabled = false;
        _col = jogador.GetComponent<CapsuleCollider>();
        _col.center = new Vector3(0, 1, 0);
        rb = jogador.GetComponent<Rigidbody>();
        //sJ.jPeso = Vector3.up * gravidade * rb.mass;
        //sJ.jNormal = sJ.jPeso * (-1);
        //sJ.jResultante = Vector3.zero + sJ.jNormal + sJ.jPeso;
        cameraOrto.SetActive(false);
        print_DesabilitarJg = false;
        jogadorSelecionado = true;
    } //Tudo certo
    public void DesabilitarDadosJogador()
    {
        if(!print_DesabilitarJg) { print("Desabilitando dados do Jogador"); print_DesabilitarJg = true; }
        jogador.tag = "Player";
        _col.center = Vector3.zero;
        jogador.GetComponentInChildren<Camera>().enabled = false;
        jogador.GetComponentInChildren<AudioListener>().enabled = false;
        //jogador.GetComponent<ForcaAtrito>().enabled = true;
    } //Tudo Certo
    public void DesabilitarComponentesDosNaoSelecionados()
    {
        if (GameObject.FindGameObjectsWithTag("Player") != null)
        {
            foreach (GameObject j in GameObject.FindGameObjectsWithTag("Player"))
            {
                j.GetComponentInChildren<Camera>().enabled = false;
                j.GetComponentInChildren<AudioListener>().enabled = false;
            }
        }
    } //Tudo certo
    #endregion

    #region Seleção de Jogadores
    public void SelecionarOutroJogador()
    {
        jogadorSelecionado = false;
        DesabilitarDadosJogador();
        cameraOrto.SetActive(true);
    } //Tudo certo
    public void SelecionarJogador()
    {
        RaycastHit hit;
        if (vezJ1)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, Player1))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    if (!print_escolhaJg) { Debug.Log("Colidiu Jg"); print_escolhaJg = true; }
                    if (Input.GetMouseButtonDown(0))
                    {
                        jogador = hit.collider.gameObject;
                        DadosJogador();
                        EstadosDosBotoes(4);
                        print_escolhaJg = false;
                    }
                }
            }
        }
        else if (vezJ2)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, Player2))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    if (!print_escolhaJg) { Debug.Log("Colidiu Jg"); print_escolhaJg = true; }
                    if (Input.GetMouseButtonDown(0))
                    {
                        jogador = hit.collider.gameObject;
                        DadosJogador();
                        EstadosDosBotoes(4);
                        print_escolhaJg = false;
                    }
                }
            }
        }
        else return;
    } //Tudo certo
    public void QuemEstaMaisPerto()
    {
        /*
        if (vezJ1)
        {
            float distancia, distanciaMenor;
            distanciaMenor = 150;
            foreach (GameObject Jg in jogadoresT1)
            {
                //distancia = (sB.transform.position - Jg.transform.position).magnitude;
                if (distancia <= distanciaMenor)
                {
                    distanciaMenor = distancia;
                    jogador = Jg;
                }
                else continue;
            
        }
        else
        {
            float distancia, distanciaMenor;
            distanciaMenor = 150;
            foreach (GameObject Jg in jogadoresT2)
            {
                //distancia = (sB.transform.position - Jg.transform.position).magnitude;
                if (distancia <= distanciaMenor)
                {
                    distanciaMenor = distancia;
                    jogador = Jg;
                }
                else continue;
            }
        }*/

    } //Tudo certo
    void AdicionarJogadoresNosTimes()
    {
        foreach (GameObject j in GameObject.FindGameObjectsWithTag("Player"))
        {
            //j.transform.LookAt(sB.transform.position);
            if (j.layer == 9) jogadoresT1.Add(j);
            else jogadoresT2.Add(j);
        }
    } // Tudo certo
    void TrocarVez()
    {
        if (!trocouVez)
        {
            if (vezJ1) { vezJ2 = true; vezJ1 = false; }
            else { vezJ1 = true; vezJ2 = false; }
            trocouVez = true;
        }
        print("Desabilitando os dados");
        StartCoroutine(DesabilitarApos3Jogadas());
    } //Tudo Certo
    public void FimDaVez()
    {
        print("Trocar Vez");
        TrocarVez();
        tempoJogada = 0;
    } //Tudo Certo
    #endregion

    #region Estados do Jogo
    public void Fora()
    {
        if (foraLateralD)
        {
            if (!print_fora) { print("Fora Lateral Direita"); print_fora = true; }
            jogoParado = true;
            if (ultimoToque == 1)
            {
                jogadas = 0;
                tempoJogada = 0;
                print("Desabilitando o jogador");
                DesabilitarDadosJogador();
                vezJ1 = false;
                jogadorSelecionado = false;

                if (!jogadorSelecionado)
                {
                    print("Começando a busca");
                    vezJ2 = true;
                    QuemEstaMaisPerto();
                    DadosJogador();
                    jogador.GetComponentInChildren<Camera>().enabled = false;
                    jogador.GetComponentInChildren<AudioListener>().enabled = false;
                    cameraOrto.SetActive(true);
                }
                StartCoroutine(SpawnarLat(foraLateralD));
            }
            else
            {
                jogadas = 0;
                tempoJogada = 0;
                print("Desabilitando o jogador");
                DesabilitarDadosJogador();
                vezJ2 = false;
                jogadorSelecionado = false;

                if (!jogadorSelecionado)
                {
                    print("Começando a busca");
                    vezJ1 = true;
                    QuemEstaMaisPerto();
                    DadosJogador();
                    jogador.GetComponentInChildren<Camera>().enabled = false;
                    jogador.GetComponentInChildren<AudioListener>().enabled = false;
                    cameraOrto.SetActive(true);
                }
                StartCoroutine(SpawnarLat(foraLateralD));
            }
        }//Tudo certo
        if (foraLateralE)
        {
            jogadas = 0;
            tempoJogada = 0;
            if (!print_fora) { print("Fora Lateral Esquerda"); print_fora = true; }
            jogoParado = true;
            if (ultimoToque == 1)
            {
                print("Desabilitando o jogador");
                DesabilitarDadosJogador();
                vezJ1 = false;
                jogadorSelecionado = false;

                if (!jogadorSelecionado)
                {
                    print("Começando a busca");
                    vezJ2 = true;
                    QuemEstaMaisPerto();
                    DadosJogador();
                    jogador.GetComponentInChildren<Camera>().enabled = false;
                    jogador.GetComponentInChildren<AudioListener>().enabled = false;
                    cameraOrto.SetActive(true);
                }
                StartCoroutine(SpawnarLat(foraLateralE));
            }
            else
            {
                jogadas = 0;
                tempoJogada = 0;
                print("Desabilitando o jogador");
                DesabilitarDadosJogador();
                vezJ2 = false;
                jogadorSelecionado = false;

                if (!jogadorSelecionado)
                {
                    print("Começando a busca");
                    vezJ1 = true;
                    QuemEstaMaisPerto();
                    DadosJogador();
                    jogador.GetComponentInChildren<Camera>().enabled = false;
                    jogador.GetComponentInChildren<AudioListener>().enabled = false;
                    cameraOrto.SetActive(true);
                }
                StartCoroutine(SpawnarLat(foraLateralE));
            }
        }//Tudo certo
        if (foraFundo)
        {
            if (!print_fora) { print("Fora Fundo"); print_fora = true; }
            jogoParado = true;
            if (fundo1)
            {
                if (ultimoToque == 1)
                {
                    /*if (sJ.goleiro != null) 
                    {
                        sJ.goleiroT1 = sJ.goleiroT2 = false;
                        sJ.ComponentesGoleiro(false);
                        sJ.goleiro = null;
                    }*/
                    jogadas = 0;
                    tempoJogada = 0;
                    print("Desabilitando o jogador");
                    PosicaoBolaFora();
                    DesabilitarDadosJogador();
                    cameraOrto.SetActive(true);
                    vezJ1 = false;
                    jogadorSelecionado = false;

                    if (!jogadorSelecionado)
                    {
                        print("Começa a busca");
                        vezJ2 = true;
                        QuemEstaMaisPerto();
                        DadosJogador();
                        jogador.GetComponentInChildren<Camera>().enabled = false;
                        jogador.GetComponentInChildren<AudioListener>().enabled = false;
                        cameraOrto.SetActive(true);
                    }
                    StartCoroutine(SpawnarEscanteio(fundo1));
                }//Escanteio
                else
                {
                    jogadas = 0;
                    tempoJogada = 0;
                    print("Desabilitando o jogador");
                    DesabilitarDadosJogador();
                    vezJ2 = false;
                    print("Aciona goleiro");
                    cameraOrto.SetActive(true);
                    PosicaoBolaFora();
                    StartCoroutine(SpawnarTiroDeMeta(fundo1));
                    vezJ1 = true;
                }//Tiro de Meta
            }            
            if (fundo2) //Fundo2
            {
                if (ultimoToque == 2)
                {
                    /*if (sJ.goleiro != null)
                    {
                        sJ.goleiroT1 = sJ.goleiroT2 = false;
                        sJ.ComponentesGoleiro(false);
                        sJ.goleiro = null;
                    }*/
                    jogadas = 0;
                    tempoJogada = 0;
                    print("Desabilitando o jogador");
                    DesabilitarDadosJogador();
                    cameraOrto.SetActive(true);
                    vezJ2 = false;
                    jogadorSelecionado = false;

                    if (!jogadorSelecionado)
                    {
                        print("Começa a busca");
                        vezJ1 = true;
                        QuemEstaMaisPerto();
                        DadosJogador();
                        jogador.GetComponentInChildren<Camera>().enabled = false;
                        jogador.GetComponentInChildren<AudioListener>().enabled = false;
                        cameraOrto.SetActive(true);
                    }
                    StartCoroutine(SpawnarEscanteio(fundo2));
                }//Escanteio
                else
                {
                    jogadas = 0;
                    tempoJogada = 0;
                    print("Desabilitando o jogador");
                    DesabilitarDadosJogador();
                    vezJ1 = false;
                    print("Aciona o Goleiro");
                    cameraOrto.SetActive(true);
                    PosicaoBolaFora();
                    StartCoroutine(SpawnarTiroDeMeta(fundo2));
                    vezJ2 = true;
                }//Tiro de Meta
            }
        }//Tudo certo
    }//Tudo certo
    public void MarcouGol()
    {
        if (golT1)
        {
            placarT1 += 1;            
            GameObject.Find("Bola").transform.position = new Vector3(0, 0.5f, 0);
            for (int i = 0; i < jogadoresT1.Count; i++)
            {
                //jogadoresT1[i].transform.position = GameManager.Instance.DefE[i].transform.position;
                //jogadoresT2[i].transform.position = GameManager.Instance.AtaD[i].transform.position;
            }
            primeiroToque = false;
            vezJ1 = false;
            vezJ2 = true;
            golT2 = false;
            golT1 = false;
        }
        if (golT2)
        {
            placarT2 += 1;
            GameObject.Find("Bola").transform.position = new Vector3(0, 0.5f, 0);
            for (int i = 0; i < jogadoresT1.Count; i++)
            {
                //jogadoresT1[i].transform.position = GameManager.Instance.AtaE[i].transform.position;
                //jogadoresT2[i].transform.position = GameManager.Instance.DefD[i].transform.position;
            }
            primeiroToque = false;
            vezJ1 = true;
            vezJ2 = false;
            golT1 = false;
            golT2 = false;
        }
    } //Tudo certo
    public void ChuteAoGol(bool b)
    {
        chuteGol = b;
    } //Tudo Certo
    public void Jogadas()
    {
        if (chuteGol) StartCoroutine(GoleiroDefender());

        if (jogadas == 3 && !chuteGol) FimDaVez();
    }
    public void PararJogo(bool b)
    {
        jogoParado = b;
    }
    #endregion

    #region Metodos relacionados ao Tempo
    public void TempoAcabou()
    {
        if (empate)
        {
            tempoCorrido = 0;
            tempoPartida = tempoPartida - 40;
            Moeda();
            if (moeda <= 4)
            {
                vezJ1 = true;
                vezJ2 = false;
                for (int i = 0; i < jogadoresT1.Count; i++)
                {
                    //jogadoresT1[i].transform.position = GameManager.Instance.AtaE[i].transform.position;
                    //jogadoresT1[i].transform.LookAt(sB.transform);
                    //jogadoresT2[i].transform.position = GameManager.Instance.DefD[i].transform.position;
                    //jogadoresT2[i].transform.LookAt(sB.transform);
                }
            }
            else
            {
                vezJ1 = false;
                vezJ2 = true;
                for (int i = 0; i < jogadoresT1.Count; i++)
                {
                    //jogadoresT1[i].transform.position = GameManager.Instance.DefE[i].transform.position;
                    //jogadoresT1[i].transform.LookAt(sB.transform);
                    //jogadoresT2[i].transform.position = GameManager.Instance.AtaD[i].transform.position;
                    //jogadoresT2[i].transform.LookAt(sB.transform);
                }
            }
        }
        else
        {
            Fechamento();
            Debug.Log("Parabens Campeão");
        }
        jogoComecou = false;
    } //Precisa de Scene Management
    #endregion

    #region Estado dos Botões
    public void EstadosDosBotoes(int situacao)
    {
        switch (situacao)
        {
            case 0: //Bola dentro da pequena Area, Goleiro
                jogador.GetComponentInChildren<Camera>().enabled = false;
                jogador.GetComponentInChildren<AudioListener>().enabled = false;
                botaoBolaRasteira.SetActive(true);
                botaoChuteAoGol.SetActive(false);
                botaoSelecionarOutroJogador.SetActive(true);
                botaoParaRotacionarGoleiro.SetActive(true);
                break;
            case 1: // Tiro de meta, Goleiro
                jogador.GetComponentInChildren<Camera>().enabled = false;
                jogador.GetComponentInChildren<AudioListener>().enabled = false;
                botaoBolaRasteira.SetActive(true);
                botaoChuteAoGol.SetActive(false);
                botaoSelecionarOutroJogador.SetActive(false);
                botaoParaRotacionarGoleiro.SetActive(false);
                break;
            case 2: //Chute ao Gol, Goleiro
                jogador.GetComponentInChildren<Camera>().enabled = false;
                jogador.GetComponentInChildren<AudioListener>().enabled = false;
                botaoBolaRasteira.SetActive(false);
                botaoChuteAoGol.SetActive(false);
                botaoSelecionarOutroJogador.SetActive(false);
                botaoParaRotacionarGoleiro.SetActive(true);
                break;
            case 3: // Lateral e Escanteio, Jogador
                jogador.GetComponentInChildren<Camera>().enabled = true;
                jogador.GetComponentInChildren<AudioListener>().enabled = true;
                botaoBolaRasteira.SetActive(true);
                botaoChuteAoGol.SetActive(false);
                botaoSelecionarOutroJogador.SetActive(false);
                botaoParaRotacionarGoleiro.SetActive(false);
                break;
            default : // Normal, Jogador
                jogador.GetComponentInChildren<Camera>().enabled = true;
                jogador.GetComponentInChildren<AudioListener>().enabled = true;
                botaoBolaRasteira.SetActive(true);
                botaoChuteAoGol.SetActive(true);
                botaoSelecionarOutroJogador.SetActive(true);
                botaoParaRotacionarGoleiro.SetActive(false);
                break;
        }
    }
    public void BotoesOFF()
    {
        botaoBolaRasteira.SetActive(false);
        botaoChuteAoGol.SetActive(false);
        botaoParaRotacionarGoleiro.SetActive(false);
        botaoSelecionarOutroJogador.SetActive(false);
    }
    #endregion

    #region Estados da Bola
    public void AcionarBolaRasteira(bool b)
    {
        //sJ.bolaRasteira = b;
    } //Tudo Certo
    private void PosicaoBolaFora()
    {
        /*sB.transform.position = sB.posFundo;
        sB.rbBola.freezeRotation = true;
        sB.rbBola.velocity = Vector3.zero;*/
    } // Tudo certo
    #endregion

    #region Routines

    #region Inicio
    IEnumerator TempoParaAbertura()
    {
        print("Começando a Abertura");
        animacaoAbertura = false;
        yield return new WaitForSeconds(32f);
        if (!pulouAbertura)
        {
            print("Acabou a Abertura");
            Abertura();
            aberturaAnimator.SetBool("Abertura", false);
            placar.SetActive(true);
            cameraAbertura.SetActive(false);
            tempoTexto = GameObject.Find("Contagem").GetComponent<TextMeshProUGUI>();
            GameObject.Find("Pular Abertura Botao").SetActive(false);
        }
    }
    IEnumerator ChuteInicial()
    {
        visualizarTempos.SetActive(true);
        jogadaDepoisGol = false;
        print("7 segundos para dar o chute Inicial");
        yield return new WaitForSeconds(7f);
        if (!primeiroToque) {
            AposPrimeiroToque();
        }
    }
    IEnumerator AnimacaoTorcida()
    {
        print("Começando a animação da torcida");
        cameraOrto.SetActive(false);
        cameraTiroDeMeta.SetActive(false);
        cameraTorcida.SetActive(true);
        gol = false;
        yield return new WaitForSeconds(5f);
        MarcouGol();
        primeiroToque = false;
        jogadaDepoisGol = true;
    }
    #endregion

    #region Chutes Automaticos
    IEnumerator TempoParaTirarBolaDaPequenaArea()
    {
        yield return new WaitForSeconds(7);
        if (!print_chuteAutomatico && bolaPermaneceNaPequenaArea) { print("Chute Automatico: Pequena Area"); print_chuteAutomatico = true; }
        //if (bolaPermaneceNaPequenaArea) { sJ.GoleiroChutar(); print(1); }
    }
    IEnumerator TempoParaTiroDeMeta()
    {
        yield return new WaitForSeconds(7);
        if (!print_chuteAutomatico && foraFundo) { print("Chute Automatico: Tiro de Meta"); print_chuteAutomatico = true; }
        if(foraFundo) 
        { 
            //sJ.GoleiroChutar();
            StartCoroutine(CameraTiroMeta());
        }
    }
    #endregion

    IEnumerator DesabilitarApos3Jogadas()
    {
        print("Trocando a vez");
        yield return new WaitForSeconds(1);
        cameraOrto.SetActive(true);
        DesabilitarDadosJogador();
        jogadorSelecionado = false;
        jogadas = 0;
        trocouVez = false;
    }    

    public IEnumerator GoleiroDefender()
    {
        print("Posicione o goleiro");
        jogoParado = true;
        if (vezJ1)
        {
            /*sJ.goleiroT2 = true;
            sJ.goleiro = GameObject.FindGameObjectWithTag("Goleiro2");
            sJ.ComponentesGoleiro(true);*/
        }
        else 
        { 
            /*sJ.goleiroT1 = true;
            sJ.goleiro = GameObject.FindGameObjectWithTag("Goleiro1");
            sJ.ComponentesGoleiro(true);*/
        }
        EstadosDosBotoes(2);
        chuteGol = false;
        auxChuteGol = true;
        yield return new WaitForSeconds(8);
        /*if(sJ.goleiroT1 || sJ.goleiroT2)
        {
            botaoChuteAoGol.GetComponentInChildren<Toggle>().isOn = false;
            print("Hora de CHUTAR!!");
            sJ.ComponentesGoleiro(false);
            EstadosDosBotoes(3);
            sJ.goleiroT1 = sJ.goleiroT2 = false;
            sJ.goleiro = null;
            jogoParado = false;
        }*/
    }

    #region Fora
    IEnumerator SpawnarLat(bool lateral)
    {
        BotoesOFF();
        esperandoTrocas = true;
        yield return new WaitForSeconds(1.5f);
        esperandoTrocas = false;
        EstadosDosBotoes(3);
        cameraOrto.SetActive(false);
        /*if (lateral == foraLateralD) { jogador.transform.position = sB.posLateral + new Vector3(1.5f, 0, 0); 
            jogador.transform.LookAt(sB.transform); rb.velocity = Vector3.zero; foraLateralD = false; }
        if (lateral == foraLateralE) { jogador.transform.position = sB.posLateral - new Vector3(1.5f, 0, 0); 
            jogador.transform.LookAt(sB.transform); rb.velocity = Vector3.zero; foraLateralE = false; }*/
    }

    IEnumerator SpawnarEscanteio(bool fundo)
    {
        esperandoTrocas = true;
        yield return new WaitForSeconds(2f);
        esperandoTrocas = false;
        jogador.GetComponentInChildren<Camera>().enabled = true;
        jogador.GetComponentInChildren<AudioListener>().enabled = true;
        //sJ.barraChute.SetActive(true);
        cameraOrto.SetActive(false);
        if(fundo == fundo1)
        {
            /*if (sB.transform.position.x < 0) jogador.transform.position = sB.posFundo - new Vector3(3, 0, 3);
            else jogador.transform.position = sB.posFundo + new Vector3(3, 0, -3);
            jogador.transform.LookAt(sB.transform);*/
            fundo1 = false;
        }
        if(fundo == fundo2)
        {
            /*if (sB.transform.position.x < 0) jogador.transform.position = sB.posFundo + new Vector3(-3, 0, 3);
            else jogador.transform.position = sB.posFundo + new Vector3(3, 0, 3);
            jogador.transform.LookAt(sB.transform);*/
            fundo2 = false;
        }
    }

    IEnumerator SpawnarTiroDeMeta(bool fundo)
    {
        BotoesOFF();
        esperandoTrocas = true;
        yield return new WaitForSeconds(2f);
        esperandoTrocas = false;
        cameraOrto.SetActive(false);
        if (fundo == fundo2) 
        { 
            tiroDeMeta = true;
            /*sJ.goleiroT2 = true;
            sJ.goleiro = GameObject.FindGameObjectWithTag("Goleiro2");
            sJ.ComponentesGoleiro(true);
            sJ.barraChute.SetActive(true);*/
            fundo2 = false; 
        }
        if (fundo == fundo1) 
        {
            tiroDeMeta = true;
            /*sJ.goleiroT1 = true;
            sJ.goleiro = GameObject.FindGameObjectWithTag("Goleiro1");
            sJ.ComponentesGoleiro(true);
            sJ.barraChute.SetActive(true);*/
            fundo1 = false; 
        }
        //sJ.goleiro.transform.LookAt(sB.transform.position);
        EstadosDosBotoes(1);
    }

    public IEnumerator CameraTiroMeta()
    {
        tiroDeMeta = false;
        esperandoTrocas = true;
        BotoesOFF();
        cameraTiroDeMeta.SetActive(true);
        yield return new WaitForSeconds(2);
        esperandoTrocas = false;
        //sJ.barraChute.GetComponent<ChuteBarra>().ValorJogadorNormal();
        //foraFundo = false;
        cameraTiroDeMeta.SetActive(false);
        cameraOrto.SetActive(true);
        jogadorSelecionado = false;
    }
    #endregion

    #endregion
}
