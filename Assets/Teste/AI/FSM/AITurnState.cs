using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurnState : State
{
    public AITurnState(StateSystem state, AISystem ai) : base(state, ai)
    { }

    public override IEnumerator Estado_Start()
    {
        _StateSystem.jogadas = 0;
        _StateSystem.tempoJogada = 0;
        _StateSystem.contagem = true;
        Debug.Log("--------------------------------------");
        Debug.Log("AI: TURN");
        yield return new WaitForSeconds(1);

        _StateSystem._estadoAtual = StateSystem.Estado.ESPERANDO_DECISAO;
        _StateSystem.OnEsperar();
    }

    public override IEnumerator Estado_Mover()
    {
        Debug.Log("AI: MOVER");
        _AiSystem.SetAction(new AIMovement(_AiSystem, _AiSystem.ai_player));
        yield break;
    }

    public override IEnumerator Estado_ChutarNormal()
    {
        _AiSystem.SetAction(new AIStrike(_AiSystem, _AiSystem.ai_player));
        //_StateSystem.OnEnd();
        yield break;
    }

    public override IEnumerator Estado_Chutar_ao_Gol()
    {
        _AiSystem.SetAction(new AIStrike(_AiSystem, _AiSystem.ai_player));
        //_StateSystem.OnChutar_ao_Gol();
        yield break;
    }

    public override IEnumerator Estado_Especial()
    {
        _AiSystem.SetAction(new AISpecial(_AiSystem, _AiSystem.ai_player));
        //_StateSystem.OnEnd();
        yield break;
    }

    public override IEnumerator Estado_Esperar()
    {
        yield return new WaitForSeconds(0.5f);

        if (_StateSystem.tempoJogada >= 20/*LogisticaVars.tempoJogada >= 20*/) _StateSystem.OnEnd();
        else
        {
            if(_AiSystem.GetDecisao() == AISystem.Decisao.NONE)
            {
                Debug.Log("AI: just waiting to choose an action");
                //Escolher o jogador mais perto da bola
                _StateSystem.OnDecisao();
                Debug.Log(_AiSystem.GetDecisao());
                _StateSystem.OnMover();
            }
            else
            {
                int random = Random.Range(0, 3);
                if(random == 0)
                {
                    _StateSystem.OnEsperar();
                }
                else
                {
                    _StateSystem.OnMover();
                }
            }
            
        }
    }

    public override IEnumerator Estado_End()
    {
        _StateSystem._estadoAtual = StateSystem.Estado.ESPERANDO_JOGADOR;
        _StateSystem.contagem = false;
        _StateSystem.SetState(new PlayerTurnState(_StateSystem, _AiSystem));
        yield break;
    }
}
