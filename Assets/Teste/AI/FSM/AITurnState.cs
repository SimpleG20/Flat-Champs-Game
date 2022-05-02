using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurnState : State
{
    public AITurnState(StateSystem system, AISystem ai) : base(system, ai)
    { }

    public override IEnumerator Start()
    {
        _system.contagem = true;
        Debug.Log("AI: Choose");
        yield return new WaitForSeconds(1);

        _system.OnEsperar();
    }

    public override IEnumerator Mover()
    {
        AIDecision decisao = new AIDecision();
        decisao.SetAction(new AIMovement(_iSystem), AIDecision.Decisao.MOVER);

        yield break;
    }

    public override IEnumerator Especial()
    {
        AIDecision decisao = new AIDecision();
        //decisao.SetAction(new AISpecial(_iSystem), AIDecision.Decisao.ESPECIAL);
        _system.OnEnd();
        yield break;
    }

    public override IEnumerator Chutar()
    {
        AIDecision decisao = new AIDecision();
        //decisao.SetAction(new AIStrike(_iSystem), AIDecision.Decisao.CHUTAR);
        _system.OnEnd();
        yield break;
    }

    public override IEnumerator Esperar()
    {
        Debug.Log("AI: Nothing, just waiting");

        //Escolher o Jogador mais perto da bola

        yield return new WaitForSeconds(2);

        int random = Random.Range(0, 4);

        switch (random)
        {
            case 0:
                _system.OnEsperar();
                break;
            case 1:
                _system.OnMover();
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
        _system.SetState(new PlayerTurnState(_system, _iSystem));
        yield break;
    }
}
