using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirecionalDaBola : MonoBehaviour
{
    private FisicaBola bola;
    MovimentacaoDoJogador movimentacaoDoJogador;
    MovimentacaoDoGoleiro movimentacaoDoGoleiro;

    private void Start()
    {
        bola = FindObjectOfType<FisicaBola>();
        movimentacaoDoJogador = FindObjectOfType<MovimentacaoDoJogador>();
        movimentacaoDoGoleiro = FindObjectOfType<MovimentacaoDoGoleiro>();
    }

    void Update()
    {
        if(!LogisticaVars.goleiroT1 && !LogisticaVars.goleiroT2)
        {
            if (LogisticaVars.jogadorSelecionado)
            {
                transform.position = bola.transform.position;
                transform.eulerAngles = new Vector3(0, 360 - movimentacaoDoJogador.GetAnguloBola(), Mathf.Atan(movimentacaoDoJogador.GetDirecaoBola().y) * Mathf.Rad2Deg);
            }
        }
        else
        {
            transform.position = bola.transform.position;
            transform.eulerAngles = new Vector3(0, 360 - movimentacaoDoGoleiro.GetAnguloBola(), Mathf.Atan(movimentacaoDoGoleiro.GetDirecaoBola().y) * Mathf.Rad2Deg);
        }
        
    }
}
