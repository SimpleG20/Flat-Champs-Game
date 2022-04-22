using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject botaoLevelMenu, botaoPegarRecompensas;
    [SerializeField] List<GameObject> slots;

    [SerializeField] TextMeshProUGUI levelTx;
    [SerializeField] Slider xpSlider;
    [SerializeField] Scrollbar scrollbar;
    [SerializeField] ScrollRect scroll;

    [SerializeField] public Sprite iconeCor, iconeAdesivo, iconeTextura, iconeAdesivoTextura, iconeCorAdesivo, iconeCorTextura;

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    // Start is called before the first frame update
    void Start()
    {
        m_Raycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
        m_EventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("Slot Recompensa") && 
                    result.gameObject.GetComponent<RecompensaLevel>().GetLevel() <= GameManager.Instance.m_usuario.m_level &&
                    GameManager.Instance.m_usuario.m_recompensasLevel[result.gameObject.GetComponent<RecompensaLevel>().GetLevel()-1] == 0) 
                    ReceberRecompensa(result.gameObject);
            }
        }
    }


    void ReceberRecompensa(GameObject slot)
    {
        GameManager.Instance.m_usuario.m_recompensasLevel[slot.GetComponent<RecompensaLevel>().GetLevel() - 1] = 1;
        slot.GetComponent<RecompensaLevel>().SetPegouRecompensa(true);
        slot.GetComponent<CanvasGroup>().alpha = 0.3f;
        //print("Recompensa Pega");
    }
    public void PegarVariasRecompensas()
    {
        float posPrimeiroSlot = 0;

        foreach(GameObject s in slots)
        {
            if(s.GetComponent<RecompensaLevel>().GetDisponivel() && !s.GetComponent<RecompensaLevel>().GetPegouRecompensa())
            {
                ReceberRecompensa(s);
            }
            else
            {
                posPrimeiroSlot = s.transform.position.x;
                break;
            }
        }

        if (posPrimeiroSlot > 2000)
            scrollbar.value = posPrimeiroSlot / scroll.content.sizeDelta.x;
        else scrollbar.value = 0;
        botaoPegarRecompensas.SetActive(false);
    }


    public Sprite RetornarBotaoSprite(int level)
    {
        PlayerButton p;
        switch (level)
        {
            case 10:
                p = Resources.Load("Testes/Scriptable Objects/Botoes/Tipo 3") as PlayerButton;
                return p.m_imagem;
            case 20:
                p = Resources.Load("Testes/Scriptable Objects/Botoes/Tipo 4") as PlayerButton;
                return p.m_imagem;
            case 30:
                p = Resources.Load("Testes/Scriptable Objects/Botoes/Tipo 5") as PlayerButton;
                return p.m_imagem;
            case 40:
                p = Resources.Load("Testes/Scriptable Objects/Botoes/Tipo 6") as PlayerButton;
                return p.m_imagem;
            case 50:
                p = Resources.Load("Testes/Scriptable Objects/Botoes/Tipo 7") as PlayerButton;
                return p.m_imagem;
            case 60:
                p = Resources.Load("Testes/Scriptable Objects/Botoes/Tipo 8") as PlayerButton;
                return p.m_imagem;
            case 75:
                p = Resources.Load("Testes/Scriptable Objects/Botoes/Tipo 9") as PlayerButton;
                return p.m_imagem;
            default:
                return null;
        }
    }


    public void SetarVariaveis()
    {
        levelTx.text = GameManager.Instance.m_usuario.m_level.ToString();
        xpSlider.minValue = GameManager.Instance.m_usuario.m_xpReferenciaAnterior;
        xpSlider.maxValue = GameManager.Instance.m_usuario.m_xpReferencia;
        xpSlider.value = GameManager.Instance.m_usuario.m_xp;
        SetarSlots();
        ChecarNovidade();
    }
    void SetarSlots()
    {
        //Toda vez aparece na recompensa que nao pegou

        foreach (GameObject s in slots)
        {
            if (s.GetComponent<RecompensaLevel>().GetLevel() <= GameManager.Instance.m_usuario.m_level) s.GetComponent<RecompensaLevel>().SetDisponivel(true);
            else s.GetComponent<RecompensaLevel>().SetDisponivel(false);

            if (GameManager.Instance.m_usuario.m_recompensasLevel[s.GetComponent<RecompensaLevel>().GetLevel() - 1] == 1)
                s.GetComponent<RecompensaLevel>().SetPegouRecompensa(true);
            else s.GetComponent<RecompensaLevel>().SetPegouRecompensa(false);

            if (s.GetComponent<RecompensaLevel>().GetDisponivel() && !s.GetComponent<RecompensaLevel>().GetPegouRecompensa())
                botaoLevelMenu.transform.GetChild(2).gameObject.SetActive(true);
        }
    }
    public void IniciarSlots()
    {
        bool pegouPrimeiroSlot = false;
        float posPrimeiroSlot = 0;
        int numeroRecompensas = 0;

        SetarSlots();

        foreach(GameObject s in slots)
        {
            if (!s.GetComponent<RecompensaLevel>().GetDisponivel())
            {
                if (!pegouPrimeiroSlot) 
                {
                    posPrimeiroSlot = s.transform.position.x; 
                    pegouPrimeiroSlot = true; 
                }
            }
            else
            {
                if (!s.GetComponent<RecompensaLevel>().GetPegouRecompensa())
                {
                    if (!pegouPrimeiroSlot) { posPrimeiroSlot = s.transform.position.x; pegouPrimeiroSlot = true; }
                    //botaoLevelMenu.transform.GetChild(1).gameObject.SetActive(true);
                    numeroRecompensas++;
                }
            }
            s.GetComponent<RecompensaLevel>().SituacaoSlot();
        }

        if (numeroRecompensas >= 5) botaoPegarRecompensas.SetActive(true);

        //print(posPrimeiroSlot / FindObjectOfType<ScrollRect>().content.sizeDelta.x);

        if (posPrimeiroSlot > 2000)
            scrollbar.value = posPrimeiroSlot / scroll.content.sizeDelta.x;
        else scrollbar.value = 0;
    }
    void ChecarNovidade()
    {
        int i = 1;
        bool b = false;
        foreach (GameObject s in slots)
        {
            i++;
            if (s.GetComponent<RecompensaLevel>().GetDisponivel() && !s.GetComponent<RecompensaLevel>().GetPegouRecompensa())
            {
                print("Novidade Slot Level " + i);
                b = true;
                break;
            }
        }
        botaoLevelMenu.transform.GetChild(2).gameObject.SetActive(b);
    }

    public void SairCenaLevel()
    {
        //SaveSystem.SavePlayer(GameManager.Instance.m_usuario);
        scrollbar.value = 0;
        ChecarNovidade();
        FindObjectOfType<EventsManager>().ClickInFirstScene("cena menu");
    }
}
