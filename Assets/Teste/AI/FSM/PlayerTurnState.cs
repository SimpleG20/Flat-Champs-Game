using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnState : State
{
    public PlayerTurnState(StateSystem StateSystem, AISystem ai) : base(StateSystem, ai) 
    { }

    public override IEnumerator Estado_Start()
    {
        _StateSystem.jogadas = 0;
        _StateSystem.tempoJogada = 0;
        _StateSystem.contagem = true;
        Debug.Log("PLAYER: TURN");
        yield break;
    }

    public override IEnumerator Estado_Mover()
    {
        float randomForca = Random.Range(50, 420);
        Debug.Log("PLAYER: Random Forca " + randomForca);
        _StateSystem.jogadas++;

        yield return new WaitForSeconds(1);

        if (_StateSystem.jogadas >= 3)
        {
            _StateSystem.OnEnd();
        }
        else
        {
            _StateSystem.OnEsperar();
        }
    }

    public override IEnumerator Estado_Esperar()
    {
        Debug.Log("PLAYER: Waiting players action");
        yield break;
    }

    public override IEnumerator Estado_End()
    {
        _StateSystem._estadoAtual = StateSystem.Estado.ESPERANDO_DECISAO;
        _StateSystem.contagem = false;
        _StateSystem.SetState(new AITurnState(_StateSystem, _AiSystem));
        yield break;
    }
}
