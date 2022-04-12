using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcaAtrito : MonoBehaviour
{

    public float velocidadeinicial, velocidadeAtual;

    private Vector3 vetorForcaResistente;
    private Vector3 vetorForcaFat, vetorforcaPeso, vetorforcaNormal;
    private Vector3 vetorVelocidade, vetorVelocidadeInicial, vetorVelocidadeNormalizado;

    public bool rasteira;
    private bool p;
    public bool m_correndo, noChao;
    private Rigidbody m_rb;
    FisicaJogador jg;

    // Start is called before the first frame update
    void Start()
    {
        jg = FindObjectOfType<FisicaJogador>();
        m_rb = GetComponent<Rigidbody>();

        vetorforcaPeso = Vector3.down * 9.81f * m_rb.mass;
        vetorforcaNormal = -vetorforcaPeso;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        vetorVelocidade = m_rb.velocity;
        velocidadeAtual = m_rb.velocity.magnitude;

        if (rasteira) m_rb.constraints = RigidbodyConstraints.FreezePositionY;
        else m_rb.constraints = RigidbodyConstraints.None;

        #region Fisica
        if (transform.position.y > 0.6f)
        {
            //m_rb.AddForce(vetorforcaPeso, ForceMode.Force);
            noChao = false;
        }
        else noChao = true;

        if (noChao)
        {
            if (m_rb.velocity.magnitude < 0.05f)
            {
                vetorVelocidadeNormalizado = new Vector3(0, 0, 0);
            }
            else
            {
                vetorVelocidadeNormalizado = new Vector3(-m_rb.velocity.x / m_rb.velocity.magnitude, 0, -m_rb.velocity.z / m_rb.velocity.magnitude);
            }

            if (m_correndo) vetorForcaFat = new Vector3(AtributosFisicos.coefAtritoDiBola * vetorforcaNormal.y, 0, AtributosFisicos.coefAtritoDiBola * vetorforcaNormal.y);
            else vetorForcaFat = new Vector3(AtributosFisicos.coefAtritoEsBola * vetorforcaNormal.y, 0, AtributosFisicos.coefAtritoEsBola * vetorforcaNormal.y);

            vetorForcaResistente = new Vector3(vetorForcaFat.x * vetorVelocidadeNormalizado.x, 0, vetorForcaFat.z * vetorVelocidadeNormalizado.z);


            #region Resistencia com base no rb.velocity
            if (vetorForcaResistente.magnitude != 0) m_correndo = true;
            else m_correndo = false;

            if (m_correndo)
            {
                m_rb.AddForce(vetorForcaResistente, ForceMode.Force);
                p = false;
            }
            else
            {
                if (!p)
                {
                    print("Forca Resultante nula");
                    m_rb.velocity = Vector3.zero;
                    p = true;
                }
            }
            #endregion

        }
        #endregion
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            float e = 0.5f;

            float velocidade = e * (collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude);

            float m_quantidadeDeMovimentoBola = (velocidade * m_rb.mass);

           // if(rasteira) m_rb.AddForce(direcaoBola.normalized * m_quantidadeDeMovimentoBola * velocidade / 2, ForceMode.Impulse);
        }
    }
}
