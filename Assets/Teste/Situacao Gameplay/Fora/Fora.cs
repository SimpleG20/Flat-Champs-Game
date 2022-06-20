using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fora : Situacao
{
    public Fora(Gameplay gameplay, VariaveisUIsGameplay ui, CamerasSettings camera) : base(gameplay, ui, camera)
    { }

    protected bool trocou;
    protected void SetarFora()
    {
        //Debug.Log("FORA GERAL INICIO");
        _gameplay._atual = Gameplay.Situacoes.FORA;
        UI_Inicio();
        LogisticaVars.saiuFora = false;
        LogisticaVars.continuaSendoFora = true;
        
        JogadorVars.m_aplicarChute = false;
        JogadorMetodos.ResetarValoresChute();

        EstadoJogo.JogoParado();
        EstadoJogo.TempoJogada(false);

        if (LogisticaVars.ultimoToque == 1 && LogisticaVars.vezJ1 || LogisticaVars.ultimoToque == 2 && LogisticaVars.vezJ2)
        { /*Debug.Log("FORA: TROCOU VEZ");*/ trocou = true; LogisticaVars.tempoJogada = 0; LogisticaVars.jogadas = 0; }

        if (LogisticaVars.tempoJogada > 15) LogisticaVars.tempoJogada = 14;
        if (LogisticaVars.jogadas > 1) LogisticaVars.jogadas--;

        if (trocou)
        {
            bool aux = LogisticaVars.vezJ1;
            LogisticaVars.vezJ1 = LogisticaVars.vezJ2;
            LogisticaVars.vezJ2 = aux;
        }
        /*LogisticaVars.vezJ1 = LogisticaVars.vezJ2 = false;
        if (LogisticaVars.ultimoToque == 1) LogisticaVars.vezJ2 = true;
        else LogisticaVars.vezJ1 = true;*/

        if (LogisticaVars.vezJ2 && _gameplay.modoPartida == Partida.Modo.JOGADOR_VERSUS_AI) LogisticaVars.vezAI = true;
        else LogisticaVars.vezAI = false;
        //_gameplay.GetStateSystem().OnEnd();

        SelecaoMetodos.DesabilitarDadosJogadorAtual();
        //SelecaoMetodos.DesabilitarDadosPlayer();
    }

    public override void Camera_Situacao(string s)
    {
        _camera.SituacoesCameras(s);
    }

    void UI_Inicio()
    {
        _ui.EstadoTodosOsBotoes(false);
        _ui.m_placar.SetActive(true);
        _ui.pausarBt.gameObject.SetActive(true);
    }
    public virtual void UI_Meio()
    {
        _ui.EstadoBotoesGoleiro(false);
        _ui.EstadoBotoesJogador(false);
        _ui.EstadoBotoesCentral(false);

        _ui.centralBotoes.SetActive(true);
        _ui.barraEspecial.SetActive(true);
        _ui.direcaoBolaBt.gameObject.SetActive(true);
        _ui.botaoBaixo.SetActive(true);
        _ui.botaoMeio.SetActive(true);
        _ui.joystick.SetActive(true);
    }

}
