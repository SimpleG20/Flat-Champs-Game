using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuScenesManager : MonoBehaviour
{
    [Header("Animators")]
    [SerializeField] Animator m_canvas;
    [SerializeField] Animator m_playerBotao, m_mainCamera, m_playerGoleiro;

    [Header("Cenas")]
    [SerializeField] GameObject cenaMenu;
    [SerializeField] GameObject cenaJogador, cenaTime, cenaLevel, cenaConfig, cenaReward, cenaNews, cenaModoJogo, cenaOffline, cenaOnline, cenaExtra, letreiro;

    [Header("Botoes Menu")]
    [SerializeField] GameObject m_singlePlayer_Button;
    [SerializeField] GameObject m_multiPlayer_Button, m_extraXp_Button, m_dailyQuests_Button;

    public string cenaAtual;

    private void Start()
    {
        EventsManager.current.onClickMenu += OnClickMenu;
        cenaAtual = "Menu";
        m_canvas.enabled = false;;
    }

    private void OnClickMenu(string s)
    {
        switch (s)
        {
            case "cena extra":
                PlusCena();
                break;
            case "cena jogador":
                PlayerEditionCena();
                break;
            case "cena time":
                TimeEditionCena();
                break;
            case "cena singleplay":
                SingleplayerCena();
                break;
            case "cena multiplay":
                MultiplayerCena();
                break;
            case "cena sair modo jogo":
                SairModoDeJogo();
                break;
            case "configuracoes":
                ConfiguracoesCena();
                break;
            case "cena level":
                LevelCena();
                break;
            case "cena menu":
                MenuCena();
                break;
            case "sair jogo":
                SairJogo();
                break;
        }
    }

    public void BotoesMenuInterativos(bool b)
    {
        foreach (GameObject button in GameObject.FindGameObjectsWithTag("Botao do Menu"))
        {
            button.GetComponentInChildren<Button>().interactable = b;
            if (b == false)
            {
                if (button.GetComponentInChildren<TextMeshProUGUI>() != null) button.GetComponentInChildren<TextMeshProUGUI>().alpha = 0.5f;
            }
            else
            {
                if (button.GetComponentInChildren<TextMeshProUGUI>() != null) button.GetComponentInChildren<TextMeshProUGUI>().alpha = 1f;
            }
        }
    }

    #region Cenas
    void PlusCena()
    {
        BotoesMenuInterativos(false);
        m_canvas.GetComponent<Animator>().enabled = true;
        m_canvas.SetBool("PlusScene", true);
        m_playerBotao.SetBool("PlusScene", true);
        cenaAtual = "Extra";
    }
    void PlayerEditionCena()
    {
        m_canvas.GetComponent<Animator>().enabled = true;
        m_playerGoleiro.SetBool("PlayerEdition", true);
        m_mainCamera.SetBool("PlayerEdition", true);
        m_playerBotao.SetBool("Entrar Edition", true);
        m_canvas.SetBool("PlayerEdition", true);
        StartCoroutine(EsperarAnimacaoParaPlayer(m_playerBotao.GetCurrentAnimatorStateInfo(0).length * 2.2f));
    }
    void TimeEditionCena()
    {
        letreiro.SetActive(false);
        cenaMenu.SetActive(false);
        cenaTime.SetActive(true);
        cenaAtual = "Time Edition";
    }
    void MenuCena()
    {
        if (cenaAtual == "Extra")
        {
            m_canvas.SetBool("PlusScene", false);
            m_playerBotao.SetBool("PlusScene", false);
            //SaveSystem.CarregarData();
            StartCoroutine(EsperarAnimacaoParaMenu(m_canvas.GetCurrentAnimatorStateInfo(0).length));
        }
        else if (cenaAtual == "Jogador Edition")
        {
            m_canvas.GetComponent<Animator>().enabled = true;
            m_canvas.enabled = true;
            m_playerBotao.enabled = true;
            m_canvas.SetBool("PlayerEdition", false);
            m_playerBotao.SetBool("Entrar Edition", false);
            m_mainCamera.SetBool("PlayerEdition", false);
            m_playerGoleiro.SetBool("PlayerEdition", false);
            //SaveSystem.CarregarData();
            StartCoroutine(EsperarAnimacaoParaMenu(m_canvas.GetCurrentAnimatorStateInfo(0).length));
        }
        else
        {
            cenaConfig.SetActive(false);
            cenaLevel.SetActive(false);
            cenaTime.SetActive(false);
            cenaNews.SetActive(false);
            cenaModoJogo.SetActive(false);
            //cenaReward.SetActive(false);
            cenaMenu.SetActive(true);
            letreiro.SetActive(true);
            BotoesMenuInterativos(true);
            //SaveSystem.CarregarData();
            cenaAtual = "Menu";
        }
        
    }
    void SairModoDeJogo()
    {
        MenuCena();
    }
    void SingleplayerCena()
    {
        BotoesMenuInterativos(false);
        cenaModoJogo.SetActive(true);
        cenaOffline.SetActive(true);
    }
    void MultiplayerCena()
    {
        if (GameManager.Instance.InternetConnectivity())
        {
            BotoesMenuInterativos(false);
            cenaModoJogo.SetActive(true);
            cenaOnline.SetActive(true);
        }        
    }
    void ConfiguracoesCena()
    {
        GameManager.Instance.m_configurationManager.SetarVariaveis();
        cenaConfig.SetActive(true);
        cenaAtual = "Configuracoes";
    }
    void LevelCena()
    {
        //GameManager.Instance.m_levelManager.SetarVariaveis();
        cenaLevel.SetActive(true);
        GameManager.Instance.m_levelManager.IniciarSlots();
        cenaAtual = "Level";
    }
    #endregion

    #region Modos de Jogo ON e OFF

    public void Offline_Classico(bool b)
    {
        if (b) GameManager.Instance.JogoOff_JogadorJogador(1);
        else GameManager.Instance.JogoOff_JogadorAi(1);
        LoadManager.Instance.CenaEstadio();
    }
    public void Offline_Quick(bool b)
    {
        if (b) GameManager.Instance.JogoOff_JogadorJogador(2);
        else GameManager.Instance.JogoOff_JogadorAi(2);
        LoadManager.Instance.CenaEstadio();
    }
    public void Offline_Versus3(bool b)
    {
        if (b) GameManager.Instance.JogoOff_JogadorJogador(3);
        else GameManager.Instance.JogoOff_JogadorAi(3);
        LoadManager.Instance.CenaEstadio();
    }
    public void Offline_Versus1()
    {
        GameManager.Instance.JogoOff_JogadorJogador(4);
        LoadManager.Instance.CenaEstadio();
    }

    public void Online_Partida(string tipo)
    {
        switch (tipo)
        {
            case "CLASSICO":
                break;
            case "QUICK":
                break;
            case "VERSUS3 TEAM":
                break;
            case "VERSUS3 RANDOM":
                break;
            case "VERSUS2":
                break;
            case "VERSUS1":
                break;
        }
    }
    #endregion

    public void SalvarDataTemporaria()
    {
        SaveSystem.SavePlayer(GameManager.Instance.m_usuario);
        SaveSystem.SaveConfigurations(GameManager.Instance.m_config);
    }
    public void CarregarDataTemporaria()
    {
        print("Carregando datas");
        SaveSystem.CarregarPlayer();
        SaveSystem.CarregarMenu();
        SaveSystem.CarregarConfiguration();
    }
    public void FirstTimeOpening()
    {
        GameManager.Instance.SimularPrimeiraEntrada();
    }

    public void SairJogo()
    {
        //GameManager.Instance.SalvarData();
        //GameManager.Instance.SalvarConfiguracoes();
        Application.Quit();
    }//Tirar comentarios de dentro quando acabar

    IEnumerator EsperarAnimacaoParaMenu(float f)
    {
        print("Esperando voltar para o menu");
        yield return new WaitForSeconds(f);
        BotoesMenuInterativos(true);
        m_canvas.enabled = false;
        cenaAtual = "Menu";
    }
    IEnumerator EsperarAnimacaoParaPlayer(float f)
    {
        print("Esperando cena Player Edition");
        yield return new WaitForSeconds(f + 0.5f);
        cenaAtual = "Jogador Edition";
        m_playerBotao.enabled = false;
    }

}
