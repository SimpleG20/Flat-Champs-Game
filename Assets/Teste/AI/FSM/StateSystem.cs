using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSystem : StateMachine
{
    public enum Estado { NONE, ESPERANDO_JOGADOR, ESPERANDO_DECISAO, DECISAO_TOMADA, MOVER, CHUTAR, CHUTAR_GOL, ESPECIAL, GOLEIRO }

    public Estado _estadoAtual_AI;
    AISystem AiSystem;

    private void Start()
    {
        AiSystem = GetComponent<AISystem>();
    }

    private void Update()
    {
        /*if (comecar)
        {
            SetState(new BeginState(this, AiSystem));
            comecar = false;
        }*/

        /*if (moverPlayer)
        {
            moverPlayer = false;
            OnMover();
        }*/

        /*if (contagem)
        {
            tempoJogada += Time.deltaTime;
            if (tempoJogada >= 20) OnEnd();
        }*/
    }

    #region Estado conforme a situacao da Bola

    #endregion


    #region Execucao States
    public void OnDecisao()
    {
        AiSystem.TomarDecisao();
        _estadoAtual_AI = Estado.DECISAO_TOMADA;
    }

    public void OnMover()
    {
        StartCoroutine(_state.Estado_Mover());
    }

    public void OnChutarNormal()
    {
        _estadoAtual_AI = Estado.CHUTAR;
        StartCoroutine(_state.Estado_ChutarNormal());
    }

    public void OnChutar_ao_Gol()
    {
        Gameplay._current.SetSituacao("chute ao gol");
        _estadoAtual_AI = Estado.CHUTAR_GOL;
        StartCoroutine(_state.Estado_Chutar_ao_Gol());
    }

    public void OnEspecial()
    {
        Gameplay._current.SetSituacao("especial");
        _estadoAtual_AI = Estado.ESPECIAL;
        StartCoroutine(_state.Estado_Especial());
    }

    public void OnEsperar()
    {
        _estadoAtual_AI = Estado.ESPERANDO_JOGADOR;
        StartCoroutine(_state.Estado_Esperar());
    }

    public void OnGoleiro()
    {
        Gameplay._current.SetSituacao("pequena area");
        _estadoAtual_AI = Estado.GOLEIRO;
        StartCoroutine(_state.Estado_Goleiro());
    }

    public void OnEnd()
    {
        StartCoroutine(_state.Estado_End());
    }
    #endregion
}

