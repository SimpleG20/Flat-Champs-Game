using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameplayOff : MonoBehaviour
{
    /* Abreviacoes:
        MJ: movimentacaoDoJogador
        MG: movimentacaoDoGoleiro
        UI: UIMetodosGameplay
     */
    bool comecarContagemJogada, comecarContagemSelecao;

    GameObject canvas;
    Vector3 posGol1, posGol2;

    CinemachineBrain camPrincipal;
    FisicaBola bola;
    EventsManager events;
    UIMetodosGameplay ui;

    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<UIMetodosGameplay>();
        bola = FindObjectOfType<FisicaBola>();
        camPrincipal = FindObjectOfType<CinemachineBrain>();

        canvas = GameObject.Find("Canvas");
        posGol1 = GameObject.FindGameObjectWithTag("Gol1").transform.position;
        posGol2 = GameObject.FindGameObjectWithTag("Gol2").transform.position;

        events = EventsManager.current;
        events.onSituacaoGameplay += Situacoes;
        events.onAplicarMetodosUiComBotao += UiMetodos;

        ToqueInicial_ou_PosGol();
        LogisticaVars.m_especialAtualT1 = LogisticaVars.m_especialAtualT2 = 0;
        BarraEspecial(0, LogisticaVars.m_maxEspecial);
        LogisticaVars.jogoParado = false;
    }

    void Situacoes(string situacao)
    {
        switch (situacao)
        {
            case "jogo normal":
                Segue_O_Jogo();
                break;
            case "jogo parado":
                JogoParado();
                break;
            case "fora lateral":
                LogisticaVars.continuaSendoFora = true;
                ForaGeral();
                ForaLateral();
                break;
            case "fora escanteio":
                LogisticaVars.continuaSendoFora = true;
                ForaGeral();
                ForaEscanteio();
                break;
            case "fora tiro de meta":
                LogisticaVars.continuaSendoFora = true;
                LogisticaVars.tiroDeMeta = true;
                ForaGeral();
                ForaTiroDeMeta();
                break;
            case "gol marcado":
                GolMarcado();
                break;
            case "reiniciar pos gol":
                ReiniciarPosGol();
                break;
            case "aplicar toque inicial":
                AplicarToqueInicialAuto();
                break;
            case "toque pos gol":
                ToqueInicial_ou_PosGol();
                break;
            case "verificar acionamento especial":
                VerificarSeAcionaEspecial();
                break;
            case "fim especial":
                FimEspecial();
                break;
        }
    }

    void UiMetodos(string metodo)
    {
        switch (metodo)
        {
            case "pausar jogo":
                PararJogo();
                break;
            case "despausar jogo":
                DespausarJogo();
                break;
            case "quit jogo":
                QuitarJogo();
                break;
            case "direcionar jogador":
                DirecionamentoAuto();
                break;
            case "selecionar outro":
                SelecionarOutroJogador();
                break;
            case "jogador selecionado":
                JogadorSelecionado();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        events.OnAtualizarNumeros();
        if(LogisticaVars.jogoComecou && !LogisticaVars.jogoParado) LogisticaVars.tempoCorrido += Time.deltaTime;

        TempoJogo();

        if (LogisticaVars.especialT1Disponivel && LogisticaVars.vezJ1 || LogisticaVars.especialT2Disponivel && LogisticaVars.vezJ2) ui.especialBt.interactable = true;
        else ui.especialBt.interactable = false;

        if (comecarContagemSelecao)
        {
            LogisticaVars.tempoEscolherJogador += Time.deltaTime;
        }


        if (comecarContagemJogada)
        {
            LogisticaVars.tempoJogada += Time.deltaTime;

            if (LogisticaVars.vezJ1)
            {
                if (!LogisticaVars.especial) LogisticaVars.m_especialAtualT1 += Time.deltaTime * 5;

                if (LogisticaVars.m_especialAtualT1 >= LogisticaVars.m_maxEspecial && !LogisticaVars.especialT1Disponivel) 
                { LogisticaVars.m_especialAtualT1 = LogisticaVars.m_maxEspecial;  LogisticaVars.especialT1Disponivel = true; }
                BarraEspecial(LogisticaVars.m_especialAtualT1, LogisticaVars.m_maxEspecial);
            }
            else
            {
                if (!LogisticaVars.especial) LogisticaVars.m_especialAtualT2 += Time.deltaTime * 0f; //mudar para 0.5f
                
                if(LogisticaVars.m_especialAtualT2 >= LogisticaVars.m_maxEspecial && !LogisticaVars.especialT2Disponivel)
                { LogisticaVars.m_especialAtualT2 = LogisticaVars.m_maxEspecial; LogisticaVars.especialT2Disponivel = true; }
                BarraEspecial(LogisticaVars.m_especialAtualT2, LogisticaVars.m_maxEspecial);
            }
        }

        if (LogisticaVars.tempoJogada >= 20) events.OnTrocarVez();
    }

    void TempoJogo()
    {
        LogisticaVars.segundosCorridos = Mathf.RoundToInt(LogisticaVars.tempoCorrido - (60 * LogisticaVars.minutosCorridos));
        if (LogisticaVars.tempoCorrido - (60 * LogisticaVars.minutosCorridos) >= 60)
        {
            LogisticaVars.minutosCorridos++;
        }
    }
    void PararJogo()
    {
        print("pause");
        Time.timeScale = 0;
        events.OnAplicarMetodosUiSemBotao("pause");
        LogisticaVars.jogoParado = true;
        comecarContagemJogada = false;
    }
    void DespausarJogo()
    {
        print("unpause");
        Time.timeScale = 1;
        events.OnAplicarMetodosUiSemBotao("unpause");
        LogisticaVars.jogoParado = false;
        comecarContagemJogada = true;
    }
    void QuitarJogo()
    {
        print("Quitar");
    }

    void DirecionamentoAuto()
    {
        if (ui.rotacaoAutoBt.isOn)
        {
            ui.rotacaoAutoBt.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            LogisticaVars.redirecionamentoAutomatico = true;
            //bola.RedirecionarJogadorEscolhido(bola.transform);
        }
        else
        {
            ui.rotacaoAutoBt.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            LogisticaVars.redirecionamentoAutomatico = false;
        }
    }

    

    #region Situacoes Gameplay

    void Segue_O_Jogo()
    {
        LogisticaVars.jogoParado = false;
        comecarContagemJogada = true;
    }
    void JogoParado()
    {
        comecarContagemJogada = false;
    }

    #region Inicio
    void AplicarToqueInicialAuto()
    {
        comecarContagemJogada = true;
        Vector3 dir = LogisticaVars.m_jogadorEscolhido.transform.position - bola.transform.position;
        bola.m_rbBola.AddForce(dir * 2, ForceMode.Impulse);
        LogisticaVars.aplicouPrimeiroToque = LogisticaVars.jogoComecou = true;
        LogisticaVars.ultimoToque = LogisticaVars.vezJ1 ? 1 : 2;
        LogisticaVars.jogadas++;
        EventsManager.current.OnAtualizarNumeros();
        events.OnAplicarMetodosUiSemBotao("estados dos botoes", "normal");
    }
    void ToqueInicial_ou_PosGol()
    {
        events.OnAplicarRotinas("rotina chute inicial");

        events.EscolherJogador();
        SelecaoMetodos.DadosJogador();
        SelecaoMetodos.DesabilitarComponentesDosNaoSelecionados();

        StartCoroutine(EsperarTransicaoCameraInicio());
    }
    IEnumerator EsperarTransicaoCameraInicio()
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !FindObjectOfType<CinemachineBrain>().IsBlending);

        ui.m_placar.SetActive(true);
        events.OnAplicarMetodosUiSemBotao("estados dos botoes", "primeiro toque");
        FindObjectOfType<FollowWorld>().lookAt = LogisticaVars.m_jogadorEscolhido.transform;
        JogadorMetodos.ResetarValoresChute();

        FindObjectOfType<CinemachineBrain>().m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
    }
    #endregion

    #region Selecao
    void SelecionarOutroJogador()
    {
        LogisticaVars.tempoEscolherJogador = 0;
        comecarContagemSelecao = true;
    }
    void JogadorSelecionado()
    {
        comecarContagemSelecao = false;
        LogisticaVars.tempoEscolherJogador = 0;
    }
    #endregion

    #region Especial
    void BarraEspecial(float atual, float max)
    {
        ui.barraEspecial.GetComponent<Image>().fillAmount = atual / max;
    }
    void VerificarSeAcionaEspecial()
    {
        float distanciaJogadorGol, distanciaBolaGol;
        bool especialPronto = false;

        if (LogisticaVars.vezJ1)
        {
            distanciaBolaGol = (bola.transform.position - posGol2).magnitude;
            distanciaJogadorGol = (LogisticaVars.m_jogadorEscolhido.transform.position - posGol2).magnitude;
            if (LogisticaVars.especialT1Disponivel) especialPronto = true;
        }
        else
        {
            distanciaBolaGol = (bola.transform.position - posGol1).magnitude;
            distanciaJogadorGol = (LogisticaVars.m_jogadorEscolhido.transform.position - posGol1).magnitude;
            if (LogisticaVars.especialT2Disponivel) especialPronto = true;
        }

        if (distanciaBolaGol < distanciaJogadorGol && bola.m_vetorDistanciaDoJogador.magnitude < 3.2f && 
            !LogisticaVars.continuaSendoFora && !LogisticaVars.auxChuteAoGol && especialPronto) AcionaEspecial();
        else print("Posicione melhor para Acionar o Especial");
    }
    void AcionaEspecial()
    {
        LogisticaVars.aplicouEspecial = false;
        JogoParado();
        LogisticaVars.especial = true;

        if (LogisticaVars.vezJ1)
        {
            LogisticaVars.especialT1Disponivel = false; 
            LogisticaVars.m_especialAtualT1 = 0;
            LogisticaVars.m_jogadorEscolhido.transform.LookAt(posGol2);
        }
        else
        {
            LogisticaVars.especialT2Disponivel = false; 
            LogisticaVars.m_especialAtualT2 = 0;
            LogisticaVars.m_jogadorEscolhido.transform.LookAt(posGol1);
        }

        LogisticaVars.m_jogadorEscolhido.transform.eulerAngles = new Vector3(-90, LogisticaVars.m_jogadorEscolhido.transform.eulerAngles.y, LogisticaVars.m_jogadorEscolhido.transform.eulerAngles.z);

        if (LogisticaVars.vezJ1) GameObject.FindGameObjectWithTag("Direcao Especial").transform.position = posGol2;
        else GameObject.FindGameObjectWithTag("Direcao Especial").transform.position = posGol1;

        Instantiate(ui.miraEspecial, FindObjectOfType<Camera>().WorldToScreenPoint(GameObject.FindGameObjectWithTag("Direcao Especial").transform.position,
            Camera.MonoOrStereoscopicEye.Mono), Quaternion.identity, canvas.transform.GetChild(2));


        LogisticaVars.cameraJogador.m_Priority = 0;
        LogisticaVars.cameraJogador = LogisticaVars.m_jogadorEscolhido.transform.GetChild(1).GetChild(2).GetComponent<CinemachineVirtualCamera>();
        LogisticaVars.cameraJogador.m_Priority = 99;

        ui.especialBt.gameObject.SetActive(false);
        ui.travarMiraBt.gameObject.SetActive(true);
        events.SituacaoGameplay("acionar camera especial");
        events.OnAplicarRotinas("rotina tempo especial");
    }
    void FimEspecial()
    {
        bola.GetComponent<Rigidbody>().useGravity = true;
        Physics.gravity = Vector3.down * 9.81f;
        Destroy(GameObject.FindGameObjectWithTag("Mira Especial"));
        Destroy(GameObject.FindGameObjectWithTag("Trajetoria Especial"));
        events.OnAplicarMetodosUiSemBotao("fim especial");
        LogisticaVars.cameraJogador.m_Priority = 0;
        LogisticaVars.cameraJogador = LogisticaVars.m_jogadorEscolhido.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>();
        LogisticaVars.cameraJogador.m_Priority = 99;

        LogisticaVars.especial = false;
        //LogisticaVars.aplicouEspecial = true;
    }
    #endregion

    #region Gol
    void GolMarcado()
    {
        canvas.transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 0;
        JogoParado();
        comecarContagemJogada = false;
        LogisticaVars.tempoJogada = LogisticaVars.tempoEscolherJogador = 0;
        LogisticaVars.jogadas = 0;
        LogisticaVars.jogoParado = true;

        events.OnAplicarRotinas("rotina animacao gol");
        if (LogisticaVars.golT1)
        {
            LogisticaVars.placarT1 += 1;
            LogisticaVars.vezJ1 = false;
            LogisticaVars.vezJ2 = true;
        }
        if (LogisticaVars.golT2)
        {
            LogisticaVars.placarT2 += 1;
            LogisticaVars.vezJ1 = true;
            LogisticaVars.vezJ2 = false;
        }
    } //Ativar Rotina da animacao do gol, computa os gols e posiciona os jogadores
    void ReiniciarPosGol()
    {
        bola.PosicionarAposGol();
        bola.RedirecionarJogadores(true);
        bola.RedirecionarGoleiros();

        LogisticaVars.bolaPermaneceNaPequenaArea = LogisticaVars.auxChuteAoGol = LogisticaVars.acionouChuteAoGol = false;
        LogisticaVars.lateral = LogisticaVars.foraFundo = false;
        LogisticaVars.continuaSendoFora = false;

        LogisticaVars.primeiraJogada = true;
        LogisticaVars.aplicouPrimeiroToque = false;
        LogisticaVars.jogoComecou = false;

        LogisticaVars.bolaEntrouPequenaArea = false;
        LogisticaVars.gol = false;
        LogisticaVars.golT2 = false;
        LogisticaVars.golT1 = false;
        LogisticaVars.goleiroT1 = LogisticaVars.goleiroT2 = false;
        LogisticaVars.posGol = false;
        LogisticaVars.jogadaDepoisGol = true;
        LogisticaVars.primeiraJogada = true;
        LogisticaVars.aplicouPrimeiroToque = false;

        Physics.gravity = Vector3.down * 9.81f;
        if (LogisticaVars.vezJ1) BarraEspecial(LogisticaVars.m_especialAtualT1, LogisticaVars.m_maxEspecial);
        else BarraEspecial(LogisticaVars.m_especialAtualT2, LogisticaVars.m_maxEspecial);
    }
    #endregion

    public static void BolaNaPequenaArea(int i)
    {
        Debug.Log("Bola permanece na pequena Area");
        EventsManager.current.OnAplicarMetodosUiSemBotao("estado jogador e goleiro", "", false);
        //EventsManager.current.OnAplicarMetodosUiSemBotao("estados dos botoes", "bola na pequena area"); //.EstadoBotoesJogador(false);
        if (i == 1 && !LogisticaVars.vezJ1)
        {
            if (LogisticaVars.ultimoToque == 2)
            {
                LogisticaVars.vezJ2 = false;
                LogisticaVars.vezJ1 = true;
                LogisticaVars.tempoJogada = 0;
                LogisticaVars.jogadas = 0;
            }

            LogisticaVars.goleiroT1 = true;
            LogisticaVars.m_goleiroGameObject = GameObject.FindGameObjectWithTag("Goleiro1");
        }
        else if (i == 1 && LogisticaVars.vezJ1)
        {
            LogisticaVars.goleiroT1 = true;
            LogisticaVars.m_goleiroGameObject = GameObject.FindGameObjectWithTag("Goleiro1");
        }
        else if (i == 2 && !LogisticaVars.vezJ2)
        {
            if (LogisticaVars.ultimoToque == 1)
            {
                LogisticaVars.vezJ1 = false;
                LogisticaVars.vezJ2 = true;
                LogisticaVars.tempoJogada = 0;
                LogisticaVars.jogadas = 0;
            }

            LogisticaVars.goleiroT2 = true;
            LogisticaVars.m_goleiroGameObject = GameObject.FindGameObjectWithTag("Goleiro2");
        }
        else if (i == 2 && LogisticaVars.vezJ2)
        {
            LogisticaVars.goleiroT2 = true;
            LogisticaVars.m_goleiroGameObject = GameObject.FindGameObjectWithTag("Goleiro2");
        }

        GoleiroMetodos.ComponentesParaGoleiro(true);
        SelecaoMetodos.DesabilitarDadosJogador();
        EventsManager.current.OnAplicarMetodosUiSemBotao("estados dos botoes", "bola pequena area");
        //EstadosDosBotoesEmCertasSituacoes("bola pequena area");
    }

    #region Fora
    void ForaLateral()
    {
        events.OnAplicarRotinas("rotina tempo lateral");
    }
    void ForaEscanteio()
    {
        if (LogisticaVars.fundo1 && LogisticaVars.ultimoToque == 1)
        {
            Debug.Log("Escanteio");
        }
        if (LogisticaVars.fundo2 && LogisticaVars.ultimoToque == 2) //Fundo2
        {
            Debug.Log("Escanteio");
        }
        LogisticaVars.foraFundo = false;
        events.OnAplicarRotinas("rotina tempo escanteio");
    }
    void ForaTiroDeMeta()
    {
        if(LogisticaVars.fundo1 && LogisticaVars.ultimoToque != 1)
        {
            LogisticaVars.vezJ1 = true;
            LogisticaVars.vezJ2 = false;
        }

        if (LogisticaVars.fundo2 && LogisticaVars.ultimoToque != 2)
        {
            LogisticaVars.vezJ2 = true;
            LogisticaVars.vezJ1 = false;
        }

        events.OnAplicarRotinas("rotina tempo tiro de meta");
    }
    void ForaGeral()
    {
        if (!LogisticaVars.gol)
        {
            JogadorMetodos.ResetarValoresChute();
            JogadorVars.m_aplicarChute = false;

            events.OnAplicarMetodosUiComBotao("desistir selecao");

            print("Fora");
            JogoParado();
            LogisticaVars.jogoParado = true;
            comecarContagemJogada = false;

            if (LogisticaVars.ultimoToque == 1 && LogisticaVars.vezJ1 || LogisticaVars.ultimoToque == 2 && LogisticaVars.vezJ2) { LogisticaVars.tempoJogada = 0; LogisticaVars.jogadas = 0; }

            if (LogisticaVars.tempoJogada > 15) LogisticaVars.tempoJogada = 14;
            if (LogisticaVars.jogadas != 0 && LogisticaVars.jogadas != 1) LogisticaVars.jogadas--;

            LogisticaVars.vezJ1 = LogisticaVars.vezJ2 = false;
            if (LogisticaVars.ultimoToque == 1) LogisticaVars.vezJ2 = true;
            else LogisticaVars.vezJ1 = true;

            events.OnAplicarMetodosUiSemBotao("estados dos botoes", "camera tiro de meta");

            SelecaoMetodos.DesabilitarDadosJogador();
            if (!LogisticaVars.tiroDeMeta) EventsManager.current.SelecaoAutomatica();
            events.SituacaoGameplay("habilitar camera fora");
        }
    }
    #endregion

    #endregion


    public static float Modulo(float numero)
    {
        float modulo = 0;

        modulo = Mathf.Sqrt(Mathf.Pow(numero, 2));
        return modulo;
    }
}
