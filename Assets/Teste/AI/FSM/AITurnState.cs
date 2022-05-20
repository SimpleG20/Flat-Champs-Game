using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurnState : State
{
    public AITurnState(Gameplay gameplay, StateSystem state, AISystem ai) : base(gameplay, state, ai)
    { }

    public override IEnumerator Estado_Start()
    {
        //Debug.Log("* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *");
        //Debug.Log("AI: TURN");
        Gameplay._current.rotacaoCamera.transform.position = _AiSystem.ai_player.transform.position - _AiSystem.ai_player.transform.up;
        
        yield return new WaitForSeconds(1);
        if (!LogisticaVars.aplicouPrimeiroToque)
        {
            Debug.Log("AI: Primeiro Toque");
            _StateSystem._estadoAtual_AI = StateSystem.Estado.MOVER;
            _AiSystem.SetDecisao(AISystem.Decisao.AVANCAR);
            _AiSystem.SetAction(new AIMovement(_AiSystem, _AiSystem.ai_player));
            yield break;
        }
        if (LogisticaVars.lateral)
        {
            Debug.Log("AI: Lateral");
            _StateSystem._estadoAtual_AI = StateSystem.Estado.CHUTAR;
            _AiSystem.SetDecisao(AISystem.Decisao.LATERAL);
            _AiSystem.SetAction(new AIStrike(_AiSystem, _AiSystem.ai_player));
            yield break;
        }
        if (LogisticaVars.tiroDeMeta)
        {
            Debug.Log("AI: Tiro de meta");
            _StateSystem._estadoAtual_AI = StateSystem.Estado.CHUTAR;
            _AiSystem.SetDecisao(AISystem.Decisao.CHUTE_GOLEIRO);
            _AiSystem.SetAction(new AIStrike(_AiSystem, _AiSystem.ai_player));
            yield break;
        }
        if (LogisticaVars.foraFundo)
        {
            Debug.Log("AI: Escanteio");
            _StateSystem._estadoAtual_AI = StateSystem.Estado.CHUTAR;
            _AiSystem.SetDecisao(AISystem.Decisao.ESCANTEIO);
            _AiSystem.SetAction(new AIStrike(_AiSystem, _AiSystem.ai_player));
            yield break;
        }
        _StateSystem._estadoAtual_AI = StateSystem.Estado.ESPERANDO_DECISAO;
        _StateSystem.OnEsperar();
    }

    public override IEnumerator Estado_Mover()
    {
        //Debug.Log("AI ESTADO: MOVER");
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
        Debug.Log("AI: Waiting");

        yield return new WaitForSeconds(0.5f);

        if (!LogisticaVars.vezAI || LogisticaVars.jogadas >= 3) { _StateSystem.OnEnd(); yield break; }
        if (_AiSystem.GetDecisao() == AISystem.Decisao.NONE || _AiSystem.GetDecisao() == AISystem.Decisao.AVANCAR || _AiSystem._passouBola)
        {
            //Debug.Log("AI: just waiting to choose an action");
            EventsManager.current.SelecaoAutomatica();
            Debug.Log("AI PLAYER: " + LogisticaVars.m_jogadorEscolhido_Atual.name);
            _StateSystem.OnDecisao();
            _AiSystem._passouBola = false;
            _AiSystem._novaDecisao = true;
            //Debug.Log("* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *");
            //Debug.Log("AI DECISAO: " + _AiSystem.GetDecisao());
            _StateSystem.OnMover();
            yield break;
        }
        else
        {

            int random = Random.Range(0, 3);
            if (random == 0)
            {
                _StateSystem.OnEsperar();
            }
            else
            {
                //Debug.Log("* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *");
                _StateSystem.OnMover();
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
        if (LogisticaVars.jogadas == 3 && !LogisticaVars.trocarVez) 
        { 
            Debug.Log("TROCA DE VEZ - AI TURN STATE"); 
            _Gameplay.SetSituacao("trocar vez");
            yield break;
        }
        Debug.Log("AI: TURN ENDED");
        Debug.Log("* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *");
        _StateSystem._estadoAtual_AI = StateSystem.Estado.ESPERANDO_JOGADOR;
        _AiSystem.SetDecisao(AISystem.Decisao.NONE);
        yield break;
    }
}
