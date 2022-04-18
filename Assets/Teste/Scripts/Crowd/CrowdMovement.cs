using System.Collections;
using UnityEngine;

public class CrowdMovement : MonoBehaviour
{
    public GameObject torcedor;
    public float tempo;
    public bool m, gol;


    private void Update()
    {
        if (m)
        {
            gol = true;
            foreach (GameObject t in GameObject.FindGameObjectsWithTag("TorcidaAleatoria"))
            {
                float random = Random.Range(0.5f, 2.5f);
                LeanTween.moveLocalY(t, random, 0.1f + random / 10).setEaseLinear().setLoopPingPong(20); //Em media 15 segundos
            }
            m = false;
        }
    }

}
