using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSystem : StateMachine
{
    public int jogadas;
    public float tempoJogada;
    public bool contagem;

    private void Start()
    {
        SetState(new BeginState(this));
    }

    private void Update()
    {
        if (contagem)
        {
            tempoJogada += Time.deltaTime;
            if (tempoJogada >= 20) OnEnd();
        }
    }


    #region Execucao State
    [ContextMenu("Mover Player")]
    public void OnMover()
    {
        StartCoroutine(_state.Mover());
    }

    public void OnChutar()
    {
        StartCoroutine(_state.Chutar());
    }

    public void OnEspecial()
    {
        StartCoroutine(_state.Especial());
    }

    public void OnEsperar()
    {
        StartCoroutine(_state.Esperar());
    }

    public void OnDirecionar()
    {
        StartCoroutine(_state.Direcionar());
    }

    public void OnEnd()
    {
        _state.End();
    }
    #endregion
}

