using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Situacao
{
    protected readonly Gameplay _gameplay;
    public Situacao(Gameplay gameplay)
    {
        _gameplay = gameplay;
    }

    public virtual IEnumerator Inicio()
    {
        yield break;
    }

    public virtual void UI_Situacao()
    {

    }

    public virtual void Camera_Situacao()
    {

    }

    public virtual IEnumerator Fim()
    {
        yield break;
    }
}
