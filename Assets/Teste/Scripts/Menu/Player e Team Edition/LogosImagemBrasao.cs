using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogosImagemBrasao : ImagemsInterativas
{
    public Logos m_logo;
    bool disponivel;

    public override void SetarSlots()
    {
        Player p = GameManager.Instance.GetComponent<Player>();
        if (m_logo != null)
        {
            if (p.m_level >= m_logo.m_levelDesbloquear && p.m_recompensasLevel[m_logo.m_levelDesbloquear - 1] == 1)
            {
                if (m_logo.m_levelDesbloquear == 0) transform.GetChild(0).GetComponent<Image>().sprite = m_logo.m_locked;
                else
                {
                    transform.GetChild(0).GetComponent<Image>().sprite = m_logo.m_tipo1;
                    transform.GetChild(0).GetComponent<Image>().color = Color.white;
                }
                disponivel = true;
            }
            else
            {
                transform.GetChild(0).GetComponent<Image>().sprite = m_logo.m_locked;
                Color c = Color.white;
                c.a = 0.7f;
                transform.GetChild(0).GetComponent<Image>().color = c;
                disponivel = false;
            }
            
        }
        else gameObject.SetActive(false);
    }

    public bool getDisponivel()
    {
        return disponivel;
    }
}
