using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lateral : Fora
{
    public Lateral(Gameplay gameplay, VariaveisUIsGameplay ui, CamerasSettings camera) : base(gameplay, ui, camera)
    {
    }

    public override IEnumerator Inicio()
    {
        SetarFora();
        Camera_Situacao("habilitar cam lateral");

        yield return new WaitForSeconds(1);
        EventsManager.current.SelecaoAutomatica();
        _gameplay.Spawnar("lateral");
    }

    public override IEnumerator Spawnar(string lado)
    {
        Debug.Log("LATERAL: Spawnar Lateral");

        yield return new WaitForSeconds(0.75f);
        if (lado  == "lateral direita")
        {
            LogisticaVars.m_jogadorEscolhido_Atual.transform.position = 
                new Vector3(_gameplay._bola.m_posLateral.x + 3f, LogisticaVars.m_jogadorEscolhido_Atual.transform.position.y, _gameplay._bola.m_posLateral.z);

            LogisticaVars.foraLateralD = true;
        }
        if (lado == "lateral esquerda")
        {
            LogisticaVars.m_jogadorEscolhido_Atual.transform.position = 
                new Vector3(_gameplay._bola.m_posLateral.x - 3f, LogisticaVars.m_jogadorEscolhido_Atual.transform.position.y, _gameplay._bola.m_posLateral.z);

            LogisticaVars.foraLateralE = false;
        }
        LogisticaVars.m_rbJogadorEscolhido.velocity = Vector3.zero;

        yield return new WaitForSeconds(1f);
        Camera_Situacao("desabilitar cam lateral");

        LogisticaVars.m_jogadorEscolhido_Atual.transform.GetChild(3).gameObject.SetActive(true);
        _gameplay._bola.RedirecionarJogadorEscolhido(_gameplay._bola.transform);
        LogisticaVars.podeRedirecionar = true;

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !_camera.GetPrincipal().IsBlending);
        EstadoJogo.JogoNormal();
        UI_Meio();
        EventsManager.current.OnFora("rotina tempo lateral");

        yield return new WaitUntil(() => LogisticaVars.saiuFora);
        Finalizar();
        _gameplay.Fim();
    }

    void Finalizar()
    {
        EstadoJogo.TempoJogada(true);
        LogisticaVars.lateral = false;
        LogisticaVars.continuaSendoFora = false;
        JogadorVars.m_aplicarChute = true;
        UI_Normal();
        JogadorMetodos.ResetarValoresChute();
    }

    public override void UI_Meio()
    {
        _ui.EstadoBotoesGoleiro(false);
        _ui.EstadoBotoesJogador(false);
        _ui.EstadoBotoesCentral(false);

        _ui.centralBotoes.SetActive(true);
        _ui.barraEspecial.SetActive(true);
        _ui.direcaoBolaBt.gameObject.SetActive(true);
        _ui.mostrarDirecionalBolaBt.gameObject.SetActive(true);
        _ui.botaoBaixo.SetActive(true);
        _ui.botaoMeio.SetActive(true);
        _ui.botaoLivre2.SetActive(true);
        _ui.joystick.SetActive(true);
        _ui.lateralBt.gameObject.SetActive(true);
    }
}
