using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRotation : AIAction
{
    public AIRotation(AISystem system, GameObject ai) : base(system, ai)
    {
    }


    public override IEnumerator Rotacionar_Alvo(Vector3 alvo)
    {
        yield return new WaitForSeconds(0.01f);
        float step = 0.5f;
        ai_System.rotacaoCamera.transform.position = Vector3.MoveTowards(ai_System.rotacaoCamera.transform.position, alvo, step);

        Vector3 direction = ai_System.rotacaoCamera.transform.position - ai_player.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        ai_player.transform.rotation = rotation;
        ai_player.transform.eulerAngles = new Vector3(-90, ai_player.transform.eulerAngles.y, ai_player.transform.eulerAngles.z);

        if (ai_System.rotacaoAnt != rotation)
        { ai_System.rotacaoAnt = rotation; ai_System.RotacionarParaAlvo(alvo); }
        else 
        {
            ai_System.rotacaoCamera.transform.position = ai_player.transform.position - ai_player.transform.up;
            Debug.Log("ROTACIONOU para o ALVO");
            ai_System.VerificarProximaJogada(); 
        }
    }
    public override IEnumerator Rotacionar_VasculharArea()
    {
        Debug.Log("ROTACIONAR: Vasculhando");
        ai_System.DetectarJogadores();
        ai_System.menorDistanciaAoGol_JogadoresAmigos = 1000;
        ai_System.menorDistancia_JogadoresInimigos = 1000;

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !ai_System.bola.GetComponent<FisicaBola>().m_bolaCorrendo);
        Debug.Log("ROTACIONAR: Terminou Vasculhamento");
        ai_System.DecidirPosicao();
    }
    public override IEnumerator Rotacionar_Posicao()
    {
        yield return new WaitForSeconds(0.01f);
        float step = 0.5f;
        ai_System.rotacaoCamera.transform.position = Vector3.MoveTowards(ai_System.rotacaoCamera.transform.position, ai_System.posParaChute, step);

        Vector3 direction = ai_System.rotacaoCamera.transform.position - ai_player.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        ai_player.transform.rotation = rotation;
        ai_player.transform.eulerAngles = new Vector3(-90, ai_player.transform.eulerAngles.y, ai_player.transform.eulerAngles.z);

        if (ai_System.rotacaoAnt != rotation) { ai_System.rotacaoAnt = rotation; ai_System.RotacionarParaPosicao(); }
        else
        {
            ai_System.rotacaoAnt = Quaternion.identity;
            yield return new WaitForSeconds(1);
            ai_System.VerificarObstaculos();
        }
    }
    public override IEnumerator Rotacionar_FicarLivre(float direcao)
    {
        //Debug.Log("Rotacionar");
        ai_player.transform.eulerAngles += Vector3.up * (360f * 0.01f) * direcao;

        yield return new WaitForSeconds(0.01f);

        bool esq, dir, frente;
        if (ai_System.HaObstaculo(out esq, out dir, out frente)) ai_System.RotacionarAteFicarLivre(direcao);
        else ai_System.MoverParaPosicao();
    }
    public override IEnumerator Rotacionar_GoleiroChute()
    {
        return base.Rotacionar_GoleiroChute();
    }
    public override IEnumerator Rotacionar_GoleiroDefender()
    {
        return base.Rotacionar_GoleiroDefender();
    }

}
