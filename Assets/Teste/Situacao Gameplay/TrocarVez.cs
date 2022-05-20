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
        EstadoJogo.TempoJogada(false);
        UI_Situacao("trocar");
        Debug.Log("TROCAR VEZ INICIO");

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !_gameplay._bola.m_bolaCorrendo);
        if (LogisticaVars.continuaSendoFora || LogisticaVars.tiroDeMeta || LogisticaVars.gol) { Debug.Log("INTERRUPCAO TROCAR VEZ");  yield break; }

        //Debug.Log("COMECAR TROCA TROCA");
        LogisticaVars.trocarVez = true;
        LogisticaVars.numControle++;
        LogisticaVars.tempoJogada = 0;
        LogisticaVars.jogadas = 0;
        Camera_Situacao();

        yield return new WaitForSeconds(1);
        Trocar();
        SituacaoBolaRasteira();

        yield return new WaitForSeconds(1);
        UI_Normal();
        _gameplay.Fim();
    }
    void Trocar()
    {
        Debug.Log("V1 :" + LogisticaVars.vezJ1 + " - V2 :" + LogisticaVars.vezJ2 + " - V_AI :" + LogisticaVars.vezAI);
        bool aux = LogisticaVars.vezJ1;
        LogisticaVars.vezJ1 = LogisticaVars.vezJ2;
        LogisticaVars.vezJ2 = aux;
        if (LogisticaVars.vezJ2 && _gameplay.modoPartida == Partida.Modo.JOGADOR_VERSUS_AI) LogisticaVars.vezAI = true;
        else LogisticaVars.vezAI = false;

        Debug.Log("V1 :" + LogisticaVars.vezJ1 + " - V2 :" + LogisticaVars.vezJ2 + " - V_AI :" + LogisticaVars.vezAI);

        _gameplay.GetStateSystem().OnEnd();

        if (!LogisticaVars.vezAI && LogisticaVars.m_jogadorPlayer != null)
        {
            Debug.Log("TROCAR VEZ: Continuar jogador");
            Debug.Log(LogisticaVars.m_jogadorEscolhido_Atual.name);
            SelecaoMetodos.DesabilitarDadosJogador(); // jogador anterior
            LogisticaVars.m_jogadorEscolhido_Atual = LogisticaVars.m_jogadorPlayer;
            Debug.Log(LogisticaVars.m_jogadorEscolhido_Atual.name);
            SelecaoMetodos.DadosJogador(); // novo
        }
        else
        {
            EventsManager.current.SelecaoAutomatica();
        }
        _gameplay._bola.RedirecionarJogadores(true);
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
        }
    }
    public override void Camera_Situacao(string s = default)
    {
        _camera.MudarBlendCamera(Cinemachine.CinemachineBlendDefinition.Style.Cut);
    }
    public override IEnumerator Fim()
    {
        if (LogisticaVars.vezAI) _gameplay.GetStateSystem().SetState(new AITurnState(_gameplay, _gameplay.GetStateSystem(), _gameplay.GetAISystem()));
        else 
        { 
            _gameplay.GetStateSystem().SetState(new PlayerTurnState(_gameplay, _gameplay.GetStateSystem(), _gameplay.GetAISystem()));
            _gameplay.AjeitarBarraChute();
            JogadorMetodos.ResetarValoresChute();
        }

        LogisticaVars.trocarVez = false;
        EstadoJogo.TempoJogada(true);
        /*Debug.Log("V1 : " + LogisticaVars.vezJ1);
        Debug.Log("V2 : " + LogisticaVars.vezJ2);*/
        return base.Fim();
    }
}
