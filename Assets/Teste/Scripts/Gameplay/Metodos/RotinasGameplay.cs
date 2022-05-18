using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RotinasGameplay : MonoBehaviour
{
    FisicaBola bola;
    VariaveisUIsGameplay ui;
    EventsManager events;

    private void Start()
    {
        ui = VariaveisUIsGameplay._current;

        events = EventsManager.current;
        events.onChuteAoGol += RotinasChuteAoGol;
        events.onGol += RotinasGol;
        events.onFora += RotinasFora;
        //events.onEspecial += RotinasEspecial;
        events.onGoleiro += RotinasGoleiro;
    }
    private void RotinasChuteAoGol(string s)
    {
        if (bola == null) bola = Gameplay._current._bola;
        switch (s)
        {
            case "rotina tempo chute ao gol":
                StartCoroutine(TempoParaChuteAoGol());
                break;
        }
    }
    private void RotinasFora(string s)
    {
        if (bola == null) bola = Gameplay._current._bola;
        switch (s)
        {
            case "rotina sair lateral":
                StartCoroutine(SairLateral());
                break;
            case "rotina sair escanteio":
                StartCoroutine(SairEscanteio());
                break;
            case "rotina tempo lateral":
                StartCoroutine(TempoParaLateral());
                break;
            case "rotina tempo escanteio":
                StartCoroutine(TempoParaEscanteio());
                break;
            case "rotina tempo tiro de meta":
                StartCoroutine(TempoParaTiroDeMeta());
                break;
        }
    }
    private void RotinasGoleiro(string s)
    {
        if (bola == null) bola = Gameplay._current._bola;
        switch (s)
        {
            case "rotina tempo pequena area":
                StartCoroutine(TempoParaTirarBolaDaPequenaArea());
                break;
            case "rotina pos chute goleiro":
                StartCoroutine(RotinaAposChuteGoleiro());
                break;
        }
    }
    private void RotinasGol(string s)
    {
        if (bola == null) bola = Gameplay._current._bola;
        switch (s)
        {
            case "rotina animacao torcida":
                StartCoroutine(AnimacaoTorcida());
                break;
        }
    }

    #region Chutes Automaticos
    IEnumerator TempoParaTirarBolaDaPequenaArea()
    {
        yield return new WaitForSeconds(9);
        if (LogisticaVars.bolaPermaneceNaPequenaArea)
        {
            GoleiroMetodos.ChuteAutomatico();
            LogisticaVars.bolaPermaneceNaPequenaArea = false;
        }
    }
    IEnumerator TempoParaTiroDeMeta()
    {
        yield return new WaitForSeconds(11);
        if (LogisticaVars.tiroDeMeta)
        {
            GoleiroMetodos.ChuteAutomatico();
        }
    }
    IEnumerator TempoParaLateral()
    {
        yield return new WaitForSeconds(9);
        if (LogisticaVars.lateral)
        {
            Debug.Log("Lateral Automatico");
            JogadorMetodos.AutoChuteLateral();
        }
    }
    IEnumerator TempoParaEscanteio()
    {
        yield return new WaitForSeconds(12);
        if (LogisticaVars.continuaSendoFora)
        {
            Debug.Log("Escanteio Automatico");
            JogadorVars.m_forca = 15;
            JogadorMetodos.AutoChuteEscanteio();
        }
    }
    IEnumerator TempoParaChuteAoGol()
    {
        yield return new WaitForSeconds(7);
        if (LogisticaVars.auxChuteAoGol)
        {
            Debug.Log("TempoParaChuteAoGol(): Jogadas = 3");
            LogisticaVars.jogadas = 3;
        }
        LogisticaVars.auxChuteAoGol = false;
    }
    #endregion

    #region Gol
    IEnumerator AnimacaoTorcida()
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !CamerasSettings._current.GetPrincipal().IsBlending);
        Debug.Log("Começando a animacao da torcida");

        //Fazer Torcida se Movimentar

        Player jogador1 = GameManager.Instance.m_usuario;

        if (LogisticaVars.golT1)
        {
            ui.golMarcadoGO.GetComponent<GolComponentes>().SpriteTimeMarcou(jogador1.m_baseLogo,
                jogador1.m_fundoLogo, jogador1.m_simboloLogo, jogador1.m_corPrimaria, jogador1.m_corSecundaria, jogador1.m_corTerciaria);
        }
        else
        {
            if (Gameplay._current.conexaoPartida == Partida.Conexao.OFFLINE)
            {
                Teams jogador2 = GameManager.Instance.GetTimeOff();
                ui.golMarcadoGO.GetComponent<GolComponentes>().SpriteTimeMarcou(jogador2.m_base,
                    jogador2.m_fundo, jogador2.m_simbolo, jogador2.m_corPrimaria, jogador2.m_corSecundaria, jogador2.m_corTerciaria);
            }
            else
            {

            }
        }
        ui.golMarcadoGO.SetActive(true);
        ui.golMarcadoGO.GetComponent<Animator>().SetBool("Gol", true);

        yield return new WaitForSeconds(0.5f);
        ui.golMarcadoGO.GetComponent<Animator>().SetBool("Gol", false);

        yield return new WaitForSeconds(4f);
        LogisticaVars.fimAnimacaoGol = true;
    }
    #endregion

    #region Goleiro
    IEnumerator RotinaAposChuteGoleiro()
    {
        Debug.Log("CAMERA: Tiro de meta");
        ui.EstadoTodosOsBotoes(false);
        ui.m_placar.SetActive(true);
        ui.pausarBt.gameObject.SetActive(true);
        CamerasSettings._current.SituacoesCameras("habilitar camera tiro de meta");
        LogisticaVars.bolaPermaneceNaPequenaArea = false;
        LogisticaVars.podeRedirecionar = true;
        LogisticaVars.fundo1 = LogisticaVars.fundo2 = LogisticaVars.tiroDeMeta = false;
        //LogisticaVars.continuaSendoFora = false;

        yield return new WaitForSeconds(1);
        StartCoroutine(PosChuteGoleiro());

        yield return new WaitForSeconds(0.5f);
        if (LogisticaVars.goleiroT2) LogisticaVars.m_goleiroGameObject.transform.position = new Vector3(0, LogisticaVars.m_goleiroGameObject.transform.position.y, Gameplay._current.abertura.PosicaoGoleiro(2).z);
        else LogisticaVars.m_goleiroGameObject.transform.position = new Vector3(0, LogisticaVars.m_goleiroGameObject.transform.position.y, Gameplay._current.abertura.PosicaoGoleiro(1).z);
        GoleiroMetodos.ComponentesParaGoleiro(false);

        LogisticaVars.goleiroT2 = LogisticaVars.goleiroT1 = false;
        LogisticaVars.m_goleiroGameObject = null;
    }
    IEnumerator PosChuteGoleiro()
    {
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => !bola.m_bolaCorrendo);
        /*if (!LogisticaVars.bolaRasteiraT1 && !LogisticaVars.bolaRasteiraT2) yield return new WaitUntil(() => bola.m_toqueChao);
        else yield return new WaitUntil(() => !bola.m_bolaCorrendo);*/
        Debug.Log("Bola relou no chao");

        LogisticaVars.bolaEntrouPequenaArea = false;
        LogisticaVars.bolaPermaneceNaPequenaArea = false;
        LogisticaVars.continuaSendoFora = false;
        JogadorMetodos.ResetarValoresChute();

        events.SelecaoAutomatica();
        CamerasSettings._current.SituacoesCameras("desabilitar camera tiro de meta");

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !CamerasSettings._current.GetPrincipal().IsBlending);
        bola.RedirecionarJogadores(true);
        EstadoJogo.TempoJogada(true);
        Gameplay._current.GetSituacao().UI_Normal();
    }
    #endregion

    #region Fora
    IEnumerator SairLateral()
    {
        LogisticaVars.lateral = false;
        ui.botaoMeio.SetActive(false);
        ui.lateralBt.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);
        Transform jogador = LogisticaVars.m_jogadorEscolhido_Atual.transform;

        Debug.Log("Deslocando Jogador");
        Vector3 target;
        if (jogador.position.x < 0) target = new Vector3(bola.m_posLateral.x + 2f, jogador.position.y, bola.m_posLateral.z);
        else target = new Vector3(bola.m_posLateral.x - 2f, jogador.position.y, bola.m_posLateral.z);
        LogisticaVars.m_rbJogadorEscolhido.isKinematic = true;
        StartCoroutine(MovimentoSairFora(target));
        yield break;
    }
    IEnumerator SairEscanteio()
    {
        ui.botaoMeio.SetActive(false);
        ui.escanteioBt.gameObject.SetActive(false);
        LogisticaVars.foraFundo = false;

        yield return new WaitForSeconds(1);
        Transform jogador = LogisticaVars.m_jogadorEscolhido_Atual.transform;

        Vector3 target = new Vector3(bola.m_posicaoFundo.x, jogador.position.y, bola.m_posicaoFundo.z);

        if(jogador.position.z < 0)
        {
            if (jogador.position.x < 0) target += Vector3.forward * 1.5f + Vector3.right * 1.5f;
            else target += Vector3.forward * 1.5f + Vector3.left * 1.5f;
        }
        else
        {
            if (jogador.position.x < 0) target += Vector3.back * 1.5f + Vector3.right * 1.5f;
            else target += Vector3.back * 1.5f + Vector3.left * 1.5f;
        }
        if (LogisticaVars.vezJ1) LogisticaVars.ultimoToque = 1;
        else LogisticaVars.ultimoToque = 2;
        LogisticaVars.m_rbJogadorEscolhido.isKinematic = true;
        StartCoroutine(MovimentoSairFora(target));
        yield break;
    }
    IEnumerator MovimentoSairFora(Vector3 target)
    {
        float fator = 0;

        yield return new WaitForSeconds(0.01f);
        fator = 0.3f;
        LogisticaVars.m_jogadorEscolhido_Atual.transform.position = Vector3.MoveTowards(LogisticaVars.m_jogadorEscolhido_Atual.transform.position, target, fator);

        if (LogisticaVars.m_jogadorEscolhido_Atual.transform.position != target) StartCoroutine(MovimentoSairFora(target));
        else
        {
            //JogadorMetodos.ResetarValoresChute();
            GameObject.Find("Bandeira Dir").transform.GetChild(0).gameObject.SetActive(true);
            GameObject.Find("Bandeira Esq").transform.GetChild(0).gameObject.SetActive(true);
            GameObject.Find("Bandeira Dir 2").transform.GetChild(0).gameObject.SetActive(true);
            GameObject.Find("Bandeira Esq 2").transform.GetChild(0).gameObject.SetActive(true);
            LogisticaVars.m_rbJogadorEscolhido.isKinematic = false;
            LogisticaVars.saiuFora = true;
            print("LATERAL: TERMINOU MOVIMENTO");
        }
    }
    
    #endregion
}