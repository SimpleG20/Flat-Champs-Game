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

    public int m_vitoriaAtual, m_derrotasAtual, m_empatesAtual, m_levelAtual;
    public float m_xpReferencia, m_xpReferenciaAnterior, m_xpAtual;

    private void Awake()
    {
        m_usuario = GameManager.Instance.GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(AumentarBarraXP(0, 1000));
        }
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

        m_xpReferenciaAnterior = TabelaLevel.getMinXPLevel(m_usuario.m_level);
        m_xpReferencia = TabelaLevel.getMaxXPLevel(m_usuario.m_level);

        m_xpBar.GetComponent<Slider>().minValue = m_xpReferenciaAnterior;
        m_xpBar.GetComponent<Slider>().maxValue = m_xpReferencia;
        m_xpBar.GetComponent<Slider>().value = m_usuario.m_xp;
        
        m_xpAtual = m_usuario.m_xp;
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

    public void AtualizarLevel(float xp)
    {
        m_xpAtual = m_usuario.m_xp = xp;
        m_xpBar.GetComponent<Slider>().value = m_usuario.m_xp;

        if (m_usuario.m_xp >= m_xpReferencia) SubirLevel();
    }
    void SubirLevel()
    {
        m_usuario.m_level++;
        m_usuario.m_xpReferenciaAnterior = m_xpReferenciaAnterior = TabelaLevel.getMinXPLevel(m_usuario.m_level);
        m_usuario.m_xpReferencia = m_xpReferencia = TabelaLevel.getMaxXPLevel(m_usuario.m_level);

        m_xpBar.GetComponent<Slider>().minValue = m_xpReferenciaAnterior;
        m_xpBar.GetComponent<Slider>().maxValue = m_xpReferencia;
        m_level.GetComponent<TextMeshProUGUI>().text = m_usuario.m_level.ToString();
        m_levelAtual = m_usuario.m_level;
        print("PARABÉNS VOCÊ SUBIU DE NIVEL!!!!");

        //Aparecer uma imagem
    }

    public IEnumerator AumentarBarraXP(float xpAnterior, float xp)
    {
        yield return new WaitForSeconds(0.01f);
        if(m_xpAtual <= xpAnterior + xp)
        {
            print("Aumentando XP BAR");
            m_xpBar.GetComponent<Slider>().value += 10;
            AtualizarLevel(m_xpBar.GetComponent<Slider>().value);
            StartCoroutine(AumentarBarraXP(xpAnterior, xp));
        }
        else
        {
            if (m_xpBar.GetComponent<Slider>().value > xpAnterior + xp) m_xpBar.GetComponent<Slider>().value = xpAnterior + xp;
            GameManager.Instance.m_usuario.m_xpPendente_qnt = 0;
            GameManager.Instance.m_usuario.m_onPendendente_XP = false;
            GameManager.Instance.m_sceneManager.BotoesMenuInterativos(true);
        }
    }
}
