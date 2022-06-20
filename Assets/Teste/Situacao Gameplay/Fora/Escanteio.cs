using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escanteio : Fora
{
    public Escanteio(Gameplay gameplay, VariaveisUIsGameplay ui, CamerasSettings camera) : base(gameplay, ui, camera)
    { }

    public override IEnumerator Inicio()
    {
        SetarFora();
        if (!LogisticaVars.vezAI) Camera_Situacao("habilitar cam lateral");

        yield return new WaitForSeconds(1);
        EventsManager.current.SelecaoAutomatica();
        _gameplay.Spawnar("escanteio");
    }

    public override IEnumerator Spawnar(string lado)
    {
        Debug.Log("ESCANTEIO: Spawnar Escanteio");

        yield return new WaitForSeconds(0.75f);
        Vector3 novaPos = new Vector3(_gameplay._bola.m_posicaoFundo.x, LogisticaVars.m_jogadorEscolhido_Atual.transform.position.y + 0.1f, _gameplay._bola.m_posicaoFundo.z);
        if (lado == "fundo 1")
        {
            if (_gameplay._bola.transform.position.x < 0) LogisticaVars.m_jogadorEscolhido_Atual.transform.position = novaPos + new Vector3(-2f, 0, -1.5f);
            else LogisticaVars.m_jogadorEscolhido_Atual.transform.position = novaPos + new Vector3(+2f, 0, -1.5f);

            LogisticaVars.fundo1 = true;
        }
        else if (lado == "fundo 2")
        {
            if (_gameplay._bola.transform.position.x < 0) LogisticaVars.m_jogadorEscolhido_Atual.transform.position = novaPos + new Vector3(-2f, 0, +1.5f);
            else LogisticaVars.m_jogadorEscolhido_Atual.transform.position = novaPos + new Vector3(+2f, 0, +1.5f);

            LogisticaVars.fundo2 = true;
        }

        if (!LogisticaVars.vezAI)
        {
            yield return new WaitForSeconds(1.25f);
            Camera_Situacao("desabilitar cam lateral");

            _gameplay._bola.RedirecionarJogadorEscolhido(_gameplay._bola.transform);
            LogisticaVars.podeRedirecionar = true;

            yield return new WaitForSeconds(0.5f);
            yield return new WaitUntil(() => !_camera.GetPrincipal().IsBlending);
            EstadoJogo.JogoNormal();
            UI_Meio();
        }
        
        EventsManager.current.OnFora("rotina tempo escanteio");

        yield return new WaitUntil(() => LogisticaVars.saiuFora);
        yield return new WaitForSeconds(0.5f);
        Finalizar();
        _gameplay.Fim();
    }

    public override void UI_Meio()
    {
        _ui.EstadoBotoesGoleiro(false);
        _ui.EstadoBotoesJogador(false);
        _ui.EstadoBotoesCentral(false);

        _ui.centralBotoes.SetActive(true);
        _ui.barraEspecial.SetActive(true);
        _ui.direcaoBolaBt.gameObject.SetActive(true);
        _ui.botaoBaixo.SetActive(true);
        _ui.botaoMeio.SetActive(true);
        _ui.joystick.SetActive(true);
        _ui.escanteioBt.gameObject.SetActive(true);
        _ui.barraChuteJogador.SetActive(true);

        if (LogisticaVars.vezJ1) _gameplay.BarraEspecial(LogisticaVars.m_especialAtualT1, LogisticaVars.m_maxEspecial);
        else _gameplay.BarraEspecial(LogisticaVars.m_especialAtualT2, LogisticaVars.m_maxEspecial);
    }

    void Finalizar()
    {
        LogisticaVars.foraFundo = false;
        LogisticaVars.continuaSendoFora = false;
        JogadorVars.m_aplicarChute = true;
        EstadoJogo.TempoJogada(true);
        UI_Normal();
        JogadorMetodos.ResetarValoresChute();
    }
}
