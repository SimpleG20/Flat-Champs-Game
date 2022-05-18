using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EstadoJogo 
{
    public static void UI_Situacao(string s)
    {
        switch (s)
        {
            case "pause":
                if (Gameplay._current.VerificarIconesSelecao())
                {
                    foreach (GameObject l in Gameplay._current.ListaIconesSelecao()) l.GetComponent<Button>().interactable = false;
                }
                Gameplay._current.canvas.transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 0;
                break;
            case "unpause":
                if (Gameplay._current.VerificarIconesSelecao())
                {
                    foreach (GameObject l in Gameplay._current.ListaIconesSelecao()) l.GetComponent<Button>().interactable = true;
                }
                Gameplay._current.canvas.transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 1;
                break;
        }
    }

    public static void JogoParado()
    {
        LogisticaVars.jogoParado = true;
    }
    public static void JogoNormal()
    {
        LogisticaVars.jogoParado = false;
    }
    public static void PausarJogo()
    {
        Debug.Log("ESTADO: PAUSE");
        Time.timeScale = 0;
        UI_Situacao("pause");
        CamerasSettings._current.AplicarBlur(LogisticaVars.cameraJogador);
        TempoJogada(false);
        JogoParado();
    }
    public static void DespausarJogo()
    {
        Debug.Log("ESTADO: UNPAUSE");
        Time.timeScale = 1;
        UI_Situacao("unpause");
        CamerasSettings._current.RetirarBlur(LogisticaVars.cameraJogador);
        JogoNormal();
        TempoJogada(true);
    }
    public static void QuitarJogo()
    {
        Debug.Log("Quitar");
        /*if(LogisticaVars.tempoCorrido != LogisticaVars.tempoPartida)
        {
            Gameplay._current.quitou = true;
        }*/
        Gameplay._current.SetSituacao("fim");
    }

    public static void TempoJogada(bool b)
    {
        if (b == false) LogisticaVars.contarTempoJogada = b;
        else
        {
            if (LogisticaVars.jogoComecou)
            {
                if (!LogisticaVars.continuaSendoFora && !LogisticaVars.especial && 
                    !LogisticaVars.gol && !LogisticaVars.trocarVez && !LogisticaVars.defenderGoleiro)
                    LogisticaVars.contarTempoJogada = b;
            }
        }
        
    }
}
