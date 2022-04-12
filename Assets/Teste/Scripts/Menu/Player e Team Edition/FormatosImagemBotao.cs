using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FormatosImagemBotao : ImagemsInterativas
{
    public PlayerButton m_botao;
    bool disponivel;

    public override void SetarSlots()
    {
        if (m_botao == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Player p = GameManager.Instance.GetComponent<Player>();
            if (p.m_level >= m_botao.m_levelDesbloquear && p.m_recompensasLevel[m_botao.m_levelDesbloquear - 1] == 1)
            {
                transform.GetChild(0).GetComponent<Image>().sprite = m_botao.m_imagem;
                transform.GetChild(0).GetComponent<Image>().color = Color.white;

                if (transform.childCount >= 2)
                {
                    transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = m_botao.nome;
                    transform.GetChild(0).localPosition = Vector3.up * 15;
                    transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(188, 188);
                } // Slots Normais
                else transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(150, 150); //Slots no Team Custom

                disponivel = true;
            }
            else
            {
                transform.GetChild(0).GetComponent<Image>().sprite = m_botao.m_locked;
                transform.GetChild(0).localPosition = Vector3.zero;
                transform.GetChild(0).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(100, 100);
                Color c = Color.white;
                c.a = 0.7f;
                transform.GetChild(0).GetComponent<Image>().color = c;
                
                if (transform.childCount >= 2) transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "LOCKED";
                disponivel = false;
            }
            

            #region Verificar se o Objeto ja estava escolhido
            if (transform.childCount > 2)
            {
                PlayerEditionManager pEM = FindObjectOfType<PlayerEditionManager>();

                if (m_botao.tipoBotao == PlayerButton.TipoBotao.Jogador)
                {
                    if (m_botao.cod == pEM.m_codBotaoEscolhido) transform.GetChild(2).gameObject.SetActive(true);
                    else transform.GetChild(2).gameObject.SetActive(false);
                }
                else
                {
                    if (m_botao.cod == pEM.m_codGoleiroEscolhido) transform.GetChild(2).gameObject.SetActive(true);
                    else transform.GetChild(2).gameObject.SetActive(false);
                }
            }
            #endregion

            CorSlot(m_botao.raridade);
        }
    }

    public bool getDisponivel()
    {
        return disponivel;
    }

    void CorSlot( PlayerButton.Raridade r)
    {
        float H, S, V;
        Color c1 = Color.white;
        switch (r)
        {
            case PlayerButton.Raridade.Verde:
                ColorUtility.TryParseHtmlString("#16AE00", out c1);
                break;
            case PlayerButton.Raridade.Azul:
                ColorUtility.TryParseHtmlString("#16AEAE", out c1);
                break;
            case PlayerButton.Raridade.Roxo:
                ColorUtility.TryParseHtmlString("#D634EC", out c1);
                break;
            case PlayerButton.Raridade.Amarelo:
                ColorUtility.TryParseHtmlString("#ECBB34", out c1);
                break;
            case PlayerButton.Raridade.Laranja:
                ColorUtility.TryParseHtmlString("#FF7E3C", out c1);
                break;
        }

        c1.a = 0.9f;
        gameObject.GetComponent<Image>().color = c1;
        c1.a = 1;
        Color.RGBToHSV(c1, out H, out S, out V);
        V = 1;
        c1 = Color.HSVToRGB(H, S, V);

        if(transform.childCount > 2)
        {
            if (transform.GetChild(1).gameObject != null) transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = c1;
            if (transform.GetChild(2).gameObject != null) transform.GetChild(2).GetComponent<Image>().color = c1;
        }
    }

}
