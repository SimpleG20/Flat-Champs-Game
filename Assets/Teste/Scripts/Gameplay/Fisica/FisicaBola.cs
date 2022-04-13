using System;
using System.Collections;
using UnityEngine;

public class FisicaBola : MonoBehaviour
{
    public float forca, m_velocidadeBola;
    private float tempoNaPequenaArea, m_quantidadeDeMovimentoBola;

    //public GameObject direcao;

    [Header("Logica Bola")]
    public bool m_bolaCorrendo;
    public bool m_bolaNoChao, m_toqueChao, m_encostouJogador;
    private bool m_contagemPequenaArea;

    
    [Header("Vetores da Bola")]
    public Vector3 m_posLateral;
    public Vector3 m_posicaoFundo, m_posicaoJogador, m_vetorDistanciaDoJogador, m_vetorVelocidade, direcaoBola;

    [Header("Rigidbody")]
    public Rigidbody m_rbBola;

    private Vector3 vetorVelocidadeNormalizado, vetorForcaFat, vetorforcaNormal, vetorForcaResistente, vetorForcaPeso;
    private bool p;
    private bool posicionouJogador;

    void Start()
    {
        m_rbBola = GetComponent<Rigidbody>();

        vetorForcaPeso = Vector3.down * 9.81f * m_rbBola.mass;
        vetorforcaNormal = -vetorForcaPeso;
    }

    void FixedUpdate()
    {        
        #region Dados Bola
        if (LogisticaVars.bolaRasteiraT1 && LogisticaVars.vezJ1 || LogisticaVars.bolaRasteiraT2 && LogisticaVars.vezJ2) m_rbBola.constraints = RigidbodyConstraints.FreezePositionY;
        else m_rbBola.constraints = RigidbodyConstraints.None;

        if (transform.position.y > 0.6f)
        {
            m_bolaNoChao = false;
            vetorForcaResistente = Vector3.down * AtributosFisicos.gravidade;
        }
        else m_bolaNoChao = true;

        if (LogisticaVars.m_jogadorEscolhido != null)
        {
            m_posicaoJogador = LogisticaVars.m_jogadorEscolhido.transform.position;

            if(!LogisticaVars.especial) direcaoBola = new Vector3(transform.position.x - LogisticaVars.m_jogadorEscolhido.transform.position.x, 0, transform.position.z - LogisticaVars.m_jogadorEscolhido.transform.position.z);
            //if(!LogisticaVars.especial) direcaoBola = new Vector3(JogadorVars.direcaoChute.x + cosBola, 0, JogadorVars.direcaoChute.z);

            m_vetorDistanciaDoJogador = transform.position - m_posicaoJogador;
        }

        if (m_contagemPequenaArea && !LogisticaVars.tiroDeMeta && !LogisticaVars.auxChuteAoGol)
        { tempoNaPequenaArea += Time.deltaTime; } //print("Tempo na Pequena Area: " + Mathf.Round(tempoNaPequenaArea)); }

        if (vetorForcaResistente.magnitude != 0 || !m_bolaNoChao) m_bolaCorrendo = true;
        else m_bolaCorrendo = false;

        //if (m_vetorDistanciaDoJogador.magnitude < 7 && LogisticaVars.mostrarDirecaoBola && LogisticaVars.jogadorSelecionado) direcao.SetActive(true);
        //else direcao.SetActive(false);
        #endregion

        #region Dinamica da Bola
        if (m_bolaNoChao)
        {
            if (m_rbBola.velocity.magnitude < 0.05f) vetorVelocidadeNormalizado = new Vector3(0, 0, 0);
            else vetorVelocidadeNormalizado = new Vector3(-m_rbBola.velocity.x / m_rbBola.velocity.magnitude, 0, -m_rbBola.velocity.z / m_rbBola.velocity.magnitude);

            /*if (m_bolaCorrendo) vetorForcaFat = new Vector3(AtributosFisicos.coefAtritoDiBola * vetorforcaNormal.y, 0, AtributosFisicos.coefAtritoDiBola * vetorforcaNormal.y);
            else vetorForcaFat = new Vector3(AtributosFisicos.coefAtritoEsBola * vetorforcaNormal.y, 0, AtributosFisicos.coefAtritoEsBola * vetorforcaNormal.y);*/

            if (vetorVelocidadeNormalizado.magnitude != 0) vetorForcaFat = new Vector3(AtributosFisicos.coefAtritoDiBola * vetorforcaNormal.y, 0, AtributosFisicos.coefAtritoDiBola * vetorforcaNormal.y);
            else vetorForcaFat = new Vector3(AtributosFisicos.coefAtritoEsBola * vetorforcaNormal.y, 0, AtributosFisicos.coefAtritoEsBola * vetorforcaNormal.y);

            vetorForcaResistente = new Vector3(vetorForcaFat.x * vetorVelocidadeNormalizado.x, 0, vetorForcaFat.z * vetorVelocidadeNormalizado.z);

            if (m_bolaCorrendo && m_bolaNoChao)
            {
                m_rbBola.AddForce(vetorForcaResistente, ForceMode.Force);
                p = false;
            }
            else if(!m_bolaCorrendo && m_bolaNoChao)
            {
                if (!p)
                {
                    //print("Forca Resultante nula");
                    m_rbBola.velocity = Vector3.zero;
                    RedirecionarJogadores(true);
                    p = true;
                    m_encostouJogador = false;
                    m_rbBola.freezeRotation = true;
                }
            }
        }
        else
        {
            //if (m_rbBola.velocity.magnitude != 0) m_bolaCorrendo = true;
        }
        #endregion
    }

