using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpecial : AIAction
{
    public AISpecial(AISystem AiSystem, GameObject ai) : base(AiSystem, ai)
    {
    }

    public override void IniciarAction()
    {
        ai_System.GetStateSystem().jogadas = 3;
    }
}
