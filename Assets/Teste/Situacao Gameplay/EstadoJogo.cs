using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EstadoJogo : Situacao
{
    public EstadoJogo(Gameplay gameplay, VariaveisUIsGameplay ui, CamerasSettings camera) : base(gameplay, ui, camera)
    { }

    public override void UI_Situacao(string s)
    {
        switch (s)
        {
            case "pause":
                if (_gameplay.VerificarIconesSelecao())
                {
                    foreach (GameObject l in _gameplay.ListaIconesSelecao()) l.GetComponent<Button>().interactable = false;
                }
                _gameplay.canvas.transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 0;
                break;
            case "unpause":
                if (_gameplay.VerificarIconesSelecao())
                {
                    foreach (GameObject l in _gameplay.ListaIconesSelecao()) l.GetComponent<Button>().interactable = true;
                }
                _gameplay.canvas.transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 1;
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
    public void PararJogo()
    {
        Debug.Log("ESTADO: PAUSE");
        Time.timeScale = 0;
        UI_Situacao("pause");
        TempoJogada(false);
        JogoParado();
    }
    public void DespausarJogo()
    {
        Debug.Log("ESTADO: UNPAUSE");
        Time.timeScale = 1;
        UI_Situacao("unpause");
        JogoNormal();
        TempoJogada(true);
    }
    void QuitarJogo()
    {
        Debug.Log("Quitar");
    }

    public static void TempoJogada(bool b)
    {
        LogisticaVars.contarTempoJogada = b;
    }
}