    public void RedirecionarJogadores(bool b)
    {
        if (b == true)
        {
            if (GameObject.FindGameObjectsWithTag("Player") != null)
            {
                foreach (GameObject j in GameObject.FindGameObjectsWithTag("Player"))
                {
                    j.transform.LookAt(GameObject.FindGameObjectWithTag("Bola").transform.transform);
                    j.transform.eulerAngles = new Vector3(-90, j.transform.eulerAngles.y, j.transform.eulerAngles.z);
                }
            }
        }
        else return;
    }
    public void RedirecionarJogadorEscolhido(Transform target)
    {
        LogisticaVars.m_jogadorEscolhido.transform.LookAt(target);
        LogisticaVars.m_jogadorEscolhido.transform.eulerAngles = 
            new Vector3(-90, LogisticaVars.m_jogadorEscolhido.transform.eulerAngles.y, LogisticaVars.m_jogadorEscolhido.transform.eulerAngles.z);
    }
    public void RedirecionarGoleiros()
    {
        if (GameObject.FindGameObjectWithTag("Goleiro1") != null)
        {
            GameObject g = GameObject.FindGameObjectWithTag("Goleiro1");
            g.transform.LookAt(transform);
            g.transform.eulerAngles = new Vector3(-90, g.transform.eulerAngles.y, g.transform.eulerAngles.z);
        }
        if (GameObject.FindGameObjectWithTag("Goleiro2") != null)
        {
            GameObject g = GameObject.FindGameObjectWithTag("Goleiro2");
            g.transform.LookAt(transform);
            g.transform.eulerAngles = new Vector3(-90, g.transform.eulerAngles.y, g.transform.eulerAngles.z);
        }
    }

    private void PosicionarLateral()
    {
        LogisticaVars.lateral = true;
        m_posLateral = new Vector3(transform.position.x, .5f, transform.position.z);
        transform.position = m_posLateral;
        m_rbBola.velocity = Vector3.zero;
        m_rbBola.freezeRotation = true;
        LogisticaVars.m_rbJogadorEscolhido.velocity = Vector3.zero;
    }
    public void PosicionarAposGol()
    {
        m_rbBola.velocity = Vector3.zero;
        m_rbBola.freezeRotation = true;
        transform.position = new Vector3(0, 0.55f, 0);
    }

