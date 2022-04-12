using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TempoJogada : MonoBehaviour
{
    private void Start()
    {
        EventsManager.current.onAtualizarNumeros += AtualizarTempoJogadas;
    }
    void AtualizarTempoJogadas()
    {
        GetComponent<TextMeshProUGUI>().text = Mathf.Round(LogisticaVars.tempoJogada) + "s";
    }
}
