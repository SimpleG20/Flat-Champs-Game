using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class PlayerEditionManager : EditionManager
{
    #region Variaveis
    [Header("Secoes")]
    [SerializeField] GameObject m_formatosSecao;
    [SerializeField] GameObject m_coresSecao, m_texturesSecao, m_adesivosSecao, m_botoesSecao, m_goleirosSecao;

    [Header("Player Mesh")]
    [SerializeField] MeshFilter m_playerMeshMenu;
    [SerializeField] MeshFilter m_goleiroMeshMenu;
    [SerializeField] Renderer m_playerMeshRendererMenu, m_goleiroMeshRendererMenu;

    [Header("Outros")]
    [SerializeField] List<GameObject> m_formatosToggles;
    [SerializeField] List<GameObject> m_adesivosToggles;
    [SerializeField] GameObject m_visualizarToggles, m_setaProxAdesivo, m_setaAntAdesivo;
    public int m_codBotaoEscolhido, m_codGoleiroEscolhido, m_adesivoEscolhido, m_cor1Escolhida, m_cor2Escolhida, m_cor3Escolhida, m_texturaEscolhida;

    Color m_corAdesivo;
    
    public int m_opcaoCorAdesivo;
    int posicaoTogglesAdesivo;
    bool m_podecolorir, m_podeAdesivo;
    #endregion

    private void Start()
    {
        SetarVariaveis();


        //EventsManager.current.onIniciarSlots += IniciarSlots;
        /*
         Atualizar shape dos botoes
         */

        InicializarBotoesSpritesDoMenu(m_playerMenu);
        InicializarBotoesSpritesDoMenu(m_goleiroMenu);
    }

    private void Update()
    {
        //Mudar cores, formatos, adesivos e logos
        if (Input.GetMouseButtonUp(0))
        {
            //Para UI não se usa o Raycast, mas esse Esquema
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);

            switch (secao)
            {
                case "formato":
                    foreach (RaycastResult result in results)
                    {
                        if (result.gameObject.CompareTag("Imagem Formatos Player") && result.gameObject.GetComponent<FormatosImagemBotao>().getDisponivel())
                        {
                            //Mudar Mesh e Rotacao Mesh
                            m_playerMeshMenu.mesh = result.gameObject.GetComponent<FormatosImagemBotao>().m_botao.m_formato.GetComponent<MeshFilter>().sharedMesh;
                            m_playerMenu.transform.eulerAngles = new Vector3(-90, 0, 160 + result.gameObject.GetComponent<FormatosImagemBotao>().m_botao.m_rotacao);
                            FindObjectOfType<RotacionarPlayerMenu>().anguloPartida = 160 + result.gameObject.GetComponent<FormatosImagemBotao>().m_botao.m_rotacao;

                            //Mudar Posicao e Rotacao do Logo
                            m_playerMenu.transform.GetChild(0).localPosition = new Vector3(0, 0, result.gameObject.GetComponent<FormatosImagemBotao>().m_botao.m_formato.transform.GetChild(0).localPosition.z);
                            m_playerMenu.transform.GetChild(0).localEulerAngles = result.gameObject.GetComponent<FormatosImagemBotao>().m_botao.m_formato.transform.GetChild(0).localEulerAngles;

                            //Mudar Materiais
                            MudarMateriais(m_playerMeshRendererMenu, result.gameObject.GetComponent<FormatosImagemBotao>().m_botao);

                            //Pegar tipo do botao e se pode aplicar adesivo nele
                            m_codBotaoEscolhido = result.gameObject.GetComponent<FormatosImagemBotao>().m_botao.cod;
                            m_podeAdesivo = result.gameObject.GetComponent<FormatosImagemBotao>().m_botao.m_podeAdesivo;

                            //FindObjectOfType<RotacionarPlayerMenu>().ZerarSlider();
                            //FindObjectOfType<RotacionarPlayerMenu>().SetarAnguloPartida(160 + result.gameObject.GetComponent<FormatosImagemBotao>().m_botao.m_rotacao);

                            SlotSelecionado(GameObject.FindGameObjectsWithTag("Imagem Formatos Player"), result.gameObject, 2);
                        }

                        if (result.gameObject.CompareTag("Imagem Formatos Goleiro") && result.gameObject.GetComponent<FormatosImagemBotao>().getDisponivel())
                        {
                            //Mudar Mesh
                            m_goleiroMeshMenu.mesh = result.gameObject.GetComponent<FormatosImagemBotao>().m_botao.m_formato.GetComponent<MeshFilter>().sharedMesh;

                            //Mudar Scale do Logo
                            m_goleiroMenu.transform.GetChild(0).localScale = result.gameObject.GetComponent<FormatosImagemBotao>().m_botao.m_formato.transform.GetChild(0).localScale;

                            //Mudar materiais
                            MudarMateriais(m_goleiroMeshRendererMenu, result.gameObject.GetComponent<FormatosImagemBotao>().m_botao);

                            //Pegar tipo do goleiro
                            m_codGoleiroEscolhido = result.gameObject.GetComponent<FormatosImagemBotao>().m_botao.cod;
                            SlotSelecionado(GameObject.FindGameObjectsWithTag("Imagem Formatos Goleiro"), result.gameObject, 2);
                        }
                    }
                    break;
                case "cor":
                    foreach (RaycastResult result in results)
                    {
                        if (result.gameObject.CompareTag("Cores Primarias") && result.gameObject.GetComponent<NivelCores>().getDisponivel())
                        {
                            AplicarCoresNoBotao(0, result.gameObject.GetComponent<Image>().color);
                            AplicarCoresNoGoleiro(0, result.gameObject.GetComponent<Image>().color);

                            m_cor1Escolhida = result.gameObject.GetComponent<NivelCores>().m_codigoCor;
                            SlotSelecionado(GameObject.FindGameObjectsWithTag("Cores Primarias"), result.gameObject, 1);
                        }
                        else if (result.gameObject.CompareTag("Cores Secundarias") && result.gameObject.GetComponent<NivelCores>().getDisponivel())
                        {
                            AplicarCoresNoBotao(1, result.gameObject.GetComponent<Image>().color);
                            AplicarCoresNoGoleiro(1, result.gameObject.GetComponent<Image>().color);

                            m_cor2Escolhida = result.gameObject.GetComponent<NivelCores>().m_codigoCor;
                            SlotSelecionado(GameObject.FindGameObjectsWithTag("Cores Secundarias"), result.gameObject, 1);
                        }
                        else if (result.gameObject.CompareTag("Cores Terciarias") && result.gameObject.GetComponent<NivelCores>().getDisponivel())
                        {
                            AplicarCoresNoBotao(2, result.gameObject.GetComponent<Image>().color);
                            AplicarCoresNoGoleiro(2, result.gameObject.GetComponent<Image>().color);

                            m_cor3Escolhida = result.gameObject.GetComponent<NivelCores>().m_codigoCor;
                            SlotSelecionado(GameObject.FindGameObjectsWithTag("Cores Terciarias"), result.gameObject, 1);
                        }
                        MudarCorAdesivo(m_opcaoCorAdesivo);
                    }
                    break;
                case "adesivo":
                    foreach (RaycastResult result in results)
                    {
                        if (result.gameObject.CompareTag("Imagem Adesivo"))
                        {
                            if (result.gameObject.GetComponent<AdesivosImagemBotao>().getDisponivel() && m_podeAdesivo)
                            {
                                m_playerMeshRendererMenu.materials[m_playerMeshRendererMenu.materials.Length - 1].
                                    SetTexture("_MainTex", result.gameObject.GetComponent<AdesivosImagemBotao>().m_adesivo.m_imagem.texture);
                                print(m_playerMeshRendererMenu.materials[m_playerMeshRendererMenu.materials.Length - 1].mainTexture);
                                if (result.gameObject.GetComponent<AdesivosImagemBotao>().m_podeColorir == true)
                                {
                                    print("adesivo pode ser colorio");
                                    m_corAdesivo.a = 1;
                                    m_playerMeshRendererMenu.materials[m_playerMeshRendererMenu.materials.Length - 1].color = m_corAdesivo;
                                    m_podecolorir = true;
                                }
                                else { m_playerMeshRendererMenu.materials[m_playerMeshRendererMenu.materials.Length - 1].color = Color.white; m_podecolorir = false; }
                                m_adesivoEscolhido = result.gameObject.GetComponent<AdesivosImagemBotao>().m_adesivo.m_cod;

                                SlotSelecionado(GameObject.FindGameObjectsWithTag("Imagem Adesivo"), result.gameObject, 2);
                            }
                        }
                    }
                    break;
                case "textura":
                    print(secao);
                    break;
                
            }
            results.Clear();
        }
    }

    public static void SlotSelecionado(GameObject[] slots, GameObject slotsSelecionado, int childPos)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == slotsSelecionado) slots[i].transform.GetChild(childPos).gameObject.SetActive(true);
            else slots[i].transform.GetChild(childPos).gameObject.SetActive(false);
        }
    }

    void AplicarCoresNoBotao(int ordem, Color corSelecionada)
    {
        if (ordem == 1) m_cores[1].color = corSelecionada;
        else if (ordem == 2) m_cores[2].color = corSelecionada;
        else m_cores[0].color = corSelecionada;

        m_playerMeshRendererMenu.sharedMaterials[ordem].color = corSelecionada;

        GameObject b = m_playerMenu.transform.GetChild(0).GetChild(ordem).gameObject;
        b.GetComponent<SpriteRenderer>().color = corSelecionada;
    }
    void AplicarCoresNoGoleiro(int ordem, Color corSelecionada)
    {
        if (ordem != 1)
        {
            if(ordem == 2) m_goleiroMeshRendererMenu.sharedMaterials[ordem -1].color = corSelecionada;
            else m_goleiroMeshRendererMenu.sharedMaterials[ordem].color = corSelecionada;
        }
        GameObject g = m_goleiroMenu.transform.GetChild(0).GetChild(ordem).gameObject;
        g.GetComponent<SpriteRenderer>().color = corSelecionada;
    }


    #region Durante Customizacao
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
    }
    public void MudarCorAdesivo(int i)
    {
        switch (i)
        {
            case 0:
                m_corAdesivo = Color.black;
                break;
            case 1:
                m_corAdesivo = Color.white;
                break;
            case 2:
                m_corAdesivo = m_cores[0].color;
                break;
            case 3:
                m_corAdesivo = m_cores[1].color;
                break;
            case 4:
                m_corAdesivo = m_cores[2].color;
                break;
            case 5:
                m_adesivoEscolhido = 0;
                m_corAdesivo = Color.clear;
                break;
        }

        if(m_podecolorir) m_playerMeshRendererMenu.materials[m_playerMeshRendererMenu.materials.Length - 1].color = m_corAdesivo;
        else m_playerMeshRendererMenu.materials[m_playerMeshRendererMenu.materials.Length - 1].color = Color.white;
        m_opcaoCorAdesivo = i;

        ////AtualizarToggleAdesivos();
    } //Tudo Certo
    public override void MudarSecoes(string s)
    {
        switch (s)
        {
            case "formato":
                m_formatosSecao.SetActive(true);
                m_coresSecao.SetActive(false);
                m_adesivosSecao.SetActive(false);
                m_texturesSecao.SetActive(false);
                textoSecao.text = "SHAPES";
                secao = "formato";
                AtualizarTogglesFormatos();
                m_formatosSecao.GetComponent<AtivosDefault>().SetarDefault();
                break;
            case "cor":
                m_formatosSecao.SetActive(false);
                m_coresSecao.SetActive(true);
                m_adesivosSecao.SetActive(false);
                m_texturesSecao.SetActive(false);
                textoSecao.text = "COLOR";
                secao = "cor";
                m_coresSecao.GetComponent<AtivosDefault>().SetarDefault();
                break;
            case "textura":
                m_formatosSecao.SetActive(false);
                m_coresSecao.SetActive(false);
                m_adesivosSecao.SetActive(false);
                m_texturesSecao.SetActive(true);
                textoSecao.text = "TEXTURE";
                secao = "textura";
                m_texturesSecao.GetComponent<AtivosDefault>().SetarDefault();
                break;
            case "adesivo":
                m_formatosSecao.SetActive(false);
                m_coresSecao.SetActive(false);
                m_adesivosSecao.SetActive(true);
                m_texturesSecao.SetActive(false);
                secao = "adesivo";
                textoSecao.text = "STICKER";
                m_adesivosSecao.GetComponent<AtivosDefault>().SetarDefault();
                AtualizarToggleAdesivos();
                break;
        }
        IniciarSlots();
    } //Tudo Certo

    //Preciso Rever Devido a Secao de Texturas, como vai ficar esse metodo
    public void MudarMateriais(Renderer m, PlayerButton b)
    {
        m.materials = b.m_formato.GetComponent<MeshRenderer>().sharedMaterials;
        //m_corAdesivo = m_usuario.m_corAdesivo;
        m_podeAdesivo = b.m_adesivo;
        
        for(int i = 0; i < m.materials.Length; i++)
        {
            if (b.m_exclusivoMetal)
            {
                if (i < m.materials.Length - 3) m.materials[i].color = m_cores[i].color;
                else if (i == m.materials.Length - 2)
                {
                    m.materials[i].SetFloat("_Metallic", 1);
                    m.materials[i].SetFloat("_Glossiness", 0.5f);
                    m.materials[i].color = Color.grey;
                }
                else if (i == m.materials.Length - 1)
                {
                    Adesivos a = Resources.Load("Testes/Scriptable Objects/Adesivos/Adesivo " + m_adesivoEscolhido.ToString()) as Adesivos;
                    m.materials[i].SetTexture("_MainTex", a.m_imagem.texture);
                    m.materials[i].color = m_corAdesivo;
                }
            }
            else
            {
                if (i < 3) m.materials[i].color = m_cores[i].color;
                else if (i == m.materials.Length - 1)
                {
                    Adesivos a = Resources.Load("Testes/Scriptable Objects/Adesivos/Adesivo " + m_adesivoEscolhido.ToString()) as Adesivos;
                    m.materials[i].SetTexture("_MainTex", a.m_imagem.texture);
                    m.materials[i].color = m_corAdesivo;
                }
            }
        }
    }
    #endregion

    #region Mudar Sprites do Botoes do Menu
    public void InicializarBotoesSpritesDoMenu(GameObject g)
    {
        GameObject b = g.transform.GetChild(0).GetChild(0).gameObject;
        b.GetComponent<SpriteRenderer>().sprite = m_usuario.m_baseLogo;
        b.GetComponent<SpriteRenderer>().color = m_usuario.m_corSecundaria;

        GameObject f = g.transform.GetChild(0).GetChild(1).gameObject;
        f.GetComponent<SpriteRenderer>().sprite = m_usuario.m_fundoLogo;
        f.GetComponent<SpriteRenderer>().color = m_usuario.m_corPrimaria;

        GameObject s = g.transform.GetChild(0).GetChild(2).gameObject;
        s.GetComponent<SpriteRenderer>().sprite = m_usuario.m_simboloLogo;
        s.GetComponent<SpriteRenderer>().color = m_usuario.m_corTerciaria;
    }

    #endregion

    public void MudarVisualizacaoTogglesAdesivo(bool b)
    {
        m_setaAntAdesivo.SetActive(true);
        m_setaProxAdesivo.SetActive(true);
        if(b == true)
        {
            posicaoTogglesAdesivo++;
            m_visualizarToggles.transform.position += Vector3.up * 70;
            if (posicaoTogglesAdesivo >= 2) m_setaProxAdesivo.SetActive(false);
        }
        else
        {
            posicaoTogglesAdesivo--;
            m_visualizarToggles.transform.position -= Vector3.up * 70;
            if (posicaoTogglesAdesivo <= 0) m_setaAntAdesivo.SetActive(false);
        }

        if (m_visualizarToggles.transform.position.y < 847) m_visualizarToggles.transform.position = new Vector3(589.6f, 847, 0);
        if(m_visualizarToggles.transform.position.y > 987) m_visualizarToggles.transform.position = new Vector3(589.6f, 987, 0);
    }
    public void MudarPaginaFormatos(bool b)
    {
        

        if (b == true)
        {
            m_botoesSecao.gameObject.SetActive(true);
            m_goleirosSecao.gameObject.SetActive(false);
        }
        else
        {
            m_botoesSecao.gameObject.SetActive(false);
            m_goleirosSecao.gameObject.SetActive(true);
        }
        IniciarSlots();
    }
    void AtualizarToggleAdesivos()
    {
        posicaoTogglesAdesivo = 0;
        m_visualizarToggles.transform.position = new Vector3(589.6f, 847, 0);
        foreach (GameObject t in m_adesivosToggles)
        {
            if (t.name == m_adesivosToggles[m_opcaoCorAdesivo].name) { t.GetComponent<Toggle>().isOn = true; break; }
        }
    }
    void AtualizarTogglesFormatos()
    {
        m_formatosToggles[0].GetComponent<Toggle>().isOn = true;
    }

    public void AtualizarShapeMenuBotoes()
    {
        m_playerMeshMenu.mesh = m_usuario.m_playerBotao.transform.GetChild(2).GetComponent<MeshFilter>().sharedMesh;
        m_playerMenu.transform.GetChild(0).localPosition = m_usuario.m_playerBotao.transform.GetChild(0).localPosition;
        m_goleiroMeshMenu.mesh = m_usuario.m_goleiroBotao.GetComponent<MeshFilter>().sharedMesh;
        m_goleiroMenu.transform.GetChild(0).localScale = m_usuario.m_goleiroBotao.transform.GetChild(0).localScale;

        m_playerMeshRendererMenu.materials[0].color = m_usuario.m_corPrimaria;
        m_playerMeshRendererMenu.materials[1].color = m_usuario.m_corSecundaria;
        m_playerMeshRendererMenu.materials[2].color = m_usuario.m_corTerciaria;
        m_goleiroMeshRendererMenu.materials[0].color = m_usuario.m_corPrimaria;
        m_goleiroMeshRendererMenu.materials[1].color = m_usuario.m_corTerciaria;
    }

    public void SetarVariaveis()
    {
        m_playerMeshMenu = m_playerMenu.GetComponent<MeshFilter>();
        m_playerMeshRendererMenu = m_playerMenu.GetComponent<Renderer>();
        m_goleiroMeshMenu = m_goleiroMenu.GetComponent<MeshFilter>();
        m_goleiroMeshRendererMenu = m_goleiroMenu.GetComponent<Renderer>();

        if (!m_usuario.m_usarLogo) m_playerMenu.transform.GetChild(0).gameObject.SetActive(false);
        if (m_usuario.m_podeAdesivo) m_podeAdesivo = true;
        else m_podeAdesivo = false;

        m_adesivoEscolhido = m_usuario.m_codigoAdesivo;
        m_opcaoCorAdesivo = m_usuario.m_opcaoCorAdesivo;
        MudarCorAdesivo(m_opcaoCorAdesivo);
        m_podecolorir = false;

        m_cor1Escolhida = m_usuario.m_codigoCorPrimaria;
        m_cor2Escolhida = m_usuario.m_codigoCorSecundaria;
        m_cor3Escolhida = m_usuario.m_codigoCorTerciaria;

        m_playerMeshRendererMenu.materials[0].color = m_cores[0].color = m_usuario.m_corPrimaria;
        m_playerMeshRendererMenu.materials[1].color = m_cores[1].color = m_usuario.m_corSecundaria;
        m_playerMeshRendererMenu.materials[2].color = m_cores[2].color = m_usuario.m_corTerciaria;
        m_goleiroMeshRendererMenu.materials[0].color = m_usuario.m_corPrimaria;
        m_goleiroMeshRendererMenu.materials[1].color = m_usuario.m_corTerciaria;

        m_codBotaoEscolhido = m_usuario.m_codigoBotao;
        m_codGoleiroEscolhido = m_usuario.m_codigoGoleiro;
    }

    public void SalvarMudancas()
    {
        //Salvar formatos
        if (m_codBotaoEscolhido == 0) m_codBotaoEscolhido = m_usuario.m_tipoPlayerBotaoOnline;
        if (m_codGoleiroEscolhido == 0) m_codGoleiroEscolhido = m_usuario.m_tipoGoleiroBotao;

        m_usuario.m_codigoBotao = m_codBotaoEscolhido;
        m_usuario.m_codigoGoleiro = m_codGoleiroEscolhido;
        m_usuario.m_tipoPlayerBotaoOnline = m_codBotaoEscolhido;
        m_usuario.m_tipoGoleiroBotao = m_codGoleiroEscolhido;

        //Salvar Cores
        //m_usuario.m_corPrimaria = new Color();
        //m_usuario.m_corSecundaria = new Color();
        //m_usuario.m_corTerciaria = new Color();

        m_usuario.m_corPrimaria = m_cores[0].color;
        m_usuario.m_corSecundaria = m_cores[1].color;
        m_usuario.m_corTerciaria = m_cores[2].color;
        m_usuario.m_codigoCorPrimaria = m_cor1Escolhida;
        m_usuario.m_codigoCorSecundaria = m_cor2Escolhida;
        m_usuario.m_codigoCorTerciaria = m_cor3Escolhida;

        //Salvar Texturas
        //Elementos

        //Salvar Adesivos
        m_usuario.m_opcaoCorAdesivo = m_opcaoCorAdesivo;
        m_usuario.m_codigoAdesivo = m_adesivoEscolhido;
        m_usuario.m_corAdesivo = m_corAdesivo;

        //SaveSystem.SavePlayer(m_usuario);
    }

    public void SairJogadorCustomization()
    {
        foreach(AtivosDefault a in FindObjectsOfType<AtivosDefault>())
        {
            a.SetarDefault();
            print(a.gameObject.name);
        }
        //SaveSystem.CarregarData();
        FindObjectOfType<EventsManager>().ClickInFirstScene("cena menu");
    }
}
