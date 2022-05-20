using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnState : State
{
    public PlayerTurnState(Gameplay gameplay, StateSystem StateSystem, AISystem ai) : base(gameplay, StateSystem, ai) 
    { }

    public override IEnumerator Estado_Start()
    {
        /*_StateSystem.jogadas = 0;
        _StateSystem.tempoJogada = 0;
        _StateSystem.contagem = true;*/
        Debug.Log("");
        Debug.Log("* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *");
        Debug.Log("NEW PLAYER: TURN");
        yield break;
    }

    public override IEnumerator Estado_End()
    {
        Debug.Log("PLAYER: TURN ENDED");
        Debug.Log("* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *");
        //Substituir pelo LogisticaVars
        //_StateSystem.contagem = false;
        /*if (_Gameplay.modoPartida == Partida.Modo.JOGADOR_VERSUS_AI) _StateSystem.SetState(new AITurnState(_Gameplay, _StateSystem, _AiSystem));
        else _StateSystem.SetState(new PlayerTurnState(_Gameplay, _StateSystem, _AiSystem));*/
        yield break;
    }
}
