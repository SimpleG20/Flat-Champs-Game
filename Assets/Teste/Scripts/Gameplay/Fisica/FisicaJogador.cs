using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FisicaJogador : MonoBehaviour
{
    public Vector3 seno;

    public float posicao;
    public float angulo;
    public float m_velocidadeJogador, m_energiaCinetica, m_distanciaPercorrida, m_aceleracao;
    public bool m_correndo, m_podeVirar;
    public Rigidbody m_rigidbody;

    private Vector3 vetorVelocidadeNormalizado, vetorForcaResistente, vetorForcaFat, vetorforcaNormal, vetorForcaPeso;
    private bool p;

    void Start()
    {
        m_rigidbody = GetComponentInChildren<Rigidbody>();

        vetorForcaPeso = Vector3.down * 9.81f * m_rigidbody.mass;
        vetorforcaNormal = -vetorForcaPeso;
    }

    void Update()
    {
        if (gameObject.layer == 8) seno = transform.up;
        else seno = -transform.up;

        #region Dados fisicos
        if (m_rigidbody.velocity.magnitude < 0.1f)
        {
            vetorVelocidadeNormalizado = new Vector3(0, 0, 0);
        }
        else
        {
            vetorVelocidadeNormalizado = new Vector3(-m_rigidbody.velocity.x / m_rigidbody.velocity.magnitude, 0, -m_rigidbody.velocity.z / m_rigidbody.velocity.magnitude);
        }

        if (vetorForcaResistente.magnitude != 0) m_correndo = true;
        else m_correndo = false;

        if (m_correndo) vetorForcaFat = new Vector3(AtributosFisicos.coefAtritoDiJogador * vetorforcaNormal.y, 0, AtributosFisicos.coefAtritoDiJogador * vetorforcaNormal.y);
        else vetorForcaFat = new Vector3(AtributosFisicos.coefAtritoDiJogador * vetorforcaNormal.y, 0, AtributosFisicos.coefAtritoDiJogador * vetorforcaNormal.y);

        vetorForcaResistente = new Vector3(vetorForcaFat.x * vetorVelocidadeNormalizado.x, 0, vetorForcaFat.z * vetorVelocidadeNormalizado.z);
        #endregion

        #region Forca Atrito
        m_velocidadeJogador = m_rigidbody.velocity.magnitude;

        if (m_correndo)
        {
            m_podeVirar = false;
            m_rigidbody.AddForce(vetorForcaResistente, ForceMode.Force);
            p = false;
        }
        else
        {
            if (!p)
            {
                /*if (LogisticaVars.redirecionamentoAutomatico) FindObjectOfType<FisicaBola>().RedirecionarJogadorEscolhido(FindObjectOfType<FisicaBola>().transform);*/
                p = true;
                m_podeVirar = true;
            }
        }
        #endregion
    }

    private void OnCollisionEnter(Collision collision)
    {
        #region Caso outros botoes Colidam com o Player
        if (collision.gameObject.CompareTag("Player Selecionado"))
        {
            if (collision.gameObject.layer != gameObject.layer)
            {
                //print("Bate neles mesmo!!");
                if (LogisticaVars.vezJ1) LogisticaVars.m_especialAtualT1 += 10;
                else LogisticaVars.m_especialAtualT2 += 10;
                print("Mais Especial!!");
            }
            else print("Batida entre amigos");

            Vector3 vetorDirecao;
            float quantidadeMovimento, velocidadeImpacto;

            vetorDirecao = collision.gameObject.transform.position - gameObject.transform.position;
            velocidadeImpacto = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            //print("Velocidade Impacto Jogador: " + velocidadeImpacto);
            quantidadeMovimento = velocidadeImpacto * m_rigidbody.mass;

            collision.gameObject.GetComponent<Rigidbody>().AddForce(vetorDirecao.normalized * quantidadeMovimento * 1.5f, ForceMode.Impulse);
        }
        #endregion

        #region Caso o Player colida com outros botoes
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 vetorDirecao;
            float quantidadeMovimento, velocidadeImpacto;

            vetorDirecao = collision.gameObject.transform.position - gameObject.transform.position;
            velocidadeImpacto = m_rigidbody.velocity.magnitude;
            //print("Velocidade Impacto Botao: " + velocidadeImpacto);
            quantidadeMovimento = velocidadeImpacto * collision.gameObject.GetComponent<Rigidbody>().mass;

            collision.gameObject.GetComponent<Rigidbody>().AddForce(vetorDirecao.normalized * quantidadeMovimento / 1.25f , ForceMode.Impulse);
        }
        #endregion

        /*if (collision.gameObject.CompareTag("Bola"))
        {
            m_rigidbody.AddForce(vetorVelocidadeNormalizado * 90, ForceMode.Impulse);
        }*/
    }
}
