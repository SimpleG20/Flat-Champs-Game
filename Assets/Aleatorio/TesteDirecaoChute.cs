using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesteDirecaoChute : MonoBehaviour
{
    const float maxAlturaChute = 4;
    public float senoJogador, cosJogador, distanciaDaBola, maxAnguloParaChute, alturaChute;
    public float anguloBola, anguloJogador, anguloBolaJogador, anguloDirJogadorBola;
    public float erro;

    Vector3 direcaoChute, direcaoJogadorBola, direcaoBola, ultimaDirecao;

    public GameObject direcional, jogador, bola;

    // Update is called once per frame
    void Update()
    {
        //jogador.transform.LookAt(bola.transform);
        //jogador.transform.eulerAngles = new Vector3(-90, jogador.transform.eulerAngles.y, jogador.transform.eulerAngles.z);

        senoJogador = -jogador.transform.up.z;
        cosJogador = -jogador.transform.up.x;
        direcaoChute = new Vector3(cosJogador, 0, senoJogador);
        distanciaDaBola = (bola.transform.position - jogador.transform.position).magnitude;
        direcaoJogadorBola = new Vector3(bola.transform.position.x - jogador.transform.position.x, 0, bola.transform.position.z - jogador.transform.position.z).normalized;

        if (distanciaDaBola >= 2) erro = Mathf.Sqrt((distanciaDaBola - 2) * distanciaDaBola);
        else erro = 0;

        maxAnguloParaChute = (360 / Mathf.Pow(distanciaDaBola, 2)) + Mathf.Pow(((distanciaDaBola - 2) / distanciaDaBola) + 1.25f, 2) + erro;
        anguloJogador = Mathf.Acos(direcaoChute.x) * Mathf.Rad2Deg;
        anguloBolaJogador = Mathf.Acos(direcaoJogadorBola.x / direcaoJogadorBola.magnitude) * Mathf.Rad2Deg;
        anguloDirJogadorBola = Mathf.Acos(((direcaoJogadorBola.x * direcaoChute.x) + (direcaoJogadorBola.y * direcaoChute.y) + (direcaoChute.z * direcaoJogadorBola.z)) /
            (direcaoJogadorBola.magnitude * direcaoChute.magnitude)) * Mathf.Rad2Deg;


        if (!LogisticaVars.bolaRasteiraT1 && LogisticaVars.vezJ1 || !LogisticaVars.bolaRasteiraT2 && LogisticaVars.vezJ2)
        {
            alturaChute += Input.GetAxis("Vertical") * Time.deltaTime;
        }
        else alturaChute = 0;
        if (alturaChute >= maxAlturaChute) alturaChute = maxAlturaChute;
        if (alturaChute <= 0) alturaChute = 0;

        #region Ajustes
        if (maxAnguloParaChute > 90) maxAnguloParaChute = 90;
        if (anguloDirJogadorBola > maxAnguloParaChute)
        {
            if (direcional.transform.GetChild(0).GetComponent<MeshRenderer>().enabled) direcional.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        }
        else 
        {
            if (!direcional.transform.GetChild(0).GetComponent<MeshRenderer>().enabled) direcional.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        }
        if (senoJogador < 0) anguloJogador = 360 - anguloJogador;
        if (direcaoJogadorBola.z < 0) anguloBolaJogador = 360 - anguloBolaJogador;
        if (anguloBolaJogador == 360) anguloBolaJogador = 0;

        if (anguloJogador < anguloBolaJogador)
        {
            if (anguloJogador < 0 || anguloJogador > 0) anguloDirJogadorBola = -anguloDirJogadorBola;
        }
        else
        {
            if(direcaoJogadorBola.z > 0 || anguloBolaJogador == 0)
            {
                if (anguloJogador > 360 - maxAnguloParaChute) anguloDirJogadorBola = -anguloDirJogadorBola;
            }
        }
        #endregion

        anguloBola = -(anguloDirJogadorBola / maxAnguloParaChute) * 90 + anguloBolaJogador;
        

        direcaoBola = new Vector3(Mathf.Cos(anguloBola * Mathf.Deg2Rad), Mathf.Tan(alturaChute / maxAlturaChute * 40 * Mathf.Deg2Rad), Mathf.Sin(anguloBola * Mathf.Deg2Rad));

        direcional.transform.position = bola.transform.position;
        direcional.transform.eulerAngles = new Vector3(0, 360 - anguloBola, Mathf.Atan(direcaoBola.y) * Mathf.Rad2Deg);

        Debug.DrawRay(jogador.transform.position, direcaoChute, Color.green);
        Debug.DrawRay(jogador.transform.position, direcaoJogadorBola * distanciaDaBola, Color.blue);
        Debug.DrawRay(bola.transform.position, direcaoBola);
    }
}
