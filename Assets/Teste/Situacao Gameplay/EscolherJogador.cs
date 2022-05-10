using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EscolherJogador : Situacao
{
    public EscolherJogador(Gameplay gameplay, VariaveisUIsGameplay ui, CamerasSettings camera) : base(gameplay, ui, camera)
    {
    }

    public override IEnumerator Inicio()
    {
        UI_Inicio();
        LogisticaVars.escolheu = false;

        _camera.GetPrincipal().m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseOut;
        _camera.GetPrincipal().m_DefaultBlend.m_Time = 0.5f;
        Camera_Situacao("entrar");

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !_camera.GetPrincipal().IsBlending);
        LogisticaVars.m_tempoSelecaoAnimator.SetBool("SelecionarJogador", true);
        LogisticaVars.m_tempoSelecaoAnimator.SetBool("SairSelecionarJogador", false);
        LogisticaVars.contarTempoSelecao = true;
        LogisticaVars.jogadorSelecionado = false;
        LogisticaVars.escolherOutroJogador = true;
        EventsManager.current.OnEscolherOutro("rotina tempo selecao");
        UI_Meio();

        GameObject jogadorPerto = null;

        if (LogisticaVars.vezJ1)
        {
            jogadorPerto = SelecaoMetodos.QuemEstaMaisPerto(LogisticaVars.m_jogadorEscolhido_Atual.transform.position, LogisticaVars.jogadoresT1);
            _gameplay.CriarIconesSelecao(LogisticaVars.jogadoresT1);
        }
        else
        {
            jogadorPerto = SelecaoMetodos.QuemEstaMaisPerto(LogisticaVars.m_jogadorEscolhido_Atual.transform.position, LogisticaVars.jogadoresT2);
            _gameplay.CriarIconesSelecao(LogisticaVars.jogadoresT2);
        }
        _gameplay.RotacionarJogadorPerto(jogadorPerto);

        yield return new WaitForSeconds(LogisticaVars.tempoMaxEscolhaJogador);
        Fim();
    }

    public override IEnumerator RotacionarParaJogadorMaisPerto(GameObject jogador)
    {
        yield return new WaitForSeconds(0.01f);
        float step = 0.05f;
        GameObject.Find("RotacaoCamera").transform.position =
            Vector3.MoveTowards((LogisticaVars.m_jogadorEscolhido_Atual.transform.position - LogisticaVars.m_jogadorEscolhido_Atual.transform.up), jogador.transform.position, step);

        Vector3 direction = GameObject.Find("RotacaoCamera").transform.position - LogisticaVars.m_jogadorEscolhido_Atual.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        LogisticaVars.m_jogadorEscolhido_Atual.transform.rotation = rotation;
        LogisticaVars.m_jogadorEscolhido_Atual.transform.eulerAngles = 
            new Vector3(-90, LogisticaVars.m_jogadorEscolhido_Atual.transform.eulerAngles.y, LogisticaVars.m_jogadorEscolhido_Atual.transform.eulerAngles.z);

        if (_gameplay.rotacaoAnt != rotation && !JogadorVars.m_rotacionar && !LogisticaVars.escolheu) { _gameplay.rotacaoAnt = rotation; _gameplay.RotacionarJogadorPerto(jogador); }
        else
        {
            _gameplay.rotacaoAnt = Quaternion.identity;
        }
    }

    public override void Camera_Situacao(string s)
    {
        switch (s)
        {
            case "entrar":
                LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>().m_Priority = 0;
                LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(1).GetChild(1).GetComponent<CinemachineVirtualCamera>().m_Priority = 99;
                LogisticaVars.cameraJogador = LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(1).GetChild(1).GetComponent<CinemachineVirtualCamera>();
                break;
            case "sair":
                LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>().m_Priority = 99;
                LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(1).GetChild(1).GetComponent<CinemachineVirtualCamera>().m_Priority = 0;
                LogisticaVars.cameraJogador = LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(1).GetChild(0).GetComponent<CinemachineVirtualCamera>();
                break;
        }
    }

    void UI_Inicio()
    {
        _ui.EstadoTodosOsBotoes(false);
        _ui.m_placar.SetActive(true);
        _ui.pausarBt.gameObject.SetActive(true);
    }
    void UI_Meio()
    {
        _ui.EstadoBotoesJogador(false);
        _ui.EstadoBotoesGoleiro(false);
        _ui.botaoCima.SetActive(true);
        _ui.botaoMeio.SetActive(false);
        _ui.botaoBaixo.SetActive(false);
        _ui.botaoDiagonal.SetActive(false);
        _ui.botaoLivre2.SetActive(false);
        _ui.botaoLivre1.SetActive(false);

        _ui.sairSelecaoBt.gameObject.SetActive(true);
        _ui.joystick.SetActive(true);
    }

    public override IEnumerator Fim()
    {
        LogisticaVars.tempoEscolherJogador = 0;
        LogisticaVars.contarTempoSelecao = false;
        LogisticaVars.escolheu = true;

        _gameplay.DestruirIconesSelecao();
        LogisticaVars.m_tempoSelecaoAnimator.SetBool("SelecionarJogador", false);
        LogisticaVars.m_tempoSelecaoAnimator.SetBool("SairSelecionarJogador", true);
        _ui.sairSelecaoBt.gameObject.SetActive(false);

        Camera_Situacao("sair");
        //EventsManager.current.AjeitarCamera(-1);

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !_camera.GetPrincipal().IsBlending);
        _gameplay._bola.RedirecionarJogadores(true);
        LogisticaVars.escolherOutroJogador = false;

        if (LogisticaVars.trocarVez) _gameplay.SetSituacao(new TrocarVez(_gameplay, _ui, _camera));
        else
        {
            JogadorMetodos.ResetarValoresChute();
            LogisticaVars.jogadorSelecionado = true;
            UI_Normal();
            base.Fim();
        }
    }
}
