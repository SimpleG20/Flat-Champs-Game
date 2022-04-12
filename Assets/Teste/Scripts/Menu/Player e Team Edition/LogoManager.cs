using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogoManager : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] Image m_baseStats;
    [SerializeField] Image m_fundoStats, m_simboloStats, m_bordaStats;
    [SerializeField] public List<GameObject> m_estrelasStats;

    [Header("Brasao")]
    [SerializeField] public Image m_base;
    [SerializeField] public Image m_fundo, m_simbolo, m_borda;
    [SerializeField] public List<GameObject> m_estrelas;

    [Header("Player Data")]
    [SerializeField] Player m_usuario;

    [Header("Outros")]
    [SerializeField] public TextMeshProUGUI m_timeNome;
    [SerializeField] public int m_tipoLogo;

    private void Awake()
    {
        m_usuario = GameManager.Instance.GetComponent<Player>();
    }

    private void Start()
    {
        m_timeNome.text = m_usuario.m_timeNome;
    }
    public void MudarSpriteBase(Sprite s)
    {
        m_base.sprite = s;
    }
    public void MudarSpriteFundo(Sprite s)
    {
        m_fundo.sprite = s;
    }
    public void MudarSpriteSimbolo(Sprite s)
    {
        m_simbolo.sprite = s;
    }
    public void MudarSpriteBorda(Sprite s)
    {
        m_borda.sprite = s;
    }
    public void MudarTipoBase(int i)
    {
        m_tipoLogo = i;
        //m_usuario.m_tipoBaseLogo = i;
    }
    public void MudarSpriteStats(Sprite b, Sprite fundo, Sprite simbolo, Sprite borda)
    {
        m_baseStats.sprite = b;
        m_fundoStats.sprite = fundo;
        m_simboloStats.sprite = simbolo;
        m_bordaStats.sprite = borda;
        if (GameManager.Instance.m_usuario.m_usarEstrelas)
        {
            foreach (GameObject e in m_estrelasStats) e.GetComponent<Estrelas>().EstadoEstrelas();
        }
        else foreach (GameObject e in m_estrelasStats) e.SetActive(false);
    }
    public void AtualizarLogoStats()
    {
        MudarSpriteStats(m_usuario.m_baseLogo, m_usuario.m_fundoLogo, m_usuario.m_simboloLogo, m_usuario.m_bordaLogo);
        
    }
    public void AtualizarCor()
    {
        m_base.color = m_baseStats.color = m_usuario.m_corSecundaria;
        m_fundo.color = m_fundoStats.color = m_usuario.m_corPrimaria;
        m_simbolo.color = m_simboloStats.color = m_usuario.m_corTerciaria;
    }
}
