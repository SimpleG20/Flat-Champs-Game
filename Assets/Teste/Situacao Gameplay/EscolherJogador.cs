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
        if (!LogisticaVars.vezAI)
        {
            UI_Inicio();
            Camera_Situacao("entrar");
            yield return new WaitForSeconds(0.5f);
            yield return new WaitUntil(() => !_camera.GetPrincipal().IsBlending);
        }

        

        if (LogisticaVars.vezAI)
        {
            _gameplay.CriarIconesSelecao(LogisticaVars.jogadoresT1);
            _ui.selecionarJogadorBt.gameObject.SetActive(false);
            _ui.botaoCima.SetActive(false);
        }
        else
        {
            LogisticaVars.m_tempoSelecaoAnimator.SetBool("SelecionarJogador", true);
            LogisticaVars.m_tempoSelecaoAnimator.SetBool("SairSelecionarJogador", false);
            LogisticaVars.contarTempoSelecao = true;
            LogisticaVars.escolherOutroJogador = true;
            _gameplay.rotacaoCamera.position = LogisticaVars.m_jogadorEscolhido_Atual.transform.position - LogisticaVars.m_jogadorEscolhido_Atual.transform.up;
            
            if (LogisticaVars.vezJ1)
            {
                _gameplay.CriarIconesSelecao(LogisticaVars.jogadoresT1);
                _gameplay.RotacionarJogadorPerto(PosicaoParaOlhar(LogisticaVars.jogadoresT1));
            }
            else
            {
                _gameplay.CriarIconesSelecao(LogisticaVars.jogadoresT2);
                _gameplay.RotacionarJogadorPerto(PosicaoParaOlhar(LogisticaVars.jogadoresT2));
            }
            LogisticaVars.virouSelecao = false;
            aux = LogisticaVars.numControle;

            yield return new WaitUntil(() => LogisticaVars.virouSelecao);
            if (!LogisticaVars.escolheu)
            {
                LogisticaVars.jogadorSelecionado = false;
                UI_Meio();
            }

            yield return new WaitUntil(() => LogisticaVars.tempoEscolherJogador > LogisticaVars.tempoMaxEscolhaJogador ||
            LogisticaVars.tempoJogada > LogisticaVars.tempoMaxJogada && aux == LogisticaVars.numControle);
            if (!LogisticaVars.jogadorSelecionado && LogisticaVars.numControle == aux && !LogisticaVars.vezAI) _gameplay.Fim();
        }
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
        _ui.botaoMeio.SetActive(false);
        _ui.botaoBaixo.SetActive(false);
        _ui.botaoDiagonal.SetActive(false);
        _ui.botaoLivre2.SetActive(false);
        _ui.botaoLivre1.SetActive(false);

        _ui.centralBotoes.SetActive(true);
        _ui.botaoCima.SetActive(true);
        _ui.sairSelecaoBt.gameObject.SetActive(true);
        _ui.joystick.SetActive(true);
    }
    Vector3 PosicaoParaOlhar(List<GameObject> jogadores)
    {
        Vector3 novo = LogisticaVars.m_jogadorEscolhido_Atual.transform.position;

        foreach(GameObject jogador in jogadores)
        {
            novo += jogador.transform.position - LogisticaVars.m_jogadorEscolhido_Atual.transform.position; 
        }
        novo = novo / jogadores.Count;
        return novo;
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
    public override IEnumerator RotacionarParaJogadorMaisPerto(Vector3 pos)
    {
        yield return new WaitForSeconds(0.01f);
        step += 0.05f;
        _gameplay.rotacaoCamera.position = Vector3.MoveTowards(_gameplay.rotacaoCamera.position, pos, step);

        Vector3 direction = _gameplay.rotacaoCamera.position - LogisticaVars.m_jogadorEscolhido_Atual.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        LogisticaVars.m_jogadorEscolhido_Atual.transform.rotation = rotation;
        LogisticaVars.m_jogadorEscolhido_Atual.transform.eulerAngles = 
            new Vector3(-90, LogisticaVars.m_jogadorEscolhido_Atual.transform.eulerAngles.y, LogisticaVars.m_jogadorEscolhido_Atual.transform.eulerAngles.z);

        if (_gameplay.rotacaoAnt != rotation && !JogadorVars.m_rotacionar && !LogisticaVars.escolheu) { _gameplay.rotacaoAnt = rotation; _gameplay.RotacionarJogadorPerto(pos); }
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
        Debug.Log("ESCOLHER OUTRO: FIM");
        LogisticaVars.tempoEscolherJogador = 0;
        LogisticaVars.contarTempoSelecao = false;
        LogisticaVars.escolheu = true;
        LogisticaVars.virouSelecao = true;

        _gameplay.DestruirIconesSelecao();
        LogisticaVars.m_tempoSelecaoAnimator.SetBool("SelecionarJogador", false);
        LogisticaVars.m_tempoSelecaoAnimator.SetBool("SairSelecionarJogador", true);
        _ui.sairSelecaoBt.gameObject.SetActive(false);

        if (!LogisticaVars.vezAI)
        {
            Camera_Situacao("sair");
            _gameplay._bola.RedirecionarJogadores(true);
        }
        LogisticaVars.escolherOutroJogador = false;

        JogadorMetodos.ResetarValoresChute();
        LogisticaVars.jogadorSelecionado = true;
        LogisticaVars.virouSelecao = false;
        LogisticaVars.numControle++;

        yield return new WaitUntil(() => !_camera.GetPrincipal().IsBlending);
        UI_Normal();
        //_gameplay.AjeitarBarraChute();
        base.Fim();
    }
}
