using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSystem : StateMachine
{
    public enum Estado{ ESPERANDO_JOGADOR, ESPERANDO_DECISAO, DECISAO_TOMADA, MOVER, CHUTAR, CHUTAR_GOL, ESPECIAL }

    public int jogadas;
    public float tempoJogada;
    public bool contagem, comecar;

    public Estado _estadoAtual;


    AISystem AiSystem;

    private void Awake()
    {
        AiSystem = FindObjectOfType<AISystem>();
    }

    private void Update()
    {
        if (comecar)
        {
            SetState(new BeginState(this, AiSystem));
            comecar = false;
        }

        if (contagem)
        {
            tempoJogada += Time.deltaTime;
            if (tempoJogada >= 20) OnEnd();
        }
    }

    #region Estado conforme a situacao da Bola
    
    #endregion


    #region Execucao States
    public void OnDecisao()
    {
        AiSystem.TomarDecisao();
    }

    [ContextMenu("Mover Player")]
    public void OnMover()
    {
        StartCoroutine(_state.Estado_Mover());
    }

    public void OnChutarNormal()
    {
        _estadoAtual = Estado.CHUTAR;
        StartCoroutine(_state.Estado_ChutarNormal());
    }

    public void OnChutar_ao_Gol()
    {
        _estadoAtual = Estado.CHUTAR_GOL;
        StartCoroutine(_state.Estado_Chutar_ao_Gol());
    }

    public void OnEspecial()
    {
        _estadoAtual = Estado.ESPECIAL;
        StartCoroutine(_state.Estado_Especial());
    }

    public void OnEsperar()
    {
        _estadoAtual = Estado.ESPERANDO_JOGADOR;
        StartCoroutine(_state.Estado_Esperar());
    }

    public void OnEnd()
    {
        StartCoroutine(_state.Estado_End());
    }
    #endregion
}

