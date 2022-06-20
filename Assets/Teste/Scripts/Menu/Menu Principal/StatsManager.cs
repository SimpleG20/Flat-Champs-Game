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
    Slider xpBar_slider;

    [Header("Player Data")]
    [SerializeField] Player m_usuario;

    public int m_vitoriaAtual, m_derrotasAtual, m_empatesAtual, m_levelAtual;
    public float m_xpReferencia, m_xpReferenciaAnterior, m_xpAtual;

    bool aumentouNivel = false;

    private void Awake()
    {
        m_usuario = GameManager.Instance.m_usuario;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) StartCoroutine(PreencherBarra(m_xpAtual, 500));
        if (Input.GetKeyDown(KeyCode.N)) aumentouNivel = false;
    }

    public void SetarComponentes()
    {
        m_nomeUsuario.GetComponent<TextMeshProUGUI>().text = m_usuario.m_username;
        m_nomeTime.GetComponent<TextMeshProUGUI>().text = m_usuario.m_timeNome;
        m_vitorias.GetComponent<TextMeshProUGUI>().text = m_usuario.m_vitorias.ToString();
        //m_vitoriaAtual = m_usuario.m_vitorias;
        m_derrtoas.GetComponent<TextMeshProUGUI>().text = m_usuario.m_derrotas.ToString();
        //m_derrotasAtual = m_usuario.m_derrotas;
        m_empates.GetComponent<TextMeshProUGUI>().text = m_usuario.m_empates.ToString();
        //m_empatesAtual = m_usuario.m_empates;
        m_level.GetComponent<TextMeshProUGUI>().text = m_usuario.m_level < 10 ? "0" + m_usuario.m_level.ToString() : m_usuario.m_level.ToString();
        //m_levelAtual = m_usuario.m_level;

        m_xpReferenciaAnterior = TabelaLevel.getMinXPLevel(m_usuario.m_level);
        m_xpReferencia = TabelaLevel.getMaxXPLevel(m_usuario.m_level);

        xpBar_slider = m_xpBar.GetComponent<Slider>();
        xpBar_slider.minValue = m_xpReferenciaAnterior;
        xpBar_slider.maxValue = m_xpReferencia;
        xpBar_slider.value = m_usuario.m_xp;
        
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

        if (GameManager.Instance.getOnAumentarXP())
        {
            AumentarBarraXP(m_usuario.m_xp, GameManager.Instance.getXP_ParaAumentar());
            GameManager.Instance.m_partida = null;
        }
    }

    
    void AtualizarLevel(float xpFinal)
    {
        m_xpAtual = m_usuario.m_xp = xpBar_slider.value = xpFinal;
        m_usuario.m_xpPendente_qnt = 0;
        m_usuario.m_onPendendente_XP = false;
        GameManager.Instance.setAumentarXP(false);
        GameManager.Instance.setQntXP(0);
    }
    void SubirLevel()
    {
        aumentouNivel = true;
        m_usuario.m_level++;
        m_usuario.m_xpReferenciaAnterior = m_xpReferenciaAnterior = TabelaLevel.getMinXPLevel(m_usuario.m_level);
        m_usuario.m_xpReferencia = m_xpReferencia = TabelaLevel.getMaxXPLevel(m_usuario.m_level);

        m_xpBar.GetComponent<Slider>().minValue = m_xpReferenciaAnterior;
        m_xpBar.GetComponent<Slider>().maxValue = m_xpReferencia;
        m_level.GetComponent<TextMeshProUGUI>().text = m_usuario.m_level < 10 ? "0" + m_usuario.m_level.ToString() : m_usuario.m_level.ToString();
        m_levelAtual = m_usuario.m_level;
        print("PARABÉNS VOCÊ SUBIU DE NIVEL!!!!");
        //Aparecer uma imagem
    }

    void AumentarBarraXP(float xpAnterior, float xp)
    {
        float xpFinal = xpAnterior + xp;
        StartCoroutine(PreencherBarra(xpAnterior, xpFinal));
    }

    IEnumerator PreencherBarra(float inicial, float final)
    {
        //float timeTween = 3 * (final - inicial) / m_xpReferencia;
        yield return new WaitUntil(() => !aumentouNivel);
        yield return new WaitForSeconds(0.01f);
        float velocidade = (m_xpReferencia - m_xpReferenciaAnterior) / 2.5f;
        xpBar_slider.value += (velocidade * 0.035f);
        if (xpBar_slider.value >= m_xpReferencia) SubirLevel();
        if (xpBar_slider.value >= final) AtualizarLevel(final);
        else StartCoroutine(PreencherBarra(inicial, final));
    }
}
