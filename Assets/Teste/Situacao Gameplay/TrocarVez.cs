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
        yield return new WaitUntil(() => !_gameplay._bola.m_bolaCorrendo);
        if (LogisticaVars.continuaSendoFora || LogisticaVars.tiroDeMeta) { Debug.Log("INTERRUPCAO TROCAR VEZ");  yield break; }

        Debug.Log("TROCAR VEZ");
        LogisticaVars.trocarVez = true;
        EstadoJogo.TempoJogada(false);
        bool aux = LogisticaVars.vezJ1;
        LogisticaVars.vezJ1 = LogisticaVars.vezJ2;
        LogisticaVars.vezJ2 = aux;

        LogisticaVars.tempoJogada = 0;
        LogisticaVars.jogadas = 0;

        //LogisticaVars.escolherOutroJogador = false;
        //LogisticaVars.desabilitouDadosJogador = false;

        yield return new WaitForSeconds(1);

        Trocar();
        SituacaoBolaRasteira();

        yield return new WaitForSeconds(1);
        _gameplay.Fim();
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

    void Trocar()
    { 
        SelecaoMetodos.DesabilitarDadosJogador();
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

    public override IEnumerator Fim()
    {
        LogisticaVars.trocarVez = false;
        return base.Fim();
    }
}
