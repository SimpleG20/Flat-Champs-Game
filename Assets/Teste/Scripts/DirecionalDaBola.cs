using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirecionalDaBola : MonoBehaviour
{
    private FisicaBola bola;
    MovimentacaoDoJogador movimentacaoDoJogador;

    private void Start()
    {
        bola = FindObjectOfType<FisicaBola>();
        movimentacaoDoJogador = FindObjectOfType<MovimentacaoDoJogador>();
    }

    void Update()
    {
        if (!LogisticaVars.goleiroT1 && !LogisticaVars.goleiroT2)
        {
            if (LogisticaVars.jogadorSelecionado && transform.GetChild(0).GetComponent<MeshRenderer>().enabled) {
                transform.position = bola.transform.position;
                float z = Mathf.Atan(movimentacaoDoJogador.GetDirecaoBola().y) * Mathf.Rad2Deg;
                //if (z == Mathf.Infinity || z == -Mathf.Infinity) z = 360;
                transform.eulerAngles = new Vector3(0, 360 - movimentacaoDoJogador.GetAnguloBola(), z);
            }
        }
        else
        {
            if (LogisticaVars.m_goleiroGameObject != null && transform.GetChild(0).GetComponent<MeshRenderer>().enabled)
            {
                transform.position = bola.transform.position;
                float z = Mathf.Atan(movimentacaoDoJogador.GetDirecaoBola().y) * Mathf.Rad2Deg;
                //if (z == Mathf.Infinity || z == -Mathf.Infinity) z = 360;
                transform.eulerAngles = new Vector3(0, 360 - movimentacaoDoJogador.GetAnguloBola(), z);
            }
        }
        

    }
}
