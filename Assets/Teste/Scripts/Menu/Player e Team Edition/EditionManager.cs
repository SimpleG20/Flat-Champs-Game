using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EditionManager : MonoBehaviour
{
    [SerializeField] protected string secao;
    public TextMeshProUGUI textoSecao;

    [SerializeField] protected List<Material> m_cores;
    protected LogoManager m_logoManager;
    protected Player m_usuario;

    protected GameObject m_playerMenu;
    protected GameObject m_goleiroMenu;

    [SerializeField] protected GraphicRaycaster m_Raycaster;
    [SerializeField] protected PointerEventData m_PointerEventData;
    [SerializeField] protected EventSystem m_EventSystem;

    private void Awake()
    {
        m_logoManager = FindObjectOfType<LogoManager>();
        m_usuario = GameManager.Instance.GetComponent<Player>();
        m_Raycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
        m_EventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        m_playerMenu = GameObject.Find("Player Botao");
        m_goleiroMenu = GameObject.Find("Player Goleiro");
    }

    public virtual void MudarSecoes(string s)
    {
        print("Mudou Secao");
    }
}
