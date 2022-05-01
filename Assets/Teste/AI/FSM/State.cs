using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected readonly StateSystem _system;
    public State(StateSystem system)
    {
        _system = system;
    }

    public virtual IEnumerator Start()
    {
        yield break;
    }

    public virtual IEnumerator Chutar()
    {
        yield break;
    }

    public virtual IEnumerator Mover()
    {
        yield break;
    }

    public virtual IEnumerator Direcionar()
    {
        yield break;
    }

    public virtual IEnumerator Esperar()
    {
        yield break;
    }

    public virtual IEnumerator Especial()
    {
        yield break;
    }

    public virtual IEnumerator End()
    {
        _system.jogadas = 0;
        _system.contagem = false;
        _system.tempoJogada = 0;
        yield break;
    }
}