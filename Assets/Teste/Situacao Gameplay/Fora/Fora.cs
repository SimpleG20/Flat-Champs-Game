using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fora : Situacao
{
    public Fora(Gameplay gameplay, VariaveisUIsGameplay ui, CamerasSettings camera) : base(gameplay, ui, camera)
    { }

    public override IEnumerator Inicio()
    {
        _gameplay._atual = Gameplay.Situacoes.FORA;

        LogisticaVars.continuaSendoFora = true;
        JogadorMetodos.ResetarValoresChute();
        JogadorVars.m_aplicarChute = false;

        Debug.Log("Fora");
        EstadoJogo.JogoParado();
        EstadoJogo.TempoJogada(false);

        LogisticaVars.vezJ1 = LogisticaVars.vezJ2 = false;
        if (LogisticaVars.ultimoToque == 1 && LogisticaVars.vezJ1 || LogisticaVars.ultimoToque == 2 && LogisticaVars.vezJ2) //Trocou Vez
        { LogisticaVars.tempoJogada = 0; LogisticaVars.jogadas = 0; }

        if (LogisticaVars.tempoJogada > 15) LogisticaVars.tempoJogada = 14;
        if (LogisticaVars.jogadas > 1) LogisticaVars.jogadas--;
        if (LogisticaVars.ultimoToque == 1) LogisticaVars.vezJ2 = true;
        else LogisticaVars.vezJ1 = true;
        SelecaoMetodos.DesabilitarDadosJogador();

        UI_Inicio();
        //events.SituacaoGameplay("habilitar camera fora");
        Debug.Log("Fora");
        yield break;
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
    public virtual void UI_Fim()
    {
        _ui.EstadoBotoesGoleiro(false);
        _ui.EstadoBotoesJogador(true);
        _ui.especialBt.gameObject.SetActive(true);
        _ui.barraChuteJogador.SetActive(true);
        _ui.barraEspecial.SetActive(true);
        _ui.centralBotoes.SetActive(true);

        _ui.numeroJogadasGO.SetActive(true);
        _ui.tempoEscolhaGO.SetActive(true);
        _ui.tempoJogadaGO.SetActive(true);
        _ui.pausarBt.gameObject.SetActive(true);
        _ui.sairSelecaoBt.gameObject.SetActive(false);
        _ui.especialBt.interactable = true;

        _ui.escanteioBt.gameObject.SetActive(false);
        _ui.lateralBt.gameObject.SetActive(false);
    }
}
