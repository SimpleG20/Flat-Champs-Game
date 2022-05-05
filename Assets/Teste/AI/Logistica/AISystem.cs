using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISystem : AIDecision
{
    public enum Decisao { NONE, PASSAR_AMIGO, AVANCAR, CHUTAO, CHUTAR_GOL, ESPECIAL }
    [SerializeField] Decisao _decisaoAtual;

    public const float campoVisao = 15;
    public float fatorExtraChute = 1, magnitudeChute;
    public float menorDistanciaAoGol_JogadoresAmigos = 1000, menorDistancia_JogadoresInimigos = 1000;
    public float xCampo, zCampo;
    //public int decisao;

    public bool _passouBola;

    public Vector3 golPos;
    public Vector3 posTarget, posParaChute;

    public Quaternion rotacaoAnt;
    public LayerMask layerMask;

    public GameObject rotacaoCamera;
    public GameObject ai_player;
    public List<GameObject> jogadorAmigo_MaisPerto, jogadorInimigo_MaisPerto;

    public FisicaBola bola;
    StateSystem stateSystem;

    private void Awake()
    {
        DimensaoCampo dimensaoCampo = FindObjectOfType<DimensaoCampo>();
        bola = FindObjectOfType<FisicaBola>();
        stateSystem = FindObjectOfType<StateSystem>();
        
        //jogadorAmigo_MaisPerto = LogisticaVars.jogadoresT2;
        //jogadorInimigo_MaisPerto = LogisticaVars.jogadoresT1;

        golPos = GameObject.FindGameObjectWithTag("Gol1").transform.position;
        rotacaoCamera = GameObject.Find("RotacaoCamera");

        xCampo = dimensaoCampo.TamanhoCampo().x;
        zCampo = dimensaoCampo.TamanhoCampo().y;

        _decisaoAtual = Decisao.NONE;
    }

    public StateSystem GetStateSystem()
    {
        return stateSystem;
    }

    public void VerificarProximaJogada()
    {
        if (LogisticaVars.jogadas == 3) { stateSystem.OnEnd(); return; }

        stateSystem.OnEsperar();
    }


    #region Decisao conforme a situacao da Bola
    public void TomarDecisao()
    {
        //Chutao ou avancar com a bola ou passar amigo
        if (bola.m_pos.z >= zCampo / 4) _decisaoAtual = BolaMaisRecuada();
        else
        {
            if (bola.m_pos.z <= -zCampo / 4) _decisaoAtual = Bola_Perto_Area();
            else _decisaoAtual = Bola_Mais_A_Frente();
        }
    }
    Decisao BolaMaisRecuada()
    {
        int random = Random.Range(0, 3);

        switch (random)
        {
            case 0:
                return Decisao.CHUTAO;
            case 1:
                return Decisao.AVANCAR;
            case 2:
                return Decisao.PASSAR_AMIGO;
            default:
                return Decisao.PASSAR_AMIGO;
        }
    }
    Decisao Bola_Mais_A_Frente()
    {
        int random = Random.Range(0, 10);

        if (random < 2) return Decisao.CHUTAR_GOL;
        else if (random >= 2 && random < 6) return Decisao.PASSAR_AMIGO;
        else if (random >= 6 && random < 9) return Decisao.AVANCAR;
        else return Decisao.CHUTAO;
    }
    Decisao Bola_Perto_Area()
    {
        int random = Random.Range(0, 6);

        if (random < 2) return Decisao.AVANCAR;
        else if (random >= 2 && random < 5) return Decisao.CHUTAR_GOL;
        else return Decisao.PASSAR_AMIGO;
    }

    public Decisao GetDecisao()
    {
        return _decisaoAtual;
    }
    public void SetDecisao(Decisao estado)
    {
        _decisaoAtual = estado;
    }
    #endregion


    #region Acoes

    #region Movimento
    public void OnIniciarMovimento()
    {
        iAction.IniciarAction();
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
        //_decisaoAtual = Decisao.NONE;
        iAction = null;
        stateSystem.OnEnd();
    }
    #endregion

    #region Rotacao
    public void RotacionarVasculharArea()
    {
        //print("AI_SYSTEM: ROTACIONAR POV");
        AIAction aux = iAction.GetType() != typeof(AIRotation) ? new AIRotation(this, ai_player) : iAction;
        StartCoroutine(aux.Rotacionar_VasculharArea());
    }
    public void RotacionarParaAlvo(Vector3 alvo)
    {
        AIAction aux = iAction.GetType() != typeof(AIRotation) ? new AIRotation(this, ai_player) : iAction;
        StartCoroutine(aux.Rotacionar_Alvo(alvo));
    }
    public void RotacionarParaPosicao()
    {
        AIAction aux = iAction.GetType() != typeof(AIRotation) ? new AIRotation(this, ai_player) : iAction;
        StartCoroutine(aux.Rotacionar_Posicao());
    }
    public void RotacionarAteFicarLivre(float direcao)
    {
        AIAction aux = iAction.GetType() != typeof(AIRotation) ? new AIRotation(this, ai_player) : iAction;
        StartCoroutine(aux.Rotacionar_FicarLivre(direcao));
    }
    #endregion

    #region Obstaculos
    public void DetectarJogadores()
    {
        AIAction aux = iAction.GetType() != typeof(AIVision) ? new AIVision(this, ai_player) : iAction;
        aux.DetectarJogadores();
    }
    public void VerificarObstaculos()
    {
        AIAction aux = iAction.GetType() != typeof(AIVision) ? new AIVision(this, ai_player) : iAction;
        aux.VerificarObstaculos();
    }
    public bool HaObstaculo(out bool esq, out bool dir, out bool frente)
    {
        AIAction aux = iAction.GetType() != typeof(AIVision) ? new AIVision(this, ai_player) : iAction;
        aux.HaObstaculos(out esq, out dir, out frente);

        return (esq || dir || frente);
    }
    #endregion

    #region Chute
    public void ChuteEspecial()
    {
        //StartCoroutine()
    }
    public void ChuteAvancar()
    {
        StartCoroutine(iAction.Chute_Avancar());
    }
    public void ChutePasse()
    {
        StartCoroutine(iAction.Chute_Passe());
    }
    public void ChuteChutao()
    {
        StartCoroutine(iAction.Chute_Chutao());
    }
    public void ChuteGol()
    {
        StartCoroutine(iAction.Chute_Gol());
    }
    #endregion

    #region Especial

    #endregion

    #endregion
}
