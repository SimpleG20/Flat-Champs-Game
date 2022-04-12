using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesteDirecaoBola : MonoBehaviour
{
    public bool mover, aplicouChute;

    public GameObject jogador, bola, direcionalBola;
    const float maxAltura = 3.5f;
    public float erro;
    float cosJogador, senoJogador;
    float anguloJogador, anguloBolaJogador, anguloDirJogadorBola;
    public float forcaChute, anguloBola, anguloChute, alturaChute, distancia;

    public Vector3 ultimaDirecao;

    private Vector3 direcaoJogador;
    private Vector3 direcaoJogadorBola, direcaoBola;


    // Update is called once per frame
    void Update()
    {
        erro = Mathf.Clamp(distancia, 0, 10);

        #region Dados
        float v = Input.GetAxis("Vertical");
        senoJogador = -jogador.transform.up.z;
        cosJogador = -jogador.transform.up.x;
        distancia = (bola.transform.position - jogador.transform.position).magnitude;
        direcaoJogador = new Vector3(cosJogador, 0, senoJogador);
        direcaoJogadorBola = new Vector3(bola.transform.position.x - jogador.transform.position.x, 0, bola.transform.position.z - jogador.transform.position.z);
        anguloChute = (360 / Mathf.Pow(distancia, 2)) + Mathf.Pow(((distancia - 2) / distancia) + 1.25f, 2) + erro;
        anguloJogador = Mathf.Acos(cosJogador) * Mathf.Rad2Deg;
        anguloBolaJogador = Mathf.Acos(direcaoJogadorBola.x / direcaoJogadorBola.magnitude) * Mathf.Rad2Deg;
        anguloDirJogadorBola = Mathf.Acos((direcaoJogadorBola.x * direcaoJogador.x + direcaoJogador.z * direcaoJogadorBola.z) / (direcaoJogadorBola.magnitude * direcaoJogador.magnitude)) * Mathf.Rad2Deg;

        #region Altura Chute
        //if (v != 0) alturaChute = alturaChute + v + Time.deltaTime;
        if (alturaChute >= maxAltura) alturaChute = maxAltura;
        if (alturaChute <= 0) alturaChute = 0;
        #endregion


        #region Ajustes
        if (anguloChute > 90) anguloChute = 90;
        if (anguloDirJogadorBola > anguloChute) anguloDirJogadorBola = 0;
        if (senoJogador < 0) anguloJogador = 360 - anguloJogador;
        if (direcaoJogadorBola.z < 0) anguloBolaJogador = 360 - anguloBolaJogador;
        if (anguloBolaJogador == 360) anguloBolaJogador = 0;

        if (anguloJogador < anguloBolaJogador)
        {
            if (anguloJogador < 0 || anguloJogador > 0) anguloDirJogadorBola = -anguloDirJogadorBola;
        }
        else
        {
            if (anguloJogador > 360 - anguloChute) anguloDirJogadorBola = -anguloDirJogadorBola;
        }

        anguloBola = -(anguloDirJogadorBola / anguloChute) * 90 + anguloBolaJogador;

        direcaoBola = new Vector3(Mathf.Cos(anguloBola * Mathf.Deg2Rad), Mathf.Tan(alturaChute / maxAltura * 40 * Mathf.Deg2Rad), Mathf.Sin(anguloBola * Mathf.Deg2Rad));

        direcionalBola.transform.position = bola.transform.position;
        direcionalBola.transform.eulerAngles = new Vector3(0, 360 - anguloBola, Mathf.Atan(direcaoBola.y) * Mathf.Rad2Deg);
        #endregion

        Debug.DrawRay(jogador.transform.position, direcaoJogador * 5, Color.red);
        Debug.DrawRay(jogador.transform.position, direcaoJogadorBola, Color.blue);
        Debug.DrawRay(bola.transform.position, direcaoBola, Color.green);

        #endregion

        if (mover)
        {
            ultimaDirecao = direcaoBola;
            jogador.GetComponent<Rigidbody>().AddForce(direcaoJogador * forcaChute, ForceMode.Impulse);
            mover = false;
        }

        
    }
    private void FixedUpdate()
    {
        if (distancia < 2f && !aplicouChute)
        {
            float qntMovimento = jogador.GetComponent<Rigidbody>().velocity.magnitude * 2.5f;

            bola.GetComponent<Rigidbody>().AddForce(ultimaDirecao * qntMovimento, ForceMode.Impulse);
            jogador.GetComponent<Rigidbody>().AddForce(- new Vector3(ultimaDirecao.x, 0, ultimaDirecao.z) * 50, ForceMode.Impulse);
            aplicouChute = true;
        }
    }
}
