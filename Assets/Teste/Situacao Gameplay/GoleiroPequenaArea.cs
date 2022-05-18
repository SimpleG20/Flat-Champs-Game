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
        Debug.Log("PEQUENA AREA INICIO");
        UI_Situacao("inicio");
        GoleiroVars.chutou = false;
        SelecaoMetodos.DesabilitarDadosJogador();

        if (_gameplay._bola.m_pos.z < 0)
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

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !_camera.GetPrincipal().IsBlending);
        EventsManager.current.OnGoleiro("rotina tempo pequena area");
        UI_Situacao("meio");

        yield return new WaitUntil(() => GoleiroVars.chutou);
        UI_Situacao("inicio");
        _gameplay.Fim();
    }

    public override void UI_Situacao(string s)
    {
        switch (s)
        {
            case "inicio":
                _ui.EstadoTodosOsBotoes(false);
                _ui.m_placar.SetActive(true);
                _ui.pausarBt.gameObject.SetActive(true);
                break;
            case "meio":
                _ui.EstadoBotoesJogador(false);
                _ui.EstadoBotoesGoleiro(true);
                _ui.selecionarJogadorBt.gameObject.SetActive(false);
                break;
        }
        
    }
}
