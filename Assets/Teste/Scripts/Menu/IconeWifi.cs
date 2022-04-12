using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconeWifi : MonoBehaviour
{
    float tempoVerificarWifi;

    /*void Update()
    {
        if (Time.time - tempoVerificarWifi >= 1)
        {
            if (GameManager.Instance.InternetConnectivity()) { transform.GetChild(0).gameObject.SetActive(true); transform.GetChild(1).gameObject.SetActive(false); }
            else { transform.GetChild(1).gameObject.SetActive(true); transform.GetChild(0).gameObject.SetActive(false); }
            tempoVerificarWifi = Time.time;
        }
    }*/
}
