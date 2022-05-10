using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoleiroPequenaArea : Situacao
{
    public GoleiroPequenaArea(Gameplay gameplay, VariaveisUIsGameplay ui, CamerasSettings camera) : base(gameplay, ui, camera)
    {
    }

    public override IEnumerator Inicio()
    {
        if(_gameplay._bola.m_pos.z < 0)
        {
            if (!LogisticaVars.vezJ1)
            {
                if (LogisticaVars.ultimoToque == 2)
                {
                    LogisticaVars.vezJ2 = false;
                    LogisticaVars.vezJ1 = true;
                    LogisticaVars.tempoJogada = 0;
                    LogisticaVars.jogadas = 0;
                }
            }
            LogisticaVars.goleiroT1 = true;
            LogisticaVars.m_goleiroGameObject = GameObject.FindGameObjectWithTag("Goleiro1");
        }
        else
        {
            if (!LogisticaVars.vezJ2)
            {
                if (LogisticaVars.ultimoToque == 1)
                {
                    LogisticaVars.vezJ1 = false;
                    LogisticaVars.vezJ2 = true;
                    LogisticaVars.tempoJogada = 0;
                    LogisticaVars.jogadas = 0;
                }
            }
            LogisticaVars.goleiroT2 = true;
            LogisticaVars.m_goleiroGameObject = GameObject.FindGameObjectWithTag("Goleiro2");
        }
        GoleiroMetodos.ComponentesParaGoleiro(true);
        SelecaoMetodos.DesabilitarDadosJogador();
        EventsManager.current.OnGoleiro("rotina tempo pequena area");

        yield return new WaitUntil(() => GoleiroVars.chutou);
        _gameplay.Fim();
    }

    public override void UI_Situacao(string s)
    {
        base.UI_Situacao(s);
    }
    public override void Camera_Situacao(string s)
    {
        _camera.SituacoesCameras(s);
    }
    public override IEnumerator Fim()
    {
        LogisticaVars.bolaPermaneceNaPequenaArea = false;
        LogisticaVars.podeRedirecionar = true;
        LogisticaVars.fundo1 = LogisticaVars.fundo2 = LogisticaVars.tiroDeMeta = false;
        GoleiroMetodos.ComponentesParaGoleiro(false);

        yield return new WaitForSeconds(0.5f);
        if (LogisticaVars.goleiroT2) LogisticaVars.m_goleiroGameObject.transform.position = new Vector3(0, LogisticaVars.m_goleiroGameObject.transform.position.y, _gameplay.posGol2.z);
        else LogisticaVars.m_goleiroGameObject.transform.position = new Vector3(0, LogisticaVars.m_goleiroGameObject.transform.position.y, _gameplay.posGol1.z);

        LogisticaVars.goleiroT2 = LogisticaVars.goleiroT1 = false;
        LogisticaVars.m_goleiroGameObject = null;

        yield return new WaitForSeconds(1);
        if (!LogisticaVars.bolaRasteiraT1 && !LogisticaVars.bolaRasteiraT2) yield return new WaitUntil(() => _gameplay._bola.m_toqueChao);
        else yield return new WaitUntil(() => !_gameplay._bola.m_bolaCorrendo);

        Debug.Log("FIM GOLEIRO");
        EstadoJogo.TempoJogada(true);
        LogisticaVars.continuaSendoFora = false;
        LogisticaVars.tiroDeMeta = false;
        JogadorMetodos.ResetarValoresChute();

        _gameplay._bola.RedirecionarJogadores(true);
        EventsManager.current.SelecaoAutomatica();
        base.Fim();
    }
}
