using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InicializacaoVariaveisGameplay : MonoBehaviour
{
    private void InicializarVarivaies6v6()
    {
        LogisticaVars.m_maxEspecial = 500;
        LogisticaVars.tempoMaxJogada = 20;
        LogisticaVars.tempoMaxEscolhaJogador = 8;
        LogisticaVars.tempoPartida = 480;
    }
    private void InicializarVariaveisClassico()
    {
        LogisticaVars.m_maxEspecial = 500;
        LogisticaVars.tempoMaxJogada = 20;
        LogisticaVars.tempoMaxEscolhaJogador = 8;
        LogisticaVars.tempoPartida = 600;
    }
    private void InicializarVariaveis1v1()
    {
        LogisticaVars.m_maxEspecial = 500;
        LogisticaVars.tempoMaxJogada = 20;
        LogisticaVars.tempoMaxEscolhaJogador = 8;
        LogisticaVars.tempoPartida = 360;
    }
    private void InicializarVariaveis3v3()
    {
        LogisticaVars.m_maxEspecial = 500;
        LogisticaVars.tempoMaxJogada = 20;
        LogisticaVars.tempoMaxEscolhaJogador = 8;
        LogisticaVars.tempoPartida = 420;
    }

    private void InicializarListas()
    {
        LogisticaVars.jogadoresT1 = new List<GameObject>();
        LogisticaVars.jogadoresT2 = new List<GameObject>();
    }

}
