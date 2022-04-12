using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecompensaLevel : MonoBehaviour
{
    public enum TipoRecompensa { Cor, Botao, Adesivo, Textura, CorETextura, CorEAdesivo, AdesivoETextura }

    [SerializeField] int level;
    [SerializeField] TipoRecompensa tipo;
    [SerializeField] bool disponivel, pegouRecompensa;


    public void SituacaoSlot()
    {
        LevelManager manager = FindObjectOfType<LevelManager>();
        transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = level.ToString();

        if(GameManager.Instance.m_usuario.m_level >= level) transform.GetChild(2).gameObject.SetActive(false);
        else transform.GetChild(2).gameObject.SetActive(true);

        switch (tipo)
        {
            case TipoRecompensa.Cor:
                transform.GetChild(1).GetComponent<Image>().sprite = manager.iconeCor;
                transform.GetChild(1).GetComponent<Image>().SetNativeSize();
                break;
            case TipoRecompensa.Adesivo:
                transform.GetChild(1).GetComponent<Image>().sprite = manager.iconeAdesivo;
                transform.GetChild(1).GetComponent<Image>().SetNativeSize();
                break;
            case TipoRecompensa.Botao:
                transform.GetChild(1).GetComponent<Image>().sprite = manager.RetornarBotaoSprite(level);
                transform.GetChild(1).GetComponent<Image>().rectTransform.sizeDelta = Vector2.one * 220;
                break;
            case TipoRecompensa.CorEAdesivo:
                transform.GetChild(1).GetComponent<Image>().sprite = manager.iconeCorAdesivo;
                transform.GetChild(1).GetComponent<Image>().SetNativeSize();
                break;
            case TipoRecompensa.CorETextura:
                transform.GetChild(1).GetComponent<Image>().sprite = manager.iconeCorTextura;
                transform.GetChild(1).GetComponent<Image>().SetNativeSize();
                break;
            case TipoRecompensa.Textura:
                transform.GetChild(1).GetComponent<Image>().sprite = manager.iconeTextura;
                transform.GetChild(1).GetComponent<Image>().SetNativeSize();
                break;
            case TipoRecompensa.AdesivoETextura:
                transform.GetChild(1).GetComponent<Image>().sprite = manager.iconeAdesivoTextura;
                transform.GetChild(1).GetComponent<Image>().SetNativeSize();
                break;
        }

        if (pegouRecompensa) GetComponent<CanvasGroup>().alpha = 0.5f;
        else GetComponent<CanvasGroup>().alpha = 1;
    }

    public void SetPegouRecompensa(bool b)
    {
        pegouRecompensa = b;
    }
    public void SetDisponivel(bool b)
    {
        disponivel = b;
    }
    public void Setlevel(int i)
    {
        level = i;
    }
    public int GetLevel()
    {
        return level;
    }
    public bool GetDisponivel()
    {
        return disponivel;
    }
    public bool GetPegouRecompensa()
    {
        return pegouRecompensa;
    }
}
