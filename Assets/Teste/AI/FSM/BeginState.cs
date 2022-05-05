using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginState : State
{
    public BeginState(StateSystem StateSystem, AISystem ai) : base(StateSystem, ai) 
    { }

    public override IEnumerator Estado_Start()
    {
        Debug.Log("Begin State");
        yield return new WaitForSeconds(2);

        int quemComeca = Random.Range(0, 2);

        if (quemComeca == 1) { _StateSystem.SetState(new PlayerTurnState(_StateSystem, _AiSystem)); _StateSystem._estadoAtual = StateSystem.Estado.ESPERANDO_JOGADOR; }
        else { _StateSystem.SetState(new AITurnState(_StateSystem, _AiSystem)); _StateSystem._estadoAtual = StateSystem.Estado.ESPERANDO_DECISAO; }
    }
}
