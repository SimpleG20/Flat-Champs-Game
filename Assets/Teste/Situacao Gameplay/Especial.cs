using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Especial : Situacao
{
    public Especial(Gameplay gameplay, VariaveisUIsGameplay ui, CamerasSettings camera) : base(gameplay, ui, camera)
    {
    }

    bool aplicarEspecial;

    public override IEnumerator Inicio()
    {
        VerificarSeAcionaEspecial();

        if (aplicarEspecial)
        {
            AcionaEspecial();

            if (_gameplay.modoPartida == Partida.Modo.JOGADOR_VERSUS_AI)
            {
                if (LogisticaVars.vezJ1) { _gameplay.InstanciarMira(); UI_Situacao("acionou"); }
            }
            else { _gameplay.InstanciarMira(); UI_Situacao("acionou"); }

            Camera_Situacao("inicio");

            yield return new WaitForSeconds(LogisticaVars.m_maxEspecial);
            if (!LogisticaVars.aplicouEspecial)
            {
                FimEspecial();
                Camera_Situacao("fim");
                LogisticaVars.aplicouEspecial = true;
            }
            _gameplay.Fim();
        }
        else
        {
            _gameplay.Fim();
        }
    }

    void VerificarSeAcionaEspecial()
    {
        float distanciaJogadorGol, distanciaBolaGol;
        bool especialPronto = false;

        if (LogisticaVars.vezJ1)
        {
            distanciaBolaGol = (_gameplay._bola.transform.position - _gameplay.posGol2).magnitude;
            distanciaJogadorGol = (LogisticaVars.m_jogadorEscolhido_Atual.transform.position - _gameplay.posGol2).magnitude;
            if (LogisticaVars.especialT1Disponivel) especialPronto = true;
        }
        else
        {
            distanciaBolaGol = (_gameplay._bola.transform.position - _gameplay.posGol1).magnitude;
            distanciaJogadorGol = (LogisticaVars.m_jogadorEscolhido_Atual.transform.position - _gameplay.posGol1).magnitude;
            if (LogisticaVars.especialT2Disponivel) especialPronto = true;
        }

        if (distanciaBolaGol < distanciaJogadorGol && _gameplay._bola.m_vetorDistanciaDoJogador.magnitude < 3.2f &&
            !LogisticaVars.continuaSendoFora && !LogisticaVars.auxChuteAoGol && especialPronto) aplicarEspecial = true;
        else { Debug.Log("POSICIONE-SE MELHOR PARA ATIVAR O ESPECIAL"); aplicarEspecial = false; }
    }
    void AcionaEspecial()
    {
        LogisticaVars.aplicouEspecial = false;
        EstadoJogo.TempoJogada(false);
        EstadoJogo.JogoParado();
        LogisticaVars.especial = true;

        if (LogisticaVars.vezJ1)
        {
            LogisticaVars.especialT1Disponivel = false;
            LogisticaVars.m_especialAtualT1 = 0;
            LogisticaVars.m_jogadorEscolhido_Atual.transform.LookAt(_gameplay.posGol2);
        }
        else
        {
            LogisticaVars.especialT2Disponivel = false;
            LogisticaVars.m_especialAtualT2 = 0;
            LogisticaVars.m_jogadorEscolhido_Atual.transform.LookAt(_gameplay.posGol1);
        }

        LogisticaVars.m_jogadorEscolhido_Atual.transform.eulerAngles = 
            new Vector3(-90, LogisticaVars.m_jogadorEscolhido_Atual.transform.eulerAngles.y, LogisticaVars.m_jogadorEscolhido_Atual.transform.eulerAngles.z);

        if (LogisticaVars.vezJ1) GameObject.FindGameObjectWithTag("Direcao Especial").transform.position = _gameplay.posGol2;
        else GameObject.FindGameObjectWithTag("Direcao Especial").transform.position = _gameplay.posGol1;
    }
    void FimEspecial()
    {
        _gameplay._bola.GetComponent<Rigidbody>().useGravity = true;
        Physics.gravity = Vector3.down * 9.81f;
        _gameplay.DestruirObjetosEspecial();
        LogisticaVars.especial = false;
    }

    public override void Camera_Situacao(string s)
    {
        switch (s)
        {
            case "inicio":
                LogisticaVars.cameraJogador.m_Priority = 0;
                LogisticaVars.cameraJogador = LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(1).GetChild(2).GetComponent<CinemachineVirtualCamera>();
                LogisticaVars.cameraJogador.m_Priority = 99;
                break;
            case "fim":
                LogisticaVars.cameraJogador.m_Priority = 0;
                LogisticaVars.cameraJogador = LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>();
                LogisticaVars.cameraJogador.m_Priority = 99;
                break;
        }
    }
    public override void UI_Situacao(string s)
    {
        switch (s)
        {
            case "acionou":
                _ui.EstadoBotoesJogador(false);
                _ui.barraChuteJogador.SetActive(false);
                _ui.especialBt.gameObject.SetActive(false);
                _ui.travarMiraBt.gameObject.SetActive(true);
                break;
        }
    }
}
