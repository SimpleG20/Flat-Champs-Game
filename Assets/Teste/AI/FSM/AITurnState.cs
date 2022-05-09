using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurnState : State
{
    public AITurnState(StateSystem state, AISystem ai) : base(state, ai)
    { }

    public override IEnumerator Estado_Start()
    {
        LogisticaVars.m_jogadorEscolhido_Atual = _AiSystem.ai_player;
        _StateSystem.jogadas = 0;
        _StateSystem.tempoJogada = 0;
        _StateSystem.contagem = true;
        GameObject.Find("RotacaoCamera").transform.position = _AiSystem.ai_player.transform.position - _AiSystem.ai_player.transform.up;
        Debug.Log("-----------------------------------------");
        Debug.Log("AI: TURN");
        yield return new WaitForSeconds(1);

        _StateSystem._estadoAtual = StateSystem.Estado.ESPERANDO_DECISAO;
        _StateSystem.OnEsperar();
    }

    public override IEnumerator Estado_Mover()
    {
        Debug.Log("AI ESTADO: MOVER");
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
        yield break;
    }

    public override IEnumerator Estado_Esperar()
    {
        yield return new WaitForSeconds(0.5f);

        if (_StateSystem.tempoJogada >= 20 || _StateSystem.jogadas >= 3 /*LogisticaVars.tempoJogada >= 20*/) _StateSystem.OnEnd();
        else
        {
            if(_AiSystem.GetDecisao() == AISystem.Decisao.NONE || _AiSystem.GetDecisao() == AISystem.Decisao.AVANCAR || _AiSystem._passouBola)
            {
                //Debug.Log("-----------------------------------------");
                Debug.Log("AI: just waiting to choose an action");
                //Escolher o jogador mais perto da bola
                LogisticaVars.m_jogadorEscolhido_Atual = _AiSystem.ai_player = LogisticaVars.m_jogadorAi =
                    SelecaoMetodos.QuemEstaMaisPerto(_AiSystem.bola.transform.position, _AiSystem.jogadorAmigo_MaisPerto);
                Debug.Log("AI PLAYER: " + LogisticaVars.m_jogadorEscolhido_Atual.name);
                _StateSystem.OnDecisao();
                _AiSystem._passouBola = false;
                _AiSystem._novaDecisao = true;
                Debug.Log("-----------------------------------------");
                Debug.Log("AI DECISAO: " + _AiSystem.GetDecisao());
                _StateSystem.OnMover();
                yield break;
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
                    Debug.Log("-----------------------------------------");
                    _StateSystem.OnMover();
                }
            }
            
        }
    }

    public override IEnumerator Estado_Goleiro()
    {
        _AiSystem.SetAction(new AIStrike(_AiSystem, _AiSystem.ai_player));
        yield break;
    }

    public override IEnumerator Estado_End()
    {
        _StateSystem._estadoAtual = StateSystem.Estado.ESPERANDO_JOGADOR;
        _AiSystem.SetDecisao(AISystem.Decisao.NONE);
        _StateSystem.contagem = false;
        _StateSystem.SetState(new PlayerTurnState(_StateSystem, _AiSystem));
        yield break;
    }
}
