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
        ai_player = LogisticaVars.m_jogadorEscolhido_Atual;
    }

    #region Movimento
    public virtual void IniciarAction()
    {
        
    }
    public virtual void DecidirPosicao()
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
    public float PreferenciaParaRotacionar()
    {
        Vector3 aiPos = ai_player.transform.position;

        Vector3 aux = (ai_System.posParaChute - aiPos);
        //Debug.Log(aux.x);

        if (aux.x < 0) return 1;
        else return -1;

        /*if (ai_System.posParaChute.x < aiPos.x)
        {
            if (ai_System.posParaChute.z < aiPos.z)
            {
                if ((aiPos.z - ai_System.posParaChute.z) < (aiPos.x - ai_System.posParaChute.x)) { Debug.Log("ROTACAO: Direita"); return 1; }
                else { Debug.Log("ROTACAO: Esquerda"); return -1; } //print("Esquerda");
            }
            else
            {
                if ((ai_System.posParaChute.z - aiPos.z) > (aiPos.x - ai_System.posParaChute.x)) { Debug.Log("ROTACAO: Direita"); return 1; } //print("Direita");
                else { Debug.Log("ROTACAO: Esquerda"); return -1; } // print("Esquerda");
            }
        }
        else
        {
            if (ai_System.posParaChute.z < aiPos.z)
            {
                if ((aiPos.z - ai_System.posParaChute.z) > (ai_System.posParaChute.x - aiPos.x)) { Debug.Log("ROTACAO: Direita"); return 1; } //print("Direita");
                else { Debug.Log("ROTACAO: Esquerda"); return -1; }//print("Esquerda");
            }
            else
            {
                if ((ai_System.posParaChute.z - aiPos.z) < (ai_System.posParaChute.x - aiPos.x)) { Debug.Log("ROTACAO: Direita"); return 1; }//print("Direita");
                else { Debug.Log("ROTACAO: Esquerda"); return -1; }//print("Esquerda");
            }
        }*/
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
    public virtual IEnumerator Chute_Lateral()
    {
        yield break;
    }
    public virtual IEnumerator Chute_Escanteio()
    {
        yield break;
    }

    #endregion

    #region Especial
    public virtual IEnumerator Especial_Instanciar()
    {
        yield break;
    }
    public virtual IEnumerator Especial_Mira()
    {
        yield break;
    }
    public virtual IEnumerator Especial_MoverTarget()
    {
        yield break;
    }
    public virtual IEnumerator Especial_Trajetoria()
    {
        yield break;
    }
    public virtual IEnumerator Especial_Chute()
    {
        yield break;
    }
    #endregion

    #region Goleiro
    public virtual IEnumerator Movimentar_Defender(Vector3 target)
    {
        yield break;
    }
    public virtual IEnumerator Chute_TiroDeMeta_PequenaArea()
    {
        yield break;
    }
    public virtual IEnumerator Rotacionar_GoleiroChute()
    {
        yield break;
    }
    public virtual IEnumerator Rotacionar_GoleiroDefender()
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
