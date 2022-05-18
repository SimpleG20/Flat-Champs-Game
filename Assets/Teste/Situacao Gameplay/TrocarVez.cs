using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrocarVez : Situacao
{
    public TrocarVez(Gameplay gameplay, VariaveisUIsGameplay ui, CamerasSettings camera) : base(gameplay, ui, camera)
    {
    }

    public override IEnumerator Inicio()
    {
        Debug.Log("TROCAR VEZ INICIO");
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !_gameplay._bola.m_bolaCorrendo);
        if (LogisticaVars.continuaSendoFora || LogisticaVars.tiroDeMeta || LogisticaVars.gol) { Debug.Log("INTERRUPCAO TROCAR VEZ");  yield break; }

        Debug.Log("COMECAR TROCA TROCA");
        LogisticaVars.trocarVez = true;
        LogisticaVars.numControle++;
        EstadoJogo.TempoJogada(false);

        LogisticaVars.tempoJogada = 0;
        LogisticaVars.jogadas = 0;
        Camera_Situacao();
        UI_Situacao("trocar");

        yield return new WaitForSeconds(1);
        Trocar();
        SituacaoBolaRasteira();

        yield return new WaitForSeconds(1);
        EstadoJogo.TempoJogada(true);
        UI_Normal();
        _gameplay.Fim();
    }
    void Trocar()
    {
        bool aux = LogisticaVars.vezJ1;
        LogisticaVars.vezJ1 = LogisticaVars.vezJ2;
        LogisticaVars.vezJ2 = aux;
        //if (LogisticaVars.vezJ1) Debug.Log("Time 1");
        //if (LogisticaVars.vezJ2) Debug.Log("Time 2");

        if (LogisticaVars.vezJ2 && _gameplay.modoPartida == Partida.Modo.JOGADOR_VERSUS_AI) UI_Situacao("");
        else SelecaoMetodos.DesabilitarDadosJogador();
        _gameplay._bola.RedirecionarJogadores(true);
        EventsManager.current.EscolherJogador();
        SelecaoMetodos.DadosJogador();
        SelecaoMetodos.DesabilitarComponentesDosNaoSelecionados();
    }
    void SituacaoBolaRasteira()
    {
        if (LogisticaVars.vezJ1 && LogisticaVars.bolaRasteiraT1 || LogisticaVars.vezJ2 && LogisticaVars.bolaRasteiraT2)
        {
            _ui.direcaoBolaBt.isOn = true;
            VariaveisUIsGameplay._current.UI_BolaRasteira();
        }
        else if (LogisticaVars.vezJ1 && !LogisticaVars.bolaRasteiraT1 || LogisticaVars.vezJ2 && !LogisticaVars.bolaRasteiraT2)
        {
            _ui.direcaoBolaBt.isOn = false;
            VariaveisUIsGameplay._current.UI_BolaRasteira();
        }
    }

    public override void UI_Situacao(string s)
    {
        switch (s)
        {
            case "trocar":
                _ui.EstadoBotoesGoleiro(false);
                _ui.EstadoBotoesJogador(false);
                _ui.EstadoBotoesCentral(false);
                _ui.barraChuteJogador.SetActive(false);
                _ui.especialBt.gameObject.SetActive(false);
                break;
            case "trocou":
                UI_Normal();
                break;
        }
    }
    public override void Camera_Situacao(string s = default)
    {
        _camera.MudarBlendCamera(Cinemachine.CinemachineBlendDefinition.Style.Cut);
    }
    public override IEnumerator Fim()
    {
        LogisticaVars.trocarVez = false;
        return base.Fim();
    }
}
