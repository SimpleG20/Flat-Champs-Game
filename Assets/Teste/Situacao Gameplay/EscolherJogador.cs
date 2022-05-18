using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EscolherJogador : Situacao
{
    public EscolherJogador(Gameplay gameplay, VariaveisUIsGameplay ui, CamerasSettings camera) : base(gameplay, ui, camera)
    {
    }

    int aux;
    float step = 0;

    public override IEnumerator Inicio()
    {
        //Verificar qual estado o jogador se encontra e com base nesse estado determina o andamento da situacao

        Debug.Log("ESCOLHER OUTRO INICIO");
        LogisticaVars.escolheu = false;
        UI_Inicio();
        Camera_Situacao("entrar");

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !_camera.GetPrincipal().IsBlending);
        LogisticaVars.m_tempoSelecaoAnimator.SetBool("SelecionarJogador", true);
        LogisticaVars.m_tempoSelecaoAnimator.SetBool("SairSelecionarJogador", false);
        LogisticaVars.contarTempoSelecao = true;
        LogisticaVars.escolherOutroJogador = true;
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
        _gameplay.rotacaoCamera.position = LogisticaVars.m_jogadorEscolhido_Atual.transform.position - LogisticaVars.m_jogadorEscolhido_Atual.transform.up;
        _gameplay.RotacionarJogadorPerto(jogadorPerto);

        aux = LogisticaVars.numControle;

        yield return new WaitUntil(() => LogisticaVars.virouSelecao);
        LogisticaVars.jogadorSelecionado = false;
        UI_Meio();

        yield return new WaitUntil(() => LogisticaVars.tempoEscolherJogador > LogisticaVars.tempoMaxEscolhaJogador || 
        LogisticaVars.tempoJogada > LogisticaVars.tempoMaxJogada && aux == LogisticaVars.numControle);
        if (!LogisticaVars.jogadorSelecionado) _gameplay.Fim();
    }

    void EsperandoAi()
    {
        UI_Meio();
    }
    void SelecionarJogador()
    {

    }

    void UI_Inicio()
    {
        _ui.EstadoTodosOsBotoes(false);
        _ui.m_placar.SetActive(true);
        _ui.tempoEscolhaGO.SetActive(true);
        _ui.tempoJogadaGO.SetActive(true);
        _ui.numeroJogadasGO.SetActive(true);
        _ui.pausarBt.gameObject.SetActive(true);
    }
    void UI_Meio()
    {
        _ui.EstadoTodosOsBotoes(false);
        _ui.centralBotoes.SetActive(true);
        _ui.botaoCima.SetActive(true);
        _ui.botaoCima.SetActive(true);

        /*_ui.EstadoBotoesJogador(false);
        _ui.EstadoBotoesGoleiro(false);
        _ui.centralBotoes.SetActive(true);
        _ui.botaoCima.SetActive(true);
        _ui.botaoMeio.SetActive(false);
        _ui.botaoBaixo.SetActive(false);
        _ui.botaoDiagonal.SetActive(false);
        _ui.botaoLivre2.SetActive(false);
        _ui.botaoLivre1.SetActive(false);*/

        _ui.sairSelecaoBt.gameObject.SetActive(true);
        _ui.joystick.SetActive(true);
    }

    public override void Camera_Situacao(string s)
    {
        switch (s)
        {
            case "entrar":
                _camera.GetPrincipal().m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseOut;
                _camera.GetPrincipal().m_DefaultBlend.m_Time = 0.5f;
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
    public override IEnumerator RotacionarParaJogadorMaisPerto(GameObject jogador)
    {
        yield return new WaitForSeconds(0.01f);
        step += 0.05f;
        _gameplay.rotacaoCamera.position = Vector3.MoveTowards(_gameplay.rotacaoCamera.position, jogador.transform.position, step);

        Vector3 direction = _gameplay.rotacaoCamera.position - LogisticaVars.m_jogadorEscolhido_Atual.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        LogisticaVars.m_jogadorEscolhido_Atual.transform.rotation = rotation;
        LogisticaVars.m_jogadorEscolhido_Atual.transform.eulerAngles = 
            new Vector3(-90, LogisticaVars.m_jogadorEscolhido_Atual.transform.eulerAngles.y, LogisticaVars.m_jogadorEscolhido_Atual.transform.eulerAngles.z);

        if (_gameplay.rotacaoAnt != rotation && !JogadorVars.m_rotacionar && !LogisticaVars.escolheu) { _gameplay.rotacaoAnt = rotation; _gameplay.RotacionarJogadorPerto(jogador); }
        else
        {
            Debug.Log("ESCOLHER JOGADOR: Rotacionou");
            LogisticaVars.virouSelecao = true;
            _gameplay.rotacaoAnt = Quaternion.identity;
            step = 0;
        }
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

        _gameplay._bola.RedirecionarJogadores(true);
        LogisticaVars.escolherOutroJogador = false;

        JogadorMetodos.ResetarValoresChute();
        LogisticaVars.jogadorSelecionado = true;
        LogisticaVars.virouSelecao = false;
        LogisticaVars.numControle++;
        UI_Normal();
        return base.Fim();
    }
}
