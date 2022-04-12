using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TempoEscolha : MonoBehaviour
{
    private void Start()
    {
        EventsManager.current.onAtualizarNumeros += AtualizarTempoEscolhas;
    }
    private void AtualizarTempoEscolhas()
    {
        GetComponent<TextMeshProUGUI>().text = Mathf.Round(LogisticaVars.tempoEscolherJogador).ToString();
    }
}
