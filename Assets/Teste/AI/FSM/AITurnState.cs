using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurnState : State
{
    public AITurnState(StateSystem system) : base(system) { }

    public override IEnumerator Start()
    {
        _system.contagem = true;
        Debug.Log("AI: Choose");
        yield return new WaitForSeconds(1);

        _system.OnEsperar();
    }

    public override IEnumerator Direcionar()
    {
        float randomAngulo = Random.Range(0, 90);
        Debug.Log("AI: Random Direcionamento " + randomAngulo);
        yield return new WaitForSeconds(1);

        _system.OnMover();
    }

    public override IEnumerator Mover()
    {
        float randomForca = Random.Range(50, 420);
        Debug.Log("AI: Random Forca " + randomForca);
        _system.jogadas++;

        yield return new WaitForSeconds(1);

        if (_system.jogadas == 3)
        {
            _system.OnEnd();
        }
        else
        {
            _system.OnEsperar();
        }
    }

    public override IEnumerator Especial()
    {
        Debug.Log("AI: Especial");
        _system.OnEnd();
        yield break;
    }

    public override IEnumerator Chutar()
    {
        Debug.Log("AI: Chute");
        _system.OnEnd();
        yield break;
    }

    public override IEnumerator Esperar()
    {
        Debug.Log("AI: Nothing, just waiting");
        yield return new WaitForSeconds(2);

        int random = Random.Range(0, 4);

        switch (random)
        {
            case 0:
                _system.OnEsperar();
                break;
            case 1:
                _system.OnDirecionar();
                break;
            case 2:
                _system.OnChutar();
                break;
            case 3:
                _system.OnEspecial();
                break;
        }
    }

    public override IEnumerator End()
    {
        base.End();
        _system.SetState(new PlayerTurnState(_system));
        yield break;
    }
}
