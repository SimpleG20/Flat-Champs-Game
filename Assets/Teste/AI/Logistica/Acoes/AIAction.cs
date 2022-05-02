using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIAction
{
    protected static GameObject ai;
    protected static AISystem _system;
    public AIAction(AISystem system)
    {
        _system = system;
        if (LogisticaVars.m_jogadorAi != null) ai = LogisticaVars.m_jogadorAi;
    }

    public void Iniciar_Movimento()
    {
        Vector3 aiPos = ai.transform.position;
        if (Vector3.Distance(_system.bola.m_pos, aiPos) < 4.5f) { Debug.Log("Se Arrumar para Chutar a Bola"); return; }
        if (LogisticaVars.jogadas == 3) { Debug.Log("Trocar Vez"); return; }

        _system.RotacionarPOV();
    }
    public virtual void DecidirMovimento()
    {

    }
    public virtual IEnumerator Mover_Posicao()
    {
        yield break;
    }

    public virtual void DetectarJogadores()
    {

    }
    public virtual void VerificarObstaculos()
    {

    }

    public virtual IEnumerator Rotacionar_Alvo(Vector3 alvo, bool proximaJogada)
    {
        yield break;
    }
    public virtual IEnumerator Rotacionar_POV()
    {
        yield break;
    }
    public virtual IEnumerator Rotacionar_Posicao()
    {
        yield break;
    }
    public virtual IEnumerator Rotacionar_FicarLivre(float direcao)
    {
        yield break;
    }


    public virtual void AcaoFinalizada()
    {
        Debug.Log("Fim Acao");
    }
}
