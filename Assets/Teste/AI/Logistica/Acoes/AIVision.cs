using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIVision : AIAction
{
    public AIVision(AISystem AiSystem, GameObject ai) : base(AiSystem, ai)
    {
    }
    public override void DetectarJogadores()
    {
        GameObject j;
        int index = 0;
        foreach (GameObject a in ai_System.jogadorAmigo_MaisPerto)
        {
            if ((ai_System.golPos - a.transform.position).magnitude < ai_System.menorDistanciaAoGol_JogadoresAmigos &&
                a != ai_player)
            {
                index = ai_System.jogadorAmigo_MaisPerto.IndexOf(a);
                ai_System.menorDistanciaAoGol_JogadoresAmigos = (ai_System.golPos - a.transform.position).magnitude;
            }
        }
        j = ai_System.jogadorAmigo_MaisPerto[index];
        ai_System.jogadorAmigo_MaisPerto.RemoveAt(index);
        ai_System.jogadorAmigo_MaisPerto.Insert(0, j);

        foreach (GameObject i in ai_System.jogadorInimigo_MaisPerto)
        {
            if ((ai_player.transform.position - i.transform.position).magnitude < ai_System.menorDistancia_JogadoresInimigos)
            {
                index = ai_System.jogadorInimigo_MaisPerto.IndexOf(i);
                ai_System.menorDistancia_JogadoresInimigos = (ai_player.transform.position - i.transform.position).magnitude;
            }
        }
        j = ai_System.jogadorInimigo_MaisPerto[index];
        ai_System.jogadorInimigo_MaisPerto.RemoveAt(index);
        ai_System.jogadorInimigo_MaisPerto.Insert(0, j);
    }
    public override void VerificarObstaculos()
    {
        bool esq, dir, frente;

        HaObstaculos(out esq, out dir, out frente);

        if (esq && dir && frente || !esq && !dir && !frente)
        {
            //print("Seguir em Frente");
            ai_System.MoverParaPosicao();
        }
        else
        {
            AISystem.Decisao decisao = ai_System.GetDecisao();
            int random = Random.Range(0, 3);
            //Debug.Log("Random: " + random);

            if(decisao == AISystem.Decisao.CHUTAO || decisao == AISystem.Decisao.AVANCAR)
            {
                if(random < 2) { ai_System.fatorExtraChute = 1.75f; ai_System.MoverParaPosicao(); }
                else ai_System.RotacionarAteFicarLivre(PreferenciaParaRotacionar());
            }
            else ai_System.RotacionarAteFicarLivre(PreferenciaParaRotacionar());
        }
    }
    public override bool HaObstaculos(out bool Esq, out bool Dir, out bool Frente)
    {
        Vector3 lateralD = new Vector3(ai_player.transform.position.x + (1.7f * -ai_player.transform.up.z), 0.1f, ai_player.transform.position.z - (1.7f * -ai_player.transform.up.x));
        Vector3 lateralE = new Vector3(ai_player.transform.position.x - (1.7f * -ai_player.transform.up.z), 0.1f, ai_player.transform.position.z + (1.7f * -ai_player.transform.up.x));
        bool obstaculoEsq = false, obstaculoDir = false, obstaculoFrente = false;

        RaycastHit hit;
        if (Physics.Raycast(lateralD, -ai_player.transform.up, out hit, ai_System.magnitudeChute, ai_System.layerMask))
        {
            if (hit.collider.gameObject.layer != ai_player.layer) obstaculoDir = true;
        }
        if (Physics.Raycast(lateralE, -ai_player.transform.up, out hit, ai_System.magnitudeChute, ai_System.layerMask))
        {
            if (hit.collider.gameObject.layer != ai_player.gameObject.layer) obstaculoEsq = true;
        }
        if (Physics.Raycast(ai_player.transform.position, -ai_player.transform.up, out hit, ai_System.magnitudeChute, ai_System.layerMask))
        {
            if (hit.collider.gameObject.layer != ai_player.layer) obstaculoFrente = true;
        }

        Esq = obstaculoEsq;
        Dir = obstaculoDir;
        Frente = obstaculoFrente;

        return (obstaculoFrente || obstaculoEsq || obstaculoDir);
    }
    public float PreferenciaParaRotacionar()
    {
        Vector3 aiPos = ai_player.transform.position;

        if (ai_System.posParaChute.x < aiPos.x)
        {
            if (ai_System.posParaChute.z < aiPos.z)
            {
                if ((aiPos.z - ai_System.posParaChute.z) < (aiPos.x - ai_System.posParaChute.x)) return 1; //print("Direita");
                else return -1; //print("Esquerda");
            }
            else
            {
                if ((ai_System.posParaChute.z - aiPos.z) > (aiPos.x - ai_System.posParaChute.x)) return 1; //print("Direita");
                else return -1; // print("Esquerda");
            }
        }
        else
        {
            if (ai_System.posParaChute.z < aiPos.z)
            {
                if ((aiPos.z - ai_System.posParaChute.z) > (ai_System.posParaChute.x - aiPos.x)) return 1; //print("Direita");
                else return -1; //print("Esquerda");
            }
            else
            {
                if ((ai_System.posParaChute.z - aiPos.z) < (ai_System.posParaChute.x - aiPos.x)) return 1; //print("Direita");
                else return -1;//print("Esquerda");
            }
        }
    }
}
