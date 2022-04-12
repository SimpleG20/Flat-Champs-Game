using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MudarSpriteBotaoUI : MonoBehaviour
{
    [SerializeField] List<GameObject> uis;
    [SerializeField] Sprite corBranca;
    [SerializeField] Sprite corPreta;

    void Start()
    {
        if (GameManager.Instance.m_config.m_corGameplayCustom == 0)
        {
            GetComponent<Image>().sprite = corBranca;
            Color c = Color.black;

            if (uis != null)
            { 
                foreach (GameObject g in uis)
                {
                    if (g.GetComponent<TextMeshProUGUI>() != null) g.GetComponent<TextMeshProUGUI>().color = c;
                    else g.GetComponent<Image>().color = c;
                }
            }
        }
        else
        {
            GetComponent<Image>().sprite = corPreta;
            Color c = Color.white;
            
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