    IEnumerator VoltarBounce()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<SphereCollider>().material.bounciness = 1;
        //print("Bounciness: " + GetComponent<SphereCollider>().material.bounciness);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Campo"))
        {
            if (!LogisticaVars.goleiroT1 && !LogisticaVars.goleiroT2) m_toqueChao = true;
        }

        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Player Selecionado"))
        {
            #region QuemTocou
            switch (collision.gameObject.layer)
            {
                case 8:
                    LogisticaVars.ultimoToque = 1;
                    //Debug.Log("Player 1");
                    break;
                case 9:
                    LogisticaVars.ultimoToque = 2;
                    //Debug.Log("Player 2");
                    break;
                default:
                    LogisticaVars.ultimoToque = 0;
                    break;

            }
            #endregion

            /*if (m_logisticaDoJogo.jogoParado) m_logisticaDoJogo.jogoParado = false;*/
            if (m_rbBola.freezeRotation) m_rbBola.freezeRotation = false;
        }

        if (LogisticaVars.jogadorSelecionado)
        {
            if (collision.gameObject.name == LogisticaVars.m_jogadorEscolhido.name)
            {
                GetComponent<SphereCollider>().material.bounciness = 0;
                //print("Bounciness: " + GetComponent<SphereCollider>().material.bounciness);
                m_encostouJogador = true;

                #region Ricochete
                //Vector3 ricocheteB;
                float vImpacto = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;

                m_quantidadeDeMovimentoBola = (vImpacto * m_rbBola.mass);

                /*if (LogisticaVars.bolaRasteira)
                {
                    //ricocheteB = FindObjectOfType<DirecionalDaBola>().direcaoBola.normalized * m_quantidadeDeMovimentoBola / 2.5f;
                    m_rbBola.AddForce(FindObjectOfType<DirecionalDaBola>().direcaoBola.normalized * m_quantidadeDeMovimentoBola / 2.5f, ForceMode.Impulse);
                }*/

                /*else
                    ricocheteB = new Vector3(-m_vetorDistanciaDoJogador.normalized.x * m_quantidadeDeMovimentoBola / 2.5f, 7, -m_vetorDistanciaDoJogador.normalized.z * m_quantidadeDeMovimentoBola / 2.5f);*/

                m_rbBola.AddForce(direcaoBola.normalized * m_quantidadeDeMovimentoBola / 5f, ForceMode.Impulse);
                JogadorVars.m_toqueBola = true;
                StartCoroutine(VoltarBounce());
                //GetComponent<SphereCollider>().material.bounciness = 1;
                #endregion
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Campo"))
        {
            m_toqueChao = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        #region Pequena Area
        if (other.CompareTag("PequenaArea1") || other.CompareTag("PequenaArea2"))
        {
            m_contagemPequenaArea = true;
            LogisticaVars.bolaEntrouPequenaArea = true;
            print("Bola entrou na Pequena Area");
        }
        #endregion
        #region Lateral
        if (other.gameObject.CompareTag("LateralD"))
        {
            print("Lateral");
            PosicionarLateral();
            //dar uma distancia para uma cobranca sem problemas
            if(m_vetorDistanciaDoJogador.magnitude < 4) LogisticaVars.m_jogadorEscolhido.transform.position = new Vector3(LogisticaVars.m_jogadorEscolhido.transform.position.x - 4.5f,
                    LogisticaVars.m_jogadorEscolhido.transform.position.y, LogisticaVars.m_jogadorEscolhido.transform.position.z);


            LogisticaVars.foraLateralD = true;
            EventsManager.current.SituacaoGameplay("fora lateral");

            StartCoroutine(RotinasGameplay.SpawnarLat("lateral direita", this));
        }
        if (other.gameObject.CompareTag("LateralE"))
        {
            print("Lateral");
            PosicionarLateral();
            //dar uma distancia para uma cobranca sem problemas
            if (m_vetorDistanciaDoJogador.magnitude < 4)
                LogisticaVars.m_jogadorEscolhido.transform.position = new Vector3(LogisticaVars.m_jogadorEscolhido.transform.position.x + 4.5f,
                    LogisticaVars.m_jogadorEscolhido.transform.position.y, LogisticaVars.m_jogadorEscolhido.transform.position.z);

            LogisticaVars.foraLateralE = true;
            EventsManager.current.SituacaoGameplay("fora lateral");

            StartCoroutine(RotinasGameplay.SpawnarLat("lateral esquerda", this));
        }
        #endregion
        #region Linha de Fundo
        if (other.gameObject.CompareTag("Linha Fundo G1") && !LogisticaVars.posGol)
        {
            LogisticaVars.foraFundo = true;
            LogisticaVars.fundo1 = true;
            LogisticaVars.fundo2 = false;
            LogisticaVars.m_rbJogadorEscolhido.velocity = Vector3.zero;

            if (LogisticaVars.ultimoToque == 2) //Tiro de Meta
            {
                print("Tiro de Meta");
                if (transform.position.x < 0) m_posicaoFundo = FindObjectOfType<DimensaoCampo>().PosicaoBolaTiroDeMeta(1, false);
                else m_posicaoFundo = FindObjectOfType<DimensaoCampo>().PosicaoBolaTiroDeMeta(1, true);
                transform.position = m_posicaoFundo;

                EventsManager.current.SituacaoGameplay("fora tiro de meta");
                StartCoroutine(RotinasGameplay.SpawnarTiroDeMeta("fundo 1", this));
            }
            else //Escanteio
            {
                print("Escanteio");
                if (transform.position.x < 0)
                {
                    GameObject.Find("Bandeira Esq").transform.GetChild(0).gameObject.SetActive(false);
                    m_posicaoFundo = GameObject.Find("Escanteio Esq -x").transform.position;
                    transform.position = m_posicaoFundo;

                    //dar uma distancia para uma cobranca sem problemas
                    LogisticaVars.m_jogadorEscolhido.transform.position = new Vector3(LogisticaVars.m_jogadorEscolhido.transform.position.x + 2,
                            LogisticaVars.m_jogadorEscolhido.transform.position.y, LogisticaVars.m_jogadorEscolhido.transform.position.z + 2);
                }
                else
                {
                    GameObject.Find("Bandeira Dir").transform.GetChild(0).gameObject.SetActive(false);
                    m_posicaoFundo = GameObject.Find("Escanteio Esq +x").transform.position;
                    transform.position = m_posicaoFundo;

                    //dar uma distancia para uma cobranca sem problemas
                    LogisticaVars.m_jogadorEscolhido.transform.position = new Vector3(LogisticaVars.m_jogadorEscolhido.transform.position.x - 2,
                            LogisticaVars.m_jogadorEscolhido.transform.position.y, LogisticaVars.m_jogadorEscolhido.transform.position.z + 2);
                }
                EventsManager.current.SituacaoGameplay("fora escanteio");
                StartCoroutine(RotinasGameplay.SpawnarEscanteio("fundo 1", this));
            }

            m_rbBola.freezeRotation = true;
            m_rbBola.velocity = Vector3.zero;
        }
        if (other.gameObject.CompareTag("Linha Fundo G2") && !LogisticaVars.posGol)
        {
            LogisticaVars.foraFundo = true;
            LogisticaVars.fundo1 = false;
            LogisticaVars.fundo2 = true;
            LogisticaVars.m_rbJogadorEscolhido.velocity = Vector3.zero;

            if (LogisticaVars.ultimoToque == 1) //Tiro de Meta
            {
                print("Tiro de Meta");
                if (transform.position.x < 0) m_posicaoFundo = FindObjectOfType<DimensaoCampo>().PosicaoBolaTiroDeMeta(2, false);
                else m_posicaoFundo = FindObjectOfType<DimensaoCampo>().PosicaoBolaTiroDeMeta(2, true);
                transform.position = m_posicaoFundo;

                EventsManager.current.SituacaoGameplay("fora tiro de meta");
                StartCoroutine(RotinasGameplay.SpawnarTiroDeMeta("fundo 2", this));
            } 
            else //Escanteio
            {
                print("Escanteio");
                if (transform.position.x < 0) 
                {
                    GameObject.Find("Bandeira Esq 2").transform.GetChild(0).gameObject.SetActive(false);
                    m_posicaoFundo = GameObject.Find("Escanteio Dir -x").transform.position;
                    transform.position = m_posicaoFundo;

                    //dar uma distancia para uma cobranca sem problemas
                    LogisticaVars.m_jogadorEscolhido.transform.position += new Vector3(LogisticaVars.m_jogadorEscolhido.transform.position.x + 2,
                            LogisticaVars.m_jogadorEscolhido.transform.position.y, LogisticaVars.m_jogadorEscolhido.transform.position.z - 2);
                }
                else
                {
                    GameObject.Find("Bandeira Dir 2").transform.GetChild(0).gameObject.SetActive(false);
                    m_posicaoFundo = GameObject.Find("Escanteio Dir +x").transform.position;
                    transform.position = m_posicaoFundo;

                    //dar uma distancia para uma cobranca sem problemas
                    LogisticaVars.m_jogadorEscolhido.transform.position += new Vector3(LogisticaVars.m_jogadorEscolhido.transform.position.x - 2,
                            LogisticaVars.m_jogadorEscolhido.transform.position.y, LogisticaVars.m_jogadorEscolhido.transform.position.z - 2);
                }

                EventsManager.current.SituacaoGameplay("fora escanteio");
                StartCoroutine(RotinasGameplay.SpawnarEscanteio("fundo 2", this));
            }
            m_rbBola.freezeRotation = true;
            m_rbBola.velocity = Vector3.zero;
        }
        #endregion
        #region Gol
        if (other.CompareTag("Gol1"))
        {
            print("Gol");
            LogisticaVars.gol = true;
            LogisticaVars.golT2 = true;
            EventsManager.current.SituacaoGameplay("gol marcado");
            //PosicionarAposGol();
        }
        if (other.CompareTag("Gol2"))
        {
            print("Gol");
            LogisticaVars.gol = true;
            LogisticaVars.golT1 = true;
            EventsManager.current.SituacaoGameplay("gol marcado");
            //PosicionarAposGol();
        }
        #endregion
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PequenaArea1") && !LogisticaVars.posGol)
        {
            if (tempoNaPequenaArea > 2.5f && !LogisticaVars.bolaPermaneceNaPequenaArea && LogisticaVars.bolaEntrouPequenaArea)
            {
                GameplayOff.BolaNaPequenaArea(1);
                EventsManager.current.OnAplicarRotinas("rotina tempo pequena area");
                LogisticaVars.bolaPermaneceNaPequenaArea = true;
                LogisticaVars.bolaEntrouPequenaArea = false;
            }
        }
        if (other.CompareTag("PequenaArea2") && !LogisticaVars.posGol)
        {
            if (tempoNaPequenaArea > 2.5f && !LogisticaVars.bolaPermaneceNaPequenaArea && LogisticaVars.bolaEntrouPequenaArea)
            {
                GameplayOff.BolaNaPequenaArea(2);
                EventsManager.current.OnAplicarRotinas("rotina tempo pequena area");
                LogisticaVars.bolaPermaneceNaPequenaArea = true;
                LogisticaVars.bolaEntrouPequenaArea = false;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PequenaArea1") || other.CompareTag("PequenaArea2"))
        {
            m_contagemPequenaArea = false;
            LogisticaVars.bolaEntrouPequenaArea = false;
            print("Saiu da Pequena Area");
            if (!LogisticaVars.tiroDeMeta && LogisticaVars.bolaPermaneceNaPequenaArea)
            {
                tempoNaPequenaArea = 0;
            }
            else tempoNaPequenaArea = 0;
        }
    }
}