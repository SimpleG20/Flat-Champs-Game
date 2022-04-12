using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AtualizarMedidas : MonoBehaviour
{
    public int minCod;

    public List<GameObject> slots;
    //[SerializeField] float quantidadeX, alturaSlot, larguraSlot;
    //[SerializeField] float extraW, extraH;
    //public int ultimoAtivo;
    //public float height, width;

    //[SerializeField] SlotsManager managerSlot;

    [ContextMenu("Nomear Slots")]
    public void NomearSlots()
    {
        for(int i = 0; i < slots.Count; i++)
        {
            slots[i].name = "Slot Cor " + (i + 1);
        }
    }

    [ContextMenu("Aplicar Codigo nas Cores")]
    void ColocarIdentificacao()
    {
        foreach (GameObject s in slots) 
        {
            s.GetComponent<NivelCores>().m_codigoCor = minCod;
            minCod++;
        }
    }

    [ContextMenu("Cor Slot Reward")]
    void SlotReward()
    {
        foreach(GameObject s in slots)
        {
            Color c1;
            ColorUtility.TryParseHtmlString("#FF9900", out c1);
            s.transform.GetChild(0).GetComponent<Image>().color = c1;
            if(s.GetComponent<RecompensaLevel>().GetLevel() < 15)
            {
                ColorUtility.TryParseHtmlString("#16AE00", out c1);
                s.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = c1;
            }
            else if(s.GetComponent<RecompensaLevel>().GetLevel() >= 15 && s.GetComponent<RecompensaLevel>().GetLevel() < 30)
            {
                ColorUtility.TryParseHtmlString("#16AEAE", out c1);
                s.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = c1;
            }
            else if (s.GetComponent<RecompensaLevel>().GetLevel() >= 30 && s.GetComponent<RecompensaLevel>().GetLevel() < 45)
            {
                ColorUtility.TryParseHtmlString("#D634EC", out c1);
                s.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = c1;
            }
            else if (s.GetComponent<RecompensaLevel>().GetLevel() >= 45 && s.GetComponent<RecompensaLevel>().GetLevel() < 60)
            {
                ColorUtility.TryParseHtmlString("#ECBB34", out c1);
                s.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = c1;
            }
            else
            {
                ColorUtility.TryParseHtmlString("#FF7E3C", out c1);
                s.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = c1;
            }
        }
    }

    #region Cores dos Slots de Cor
    [ContextMenu("Adicionar Cor nos Slots de Cor")]
    public void SlotsCor()
    {
        int contagem = 0;
        float H = 0, S = 0.43f, V = 1;
        
        foreach(GameObject s in slots)
        {
            s.GetComponent<Image>().color = Color.HSVToRGB(H, S, V);
            H += 0.09f;
            contagem++;
            if (contagem % 10 == 0) { S += 0.17f; V -= 0.12f; H = 0; }
        }
    }
    [ContextMenu("Cores Especiais")]
    void SlotsCorEspeciais()
    {
        int contagem = 0;
        float H = 0, S = 0f, V = 1;

        foreach (GameObject s in slots)
        {
            s.GetComponent<Image>().color = Color.HSVToRGB(H, S, V);
            if(contagem < 5)
            {
                V -= 0.2f;
            }
            else
            {
                if (contagem > 5) H += 0.08f;
                else H = 0;
                S = 1;
                V = 1;
            }
            contagem++;
        }
    }
    #endregion

    /*public void AtualizarMedida()
    {
        int i = 1;
        foreach(GameObject slot in slots)
        {
            if (slot == null || !slot.gameObject.activeSelf)
            {
                //print(slot.name);
                break;
            }
            else i++;
        }

        ultimoAtivo = i;

        if(ultimoAtivo > quantidadeX) width = quantidadeX * larguraSlot;
        else width = (ultimoAtivo) * larguraSlot;
        width += extraW;

        height = Mathf.Ceil((ultimoAtivo-1) / quantidadeX) * alturaSlot + extraH;

        /*if(FindObjectOfType<Scrollbar>() != null)
        {
            if (managerSlot.scroll.horizontal) managerSlot.scroll.horizontalScrollbar = FindObjectOfType<Scrollbar>();
            else managerSlot.scroll.verticalScrollbar = FindObjectOfType<Scrollbar>();
            FindObjectOfType<Scrollbar>().size = 0.1f;
        }
    }*/

}
