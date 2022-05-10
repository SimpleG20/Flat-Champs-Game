using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Gol : Situacao
{
    public Gol(Gameplay gameplay, VariaveisUIsGameplay ui, CamerasSettings camera) : base(gameplay, ui, camera)
    {
    }

    public override IEnumerator Inicio()
    {
        Debug.Log("SITUACAO: GOL");
        Camera_Situacao("inicio");
        EstadoJogo.JogoParado();
        EstadoJogo.TempoJogada(false);
        LogisticaVars.tempoJogada = LogisticaVars.tempoEscolherJogador = 0;
        LogisticaVars.jogadas = 0;

        
        EventsManager.current.OnGol("rotina animacao gol");
        yield return new WaitUntil(() => LogisticaVars.fimAnimacaoGol);
        _gameplay.Fim();
    }

    void ComputarGols()
    {
        if (LogisticaVars.golT1)
        {
            LogisticaVars.placarT1 += 1;
            LogisticaVars.vezJ1 = false;
            LogisticaVars.vezJ2 = true;
        }
        else
        {
            LogisticaVars.placarT2 += 1;
            LogisticaVars.vezJ1 = true;
            LogisticaVars.vezJ2 = false;
        }
    }
    void PosicionarJogadores()
    {
        for (int i = 0; i < _gameplay.abertura.QuantiaJogadores(); i++)
        {
            LogisticaVars.jogadoresT1[i].transform.position = 
                new Vector3(LogisticaVars.esquemaT1[i, 0], LogisticaVars.jogadoresT1[i].transform.position.y, LogisticaVars.esquemaT1[i, 1]);
            LogisticaVars.jogadoresT2[i].transform.position = 
                new Vector3(-LogisticaVars.esquemaT2[i, 0], LogisticaVars.jogadoresT2[i].transform.position.y, -LogisticaVars.esquemaT2[i, 1]);
        }
        GameObject.FindGameObjectWithTag("Goleiro1").transform.position = Gameplay._current.posGol1;
        GameObject.FindGameObjectWithTag("Goleiro2").transform.position = Gameplay._current.posGol2;
    }
    void ReiniciarPosGol()
    {
        _gameplay._bola.PosicionarAposGol();
        _gameplay._bola.RedirecionarJogadores(true);
        _gameplay._bola.RedirecionarGoleiros();

        LogisticaVars.bolaPermaneceNaPequenaArea = LogisticaVars.auxChuteAoGol = false;
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
        LogisticaVars.primeiraJogada = true;
        LogisticaVars.aplicouPrimeiroToque = false;

        Physics.gravity = Vector3.down * 9.81f;
        if (LogisticaVars.vezJ1) _gameplay.BarraEspecial(LogisticaVars.m_especialAtualT1, LogisticaVars.m_maxEspecial);
        else _gameplay.BarraEspecial(LogisticaVars.m_especialAtualT2, LogisticaVars.m_maxEspecial);
    }
    

    public override void UI_Situacao(string s)
    {
        switch (s)
        {
            case "inicio":
                _gameplay.canvas.transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 0;
                _ui.EstadoBotoesGoleiro(false);
                _ui.EstadoBotoesJogador(false);
                _ui.EstadoBotoesCentral(false);
                _ui.barraChuteJogador.SetActive(false);
                _ui.especialBt.gameObject.SetActive(false);
                break;
        }
    }
    public override IEnumerator Fim()
    {
        LogisticaVars.fimAnimacaoGol = false;
        _ui.golMarcadoGO.SetActive(false);
        ReiniciarPosGol();

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !_camera.GetPrincipal().IsBlending);
        //JogadorMetodos.ResetarValoresChute();
        _camera.GetPrincipal().m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;

        //ToquePosGol();
        //UI_Situacao("fim");
        _gameplay.canvas.transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 1;
        _gameplay.SetSituacao(new Comecar(_gameplay, _ui, _camera));
    }
}
