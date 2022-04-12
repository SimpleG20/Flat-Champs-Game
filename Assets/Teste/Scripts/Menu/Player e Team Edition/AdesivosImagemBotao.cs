using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AdesivosImagemBotao : ImagemsInterativas
{
    public Adesivos m_adesivo;
    bool disponivel;
    public bool m_podeColorir = false;

    public override void SetarSlots()
    {
        Player p = GameManager.Instance.GetComponent<Player>();
        if (m_adesivo != null)
        {
            if (p.m_level >= m_adesivo.m_levelDesbloquear && p.m_recompensasLevel[m_adesivo.m_levelDesbloquear - 1] == 1)
            {
                if (m_adesivo.m_levelDesbloquear == 0) transform.GetChild(0).GetComponent<Image>().sprite = m_adesivo.m_locked;
                else transform.GetChild(0).GetComponent<Image>().sprite = m_adesivo.m_imagem;

                transform.GetChild(0).localPosition = Vector3.up * 15;
                transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(m_adesivo.m_tamanhoNoSlot, m_adesivo.m_tamanhoNoSlot);
                transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_adesivo.nome;
                transform.GetChild(0).GetComponent<Image>().color = Color.white;
                disponivel = true;
            }
            else
            {
                transform.GetChild(0).GetComponent<Image>().sprite = m_adesivo.m_locked;
                transform.GetChild(0).localPosition = Vector3.zero;
                transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(100, 100);
                transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "LOCKED";
                Color c = Color.white;
                c.a = 0.7f;
                transform.GetChild(0).GetComponent<Image>().color = c;
                disponivel = false;
            }

            if (m_adesivo.m_cod == FindObjectOfType<PlayerEditionManager>().m_adesivoEscolhido) transform.GetChild(2).gameObject.SetActive(true);
            else transform.GetChild(2).gameObject.SetActive(false);
            m_podeColorir = m_adesivo.m_colorir;
            CorSlot(m_adesivo.raridade);
        }
        else gameObject.SetActive(false);
    }

    public bool getDisponivel()
    {
        return disponivel;
    }

    void CorSlot(Adesivos.Raridade r)
    {
        Color c1 = Color.white;
        switch (r)
        {
            case Adesivos.Raridade.Verde:
                ColorUtility.TryParseHtmlString("#16AE00", out c1);
                break;
            case Adesivos.Raridade.Azul:
                ColorUtility.TryParseHtmlString("#16AEAE", out c1);
                break;
            case Adesivos.Raridade.Roxo:
                ColorUtility.TryParseHtmlString("#D634EC", out c1);
                break;
            case Adesivos.Raridade.Amarelo:
                ColorUtility.TryParseHtmlString("#ECBB34", out c1);
                break;
            case Adesivos.Raridade.Laranja:
                ColorUtility.TryParseHtmlString("#FF7E3C", out c1);
                break;
        }
        c1.a = 0.5f;
        GetComponent<Image>().color = c1;
    }

}
