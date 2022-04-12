using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class ConfigurationManager : MonoBehaviour
{
    [Header("Customization")]
    [SerializeField] GameObject m_visualicao;
    [SerializeField] List<GameObject> m_cores, m_customizadosPlayer;
    [SerializeField] List<GameObject> m_customizadosTeam, m_customizadosMenu, m_customizadosConfig, m_configPretoBranco,
        m_menuPretoBranco, m_timePretoBranco, m_jogadorPretoBranco;
    [SerializeField] List<Toggle> m_corToggles, m_corTogglesGameplay;
    [SerializeField] int numCorMenu, numCorJogador, numCorTime, numCorConfig, numCorGameplay, cenaAtual;
    Color corMenu, corJogador, corTime, corConfig, corPretoBranco;

    [SerializeField] List<Sprite> m_ilustracoesMenu, m_ilustracoesJogador, m_ilustracoesTime, m_ilustracoesConfig, m_ilustracoesGameplay;

    [Header("Camera")]
    [SerializeField] GameObject m_jogadorTeste;
    [SerializeField] Toggle toggleSensibilidade;
    [SerializeField] TMP_InputField m_camSensiblidadeInput;
    [SerializeField] TMP_InputField m_camPosYInput;
    [SerializeField] TMP_InputField m_camAnguloInput;

    [Header("Sons")]
    [SerializeField] Slider m_somTorcidaSlider;
    [SerializeField] Slider m_somInterfaceSlider, m_somMusicasSlider, m_somEfeitosSlider;
    [SerializeField] TextMeshProUGUI m_torcidaInput, m_interfaceInput, m_musicaInput, m_efetioInput;

    [Header("Grafico")]
    [SerializeField] List<Toggle> m_resolucoesToggles;

    [Header("Linguas")]
    [SerializeField] List<Toggle> m_idiomasToggles;

    [Header("Demonstração Camera")]
    [SerializeField] GameObject m_pivotCam;

    [Header("Gameplay")]
    [SerializeField] Image barraChute;
    

    //[Header("Datas")]
    Configuration config;

    [Header("Outros")]
    [SerializeField] GameObject setaProx;
    [SerializeField] GameObject setaAnt;

    int m_camSensibilidade;
    float m_camPosY, m_camAngulo, m_velocidadeBarra;

    int m_volumeTorcida, m_volumeInterface, m_volumeMusica, m_volumeEfeito;
    public int m_idiomaAtual, m_resolucaoAtual;
    bool m_mudouCor, m_aplicarCor, testarBarra, testarSensibilidade;

    

    //bool m_altaRes, m_mediaRes, m_baixaRes, m_lngPt, m_lngEng, m_lngEsp, m_lngFra,

    private void Awake()
    {
        config = GameManager.Instance.GetComponent<Configuration>();
    }

    private void Start()
    {
        //SetarVariaveis();
        m_camSensiblidadeInput.text = Regex.Replace(m_camSensiblidadeInput.text, @"[^0-9 .]", "");
        m_camAnguloInput.text = Regex.Replace(m_camAnguloInput.text, @"[^0-9 .]", "");
        m_camPosYInput.text = Regex.Replace(m_camPosYInput.text, @"[^0-9 .]", "");
    }

    public void Default()
    {
        #region Camera
        m_camPosYInput.GetComponent<TMP_InputField>().text = "0";
        m_camSensiblidadeInput.GetComponent<TMP_InputField>().text = "50";
        m_camAnguloInput.GetComponent<TMP_InputField>().text = "0";

        m_pivotCam.transform.localPosition = m_pivotCam.transform.eulerAngles = Vector3.zero;
        m_camAngulo = m_camPosY = 0;
        m_camSensibilidade = 50;
        #endregion

        #region Som
        m_somTorcidaSlider.value = m_volumeTorcida = 50;
        m_somMusicasSlider.value = m_volumeMusica = 50;
        m_somInterfaceSlider.value = m_volumeInterface = 50;
        m_somEfeitosSlider.value = m_volumeEfeito = 50;

        m_torcidaInput.text = m_volumeTorcida.ToString();
        m_interfaceInput.text = m_volumeInterface.ToString();
        m_musicaInput.text = m_volumeMusica.ToString();
        m_efetioInput.text = m_volumeEfeito.ToString();
        #endregion

        #region grafico
        m_resolucaoAtual = 2;
        //m_mediaRes = true;
        //m_baixaRes = m_altaRes = false;
        AtualizarToggles(m_resolucaoAtual, m_resolucoesToggles);
        #endregion

        #region Idioma
        m_idiomaAtual = 1;
        //m_lngEng = true;
        //m_lngPt = m_lngEsp = m_lngFra = false;
        AtualizarToggles(m_idiomaAtual, m_idiomasToggles);
        #endregion

        #region Custom
        numCorMenu = numCorJogador = numCorConfig = numCorTime = 2;
        numCorGameplay = 0;
        AtualizarToggles(2, m_corToggles);
        AtualizarToggles(0, m_corTogglesGameplay);
        //m_visualicao.transform.GetChild(0).GetComponent<Image>().sprite = m_ilustracoesMenu[2];
        //m_visualicao.transform.GetChild(1).GetComponent<Image>().sprite = m_ilustracoesJogador[2];
        //m_visualicao.transform.GetChild(2).GetComponent<Image>().sprite = m_ilustracoesTime[2];
        //m_visualicao.transform.GetChild(3).GetComponent<Image>().sprite = m_ilustracoesConfig[2];
        //m_visualicao.transform.GetChild(4).GetComponent<Image>().sprite = m_ilustracoesGameplay[2];
        m_mudouCor = true;
        #endregion

        #region Gameplay

        #endregion
    }

    private void Update()
    {
        if(GameManager.Instance.m_sceneManager.cenaAtual == "configuracoes")
        {
            if (testarBarra) barraChute.fillAmount += ((12 * m_velocidadeBarra * Time.deltaTime) / 360);
            if (testarSensibilidade) m_jogadorTeste.transform.Rotate(Vector3.forward * 1 * m_camSensibilidade * Time.deltaTime);
        }
    }

    #region Testes
    public void TestarSensibilidade()
    {
        if (toggleSensibilidade.isOn) testarSensibilidade = true;
        else { testarSensibilidade = false; m_jogadorTeste.transform.eulerAngles = new Vector3(-90, 0, 0); }
    }
    public void TestarBarraChute()
    {
        testarBarra = true;
    }
    public void DefaultBarra()
    {
        testarBarra = false;
        barraChute.fillAmount = 0.2f;
    }
    #endregion

    public void MudarValorVelocidadeBarra(Slider slider)
    {
        m_velocidadeBarra = slider.value;
    }
    public void ValorVelocidadeBarra(TextMeshProUGUI text)
    {
        text.text = m_velocidadeBarra.ToString();
    }
    public void MudarPagina(bool b)
    {
        setaAnt.SetActive(true);
        setaProx.SetActive(true);

        if(b == true)
        {
            cenaAtual++;
            if (cenaAtual >= 5) setaProx.SetActive(false);
        }
        else
        {
            cenaAtual--;
            if (cenaAtual <= 1) setaAnt.SetActive(false);
        }

        if (cenaAtual == 1)
        {
            m_visualicao.GetComponent<Image>().sprite = m_ilustracoesMenu[numCorMenu];
            AtualizarToggles(numCorMenu, m_corToggles);
        }
        else if (cenaAtual == 2)
        {
            m_visualicao.GetComponent<Image>().sprite = m_ilustracoesJogador[numCorJogador];
            AtualizarToggles(numCorJogador, m_corToggles);
        }
        else if (cenaAtual == 3)
        {
            m_visualicao.GetComponent<Image>().sprite = m_ilustracoesTime[numCorTime];
            AtualizarToggles(numCorTime, m_corToggles);
        }
        else if (cenaAtual == 4)
        {
            m_visualicao.GetComponent<Image>().sprite = m_ilustracoesConfig[numCorConfig];

            foreach (Toggle t in m_corToggles) t.gameObject.SetActive(true);
            foreach (Toggle t2 in m_corTogglesGameplay) t2.gameObject.SetActive(false);

            AtualizarToggles(numCorConfig, m_corToggles);
        }
        else
        {
            m_visualicao.GetComponent<Image>().sprite = m_ilustracoesGameplay[numCorGameplay];
            foreach (Toggle t in m_corToggles) t.gameObject.SetActive(false);
            foreach (Toggle t2 in m_corTogglesGameplay) t2.gameObject.SetActive(true);
            AtualizarToggles(numCorGameplay, m_corTogglesGameplay);
        }
    }

    #region Toggles
    void AtualizarToggles(int i, List<Toggle> lista)
    {
        foreach (Toggle t in lista)
        {
            if (t.name == lista[i].name) { t.isOn = true; break; }
        }
    }
    #endregion

    void DemostracaoCamera(int i)
    {
        if(i == 1) m_pivotCam.transform.localPosition = Vector3.up * m_camPosY;
        else m_pivotCam.transform.eulerAngles = Vector3.right * m_camAngulo;
    }
    public void MaximoMinimoValorCamera(int i)
    {
        switch (i)
        {
            case 1:
                if (int.Parse(m_camSensiblidadeInput.text) >= 100)
                    m_camSensiblidadeInput.text = "100";
                else if (int.Parse(m_camSensiblidadeInput.text) <= 0)
                    m_camSensiblidadeInput.text = "0";
                break;
            case 2:
                if (float.Parse(m_camPosYInput.text) >= 2f)
                    m_camPosYInput.text = "2";
                else if (float.Parse(m_camPosYInput.text) <= -1f)
                    m_camPosYInput.text = "-1";
                break;
            case 3:
                if (float.Parse(m_camAnguloInput.text) >= 7.5f)
                    m_camAnguloInput.text = "7,5";
                else if (float.Parse(m_camAnguloInput.text) <= -20f)
                    m_camAnguloInput.text = "-20";
                break;
        }
    }

    #region Setar Valores
    public void MudarCameraConfig(int i)
    {
        switch (i)
        {
            case 1:
                m_camSensibilidade = int.Parse(m_camSensiblidadeInput.text);
                break;
            case 2:
                m_camPosY = float.Parse(m_camPosYInput.text);
                DemostracaoCamera(i - 1);
                break;
            case 3:
                m_camAngulo = float.Parse(m_camAnguloInput.text);
                DemostracaoCamera(i - 1);
                break;
        }
    }
    public void MudarSomConfig(int i)
    {
        switch (i)
        {
            case 1:
                m_volumeTorcida = int.Parse(m_somTorcidaSlider.value.ToString());
                m_torcidaInput.text = m_volumeTorcida.ToString();
                break;
            case 2:
                m_volumeInterface = int.Parse(m_somInterfaceSlider.value.ToString());
                m_interfaceInput.text = m_volumeInterface.ToString();
                break;
            case 3:
                m_volumeMusica = int.Parse(m_somMusicasSlider.value.ToString());
                m_musicaInput.text = m_volumeMusica.ToString();
                break;
            case 4:
                m_volumeEfeito = int.Parse(m_somEfeitosSlider.value.ToString());
                m_efetioInput.text = m_volumeEfeito.ToString();
                break;
        }
    } //Colorcar os sons
    public void MudarResolucao(int i)
    {
        m_resolucaoAtual = i;
        /*switch (i)
        {
            case 1:
                m_baixaRes = true;
                m_altaRes = m_mediaRes = false;
                
                break;
            case 2:
                m_mediaRes = true;
                m_baixaRes = m_altaRes = false;
                break;
            case 3:
                m_altaRes = true;
                m_baixaRes = m_mediaRes = false;
                break;
        }
        */
        //AtualizarTogglesResolucao(i);
    } //Mudar de fato as resolucoes
    public void MudarIdioma(int i)
    {
        m_idiomaAtual = i;
        /*switch (i)
        {
            case 1:
                m_lngEng = true;
                m_lngPt = m_lngEsp = m_lngFra = false;
                break;
            case 2:
                m_lngPt = true;
                m_lngEsp = m_lngEng = m_lngFra = false;
                break;
            case 3:
                m_lngEsp = true;
                m_lngPt = m_lngEng = m_lngFra = false;
                break;
            case 4:
                m_lngFra = true;
                m_lngPt = m_lngEng = m_lngEsp = false;
                break;
        }*/
        //AtualizarTogglesIdiomas(i);
    } //Colocar os idiomas
    public void MudarCor(int i)
    {
        // 0 - Branco
        // 1 - Amarelo
        // 2 - Laranja
        // 3 - Vermelho
        // 4 - Rosa
        // 5 - Roxo
        // 6 - Azul
        // 7 - Verde
        // 8 - Neon
        foreach(Toggle t in m_corToggles)
        {
            if (t.isOn)
            {
                switch (cenaAtual)
                {
                    case 1:
                        corMenu = m_cores[i].GetComponent<Image>().color;
                        numCorMenu = i;
                        m_visualicao.GetComponent<Image>().sprite = m_ilustracoesMenu[numCorMenu];
                        break;
                    case 2:
                        corJogador = m_cores[i].GetComponent<Image>().color;
                        numCorJogador = i;
                        m_visualicao.GetComponent<Image>().sprite = m_ilustracoesJogador[numCorJogador];
                        break;
                    case 3:
                        corTime = m_cores[i].GetComponent<Image>().color;
                        numCorTime = i;
                        m_visualicao.GetComponent<Image>().sprite = m_ilustracoesTime[numCorTime];
                        break;
                    case 4:
                        corConfig = m_cores[i].GetComponent<Image>().color;
                        numCorConfig = i;
                        m_visualicao.GetComponent<Image>().sprite = m_ilustracoesConfig[numCorConfig];
                        break;
                }
                break;
            }
        }
        m_mudouCor = true;
        AtualizarToggles(i, m_corToggles);
    }
    public void MudarCorGameplay(int i)
    {
        foreach(Toggle t in m_corTogglesGameplay)
        {
            if (t.isOn)
            {
                numCorGameplay = i;
                m_visualicao.GetComponent<Image>().sprite = m_ilustracoesGameplay[numCorGameplay];
            }
        }
        AtualizarToggles(i, m_corTogglesGameplay);
    }
    
    #endregion

    #region Final e Comeco
    void AplicarCores()
    {
        foreach (GameObject g in m_customizadosMenu)
        {
            if (g.GetComponent<Image>() != null)
                g.GetComponent<Image>().color = corMenu;
            if (g.GetComponent<TextMeshProUGUI>() != null)
                g.GetComponent<TextMeshProUGUI>().color = corMenu;
        }
        foreach (GameObject g in m_customizadosPlayer)
        {
            if (g.GetComponent<Image>() != null)
                g.GetComponent<Image>().color = corJogador;
            if (g.GetComponent<TextMeshProUGUI>() != null)
                g.GetComponent<TextMeshProUGUI>().color = corJogador;
        }
        foreach (GameObject g in m_customizadosTeam)
        {
            if (g.GetComponent<Image>() != null)
                g.GetComponent<Image>().color = corTime;
            if (g.GetComponent<TextMeshProUGUI>() != null)
                g.GetComponent<TextMeshProUGUI>().color = corTime;
        }
        foreach (GameObject g in m_customizadosConfig)
        {
            if (g.GetComponent<Image>() != null)
                g.GetComponent<Image>().color = corConfig;
            if (g.GetComponent<TextMeshProUGUI>() != null)
                g.GetComponent<TextMeshProUGUI>().color = corConfig;

            if(g.name == "Neon Superior" || g.name == "Neon Inferior")
            {
                float H, S, V;
                Color.RGBToHSV(g.GetComponent<Image>().color, out H, out S, out V);
                g.GetComponent<Image>().color = Color.HSVToRGB(H, S, 0.5f);
            }
        }

        foreach (GameObject g in m_configPretoBranco)
        {
            if (numCorConfig == 0 || numCorConfig == 1) corPretoBranco = Color.black;
            else corPretoBranco = Color.white;

            if (g.GetComponent<Image>() != null)
                g.GetComponent<Image>().color = corPretoBranco;
            if (g.GetComponent<TextMeshProUGUI>() != null)
                g.GetComponent<TextMeshProUGUI>().color = corPretoBranco;
        }
        foreach (GameObject g in m_menuPretoBranco)
        {
            if (numCorMenu == 0 || numCorMenu == 1) corPretoBranco = Color.black;
            else corPretoBranco = Color.white;

            if (g.GetComponent<Image>() != null)
                g.GetComponent<Image>().color = corPretoBranco;
            if (g.GetComponent<TextMeshProUGUI>() != null)
                g.GetComponent<TextMeshProUGUI>().color = corPretoBranco;
        }
        foreach (GameObject g in m_jogadorPretoBranco)
        {
            if (numCorJogador == 0 || numCorJogador == 1) corPretoBranco = Color.black;
            else corPretoBranco = Color.white;

            if (g.GetComponent<Image>() != null)
                g.GetComponent<Image>().color = corPretoBranco;
            if (g.GetComponent<TextMeshProUGUI>() != null)
                g.GetComponent<TextMeshProUGUI>().color = corPretoBranco;
        }
        foreach (GameObject g in m_timePretoBranco)
        {
            if (numCorTime == 0 || numCorTime == 1) corPretoBranco = Color.black;
            else corPretoBranco = Color.white;

            if (g.GetComponent<Image>() != null)
                g.GetComponent<Image>().color = corPretoBranco;
            if (g.GetComponent<TextMeshProUGUI>() != null)
                g.GetComponent<TextMeshProUGUI>().color = corPretoBranco;
        }
    }
    public void SalvarConfiguracoes()
    {
        config.m_corMenuCustom = numCorMenu;
        config.m_corJogadorCustom = numCorJogador;
        config.m_corTimeCustom = numCorTime;
        config.m_corConfigCustom = numCorConfig;
        config.m_corGameplayCustom = numCorGameplay;

        config.m_language = m_idiomaAtual;
        config.m_resolucao = m_resolucaoAtual;

        config.m_somEfeito = m_volumeEfeito;
        config.m_somInterface = m_volumeInterface;
        config.m_somMusica = m_volumeMusica;
        config.m_somTorcida = m_volumeTorcida;

        config.m_camSensibilidade = m_camSensibilidade;
        config.m_camPosY = m_camPosY;
        config.m_camAngulo = m_camAngulo;

        if (m_mudouCor) m_aplicarCor = true;
        //SaveSystem.SaveConfigurations(config);
    }
    public void SetarVariaveis()
    {
        m_mudouCor = m_aplicarCor = testarBarra = testarSensibilidade = false;
        cenaAtual = 1;

        numCorConfig = config.m_corConfigCustom;
        numCorJogador = config.m_corJogadorCustom;
        numCorMenu = config.m_corMenuCustom;
        numCorTime = config.m_corTimeCustom;
        numCorGameplay = config.m_corGameplayCustom;

        m_visualicao.GetComponent<Image>().sprite = m_ilustracoesMenu[numCorMenu];
        corMenu = m_cores[numCorMenu].GetComponent<Image>().color;
        corJogador = m_cores[numCorJogador].GetComponent<Image>().color;
        corTime = m_cores[numCorTime].GetComponent<Image>().color;
        corConfig = m_cores[numCorConfig].GetComponent<Image>().color;

        AplicarCores();

        m_idiomaAtual = config.m_language;
        m_resolucaoAtual = config.m_resolucao;

        m_camSensibilidade = config.m_camSensibilidade;
        m_camPosY = config.m_camPosY;
        m_camAngulo = config.m_camAngulo;

        m_volumeInterface = config.m_somInterface;
        m_volumeMusica = config.m_somMusica;
        m_volumeEfeito = config.m_somEfeito;
        m_volumeTorcida = config.m_somTorcida;

        m_camSensiblidadeInput.text = m_camSensibilidade.ToString();
        m_camPosYInput.text = m_camPosY.ToString();
        m_camAnguloInput.text = m_camAngulo.ToString();

        m_somTorcidaSlider.value = m_volumeTorcida;
        m_somMusicasSlider.value = m_volumeMusica;
        m_somInterfaceSlider.value = m_volumeInterface;
        m_somEfeitosSlider.value = m_volumeEfeito;

        m_torcidaInput.text = m_volumeTorcida.ToString();
        m_interfaceInput.text = m_volumeInterface.ToString();
        m_musicaInput.text = m_volumeMusica.ToString();
        m_efetioInput.text = m_volumeEfeito.ToString();

        toggleSensibilidade.isOn = false;
        TestarSensibilidade();
        DefaultBarra();

        /*switch (m_resolucaoAtual)
        {
            case 1:
                m_baixaRes = true;
                m_altaRes = m_mediaRes = false;
                break;
            case 2:
                m_mediaRes = true;
                m_baixaRes = m_altaRes = false;
                break;
            case 3:
                m_altaRes = true;
                m_baixaRes = m_mediaRes = false;
                break;

        }

        switch (m_idiomaAtual)
        {
            case 1:
                m_lngEng = true;
                m_lngPt = m_lngEsp = m_lngFra = false;
                break;
            case 2:
                m_lngPt = true;
                m_lngEsp = m_lngEng = m_lngFra = false;
                break;
            case 3:
                m_lngEsp = true;
                m_lngPt = m_lngEng = m_lngFra = false;
                break;
            case 4:
                m_lngFra = true;
                m_lngPt = m_lngEng = m_lngEsp = false;
                break;
        }
        */
        AtualizarToggles(numCorMenu, m_corToggles);
        AtualizarToggles(m_idiomaAtual, m_idiomasToggles);
        AtualizarToggles(m_resolucaoAtual, m_resolucoesToggles);
    }
    public void SairConfig()
    {
        foreach (Toggle t in m_corToggles) t.gameObject.SetActive(true);
        foreach (Toggle t2 in m_corTogglesGameplay) t2.gameObject.SetActive(false);

        if (m_mudouCor && m_aplicarCor) AplicarCores();
        else { m_mudouCor = false; ;m_aplicarCor = false; }
        FindObjectOfType<EventsManager>().ClickInFirstScene("cena menu");
    }
    #endregion
}
