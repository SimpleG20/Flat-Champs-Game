using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirecionalDaBola : MonoBehaviour
{
    public float anguloBolaX, anguloBolaY;
    private FisicaBola bola;
    private float cosBola, senoBola, anguloAux;

    private void Start()
    {
        bola = FindObjectOfType<FisicaBola>();
    }

    void FixedUpdate()
    {
        transform.position = bola.transform.position;

        if (LogisticaVars.jogadorSelecionado)
        {
            Debug.DrawRay(LogisticaVars.m_jogadorEscolhido.transform.position, JogadorVars.direcaoChute * 5, Color.red); ;

            anguloBolaX = Mathf.Acos((JogadorVars.direcaoChute.x * bola.m_vetorDistanciaDoJogador.x + JogadorVars.direcaoChute.y * bola.m_vetorDistanciaDoJogador.y + JogadorVars.direcaoChute.z * bola.m_vetorDistanciaDoJogador.z)
                / (JogadorVars.direcaoChute.magnitude * bola.m_vetorDistanciaDoJogador.magnitude)) * Mathf.Rad2Deg;

            if (anguloBolaX < 18) transform.GetChild(0).gameObject.SetActive(true);
            else transform.GetChild(0).gameObject.SetActive(false);

            //anguloAux = SelecaoMetodos.ObterAnguloParaRedirecionamento(bola.transform.position, LogisticaVars.m_jogadorEscolhido);

            cosBola = Mathf.Cos(anguloBolaX * Mathf.Deg2Rad);
            senoBola = Mathf.Sin(anguloBolaX * Mathf.Deg2Rad);

            if (JogadorVars.direcaoChute.x > 0) bola.direcaoBola = new Vector3(-senoBola, 0, -cosBola);
            else bola.direcaoBola = new Vector3(senoBola, 0, -cosBola);

            //Debug.DrawRay(bola.transform.position, bola.direcaoBola * 2, Color.green);

            if (JogadorVars.direcaoChute.x > 0) transform.eulerAngles = new Vector3(0, -anguloAux + Mathf.Atan((-cosBola) / senoBola) * Mathf.Rad2Deg, 0);
            else transform.eulerAngles = new Vector3(0, +anguloAux + Mathf.Atan((-cosBola) / (-senoBola)) * Mathf.Rad2Deg, 0);
        }
    }
}
