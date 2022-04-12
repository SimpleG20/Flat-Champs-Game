using TMPro;
using UnityEngine;

public class NumeroJogadas : MonoBehaviour
{
    private void Start()
    {
        EventsManager.current.onAtualizarNumeros += AtualizarNumeroJogadas;
    }
    void AtualizarNumeroJogadas()
    {
        GetComponent<TextMeshProUGUI>().text = LogisticaVars.jogadas.ToString();
    }
}
