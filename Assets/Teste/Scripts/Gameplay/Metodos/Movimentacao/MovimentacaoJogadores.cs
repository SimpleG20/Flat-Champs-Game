using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentacaoJogadores : MonoBehaviour
{
    const float maxAlturaChute = 4;
    float senoJogador, cosJogador, distanciaDaBola, maxAnguloParaChute, alturaChute;
    float anguloBola, anguloJogador, anguloBolaJogador, anguloDirJogadorBola;
    protected float erro;

    protected Vector3 direcaoChute, direcaoJogadorBola, direcaoBola, ultimaDirecao;

    protected GameObject direcional;
    protected InputManager joystickManager;
    protected FisicaBola bola;

    void Start()
    {
        bola = FindObjectOfType<FisicaBola>();
        joystickManager = FindObjectOfType<InputManager>();

        if(direcional == null) direcional = GameObject.FindGameObjectWithTag("Direcional Chute");
        if(direcional.activeSelf) direcional.SetActive(false);
    }

    protected void SetDirecaoChute(GameObject jogador)
    {
        senoJogador = -jogador.transform.up.z;
        cosJogador = -jogador.transform.up.x;
        direcaoChute = new Vector3(cosJogador, 0, senoJogador);
        distanciaDaBola = (bola.transform.position - jogador.transform.position).magnitude;
        direcaoJogadorBola = bola.transform.position - jogador.transform.position;

        erro = Mathf.Clamp(distanciaDaBola, 0, 10);
        maxAnguloParaChute = (360 / Mathf.Pow(distanciaDaBola, 2)) + Mathf.Pow(((distanciaDaBola - 2) / distanciaDaBola) + 1.25f, 2) + erro;
        anguloJogador = Mathf.Acos(direcaoChute.x) * Mathf.Rad2Deg;
        anguloBolaJogador = Mathf.Acos(direcaoJogadorBola.x / direcaoJogadorBola.magnitude) * Mathf.Rad2Deg;
        anguloDirJogadorBola = Mathf.Acos((direcaoJogadorBola.x * direcaoChute.x + direcaoChute.z * direcaoJogadorBola.z) / (direcaoJogadorBola.magnitude * direcaoChute.magnitude)) * Mathf.Rad2Deg;


        if (!LogisticaVars.bolaRasteiraT1 && LogisticaVars.vezJ1 || !LogisticaVars.bolaRasteiraT2 && LogisticaVars.vezJ2)
            alturaChute += joystickManager.direcaoRight.y * Time.deltaTime;
        else alturaChute = 0;
        if (alturaChute >= maxAlturaChute) alturaChute = maxAlturaChute;
        if (alturaChute <= 0) alturaChute = 0;

        #region Ajustes
        if (maxAnguloParaChute > 90) maxAnguloParaChute = 90;
        if (anguloDirJogadorBola > maxAnguloParaChute) { direcional.SetActive(false); }
        else
        {
            if (bola.m_bolaNoChao && !bola.m_bolaCorrendo && LogisticaVars.mostrarDirecaoBola && !direcional.activeSelf) direcional.SetActive(true);
            else if (!bola.m_bolaNoChao) direcional.SetActive(false);
        }
        if (senoJogador < 0) anguloJogador = 360 - anguloJogador;
        if (bola.m_vetorDistanciaDoJogador.z < 0) anguloBolaJogador = 360 - anguloBolaJogador;
        if (anguloBolaJogador == 360) anguloBolaJogador = 0;

        if (anguloJogador < anguloBolaJogador)
        {
            if (anguloJogador < 0 || anguloJogador > 0) anguloDirJogadorBola = -anguloDirJogadorBola;
        }
        else
        {
            if (anguloJogador > 360 - maxAnguloParaChute) anguloDirJogadorBola = -anguloDirJogadorBola;
        }

        anguloBola = -(anguloDirJogadorBola / maxAnguloParaChute) * 90 + anguloBolaJogador;
        #endregion

        direcaoBola = new Vector3(Mathf.Cos(anguloBola * Mathf.Deg2Rad), Mathf.Tan(alturaChute / maxAlturaChute * 40 * Mathf.Deg2Rad), Mathf.Sin(anguloBola * Mathf.Deg2Rad));
    }

    public float GetAnguloBola()
    {
        return anguloBola;
    }
    public Vector3 GetUltimaDirecao()
    {
        ultimaDirecao = direcaoBola;
        if (LogisticaVars.bolaRasteiraT1 && LogisticaVars.vezJ1 || LogisticaVars.bolaRasteiraT2 && LogisticaVars.vezJ2)
            ultimaDirecao.y = 0;

        return ultimaDirecao;
    }
    public Vector3 GetDirecaoChute()
    {
        return direcaoBola;
    }
}
