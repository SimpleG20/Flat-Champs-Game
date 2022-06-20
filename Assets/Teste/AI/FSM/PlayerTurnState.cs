using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnState : State
{
    public PlayerTurnState(Gameplay gameplay, StateSystem StateSystem, AISystem ai) : base(gameplay, StateSystem, ai) 
    { }

    public override IEnumerator Estado_Start()
    {
        Debug.Log("");
        Debug.Log("* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *");
        Debug.Log("NEW PLAYER: TURN");
        yield break;
    }

    public override IEnumerator Estado_End()
    {
        Debug.Log("PLAYER: TURN ENDED");
        Debug.Log("* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *");
        yield break;
    }
}
