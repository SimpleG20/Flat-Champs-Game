using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISystem : AIDecision
{
    public const float campoVisao = 15;
    public float anguloJogador, fatorExtraChute = 1, magnitudeChute;
    public float menorDistanciaAoGol_JogadoresAmigos = 1000, menorDistancia_JogadoresInimigos = 1000;
    public int decisao;

    public Vector3 golPos;
    public Vector3 posDestino, posParaChute;

    public Quaternion rotacaoAnt;
    public LayerMask layerMask;

    public GameObject rotacaoCamera;
    public List<GameObject> jogadorAmigo_MaisPerto, jogadorInimigo_MaisPerto;

    public FisicaBola bola;
    public StateSystem stateSystem;

    private void Start()
    {
        jogadorAmigo_MaisPerto = LogisticaVars.jogadoresT2;
        jogadorInimigo_MaisPerto = LogisticaVars.jogadoresT1;

        golPos = GameObject.FindGameObjectWithTag("Gol1").transform.position;
        rotacaoCamera = GameObject.Find("RotacaoCamera");

        bola = FindObjectOfType<FisicaBola>();
        stateSystem = FindObjectOfType<StateSystem>();
    }

    #region Acoes

    #region Movimento
    public void OnIniciarMovimento()
    {
        iAction.Iniciar_Movimento();
    }
    public void DecidirMovimento()
    {
        iAction.DecidirMovimento();
    }
    public void MoverParaPosicao()
    {
        StartCoroutine(iAction.Mover_Posicao());
    }
    public void FimMovimento()
    {
        stateSystem.OnEsperar();
    }
    #endregion

    #region Rotacao
    public void RotacionarPOV()
    {
        StartCoroutine(iAction.Rotacionar_POV());
    }
    public void RotacionarParaAlvo(Vector3 alvo, bool proximaJogada = default)
    {
        StartCoroutine(iAction.Rotacionar_Alvo(alvo, proximaJogada));
    }
    public void RotacionarParaPosicao()
    {
        StartCoroutine(iAction.Rotacionar_Posicao());
    }
    public void RotacionarAteFicarLivre(float direcao)
    {
        StartCoroutine(iAction.Rotacionar_FicarLivre(direcao));
    }
    #endregion

    public void DetectarJogadores()
    {
        iAction.DetectarJogadores();
    }
    public void VerificarObstaculos()
    {
        iAction.VerificarObstaculos();
    }

    #region Chute

    #endregion

    #region Especial

    #endregion

    #endregion
}
