using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiro_de_Meta : Fora
{
    public Tiro_de_Meta(Gameplay gameplay) : base(gameplay)
    {
    }

    public override IEnumerator Inicio()
    {
        base.Inicio();
        /*
        if(LogisticaVars.fundo1 && LogisticaVars.ultimoToque != 1)
        {
            LogisticaVars.vezJ1 = true;
            LogisticaVars.vezJ2 = false;
        }

        if (LogisticaVars.fundo2 && LogisticaVars.ultimoToque != 2)
        {
            LogisticaVars.vezJ2 = true;
            LogisticaVars.vezJ1 = false;
        }

        events.OnAplicarRotinas("rotina tempo tiro de meta");
        */
        yield return new WaitUntil(() => true);//fim da animacao da camera
        UI_Situacao();
    }

    public override void UI_Situacao()
    {
        //EstadoBotoesJogador(false);
        //EstadoBotoesGoleiro(true);
        //selecionarJogadorBt.gameObject.SetActive(false);
        //barraChuteGoleiro.SetActive(true);
        //FindObjectOfType<CamerasSettings>().MudarBlendCamera(CinemachineBlendDefinition.Style.EaseInOut);
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
