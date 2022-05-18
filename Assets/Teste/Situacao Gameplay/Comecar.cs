using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comecar : Situacao
{
    public Comecar(Gameplay gameplay, VariaveisUIsGameplay ui, CamerasSettings camera) : base(gameplay, ui, camera)
    {
    }

    public override IEnumerator Inicio()
    {
        Debug.Log("COMECAR INICIO");
        ToqueInicial();
        _camera.SituacoesCameras("toque pos gol");

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !_camera.GetPrincipal().IsBlending);

        UI_Situacao("inicio");
        JogadorMetodos.ResetarValoresChute();

        yield return new WaitForSeconds(7);
        if (!LogisticaVars.aplicouPrimeiroToque)
        {
            AplicarToqueInicialAuto();
            _gameplay.Fim();
        }
    }

    public override void UI_Situacao(string s)
    {
        switch (s)
        {
            case "inicio":
                _ui.m_placar.SetActive(true);
                _ui.pausarBt.gameObject.SetActive(true);
                _ui.tempoEscolhaGO.SetActive(true);
                _ui.numeroJogadasGO.SetActive(true);
                _ui.tempoJogadaGO.SetActive(true);

                _ui.centralBotoes.SetActive(true);
                _ui.barraEspecial.SetActive(true);
                _ui.especialBt.gameObject.SetActive(true);
                _ui.botaoBaixo.SetActive(false);
                _ui.botaoCima.SetActive(false);
                _ui.botaoDiagonal.SetActive(false);
                _ui.botaoLivre2.SetActive(false);
                _ui.botaoLivre1.SetActive(false);
                _ui.botaoMeio.SetActive(true);

                _ui.moverJogadorBt.SetActive(true);
                _ui.joystick.SetActive(true);
                _ui.barraChuteJogador.SetActive(true);
                _gameplay.AjeitarBarraChute();
                break;
        }
    }

    void ToqueInicial()
    {
        EventsManager.current.EscolherJogador();
        SelecaoMetodos.DadosJogador();
        SelecaoMetodos.DesabilitarComponentesDosNaoSelecionados();
    }
    void AplicarToqueInicialAuto()
    {
        LogisticaVars.aplicouPrimeiroToque = LogisticaVars.jogoComecou = true;
        EstadoJogo.JogoNormal();
        EstadoJogo.TempoJogada(true);

        Vector3 dir = LogisticaVars.m_jogadorEscolhido_Atual.transform.position - _gameplay._bola.transform.position;
        _gameplay._bola.m_rbBola.AddForce(dir * 2, ForceMode.Impulse);

        LogisticaVars.ultimoToque = LogisticaVars.vezJ1 ? 1 : 2;
        LogisticaVars.jogadas++;
        EventsManager.current.OnAtualizarNumeros();
    }

    public override IEnumerator Fim()
    {
        UI_Normal();
        return base.Fim();
    }
}
