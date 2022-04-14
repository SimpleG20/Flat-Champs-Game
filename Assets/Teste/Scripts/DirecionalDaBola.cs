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
        if (LogisticaVars.jogadorSelecionado)
        {
            transform.position = bola.transform.position;
            transform.eulerAngles = new Vector3(0, 360 - movimentacaoDoJogador.GetAnguloBola(), Mathf.Atan(movimentacaoDoJogador.GetDirecaoChute().y) * Mathf.Rad2Deg);
        }
        
    }
}
