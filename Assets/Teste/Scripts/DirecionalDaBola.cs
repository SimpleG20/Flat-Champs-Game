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
            if (LogisticaVars.jogadorSelecionado) {
                transform.position = bola.transform.position;
                transform.eulerAngles = new Vector3(0, 360 - movimentacaoDoJogador.GetAnguloBola(), Mathf.Atan(movimentacaoDoJogador.GetDirecaoBola().y) * Mathf.Rad2Deg);
            }
        }
        else
        {
            if (LogisticaVars.m_goleiroGameObject != null)
            {
                transform.position = bola.transform.position;
                transform.eulerAngles = new Vector3(0, 360 - movimentacaoDoJogador.GetAnguloBola(), Mathf.Atan(movimentacaoDoJogador.GetDirecaoBola().y) * Mathf.Rad2Deg);
            }
        }
        

    }
}
