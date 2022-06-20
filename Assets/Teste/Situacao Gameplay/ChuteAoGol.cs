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
        Debug.Log("CHUTE AO GOL INICIO");
        EstadoJogo.TempoJogada(false);
        _camera.MudarBlendCamera(CinemachineBlendDefinition.Style.Cut);
        SelecaoMetodos.EscolherGoleiro();

        if (_gameplay.modoPartida == Partida.Modo.JOGADOR_VERSUS_AI && !LogisticaVars.goleiroT2 || _gameplay.modoPartida == Partida.Modo.JOGADO_VERSUS_JOGADOR)
        {
            if (_gameplay.VerificarIconesSelecao()) _gameplay.DestruirIconesSelecao();
            GoleiroMetodos.ComponentesParaGoleiro(true);
            UI_Goleiro();
        }
        else
        {
            _ui.EstadoBotoesJogador(false);
            _ui.EstadoBotoesCentral(false);
        }
        _ui.barraChuteJogador.SetActive(false);

        LogisticaVars.defenderGoleiro = true;
        if (_gameplay.modoPartida == Partida.Modo.JOGADOR_VERSUS_AI && LogisticaVars.goleiroT2)
        {
            Vector3 target = new Vector3(0, LogisticaVars.m_goleiroGameObject.transform.position.y, _gameplay.posGol2.z) + (Vector3.right * Mathf.Pow(-1, Random.Range(0, 2)) * Random.Range(0, 7f));
            Debug.Log(target);
            _gameplay.GetAISystem().MoverGoleiroDefender(target);
        }

        yield return new WaitForSeconds(8);
        if (LogisticaVars.goleiroT1 && LogisticaVars.defenderGoleiro || LogisticaVars.goleiroT2 && LogisticaVars.defenderGoleiro)
        {
            Debug.Log("Goleiro Posicionado Auto");
            _gameplay.GoleiroPosicionado();
        }
    }
    public override IEnumerator Meio()
    {
        if (_gameplay.modoPartida == Partida.Modo.JOGADOR_VERSUS_AI && !LogisticaVars.goleiroT2 || _gameplay.modoPartida == Partida.Modo.JOGADO_VERSUS_JOGADOR) GoleiroMetodos.ComponentesParaGoleiro(false);
        LogisticaVars.goleiroT1 = LogisticaVars.goleiroT2 = false;
        LogisticaVars.m_goleiroGameObject = null;

        LogisticaVars.defenderGoleiro = false;
        LogisticaVars.auxChuteAoGol = true;
        JogadorVars.m_chuteAoGol = true;

        EstadoJogo.JogoNormal();
        //EstadoJogo.TempoJogada(true);

        JogadorMetodos.ResetarValoresChute();
        EstadoJogo.TempoJogada(true);

        if (!LogisticaVars.vezAI) UI_Jogador();
        else
        {
            Debug.Log("CHUTE AO GOL: ESPERANDO AI CHUTAR");
            _ui.goleiroPosicionadoBt.gameObject.SetActive(false);
            _ui.UI_Espera();
            _gameplay.GetStateSystem().OnChutar_ao_Gol();
        }

        yield return new WaitUntil(() => !JogadorVars.m_chuteAoGol);
        Debug.Log("CHUTE AO GOL: Chutou");
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !_gameplay._bola.m_bolaCorrendo);

        EstadoJogo.TempoJogada(true);
        Debug.Log("CHUTE AO GOL: FIM");
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
        _ui.cameraEsperaBt.gameObject.SetActive(false);

        _ui.barraChuteGoleiro.SetActive(false);
        _ui.goleiroPosicionadoBt.gameObject.SetActive(true);
        _ui.joystick.SetActive(true);
    }
}
