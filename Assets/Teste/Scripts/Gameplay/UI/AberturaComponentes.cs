using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AberturaComponentes : MonoBehaviour
{
    [SerializeField] Image parteDireita, parteEsquerda;
    [SerializeField] TextMeshProUGUI m_time1, m_time2;
    [SerializeField] Image m_baseT1, m_fundoT1, m_simboloT1, m_baseT2, m_fundoT2, m_simboloT2;


    public void SetarComponentesOff()
    {
        /*Color c;
        int hue = Random.Range(0, 100);
        hue = hue / 100;
        c = Color.HSVToRGB(hue, 0.75f, 0.8f);
        FindObjectOfType<UIMetodosGameplay>().golMarcadoGO.GetComponent<GolComponentes>().SetarCor(c);
        parteDireita.color = parteEsquerda.color = c;*/

        Player a = GameManager.Instance.m_usuario;
        Teams b = GameManager.Instance.GetTimeOff();

        m_time1.text = a.m_timeNome;
        m_time2.text = b.m_nomeTime;

        m_baseT1.sprite= a.m_baseLogo;
        m_fundoT1.sprite = a.m_fundoLogo;
        m_simboloT1.sprite = a.m_simboloLogo;
        m_baseT1.color = a.m_corSecundaria;
        m_fundoT1.color = a.m_corPrimaria;
        m_simboloT1.color = a.m_corTerciaria;

        m_baseT2.sprite = b.m_base;
        m_fundoT2.sprite = b.m_fundo;
        m_simboloT2.sprite = b.m_simbolo;
        m_baseT2.color = b.m_corSecundaria;
        m_fundoT2.color = b.m_corPrimaria;
        m_simboloT2.color = b.m_corTerciaria;
    }

    public void SetarComponentesOn()
    {

    }
}
