using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class TeamEditionManager : EditionManager
{
    int numAtacantes, numMeias, numZagueiros, maxAtaAtual, maxMeiaAtual, maxDefAtual;
    int minAta, minMei, minDef, maxAtaConst, maxMeiConst, maxDefConst;

    int inputAta, inputMeia, inputDef;

    int esquemaEscolhidoAtual, esquemaEscolhidoClassico, esquemaEscolhidoRapido, esquemaEscolhido3v, jogadoresColocados, maxJogadores;
    int posicaoEscolhida, tipoAta, tipoMei, tipoDef;
    public int modoJogoEscolhido;

    bool podePosicaoNula, posicoesCustomizadas, instanciouBotoes;
    bool salvouEsquema = true, primeiraEntrada;
    EsquemasTaticos esquema;

    [Header("Secoes")]
    [SerializeField] GameObject m_logosSecao;
    [SerializeField] GameObject m_esquemaTaticosSecao, m_posicoesSecao;
    [SerializeField] GameObject m_alertaSalvarEsquema;
    [SerializeField] Button m_botaoVoltar, m_botaoInstanciarBotoes;
    [SerializeField] TextMeshProUGUI m_subtitulo;

    [Header("Esquema Tatico")]
    [SerializeField] GameObject m_botaoClassico;
    [SerializeField] GameObject m_botaoRapido, m_botao3v3;
    [SerializeField] public GameObject m_botaoPrefab;

    [Header("Cena Posicoes")]
    [SerializeField] GameObject m_inputAta;
    [SerializeField] GameObject m_inputMeias, m_inputDef, m_localBotoes;
    [SerializeField] GameObject m_botaoPosAta, m_botaoPosMei, m_botaoPosDef, m_opcoesDeBotoes, m_botaoSalvarPosicoes;
    [SerializeField] List<GameObject> m_esquemaToggles, m_esquemasPreClassico, m_esquemasPre3v, m_esquemasPreRapido;

    [Header("Logo")]
    [SerializeField] Toggle m_usarLogoToggle;
    [SerializeField] Toggle m_usarEstrelasToggle;
    int tipoFundoLogo, tipoSimboloLogo, tipoBordaLogo;
    bool usarLogo, usarEstrelas;


    void Update()
    {
        //Esquemas
        if (posicoesCustomizadas)
        {
            #region Sets
            if (jogadoresColocados == maxJogadores && !instanciouBotoes) m_botaoInstanciarBotoes.interactable = true;
            else m_botaoInstanciarBotoes.interactable = false;

            m_inputAta.GetComponent<TMP_InputField>().text = Regex.Replace(m_inputAta.GetComponent<TMP_InputField>().text, @"[^0-9]", "");
            m_inputMeias.GetComponent<TMP_InputField>().text = Regex.Replace(m_inputMeias.GetComponent<TMP_InputField>().text, @"[^0-9]", "");
            m_inputDef.GetComponent<TMP_InputField>().text = Regex.Replace(m_inputDef.GetComponent<TMP_InputField>().text, @"[^0-9]", "");

            if (m_inputAta.GetComponent<TMP_InputField>().text != "") int.TryParse(m_inputAta.GetComponent<TMP_InputField>().text, out inputAta);
            if (m_inputMeias.GetComponent<TMP_InputField>().text != "") int.TryParse(m_inputMeias.GetComponent<TMP_InputField>().text, out inputMeia);
            if (m_inputDef.GetComponent<TMP_InputField>().text != "") int.TryParse(m_inputDef.GetComponent<TMP_InputField>().text, out inputDef);
            #endregion

            if (!podePosicaoNula)
            {
                if (inputAta < minAta) { m_inputAta.GetComponent<TMP_InputField>().text = minAta.ToString(); inputDef = minAta; }
                if (inputMeia < minMei) { m_inputMeias.GetComponent<TMP_InputField>().text = minMei.ToString(); inputMeia = minMei; }
                if (inputDef < minDef) { m_inputDef.GetComponent<TMP_InputField>().text = minDef.ToString(); inputAta = minDef; }
            }

            jogadoresColocados = inputAta + inputMeia + inputDef;
        }

        //Logo e Escolher Tipos Botoes
        if (Input.GetMouseButtonUp(0))
        {
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);

            switch (secao)
            {
                case "logo":
                    foreach (RaycastResult result in results)
                    {
                        if (result.gameObject.CompareTag("Imagem Simbolo Brasao") && result.gameObject.GetComponent<LogosImagemBrasao>().getDisponivel())
                        {
                            MudarSimboloBrasao(m_logoManager.m_tipoLogo, result.gameObject.GetComponent<LogosImagemBrasao>().m_logo);
                            tipoSimboloLogo = result.gameObject.GetComponent<LogosImagemBrasao>().m_logo.m_tipo;
                        }

                        if (result.gameObject.CompareTag("Imagem Fundo Brasao") && result.gameObject.GetComponent<LogosImagemBrasao>().getDisponivel())
                        {
                            MudarFundoBrasao(m_logoManager.m_tipoLogo, result.gameObject.GetComponent<LogosImagemBrasao>().m_logo);
                            tipoFundoLogo = result.gameObject.GetComponent<LogosImagemBrasao>().m_logo.m_tipo;
                        }

                        if (result.gameObject.CompareTag("Imagem Borda Brasao") && result.gameObject.GetComponent<LogosImagemBrasao>().getDisponivel())
                        {
                            MudarBordaBrasao(result.gameObject.GetComponent<LogosImagemBrasao>().m_logo);
                            tipoBordaLogo = result.gameObject.GetComponent<LogosImagemBrasao>().m_logo.m_tipo;
                        }
                    }
                    break;
                case "alterar posicoes":
                    foreach (RaycastResult result in results)
                    {
                        if(result.gameObject.CompareTag("Imagem Formatos Player") && result.gameObject.GetComponent<FormatosImagemBotao>().getDisponivel())
                        {
                            if (posicaoEscolhida == 1)
                            {
                                tipoAta = result.gameObject.GetComponent<FormatosImagemBotao>().m_botao.m_tipo;
                                m_botaoPosAta.transform.GetChild(0).GetComponent<Image>().sprite = result.gameObject.GetComponent<FormatosImagemBotao>().m_botao.m_imagem;
                            }
                            else if (posicaoEscolhida == 2)
                            {
                                tipoMei = result.gameObject.GetComponent<FormatosImagemBotao>().m_botao.m_tipo;
                                m_botaoPosMei.transform.GetChild(0).GetComponent<Image>().sprite = result.gameObject.GetComponent<FormatosImagemBotao>().m_botao.m_imagem;
                            }
                            else
                            {
                                tipoDef = result.gameObject.GetComponent<FormatosImagemBotao>().m_botao.m_tipo;
                                m_botaoPosDef.transform.GetChild(0).GetComponent<Image>().sprite = result.gameObject.GetComponent<FormatosImagemBotao>().m_botao.m_imagem;
                            }
                            print("Escolheu");
                        }
                    }
                    break;
            }
        }
    }

    #region Logo
    void MudarFundoBrasao(int i, Logos g)
    {
        switch (i)
        {
            case 1:
                if (g.m_tipo1 == null) print("Esse tipo de logo não possui tal simbolo como complemento");
                else
                {
                    m_logoManager.m_fundo.sprite = g.m_tipo1;
                    m_usuario.m_tipoFundoLogo = g.m_tipo;
                    m_usuario.m_fundoLogo = g.m_tipo1;
                }
                break;
            case 2:
                if (g.m_tipo2 == null) print("Esse tipo de logo não possui tal simbolo como complemento");
                else
                {
                    m_logoManager.m_fundo.sprite = g.m_tipo2;
                    m_usuario.m_tipoFundoLogo = g.m_tipo;
                    m_usuario.m_fundoLogo = g.m_tipo2;
                }
                break;
            case 3:
                if (g.m_tipo3 == null) print("Esse tipo de logo não possui tal simbolo como complemento");
                else
                {
                    m_logoManager.m_fundo.sprite = g.m_tipo3;
                    m_usuario.m_tipoFundoLogo = g.m_tipo;
                    m_usuario.m_fundoLogo = g.m_tipo3;
                }
                break;
            case 4:
                if (g.m_tipo4 == null) print("Esse tipo de logo não possui tal simbolo como complemento");
                else
                {
                    m_logoManager.m_fundo.sprite = g.m_tipo4;
                    m_usuario.m_tipoFundoLogo = g.m_tipo;
                    m_usuario.m_fundoLogo = g.m_tipo4;
                }
                break;
        }
        VisualizarBaseFundoNovoBotoesMenu(m_playerMenu);
        VisualizarBaseFundoNovoBotoesMenu(m_goleiroMenu);
    } //Mudar Fundo do Brasao do Team Edition
    void MudarSimboloBrasao(int i, Logos g)
    {
        switch (i)
        {
            case 1:
                if (g.m_tipo1 == null) print("Esse tipo de logo não possui tal simbolo como complemento");
                else
                {
                    m_logoManager.m_simbolo.sprite = g.m_tipo1;
                    m_usuario.m_tipoSimboloLogo = g.m_tipo;
                    m_usuario.m_simboloLogo = g.m_tipo1;
                }
                break;
            case 2:
                if (g.m_tipo2 == null) print("Esse tipo de logo não possui tal simbolo como complemento");
                else
                {
                    m_logoManager.m_simbolo.sprite = g.m_tipo2;
                    m_usuario.m_tipoSimboloLogo = g.m_tipo;
                    m_usuario.m_simboloLogo = g.m_tipo2;
                }
                break;
            case 3:
                if (g.m_tipo3 == null) print("Esse tipo de logo não possui tal simbolo como complemento");
                else
                {
                    m_logoManager.m_simbolo.sprite = g.m_tipo3;
                    m_usuario.m_tipoSimboloLogo = g.m_tipo;
                    m_usuario.m_simboloLogo = g.m_tipo3;
                }
                break;
            case 4:
                if (g.m_tipo4 == null) print("Esse tipo de logo não possui tal simbolo como complemento");
                else
                {
                    m_logoManager.m_simbolo.sprite = g.m_tipo4;
                    m_usuario.m_tipoSimboloLogo = g.m_tipo;
                    m_usuario.m_simboloLogo = g.m_tipo4;
                }
                break;
        }
        VisualizarSimboloNovoBotoesMenu(m_playerMenu);
        VisualizarSimboloNovoBotoesMenu(m_goleiroMenu);
    } //Mudar Simbolo do Brasao do Team Edition
    void MudarBordaBrasao(Logos g)
    {
        m_logoManager.m_borda.sprite = g.m_tipo1;
        m_usuario.m_tipoBorda = g.m_tipo;
        m_usuario.m_bordaLogo= g.m_tipo1;
    } //Mudar Borda do Brasao do Team Edition

    public void VisualizarSimboloNovoBotoesMenu(GameObject g)
    {
        GameObject s = g.transform.GetChild(0).GetChild(2).gameObject;
        s.GetComponent<SpriteRenderer>().sprite = m_logoManager.m_simbolo.sprite;
        s.GetComponent<SpriteRenderer>().color = m_logoManager.m_simbolo.color;
    } //Visualizar Sprites Atualizados nos Botoes Jogadores do menu
    public void VisualizarBaseFundoNovoBotoesMenu(GameObject g)
    {
        GameObject s = g.transform.GetChild(0).GetChild(0).gameObject;
        s.GetComponent<SpriteRenderer>().sprite = m_logoManager.m_base.sprite;
        s.GetComponent<SpriteRenderer>().color = m_logoManager.m_base.color;

        GameObject f = g.transform.GetChild(0).GetChild(1).gameObject;
        f.GetComponent<SpriteRenderer>().sprite = m_logoManager.m_fundo.sprite;
        f.GetComponent<SpriteRenderer>().color = m_logoManager.m_fundo.color;
    } //Visualizar Sprites Atualizados nos Botoes Jogadores do menu
    public void UsarBrasao()
    {
        if (m_usarLogoToggle.isOn) usarLogo = true;
        else usarLogo = false;
        m_playerMenu.transform.GetChild(0).gameObject.SetActive(m_usuario.m_usarLogo);
    } //Situacao do Uso do Brasao
    public void UsarEstrelas()
    {
        if (m_usarEstrelasToggle.isOn && GameManager.Instance.m_usuario.m_level >= 65)
        {
            usarEstrelas = true;
            foreach (GameObject e in m_logoManager.m_estrelas) e.GetComponent<Estrelas>().EstadoEstrelas();
        }
        else
        {
            m_usarEstrelasToggle.isOn = false;
            usarEstrelas = false;
            foreach (GameObject e in m_logoManager.m_estrelas) e.SetActive(false);
        }
    } //Situacao do Uso das Estrelas
    #endregion

    #region Esquema Tatico

    #region Toggle
    IEnumerator IniciarToggles()
    {
        for (int j = m_localBotoes.transform.childCount-1; j >= 0; j--) Destroy(m_localBotoes.transform.GetChild(j).gameObject);

        yield return new WaitForSeconds(0.1f);
        foreach (GameObject t in m_esquemaToggles)
        {
            t.GetComponentInChildren<TextMeshProUGUI>().text = t.GetComponent<ListaDeEsquemas>().esquemas[modoJogoEscolhido - 1].nome;
            if (modoJogoEscolhido == 1)
            {
                if (t.name == "Esquema " + esquemaEscolhidoClassico)
                {
                    esquema = t.GetComponent<ListaDeEsquemas>().esquemas[modoJogoEscolhido - 1];
                    esquemaEscolhidoAtual = esquemaEscolhidoClassico;

                    numAtacantes = esquema.numAta;
                    numMeias = esquema.numMeia;
                    numZagueiros = esquema.numDef;
                    t.transform.GetChild(1).gameObject.GetComponent<Toggle>().isOn = true;
                }
            }
            else if (modoJogoEscolhido == 2)
            {
                if (t.name == "Esquema " + esquemaEscolhidoRapido)
                {
                    esquemaEscolhidoAtual = esquemaEscolhidoRapido;
                    esquema = t.GetComponent<ListaDeEsquemas>().esquemas[modoJogoEscolhido - 1];

                    numAtacantes = esquema.numAta;
                    numMeias = esquema.numMeia;
                    numZagueiros = esquema.numDef;
                    t.transform.GetChild(1).gameObject.GetComponent<Toggle>().isOn = true;
                }
            }
            else
            {
                if (t.name == "Esquema " + esquemaEscolhido3v)
                {
                    esquemaEscolhidoAtual = esquemaEscolhido3v;
                    esquema = t.GetComponent<ListaDeEsquemas>().esquemas[modoJogoEscolhido - 1];

                    numAtacantes = esquema.numAta;
                    numMeias = esquema.numMeia;
                    numZagueiros = esquema.numDef;
                    t.transform.GetChild(1).gameObject.GetComponent<Toggle>().isOn = true;
                }
            }
            t.GetComponent<ListaDeEsquemas>().esquemaDisponivel = t.GetComponent<ListaDeEsquemas>().esquemas[modoJogoEscolhido - 1];
        }
        
        InstanciarEsquemas();
        primeiraEntrada = false;
    } //Setar os toggles dos esquemas quando inicializados
    #endregion

    #region Inputs
    public void MudarValoresInput(int p)
    {
        int inputAta = 0, inputMeia = 0, inputDef = 0;
        int.TryParse(m_inputAta.GetComponent<TMP_InputField>().text, out inputAta);
        int.TryParse(m_inputMeias.GetComponent<TMP_InputField>().text, out inputMeia);
        int.TryParse(m_inputDef.GetComponent<TMP_InputField>().text, out inputDef);

        if (inputMeia > maxMeiaAtual) 
        {
            inputMeia = maxMeiaAtual;
            //print("excedeu Meia");
        }

        if (inputAta > maxAtaAtual) 
        {
            inputAta = maxAtaAtual;
            //print("excedeu Ata");
        }

        if (inputDef > maxDefAtual)
        {
            inputDef = maxDefAtual;
            //print("excedeu Zaga");
        }

        switch (p)
        {
            case 1:
                AtualizarTextoInputs(p, inputAta);
                break;
            case 2:
                AtualizarTextoInputs(p, inputMeia);
                break;
            case 3:
                AtualizarTextoInputs(p, inputDef);
                break;
        }
    } //Impedir com que os valores do input excedam e armazenar os valores dos inputs
    void AtualizarTextoInputs(int p, int i)
    {
        switch (p)
        {
            case 1://atacante
                m_inputAta.GetComponent<TMP_InputField>().text = i.ToString();
                m_inputAta.GetComponent<TMP_InputField>().placeholder.GetComponent<TextMeshProUGUI>().text = i.ToString();
                break;
            case 2://meia
                m_inputMeias.GetComponent<TMP_InputField>().text = i.ToString();
                m_inputMeias.GetComponent<TMP_InputField>().placeholder.GetComponent<TextMeshProUGUI>().text = i.ToString();
                break;
            case 3://zaga
                m_inputDef.GetComponent<TMP_InputField>().text = i.ToString();
                m_inputDef.GetComponent<TMP_InputField>().placeholder.GetComponent<TextMeshProUGUI>().text = i.ToString();
                break;
        }
    } //Manter o texo dos Inputs atualizados quando modificamos
    void AtualizarPlaceholderInputs()
    {
        m_inputAta.GetComponent<TMP_InputField>().text = numAtacantes.ToString();
        m_inputMeias.GetComponent<TMP_InputField>().text = numMeias.ToString();
        m_inputDef.GetComponent<TMP_InputField>().text = numZagueiros.ToString();

        if (esquemaEscolhidoAtual == 4)
        {
            m_inputAta.GetComponent<TMP_InputField>().interactable = m_inputMeias.GetComponent<TMP_InputField>().interactable =
                m_inputDef.GetComponent<TMP_InputField>().interactable = true;
        }
        else
        {
            m_inputAta.GetComponent<TMP_InputField>().interactable = m_inputMeias.GetComponent<TMP_InputField>().interactable =
                m_inputDef.GetComponent<TMP_InputField>().interactable = false;
        }
    } //Setar o texto dos Inputs quando inicializados
    void ArmazenarValoresInputs()
    {
        int.TryParse(m_inputAta.GetComponent<TMP_InputField>().text, out numAtacantes);
        int.TryParse(m_inputMeias.GetComponent<TMP_InputField>().text, out numMeias);
        int.TryParse(m_inputDef.GetComponent<TMP_InputField>().text, out numZagueiros);
    } //Setar o valor das variaveis numAtacantes, numMeias, numZagueiros
    public void VerificarNumerosInputs()
    {
        if (inputAta == maxAtaConst && inputMeia < maxMeiaAtual && inputDef < maxDefAtual)
        {
            maxMeiaAtual = maxJogadores - inputAta - inputDef;
            maxDefAtual = maxJogadores - inputAta - inputMeia;
        }
        else if (inputMeia == maxMeiConst && inputDef < maxDefAtual && inputAta < maxAtaAtual)
        {
            maxDefAtual = maxJogadores - inputAta - inputMeia;
            maxAtaAtual = maxJogadores - inputDef - inputMeia;
        }
        else if (inputDef == maxDefAtual && inputAta < maxAtaAtual && inputMeia < maxMeiaAtual)
        {
            maxAtaAtual = maxJogadores - inputDef - inputMeia;
            maxMeiaAtual = maxJogadores - inputDef - inputAta;
        }
        else
        {
            if (jogadoresColocados > maxJogadores)
            {
                if (inputAta == maxAtaAtual)
                {
                    inputAta -= (jogadoresColocados - maxJogadores);
                    AtualizarTextoInputs(1, inputAta);
                }
                else if (inputMeia == maxMeiaAtual)
                {
                    inputMeia -= (jogadoresColocados - maxJogadores);
                    AtualizarTextoInputs(2, inputMeia);
                }
                else
                {
                    inputDef -= (jogadoresColocados - maxJogadores);
                    AtualizarTextoInputs(3, inputDef);
                }
            }
            else
            {
                maxAtaAtual = maxJogadores - inputDef - inputMeia;
                maxMeiaAtual = maxJogadores - inputDef - inputAta;
                maxDefAtual = maxJogadores - inputAta - inputMeia;
                if (maxAtaAtual > maxAtaConst) maxAtaAtual = maxAtaConst;
                if (maxMeiaAtual > maxMeiConst) maxMeiaAtual = maxMeiConst;
                if (maxDefAtual > maxDefConst) maxDefAtual = maxDefConst;
            }

            if(inputAta + inputMeia + inputDef == minAta + minDef + minMei)
            {
                maxAtaAtual = maxAtaConst;
                maxDefAtual = maxDefConst;
                maxMeiaAtual = maxMeiConst;
            }
        }
    } //Verificar se os numeros dos Inputs unidos nao excedem o max de Jogadores
    #endregion

    #region Esquema e Modo Escolhido
    public void ModoDeJogoEscolhido(int i)
    {
        modoJogoEscolhido = i;
        if(i == 1)
        {
            maxAtaConst = maxAtaAtual = 4;
            maxMeiConst = maxMeiaAtual = 5;
            maxDefConst = maxDefAtual = 5;
            maxJogadores = 10;
            minAta = 1;
            minMei = 2;
            minDef = 2;
            podePosicaoNula = false;
            esquemaEscolhidoAtual = esquemaEscolhidoClassico;
        }
        else if(i == 2)
        {
            maxAtaConst = maxAtaAtual = 2;
            maxMeiConst = maxMeiaAtual = 4;
            maxDefConst = maxDefAtual = 4;
            maxJogadores = 6;
            minAta = minMei = minDef = 1;
            podePosicaoNula = false;
            esquemaEscolhidoAtual = esquemaEscolhidoRapido;
        }
        else
        {
            maxAtaConst = maxAtaAtual = 3;
            maxDefConst = maxDefAtual = 3;
            maxMeiConst = maxMeiaAtual = 3;
            maxJogadores = 3;
            minAta = minMei = minDef = 0;
            podePosicaoNula = true;
            esquemaEscolhidoAtual = esquemaEscolhido3v;
        }
        StartCoroutine(IniciarToggles());
    } //Escolher o modo de Jogo para estabelecer um esquema
    public void EscolherEsquema(int i)
    {

        if (!primeiraEntrada && m_esquemaToggles[i-1].transform.GetChild(1).GetComponent<Toggle>().isOn)
        {
            for (int j = 0; j < m_localBotoes.transform.childCount; j++) Destroy(m_localBotoes.transform.GetChild(0).gameObject);

            esquemaEscolhidoAtual = i;
            esquema = m_esquemaToggles[esquemaEscolhidoAtual - 1].GetComponent<ListaDeEsquemas>().esquemaDisponivel;
            //salvouEsquema = false;

            numAtacantes = esquema.numAta;
            numMeias = esquema.numMeia;
            numZagueiros = esquema.numDef;

            if (modoJogoEscolhido == 1) esquemaEscolhidoClassico = i;
            else if (modoJogoEscolhido == 2) esquemaEscolhidoRapido = i;
            else esquemaEscolhido3v = i;

            if (i == 4)
            {
                instanciouBotoes = false;
                posicoesCustomizadas = true;
                m_botaoInstanciarBotoes.gameObject.SetActive(true);
            }
            else
            {
                posicoesCustomizadas = false;
                m_botaoInstanciarBotoes.gameObject.SetActive(false);
                m_botaoSalvarPosicoes.SetActive(true);
            }

            InstanciarEsquemas();
            AtualizarPlaceholderInputs();
            salvouEsquema = false;
            m_botaoVoltar.interactable = false;
        }
    } //Escolher um esquema preferido
    void InstanciarEsquemas()
    {
        if (esquemaEscolhidoAtual != 4)
        {
            if (modoJogoEscolhido == 1) Instantiate(m_esquemasPreClassico[esquemaEscolhidoAtual - 1], m_localBotoes.transform);
            else if (modoJogoEscolhido == 2) Instantiate(m_esquemasPreRapido[esquemaEscolhidoAtual - 1], m_localBotoes.transform);
            else Instantiate(m_esquemasPre3v[esquemaEscolhidoAtual - 1], m_localBotoes.transform);
        }
        else
        {
            if(modoJogoEscolhido == 1)
            {
                if (m_usuario.posicoesCustomClassico != null)
                {
                    numAtacantes = numMeias = numZagueiros = 0;
                    for (int i = 0; i < 10; i++)
                    {
                        GameObject b = m_botaoPrefab;
                        Instantiate(b, new Vector3(m_localBotoes.transform.position.x + (m_usuario.posicoesCustomClassico[i, 1] * (478 / 2)),
                            m_localBotoes.transform.position.y + (m_usuario.posicoesCustomClassico[i, 2] * (737 / 2)), 0), Quaternion.identity, m_localBotoes.transform);
                        b.GetComponent<BotaoEscalacao>().tipo = m_usuario.posicoesCustomClassico[i, 0];
                        if (b.GetComponent<BotaoEscalacao>().tipo == 1) numAtacantes++;
                        else if (b.GetComponent<BotaoEscalacao>().tipo == 2) numMeias++;
                        else numZagueiros++;
                    }
                }
                else print("Sem esquema Customizado");
            }
            else if(modoJogoEscolhido == 2)
            {
                if(m_usuario.posicoesCustomRapido != null)
                {
                    numAtacantes = numMeias = numZagueiros = 0;
                    for (int i = 0; i < 6; i++)
                    {
                        GameObject b = m_botaoPrefab;
                        Instantiate(b, new Vector3(m_localBotoes.transform.position.x + (m_usuario.posicoesCustomRapido[i, 1] * (478 / 2)),
                            m_localBotoes.transform.position.y + (m_usuario.posicoesCustomRapido[i, 2] * (737 / 2)), 0), Quaternion.identity, m_localBotoes.transform);
                        b.GetComponent<BotaoEscalacao>().tipo = m_usuario.posicoesCustomRapido[i, 0];
                        if (b.GetComponent<BotaoEscalacao>().tipo == 1) numAtacantes++;
                        else if (b.GetComponent<BotaoEscalacao>().tipo == 2) numMeias++;
                        else numZagueiros++;
                    }
                }
                else print("Sem esquema Customizado");
            }
            else
            {
                if(m_usuario.posicoesCustom3v3 != null)
                {
                    numAtacantes = numMeias = numZagueiros = 0;
                    for (int i = 0; i < 6; i++)
                    {
                        GameObject b = m_botaoPrefab;
                        Instantiate(b, new Vector3(m_localBotoes.transform.position.x + (m_usuario.posicoesCustom3v3[i, 1] * (478 / 2)),
                            m_localBotoes.transform.position.y + (m_usuario.posicoesCustom3v3[i, 2] * (737 / 2)), 0), Quaternion.identity, m_localBotoes.transform);
                        b.GetComponent<BotaoEscalacao>().tipo = m_usuario.posicoesCustom3v3[i, 0];
                        if (b.GetComponent<BotaoEscalacao>().tipo == 1) numAtacantes++;
                        else if (b.GetComponent<BotaoEscalacao>().tipo == 2) numMeias++;
                        else numZagueiros++;
                    }
                }
                else print("Sem esquema Customizado");
            }

            if (m_localBotoes.transform.childCount > 2) AjustarTiposBotoes();
        }

        AtualizarPlaceholderInputs();
    } //Mostrar o esquema na lousa
    #endregion

    void AjustarTiposBotoes()
    {
        for(int i = 0; i < m_localBotoes.transform.childCount; i++)
        {
            if (m_localBotoes.transform.GetChild(i).position.y > 445) 
                m_localBotoes.transform.GetChild(i).GetComponent<BotaoEscalacao>().tipo = 1;
            else if(m_localBotoes.transform.GetChild(i).position.y < 445 && m_localBotoes.transform.GetChild(i).position.y > 320)
                m_localBotoes.transform.GetChild(i).GetComponent<BotaoEscalacao>().tipo = 2;
            else m_localBotoes.transform.GetChild(i).GetComponent<BotaoEscalacao>().tipo = 3;
        }
    }
    public void InstanciarBotoes()
    {
        for (int j = 0; j < m_localBotoes.transform.childCount; j++) Destroy(m_localBotoes.transform.GetChild(0).gameObject);
        ArmazenarValoresInputs();
        float sizeX = m_localBotoes.GetComponent<Image>().rectTransform.sizeDelta.x;

        for(int i = 0; i < numAtacantes; i++)
        {
            GameObject b = m_botaoPrefab;
            b.GetComponent<BotaoEscalacao>().tipo = 1;
            Instantiate(b, new Vector3(m_localBotoes.transform.position.x - (sizeX/2) + ( ( sizeX  / numAtacantes ) * i) + (sizeX / (numAtacantes * 2)),
                m_localBotoes.transform.position.y - 70), 
                Quaternion.identity, m_localBotoes.transform);
        }
        for(int j = 0; j < numMeias; j++)
        {
            GameObject b = m_botaoPrefab;
            b.GetComponent<BotaoEscalacao>().tipo = 2;
            Instantiate(b, new Vector3(m_localBotoes.transform.position.x - (sizeX / 2) + ((sizeX / numMeias) * j) + (sizeX / (numMeias* 2)),
                m_localBotoes.transform.position.y - 170),
                Quaternion.identity, m_localBotoes.transform);
        }
        for(int k = 0; k < numZagueiros; k++)
        {
            GameObject b = m_botaoPrefab;
            b.GetComponent<BotaoEscalacao>().tipo = 3;
            Instantiate(b, new Vector3(m_localBotoes.transform.position.x - (sizeX / 2) + ((sizeX / numZagueiros) * k) + (sizeX / (numZagueiros * 2)),
                m_localBotoes.transform.position.y - 270), 
                Quaternion.identity, m_localBotoes.transform);
        }
        instanciouBotoes = true;
        m_botaoSalvarPosicoes.SetActive(true);
    } //Instanciar os Botoes Interativos na lousa
    public void PosicaoParaMudarBotao(int i)
    {
        posicaoEscolhida = i;
        IniciarSlots();
    } //Estabelecer a posicao para escolher o tipo de Botao
    void IniciarSlots()
    {
        if (FindObjectOfType<SlotsManager>() != null)
        {
            foreach (SlotsManager s in FindObjectsOfType<SlotsManager>())
            {
                print(s.gameObject.name);
                s.InicializarSlots();
            }
        }
    } //Inicializar os Slots do Object Opcoes

    #endregion

    public override void MudarSecoes(string s)
    {
        if (salvouEsquema)
        {
            switch (s)
            {
                case "logo":
                    m_logosSecao.SetActive(true);
                    m_esquemaTaticosSecao.SetActive(false);
                    textoSecao.text = "LOGO";
                    secao = "logo";
                    m_logosSecao.GetComponent<AtivosDefault>().SetarDefault();
                    break;
                case "esquema tatico":
                    m_logosSecao.SetActive(false);
                    m_esquemaTaticosSecao.SetActive(true);
                    textoSecao.text = "TATICAL";
                    secao = "esquema tatico";
                    m_esquemaTaticosSecao.GetComponent<AtivosDefault>().SetarDefault();
                    break;
                case "alterar posicoes":
                    //for (int j = 0; j < m_localBotoes.transform.childCount; j++) Destroy(m_localBotoes.transform.GetChild(0).gameObject);
                    m_botao3v3.SetActive(false);
                    m_botaoClassico.SetActive(false);
                    m_botaoRapido.SetActive(false);
                    m_posicoesSecao.SetActive(true);
                    primeiraEntrada = true;
                    secao = "alterar posicoes";
                    m_subtitulo.text = "PLAYERS POSITIONS";
                    break;
                case "voltar":
                    foreach (GameObject g in m_esquemaToggles) g.transform.GetChild(1).gameObject.GetComponent<Toggle>().isOn = false;
                    m_subtitulo.text = "GAME MODE";
                    m_esquemaTaticosSecao.GetComponent<AtivosDefault>().SetarDefault();
                    break;
            }
        }
        else m_alertaSalvarEsquema.SetActive(true);
    }

    void ArmazenarPosicoes(float[,] posicoes)
    {
        //int i = 0;
        Vector2 v;

        for(int i = 0; i < m_localBotoes.transform.childCount; i++)
        {
            v = m_localBotoes.transform.GetChild(i).GetComponent<BotaoEscalacao>().Posicao();
            posicoes[i, 0] = m_localBotoes.transform.GetChild(i).GetComponent<BotaoEscalacao>().tipo;
            posicoes[i, 1] = v.x;
            posicoes[i, 2] = v.y;
        }

        /*foreach (GameObject botao in GameObject.FindGameObjectsWithTag("Botao Lousa"))
        {
            v = botao.GetComponent<BotaoEscalacao>().Posicao();
            posicoes[i, 0] = botao.GetComponent<BotaoEscalacao>().tipo;
            posicoes[i, 1] = v.x;
            posicoes[i, 2] = v.y;
            i++;
        }*/

    }
    public void MostrarAlerta()
    {
        if(!salvouEsquema) m_alertaSalvarEsquema.SetActive(true);
    } //Quando falta salvar o esquema escolhido mostra o Alerta
    public void SalvarEsquema()
    {
        ArmazenarValoresInputs();

        m_usuario.m_esquemaClassico = (esquemaEscolhidoClassico == 0 ? 1 : esquemaEscolhidoClassico);
        m_usuario.m_esquemaRapido = (esquemaEscolhidoRapido == 0 ? 1 : esquemaEscolhidoRapido);
        m_usuario.m_esquema3v3 = (esquemaEscolhido3v == 0 ? 1 : esquemaEscolhido3v);

        if (modoJogoEscolhido == 1)
        {
            if (esquemaEscolhidoAtual == 4)
            {
                m_usuario.posicoesCustomClassico = new float[10, 3];
                ArmazenarPosicoes(m_usuario.posicoesCustomClassico);
            }
        }
        else if (modoJogoEscolhido == 2)
        {
            if (esquemaEscolhidoAtual == 4)
            {
                m_usuario.posicoesCustomRapido = new float[6, 3];
                ArmazenarPosicoes(m_usuario.posicoesCustomRapido);
            }
        }
        else
        {
            if (esquemaEscolhidoAtual == 4)
            {
                m_usuario.posicoesCustom3v3 = new float[3, 3];
                ArmazenarPosicoes(m_usuario.posicoesCustom3v3);
            }
        }

        m_usuario.m_tipoBotaoAtaque = (tipoAta == 0 ? 1 : tipoAta);
        m_usuario.m_tipoBotaoMeio = (tipoMei == 0 ? 1 : tipoMei);
        m_usuario.m_tipoBotaoDefesa = (tipoDef == 0 ? 1 : tipoDef);

        salvouEsquema = true;
        m_botaoSalvarPosicoes.SetActive(false);
        m_botaoInstanciarBotoes.gameObject.SetActive(false);
        m_botaoVoltar.interactable = true;
        //SaveSystem.SavePlayer(m_usuario);
    } //Salva um esquema por vez e (quando buildar tirar o ultimo comentario)
    
    public void SalvarLogoMudancas()
    {
        m_usuario.m_tipoBaseLogo = m_logoManager.m_tipoLogo;
        m_usuario.m_tipoBorda = tipoBordaLogo;
        m_usuario.m_tipoFundoLogo = tipoFundoLogo;
        m_usuario.m_tipoSimboloLogo = tipoSimboloLogo;

        m_usuario.m_baseLogo = m_logoManager.m_base.sprite;
        m_usuario.m_fundoLogo = m_logoManager.m_fundo.sprite;
        m_usuario.m_simboloLogo = m_logoManager.m_simbolo.sprite;
        m_usuario.m_bordaLogo = m_logoManager.m_borda.sprite;

        m_usuario.m_usarLogo = usarLogo;
        m_usuario.m_usarEstrelas = usarEstrelas;
        //SaveSystem.SavePlayer(m_usuario);
    } //Salva as mudancas feitas no brasao e (quando buildar tirar o ultimo comentario)

    public void SetarVariaveis()
    {
        primeiraEntrada = true;

        #region Esquemas
        esquemaEscolhidoClassico = m_usuario.m_esquemaClassico;
        esquemaEscolhidoRapido = m_usuario.m_esquemaRapido;
        esquemaEscolhido3v = m_usuario.m_esquema3v3;

        PlayerButton b;
        tipoAta = m_usuario.m_tipoBotaoAtaque;
        if (tipoAta == 0) tipoAta = 1;
        b = Resources.Load("Testes/Scriptable Objects/Botoes/Tipo " + tipoAta) as PlayerButton;
        m_botaoPosAta.transform.GetChild(0).GetComponent<Image>().sprite = b.m_imagem;

        tipoMei = m_usuario.m_tipoBotaoMeio;
        if (tipoMei == 0) tipoMei = 1;
        b = Resources.Load("Testes/Scriptable Objects/Botoes/Tipo " + tipoMei) as PlayerButton;
        m_botaoPosMei.transform.GetChild(0).GetComponent<Image>().sprite = b.m_imagem;

        tipoDef = m_usuario.m_tipoBotaoDefesa;
        if (tipoDef == 0) tipoDef = 1;
        b = Resources.Load("Testes/Scriptable Objects/Botoes/Tipo " + tipoDef) as PlayerButton;
        m_botaoPosDef.transform.GetChild(0).GetComponent<Image>().sprite = b.m_imagem;

        salvouEsquema = true;
        #endregion

        #region Logo
        m_logoManager.m_tipoLogo = m_usuario.m_tipoBaseLogo;
        tipoFundoLogo = m_usuario.m_tipoFundoLogo;
        tipoSimboloLogo = m_usuario.m_tipoSimboloLogo;
        tipoBordaLogo = m_usuario.m_tipoBorda;

        usarLogo = m_usuario.m_usarLogo;
        m_usarLogoToggle.isOn = usarLogo;
        UsarBrasao();
        usarEstrelas = m_usuario.m_usarEstrelas;
        m_usarEstrelasToggle.isOn = usarEstrelas;
        UsarEstrelas();

        m_logoManager.MudarSpriteBase(GameManager.Instance.m_usuario.m_baseLogo);
        m_logoManager.MudarSpriteFundo(GameManager.Instance.m_usuario.m_fundoLogo);
        m_logoManager.MudarSpriteSimbolo(GameManager.Instance.m_usuario.m_simboloLogo);
        m_logoManager.MudarSpriteBorda(GameManager.Instance.m_usuario.m_bordaLogo);
        #endregion
    } //Setar as variaveis quando Iniciar o TeamEditionManager

    public void SairTimeCustomization()
    {
        if (salvouEsquema)
        {
            if(FindObjectOfType<AtivosDefault>() != null)
            {
                foreach (AtivosDefault a in FindObjectsOfType<AtivosDefault>()) a.SetarDefault();
            }
            secao = "";
            FindObjectOfType<EventsManager>().ClickInFirstScene("cena menu");
            //SaveSystem.CarregarData();
        }
        else
        {
            MostrarAlerta();
        }
    } //Sair do Time Custom e (quando buildar tirar o ultimo comentario)
}
