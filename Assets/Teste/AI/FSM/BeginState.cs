using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginState : State
{
    public BeginState(Gameplay gameplay, StateSystem StateSystem, AISystem ai) : base(gameplay, StateSystem, ai) 
    { }

    public override IEnumerator Estado_Start()
    {
        Debug.Log("BEGIN STATE");

        if(_Gameplay.modoPartida == Partida.Modo.JOGADOR_VERSUS_AI)
        {
            if (LogisticaVars.vezJ1) { _StateSystem.SetState(new PlayerTurnState(_Gameplay, _StateSystem, _AiSystem)); _StateSystem._estadoAtual_AI = StateSystem.Estado.ESPERANDO_JOGADOR; }
            else { _StateSystem.SetState(new AITurnState(_Gameplay, _StateSystem, _AiSystem)); _StateSystem._estadoAtual_AI = StateSystem.Estado.ESPERANDO_DECISAO; }
        }
        else
        {
            _StateSystem.SetState(new PlayerTurnState(_Gameplay, _StateSystem, _AiSystem)); 
            _StateSystem._estadoAtual_AI = StateSystem.Estado.ESPERANDO_JOGADOR;
        }
        yield break;
    }
}
