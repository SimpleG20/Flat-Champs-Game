using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ChuteAoGol : Situacao
{
    public ChuteAoGol(Gameplay gameplay, VariaveisUIsGameplay ui, CamerasSettings camera) : base(gameplay, ui, camera)
    { }

    public override IEnumerator Inicio()
    {
        EstadoJogo.TempoJogada(false);
        _camera.MudarBlendCamera(CinemachineBlendDefinition.Style.Cut);
        SelecaoMetodos.EscolherGoleiro();

        GoleiroMetodos.ComponentesParaGoleiro(true);
        _ui.barraChuteJogador.SetActive(false);
        UI_Goleiro();

        LogisticaVars.defenderGoleiro = true;

        yield return new WaitForSeconds(8);
        if (LogisticaVars.goleiroT1 && LogisticaVars.defenderGoleiro || LogisticaVars.goleiroT2 && LogisticaVars.defenderGoleiro)
        {
            Debug.Log("Goleiro Posicionado Auto");
            _gameplay.GoleiroPosicionado();
        }
    }
    public override IEnumerator Meio()
    {
        EstadoJogo.JogoNormal();
        EstadoJogo.TempoJogada(true);

        GoleiroMetodos.ComponentesParaGoleiro(false);
        LogisticaVars.goleiroT1 = LogisticaVars.goleiroT2 = false;
        LogisticaVars.m_goleiroGameObject = null;

        LogisticaVars.defenderGoleiro = false;
        LogisticaVars.auxChuteAoGol = true;
        JogadorVars.m_chuteAoGol = true;

        JogadorMetodos.ResetarValoresChute();

        UI_Jogador();

        yield return new WaitUntil(() => /*esperar jogador chutar*/true);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !_gameplay._bola.m_bolaCorrendo);

        if (!LogisticaVars.continuaSendoFora && !LogisticaVars.bolaPermaneceNaPequenaArea) { LogisticaVars.jogadas = 3; Fim(); }
        else yield break;
    }

    public override void UI_Situacao(string s)
    {
        switch (s)
        {
            case "jogador":
                UI_Jogador();
                break;
            case "goleiro":
                UI_Goleiro();
                break;
        }
    }
    void UI_Jogador()
    {
        _ui.EstadoBotoesJogador(false);
        _ui.EstadoBotoesGoleiro(false);
        _ui.goleiroPosicionadoBt.gameObject.SetActive(false);
        _ui.especialBt.gameObject.SetActive(false);
        _ui.botaoDiagonal.SetActive(false);

        _ui.botaoMeio.SetActive(true);
        _ui.botaoLivre2.SetActive(true);
        _ui.moverJogadorBt.SetActive(true);
        _ui.mostrarDirecionalBolaBt.gameObject.SetActive(true);

        _ui.barraChuteJogador.SetActive(true);
    }
    void UI_Goleiro()
    {
        _ui.EstadoBotoesJogador(false);
        _ui.EstadoBotoesGoleiro(false);
        _ui.EstadoBotoesCentral(false);
        _ui.especialBt.gameObject.SetActive(false);

        _ui.barraChuteGoleiro.SetActive(false);
        _ui.goleiroPosicionadoBt.gameObject.SetActive(true);
        _ui.joystick.SetActive(true);
    }
}
