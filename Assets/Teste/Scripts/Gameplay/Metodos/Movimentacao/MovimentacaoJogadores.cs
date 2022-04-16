using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentacaoJogadores : MonoBehaviour
{
    public GameObject j;

    const float maxAlturaChute = 4;
    float senoJogador, cosJogador, distanciaDaBola, maxAnguloParaChute, alturaChute;
    float anguloBola, anguloJogador, anguloBolaJogador, anguloDirJogadorBola;
    protected float erro;

    protected Vector3 direcaoChute, direcaoJogadorBola, direcaoBola, ultimaDirecao;

    protected GameObject direcional;
    protected InputManager joystickManager;
    protected FisicaBola bola;


    protected void SetDirecaoChute(GameObject jogador)
    {
        j = jogador;
        if(bola == null) bola = FindObjectOfType<FisicaBola>();
        if (joystickManager == null) joystickManager = FindObjectOfType<InputManager>();

        senoJogador = -jogador.transform.up.z;
        cosJogador = -jogador.transform.up.x;
        direcaoChute = new Vector3(cosJogador, 0, senoJogador);
        distanciaDaBola = (bola.transform.position - jogador.transform.position).magnitude;
        direcaoJogadorBola = bola.transform.position - jogador.transform.position;

        erro = Mathf.Clamp(distanciaDaBola, 0, 10);
        maxAnguloParaChute = (360 / Mathf.Pow(distanciaDaBola, 2)) + Mathf.Pow(((distanciaDaBola - 2) / distanciaDaBola) + 1.25f, 2) + erro;
        anguloJogador = Mathf.Acos(direcaoChute.x) * Mathf.Rad2Deg;
        anguloBolaJogador = Mathf.Acos(direcaoJogadorBola.x / direcaoJogadorBola.magnitude) * Mathf.Rad2Deg;
        anguloDirJogadorBola = Mathf.Acos((direcaoJogadorBola.x * direcaoChute.x + direcaoChute.z * direcaoJogadorBola.z) / 
            (direcaoJogadorBola.magnitude * direcaoChute.magnitude)) * Mathf.Rad2Deg;


        if (!LogisticaVars.bolaRasteiraT1 && LogisticaVars.vezJ1 || !LogisticaVars.bolaRasteiraT2 && LogisticaVars.vezJ2)
        {
            if (MovimentacaoDoJogador.pc) alturaChute += Input.GetAxis("Vertical") * Time.deltaTime;
            else alturaChute += joystickManager.direcaoRight.y * Time.deltaTime;
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
            if (LogisticaVars.mostrarDirecaoBola)
            {
                if (bola.m_bolaNoChao && !bola.m_bolaCorrendo && !JogadorVars.m_esperandoContato && !direcional.transform.GetChild(0).GetComponent<MeshRenderer>().enabled) 
                    direcional.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                if ((!bola.m_bolaNoChao || JogadorVars.m_esperandoContato || bola.m_bolaCorrendo || LogisticaVars.especial) && direcional.transform.GetChild(0).GetComponent<MeshRenderer>().enabled) 
                    direcional.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                if (direcional.transform.GetChild(0).GetComponent<MeshRenderer>().enabled) direcional.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            }
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

        /*Debug.DrawRay(jogador.transform.position, direcaoChute, Color.green);
        Debug.DrawRay(jogador.transform.position, direcaoJogadorBola, Color.blue);
        Debug.DrawRay(bola.transform.position, direcaoChute);*/
    }

    public void SetBola(FisicaBola b)
    {
        bola = b;
    }
    public void SetInput(InputManager i)
    {
        joystickManager = i;
    }
    public void SetDirecional(GameObject d, bool b)
    {
        direcional = d;
        if (b == true) direcional.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
    }

    public float GetAnguloBola()
    {
        return anguloBola;
    }
    public Vector3 GetUltimaDirecao()
    {
        if (LogisticaVars.continuaSendoFora) ultimaDirecao = direcaoBola;

        //ultimaDirecao = direcaoBola;
        if (LogisticaVars.bolaRasteiraT1 && LogisticaVars.vezJ1 || LogisticaVars.bolaRasteiraT2 && LogisticaVars.vezJ2)
            ultimaDirecao.y = 0;

        return ultimaDirecao;
    }
    public Vector3 GetDirecaoBola()
    {
        return direcaoBola;
    }
}
