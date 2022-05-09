using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoJogo : Situacao
{
    public EstadoJogo(Gameplay gameplay, VariaveisUIsGameplay ui, CamerasSettings camera) : base(gameplay, ui, camera)
    { }

    public override void UI_Situacao(string s)
    {
        base.UI_Situacao(s);
    }

    public static void JogoParado()
    {
        LogisticaVars.jogoParado = true;
    }

    public static void JogoNormal()
    {
        LogisticaVars.jogoParado = false;
    }

    public static void TempoJogada(bool b)
    {
        LogisticaVars.contarTempoJogada = b;
    }
}
