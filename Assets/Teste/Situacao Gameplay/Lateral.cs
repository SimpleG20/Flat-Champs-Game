using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lateral : Fora
{
    public Lateral(Gameplay gameplay) : base(gameplay)
    {
    }

    public override IEnumerator Inicio()
    {
        base.Inicio();
        //events.OnAplicarRotinas("rotina tempo lateral");

        Camera_Situacao();

        yield return new WaitUntil(() => true); //Esperar Camera terminar animacao

        UI_Situacao();
    }

    public override IEnumerator Spawnar(string lado)
    {
        Debug.Log("Spawnar Lateral");

        yield return new WaitForSeconds(1.5f);

        /*if (lateral == "lateral direita")
        {
            LogisticaVars.m_jogadorEscolhido.transform.position = new Vector3(bola.m_posLateral.x + 3f, LogisticaVars.m_jogadorEscolhido.transform.position.y, bola.m_posLateral.z);

            LogisticaVars.m_rbJogadorEscolhido.velocity = Vector3.zero;
            LogisticaVars.foraLateralD = false;
        }
        if (lateral == "lateral esquerda")
        {
            LogisticaVars.m_jogadorEscolhido.transform.position = new Vector3(bola.m_posLateral.x - 3f, LogisticaVars.m_jogadorEscolhido.transform.position.y, bola.m_posLateral.z);

            LogisticaVars.m_rbJogadorEscolhido.velocity = Vector3.zero;
            LogisticaVars.foraLateralE = false;
        }*/

        yield return new WaitForSeconds(0.5f);
        /*events.SituacaoGameplay("desabilitar camera lateral");

        LogisticaVars.m_jogadorEscolhido.transform.GetChild(3).gameObject.SetActive(true);

        bola.RedirecionarJogadorEscolhido(bola.transform);
        LogisticaVars.podeRedirecionar = true;*/
    }

    public override void UI_Situacao()
    {
        //EstadoBotoesGoleiro(false);
        //EstadoBotoesJogador(false);
        //EstadoBotoesCentral(false);

        //centralBotoes.SetActive(true);
        //barraEspecial.SetActive(true);
        //direcaoBolaBt.gameObject.SetActive(true);
        //botaoBaixo.SetActive(true);
        //botaoMeio.SetActive(true);
        //joystick.SetActive(true);

        //lateralBt.gameObject.SetActive(true);
    }

    public override void Camera_Situacao()
    {
        base.Camera_Situacao();
    }

    public override IEnumerator Fim()
    {
        return base.Fim();
    }
}
