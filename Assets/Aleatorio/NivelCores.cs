using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NivelCores : ImagemsInterativas
{
    public int m_tipoCor;
    public int m_codigoCor;
    public int m_levelDesbloquear;
    bool disponivel;

    public override void SetarSlots()
    {
        PlayerEditionManager pM = FindObjectOfType<PlayerEditionManager>();
        if (GameManager.Instance.m_usuario.m_level >= m_levelDesbloquear && GameManager.Instance.m_usuario.m_recompensasLevel[m_levelDesbloquear - 1] == 1)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            switch (m_tipoCor)
            {
                case 1:
                    if (m_codigoCor == pM.m_cor1Escolhida) transform.GetChild(1).gameObject.SetActive(true);
                    else transform.GetChild(1).gameObject.SetActive(false);
                    break;
                case 2:
                    if (m_codigoCor == pM.m_cor2Escolhida) transform.GetChild(1).gameObject.SetActive(true);
                    else transform.GetChild(1).gameObject.SetActive(false);
                    break;
                case 3:
                    if (m_codigoCor == pM.m_cor3Escolhida) transform.GetChild(1).gameObject.SetActive(true);
                    else transform.GetChild(1).gameObject.SetActive(false);
                    break;
            }
            disponivel = true;
        }
        else { transform.GetChild(0).gameObject.SetActive(true); disponivel = false; }
    }

    public bool getDisponivel()
    {
        return disponivel;
    }
}
