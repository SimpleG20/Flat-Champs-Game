using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class PrimeirasConfigs : MonoBehaviour
{
    private bool configs;

    [SerializeField] GameObject m_cena;
    [SerializeField] TMP_InputField m_nomeUsuario, m_nomeTime, m_abreviacao;
    [SerializeField] Button m_start;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("FIRSTTIMEOPENING", 1) == 1)
        { 
            print("Seja Bem vindo ao jogo");
            FindObjectOfType<MenuScenesManager>().BotoesMenuInterativos(false);
            m_cena.SetActive(true);
            m_nomeTime.characterLimit = m_nomeUsuario.characterLimit = 20;
            m_abreviacao.characterLimit = 3;
            configs = true;
            PlayerPrefs.SetInt("FIRSTTIMEOPENING", 0);
        }
        else
        {
            SaveSystem.CarregarPlayer();
            SaveSystem.CarregarMenu();
            SaveSystem.CarregarConfiguration();
            if (GameManager.Instance.m_usuario.m_timeNome == "" || 
                GameManager.Instance.m_usuario.m_abreviacao == "" || GameManager.Instance.m_usuario.m_username == "")
            {
                FindObjectOfType<MenuScenesManager>().BotoesMenuInterativos(false);
                m_cena.SetActive(true);
                m_nomeTime.characterLimit = m_nomeUsuario.characterLimit = 20;
                m_abreviacao.characterLimit = 3;
                configs = true;
            }
            else
            {
                configs = false;
                m_cena.SetActive(false);
                print("Bem vindo de volta");
                enabled = false;
            }
        }
    }

    void Update()
    {
        if (configs)
        {
            m_nomeUsuario.text = Regex.Replace(m_nomeUsuario.text, @"[^a-zA-Z0-9 _-]", "");
            m_nomeTime.text = Regex.Replace(m_nomeTime.text, @"[^a-zA-Z ]", "");
            m_abreviacao.text = Regex.Replace(m_abreviacao.text, @"[^a-zA-Z ]", "");

            if (m_nomeTime.text.Length < 3 || m_nomeTime.text.Length < 3 || m_abreviacao.text.Length < 3) m_start.interactable = false;

            if (m_nomeTime.text.Length >= 3 && m_nomeUsuario.text.Length >= 3 && m_abreviacao.text.Length == 3) m_start.interactable = true;
        }
    }

    public void VerificarSeExisteNome()
    {

    }

    public void SubmeterValores()
    {
        if (m_start.interactable)
        {
            GameManager.Instance.m_usuario.m_timeNome = m_nomeTime.text;
            GameManager.Instance.m_usuario.m_username = m_nomeUsuario.text;
            GameManager.Instance.m_usuario.m_abreviacao = m_abreviacao.text.ToUpper();

            SaveSystem.SavePlayer(GameManager.Instance.m_usuario);
            SaveSystem.CarregarPlayer();
            SaveSystem.CarregarMenu();
            FindObjectOfType<StatsManager>().SetarComponentes();

            m_cena.SetActive(false);
            configs = false;

            FindObjectOfType<MenuScenesManager>().BotoesMenuInterativos(true);
            FindObjectOfType<LogoManager>().m_timeNome.text = m_nomeTime.text;
        }
    }
}
