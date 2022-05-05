using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    protected State _state;

    public void SetState(State state)
    {
        _state = state;
        StartCoroutine(_state.Estado_Start());
    }
}
