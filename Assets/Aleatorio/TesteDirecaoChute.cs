using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesteDirecaoChute : MonoBehaviour
{
    public int i;
    public float alcanceChute;

    public LayerMask layerMask;

    public FisicaJogador jogador;
    public FisicaBola bola;


    // Update is called once per frame
    void Update()
    {
        Vector3 lateralD = new Vector3(jogador.transform.position.x + (1.7f * -jogador.transform.up.z), 0.3f, jogador.transform.position.z - (1.7f * -jogador.transform.up.x));
        Vector3 lateralE = new Vector3(jogador.transform.position.x - (1.7f * -jogador.transform.up.z), 0.3f, jogador.transform.position.z + (1.7f * -jogador.transform.up.x));
        Vector3 lateralDC = new Vector3(jogador.transform.position.x + (0.85f * -jogador.transform.up.z), 0.3f, jogador.transform.position.z - (0.85f * -jogador.transform.up.x));
        Vector3 lateralEC = new Vector3(jogador.transform.position.x - (0.85f * -jogador.transform.up.z), 0.3f, jogador.transform.position.z + (0.85f * -jogador.transform.up.x));

        Debug.DrawRay(lateralEC, -jogador.transform.up * alcanceChute, Color.green);
        Debug.DrawRay(lateralDC, -jogador.transform.up * alcanceChute, Color.green);
        Debug.DrawRay(lateralE, -jogador.transform.up * alcanceChute, Color.green);
        Debug.DrawRay(lateralD, -jogador.transform.up * alcanceChute, Color.green);
        Debug.DrawRay(jogador.transform.position, -jogador.transform.up * alcanceChute, Color.green);
    }
}
