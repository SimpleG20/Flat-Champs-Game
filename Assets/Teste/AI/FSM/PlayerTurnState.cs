using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnState : State
{
    public PlayerTurnState(StateSystem system) : base(system) { }

    public override IEnumerator Start()
    {
        _system.contagem = true;
        Debug.Log("Jogador: Waiting for an action");
        yield break;
    }

    public override IEnumerator Direcionar()
    {
        float randomAngulo = Random.Range(0, 90);
        Debug.Log("Jogador: Random Direcionamento " + randomAngulo);
        yield return new WaitForSeconds(1);

        _system.OnEsperar();
    }

    public override IEnumerator Mover()
    {
        float randomForca = Random.Range(50, 420);
        Debug.Log("Jogador: Random Forca " + randomForca);
        _system.jogadas++;

        yield return new WaitForSeconds(1);

        if (_system.jogadas >= 3)
        {
            _system.OnEnd();
        }
        else
        {
            _system.OnEsperar();
        }
    }

    public override IEnumerator Esperar()
    {
        Debug.Log("Jogador: Waiting players action");
        yield break;
    }

    public override IEnumerator End()
    {
        base.End();
        _system.SetState(new AITurnState(_system));
        yield break;
    }
}
