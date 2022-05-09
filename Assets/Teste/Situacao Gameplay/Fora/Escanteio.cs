using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escanteio : Fora
{
    public Escanteio(Gameplay gameplay, VariaveisUIsGameplay ui, CamerasSettings camera) : base(gameplay, ui, camera)
    { }

    public override IEnumerator Inicio()
    {
        base.Inicio();
        Camera_Situacao("habilitar cam lateral");

        yield return new WaitForSeconds(1);
        EventsManager.current.SelecaoAutomatica();
        _gameplay.Spawnar("escanteio");
    }

    public override IEnumerator Spawnar(string lado)
    {
        Debug.Log("ESCANTEIO: Spawnar Escanteio");

        yield return new WaitForSeconds(0.75f);
        Vector3 novaPos = new Vector3(_gameplay._bola.m_posicaoFundo.x, LogisticaVars.m_jogadorEscolhido_Atual.transform.position.y, _gameplay._bola.m_posicaoFundo.z);
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

        yield return new WaitForSeconds(1.25f);
        Camera_Situacao("desabilitar cam lateral");

        _gameplay._bola.RedirecionarJogadorEscolhido(_gameplay._bola.transform);
        LogisticaVars.podeRedirecionar = true;

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !_camera.GetPrincipal().IsBlending);
        EstadoJogo.JogoNormal();
        EstadoJogo.TempoJogada(true);
        EventsManager.current.OnFora("rotina tempo escanteio");
        UI_Meio();

        yield return new WaitUntil(() => /*esperar o jogador chutar e ser posicionado*/false);
        UI_Fim();
        Fim();
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
    }
}
