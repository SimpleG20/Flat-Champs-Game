using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDecision : MonoBehaviour
{
    protected AIAction iAction;

    public void SetAction(AIAction action)
    {
        iAction = action;
        iAction.IniciarAction();
    }
}
