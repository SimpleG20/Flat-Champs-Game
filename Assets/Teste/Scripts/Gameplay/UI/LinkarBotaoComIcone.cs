using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LinkarBotaoComIcone : MonoBehaviour
{
    [Header("Logic")]
    public Camera cam;

    [Header("Jogador Referenciado")]
    public GameObject jogadorReferenciado;

    void FixedUpdate()
    {
        Vector3 pos = cam.WorldToScreenPoint(jogadorReferenciado.transform.position) + Vector3.up * 110;
        if (transform.position != pos) transform.position = pos;
    }

    public void SelecionarJogador()
    {
        CamerasSettings._current.GetPrincipal().m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
        SelecaoMetodos.DesabilitarDadosJogador();
        LogisticaVars.m_jogadorEscolhido_Atual = jogadorReferenciado.gameObject;
        SelecaoMetodos.DadosJogador();

        Gameplay._current.OnJogadorSelecionado();
    }
}
