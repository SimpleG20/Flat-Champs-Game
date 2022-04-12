using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsManager : MonoBehaviour
{
    [Header("Stats Usuario")]
    [SerializeField] GameObject m_nomeUsuario;
    [SerializeField] GameObject m_nomeTime, m_vitorias, m_empates, m_derrtoas, m_level, m_xpBar;

    [Header("Player Data")]
    [SerializeField] Player m_usuario;

    public int m_xpReferencia, m_xpReferenciaAnterior;
    public int m_vitoriaAtual, m_derrotasAtual, m_empatesAtual, m_levelAtual, m_xpAtual;

    private void Awake()
    {
        m_usuario = GameManager.Instance.GetComponent<Player>();
    }
    void Start()
    {
        InicialzarComponentes();
    }

    public void InicialzarComponentes()
    {
        m_nomeUsuario.GetComponent<TextMeshProUGUI>().text = m_usuario.m_username;
        m_nomeTime.GetComponent<TextMeshProUGUI>().text = m_usuario.m_timeNome;
        m_vitorias.GetComponent<TextMeshProUGUI>().text = m_usuario.m_vitorias.ToString();
        //m_vitoriaAtual = m_usuario.m_vitorias;
        m_derrtoas.GetComponent<TextMeshProUGUI>().text = m_usuario.m_derrotas.ToString();
        //m_derrotasAtual = m_usuario.m_derrotas;
        m_empates.GetComponent<TextMeshProUGUI>().text = m_usuario.m_empates.ToString();
        //m_empatesAtual = m_usuario.m_empates;
        m_level.GetComponent<TextMeshProUGUI>().text = m_usuario.m_level.ToString();
        //m_levelAtual = m_usuario.m_level;

        m_xpBar.GetComponent<Slider>().minValue = m_usuario.m_xpReferenciaAnterior;
        m_xpBar.GetComponent<Slider>().maxValue = m_usuario.m_xpReferencia;
        m_xpBar.GetComponent<Slider>().value = m_usuario.m_xp;
        m_xpReferenciaAnterior = m_usuario.m_xpReferenciaAnterior;
        m_xpReferencia = m_usuario.m_xpReferencia;
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            print("Jogadas");
            m_usuario.m_vitorias += 20;
            m_usuario.m_derrotas += 10;
            m_usuario.m_empates += 5;
            AtualizarStats(m_usuario.m_vitorias, m_usuario.m_derrotas, m_usuario.m_empates);
        }
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            print("Ganhando XP");
            m_xpAtual = m_usuario.m_xp + 250; 
            AtualizarLevel(m_xpAtual); 
        }*/

    }

    public void AtualizarStats(int v, int d, int e) //os parametros recebem os dados do usuario atualizado
    {
        m_vitoriaAtual = v;
        m_derrotasAtual = d;
        m_empatesAtual = e;
        m_vitorias.GetComponent<TextMeshProUGUI>().text = m_vitoriaAtual.ToString();
        m_derrtoas.GetComponent<TextMeshProUGUI>().text = m_derrotasAtual.ToString();
        m_empates.GetComponent<TextMeshProUGUI>().text = m_empatesAtual.ToString();
    }

    public void AtualizarLevel(int xp)
    {
        m_xpReferenciaAnterior = m_usuario.m_xpReferenciaAnterior;
        m_xpReferencia = m_usuario.m_xpReferencia;

        m_xpBar.GetComponent<Slider>().minValue = m_xpReferenciaAnterior;
        m_xpBar.GetComponent<Slider>().maxValue = m_xpReferencia;

        m_level.GetComponent<TextMeshProUGUI>().text = m_usuario.m_level.ToString();

        m_usuario.m_xp = xp;
        m_xpBar.GetComponent<Slider>().value = m_usuario.m_xp;

        m_usuario.m_xpReferenciaAnterior = m_xpReferenciaAnterior;
        m_usuario.m_xpReferencia = m_xpReferencia;

        if (m_usuario.m_xp >= m_xpBar.GetComponent<Slider>().maxValue)
        {
            m_usuario.m_level++;
            m_levelAtual = m_usuario.m_level;
            SubirLevel();
        }
    }
    void SubirLevel()
    {
        m_xpReferenciaAnterior = m_xpReferencia;
        m_xpReferencia += Mathf.FloorToInt(m_xpReferencia * 1.15f);
        m_xpBar.GetComponent<Slider>().minValue = m_xpReferenciaAnterior;
        m_xpBar.GetComponent<Slider>().maxValue = m_xpReferencia;
        m_level.GetComponent<TextMeshProUGUI>().text = m_usuario.m_level.ToString();
        print("PARABÉNS VOCÊ SUBIU DE NIVEL!!!!");
        m_usuario.m_xpReferenciaAnterior = m_xpReferenciaAnterior;
        m_usuario.m_xpReferencia = m_xpReferencia;
    }
}
