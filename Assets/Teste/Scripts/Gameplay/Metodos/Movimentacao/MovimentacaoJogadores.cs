using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentacaoJogadores : MonoBehaviour
{
    public GameObject j;

    const float maxAlturaChute = 3f;
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
        if (bola == null) bola = Gameplay._current._bola;
        if (joystickManager == null) joystickManager = InputManager.current;

        #region Dados
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
        #endregion

        #region Altura
        if (!LogisticaVars.bolaRasteiraT1 && LogisticaVars.vezJ1 || !LogisticaVars.bolaRasteiraT2 && LogisticaVars.vezJ2)
        {
            if (MovimentacaoDoJogador.pc) alturaChute += Input.GetAxis("Vertical") * Time.deltaTime;
            else alturaChute += joystickManager.direcaoRight.y * Time.deltaTime;
        }
        else alturaChute = 0;
        if (alturaChute >= maxAlturaChute) alturaChute = maxAlturaChute;
        if (alturaChute <= 0) alturaChute = 0;
        #endregion

        #region Ajustes
        if (maxAnguloParaChute > 90) maxAnguloParaChute = 90;
        if (maxAnguloParaChute <= 1) maxAnguloParaChute = 1;

        #region Visualizacao
        if (distanciaDaBola > 10)
        {
            if (direcional.transform.GetChild(0).GetComponent<MeshRenderer>().enabled) direcional.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
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
        }
        #endregion

        if (senoJogador < 0) anguloJogador = 360 - anguloJogador;
        if (direcaoJogadorBola.z < 0) anguloBolaJogador = 360 - anguloBolaJogador;
        if (anguloBolaJogador == 360) anguloBolaJogador = 0;


        if (anguloJogador < anguloBolaJogador)
        {
            if (anguloJogador < 0 || anguloJogador > 0) anguloDirJogadorBola = -anguloDirJogadorBola;
        }
        else
        {
            if (direcaoJogadorBola.z > 0 || anguloBolaJogador == 0)
            {
                if (anguloJogador > 360 - maxAnguloParaChute) anguloDirJogadorBola = -anguloDirJogadorBola;
            }
        }
        #endregion

        
        if (LogisticaVars.continuaSendoFora || LogisticaVars.bolaPermaneceNaPequenaArea)
        {
            anguloBola = anguloJogador;
        }
        else
        {
            anguloBola = -(anguloDirJogadorBola / maxAnguloParaChute) * 90 + anguloBolaJogador;
        }
        if (float.IsNaN(anguloBola)) anguloBola = 0;

        direcaoBola = new Vector3(Mathf.Cos(anguloBola * Mathf.Deg2Rad), Mathf.Tan(alturaChute / maxAlturaChute * 30 * Mathf.Deg2Rad), Mathf.Sin(anguloBola * Mathf.Deg2Rad));

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
        if (float.IsNaN(anguloBola)) return 0;
        return anguloBola;
    }
    public Vector3 GetUltimaDirecao()
    {
        if (LogisticaVars.continuaSendoFora || LogisticaVars.goleiroT1 || LogisticaVars.goleiroT2) ultimaDirecao = direcaoBola;

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
