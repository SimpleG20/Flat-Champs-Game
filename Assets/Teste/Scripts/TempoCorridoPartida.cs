using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TempoCorridoPartida : MonoBehaviour
{
    void Update()
    {
        GetComponent<TextMeshProUGUI>().text = LogisticaVars.minutosCorridos.ToString() + ":" + LogisticaVars.segundosCorridos.ToString();
    }
}
