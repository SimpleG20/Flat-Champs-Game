using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LinkarBotaoComIcone : MonoBehaviour
{
    [Header("Logic")]
    public float offset;
    public Camera cam;

    [Header("Jogador Referenciado")]
    public GameObject jogadorReferenciado;

    void FixedUpdate()
    {
        Vector3 pos = cam.WorldToScreenPoint(jogadorReferenciado.transform.position) + Vector3.up * offset;
        if (transform.position != pos) transform.position = pos;
    }

    public void SelecionarJogador()
    {
        CamerasSettings._current.GetPrincipal().m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
        if (!LogisticaVars.vezAI)
        {
            SelecaoMetodos.DesabilitarDadosJogadorAtual();
            LogisticaVars.m_jogadorEscolhido_Atual = jogadorReferenciado.gameObject;
            SelecaoMetodos.DadosJogador();
        }
        else
        {
            LogisticaVars.m_jogadorPlayer = jogadorReferenciado.gameObject;
            if (!CamerasSettings._current.getCameraEspera()) LogisticaVars.cameraJogador.m_Priority = 0;
            LogisticaVars.cameraJogador = LogisticaVars.m_jogadorPlayer.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>();
            LogisticaVars.cameraJogador.m_Priority = 99;
            if (CamerasSettings._current.getCameraEspera())
            {
                CamerasSettings._current.MudarBlendCamera(CinemachineBlendDefinition.Style.Cut);
                CamerasSettings._current.SituacoesCameras("desabilitar camera espera");
            }
        }
        Gameplay._current.OnJogadorSelecionado();
    }
}
