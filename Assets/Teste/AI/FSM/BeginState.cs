using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginState : State
{
    public BeginState(StateSystem system, AISystem ai) : base(system, ai) 
    { }

    public override IEnumerator Start()
    {
        Debug.Log("Begin State");
        yield return new WaitForSeconds(2);

        int quemComeca = Random.Range(0, 2);

        if (quemComeca == 1) _system.SetState(new PlayerTurnState(_system, _iSystem));
        else _system.SetState(new AITurnState(_system, _iSystem));
    }
}
