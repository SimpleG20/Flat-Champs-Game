using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginState : State
{
    public BeginState(StateSystem system) : base(system) { }

    public override IEnumerator Start()
    {
        Debug.Log("Begin State");
        yield return new WaitForSeconds(2);

        int quemComeca = Random.Range(0, 2);

        if (quemComeca == 1) _system.SetState(new PlayerTurnState(_system));
        else _system.SetState(new AITurnState(_system));
    }
}
