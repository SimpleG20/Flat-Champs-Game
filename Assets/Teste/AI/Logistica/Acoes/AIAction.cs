using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIAction
{
    protected readonly GameObject ai_player;
    protected readonly AISystem ai_System;
    public AIAction(AISystem AiSystem, GameObject ai)
    {
        ai_System = AiSystem;
        ai_player = ai;
    }

    #region Movimento
    public virtual void IniciarAction()
    {
        
    }
    public virtual void DecidirMovimento()
    {

    }
    public virtual IEnumerator Mover_Posicao()
    {
        yield break;
    }
    #endregion

    #region Obstaculos
    public virtual void DetectarJogadores()
    {

    }
    public virtual void VerificarObstaculos()
    {

    }
    public virtual bool HaObstaculos(out bool esq, out bool dir, out bool frente)
    {
        esq = dir = frente = default;
        return false;
    }
    #endregion

    #region Rotacionar
    public virtual IEnumerator Rotacionar_Alvo(Vector3 alvo)
    {
        yield break;
    }
    public virtual IEnumerator Rotacionar_VasculharArea()
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
    #endregion

    #region Chutes
    public virtual IEnumerator Chute_Avancar()
    {
        yield break;
    }
    public virtual IEnumerator Chute_Chutao()
    {
        yield break;
    }
    public virtual IEnumerator Chute_Gol()
    {
        yield break;
    }
    public virtual IEnumerator Chute_Passe()
    {
        yield break;
    }
    #endregion

    public virtual void AcaoFinalizada()
    {
        ai_System.SetDecisao(AISystem.Decisao.NONE);
        Debug.Log("Fim Acao");
    }
}
