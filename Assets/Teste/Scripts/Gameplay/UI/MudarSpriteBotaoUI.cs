using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MudarSpriteBotaoUI : MonoBehaviour
{
    [SerializeField] float alpha;
    [SerializeField] List<GameObject> uis;
    [SerializeField] Sprite corBranca;
    [SerializeField] Sprite corPreta;

    void Start()
    {
        if (GameManager.Instance.m_config.m_corGameplayCustom == 0)
        {
            Color c = Color.white;
            c.a = alpha;
            GetComponent<Image>().sprite = corBranca;
            GetComponent<Image>().color = c;
            

            if (uis != null)
            {
                c = Color.black;
                c.a = alpha;

                foreach (GameObject g in uis)
                {
                    if (g.GetComponent<TextMeshProUGUI>() != null) g.GetComponent<TextMeshProUGUI>().color = c;
                    else g.GetComponent<Image>().color = c;
                }
            }
        }
        else
        {
            Color c = Color.white;
            c.a = alpha;
            GetComponent<Image>().sprite = corPreta;
            GetComponent<Image>().color = c;
            
            if(uis != null)
            { 
                foreach (GameObject g in uis)
                {
                    if (g.GetComponent<TextMeshProUGUI>() != null) g.GetComponent<TextMeshProUGUI>().color = c;
                    else g.GetComponent<Image>().color = c;
                }
            }
            
        }
    }
}
