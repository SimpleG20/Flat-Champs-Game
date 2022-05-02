using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDecision : MonoBehaviour
{
    public enum Decisao
    {
        MOVER,
        CHUTAR,
        ESPECIAL
    }
    protected AIAction iAction;

    public void SetAction(AIAction action, Decisao acao)
    {
        iAction = action;
        switch (acao)
        {
            case Decisao.MOVER:
                iAction.Iniciar_Movimento();
                break;
            case Decisao.CHUTAR:
                Debug.Log("AI: Chute");
                break;
            case Decisao.ESPECIAL:
                Debug.Log("AI: Especial");
                break;
        }
        //StartCoroutine(action.Rotacionar_Alvo());
    }
}
