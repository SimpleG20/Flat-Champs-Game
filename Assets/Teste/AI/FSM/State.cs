using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected readonly Gameplay _Gameplay;
    protected readonly AISystem _AiSystem;
    protected readonly StateSystem _StateSystem;
    public State(Gameplay gameplay, StateSystem state, AISystem ai)
    {
        _Gameplay = gameplay;
        _StateSystem = state;
        _AiSystem = _Gameplay.GetAISystem();
    }

    public virtual IEnumerator Estado_Start()
    {
        yield break;
    }

    public virtual IEnumerator Estado_ChutarNormal()
    {
        yield break;
    }

    public virtual IEnumerator Estado_Chutar_ao_Gol()
    {
        yield break;
    }

    public virtual IEnumerator Estado_Mover()
    {
        yield break;
    }

    public virtual IEnumerator Estado_Esperar()
    {
        yield break;
    }

    public virtual IEnumerator Estado_Especial()
    {
        yield break;
    }

    public virtual IEnumerator Estado_Goleiro()
    {
        yield break;
    }

    public virtual IEnumerator Estado_End()
    {
        yield break;
    }
}