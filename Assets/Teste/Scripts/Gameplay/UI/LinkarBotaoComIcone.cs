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
        LogisticaVars.escolheu = true;

        foreach (LinkarBotaoComIcone l in FindObjectsOfType<LinkarBotaoComIcone>()) Destroy(l.gameObject);

        LogisticaVars.m_tempoSelecaoAnimator.SetBool("SelecionarJogador", false);
        LogisticaVars.m_tempoSelecaoAnimator.SetBool("SairSelecionarJogador", true);

        FindObjectOfType<CinemachineBrain>().m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
        SelecaoMetodos.DesabilitarDadosJogador();
        LogisticaVars.m_jogadorEscolhido = jogadorReferenciado.gameObject;
        SelecaoMetodos.DadosJogador();

        EventsManager.current.AjeitarCamera(-1);

        EventsManager.current.OnAplicarMetodosUiComBotao("jogador selecionado");
        EventsManager.current.OnAplicarMetodosUiSemBotao("estados dos botoes", "normal");

        FindObjectOfType<FisicaBola>().RedirecionarJogadores(true);

        LogisticaVars.escolherOutroJogador = false;
        LogisticaVars.jogadorSelecionado = true;
        LogisticaVars.prontoParaEscolher = false;

        JogadorMetodos.ResetarValoresChute();
    }
}
