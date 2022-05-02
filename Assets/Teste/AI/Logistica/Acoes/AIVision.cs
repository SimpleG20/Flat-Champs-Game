using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIVision : AIAction
{
    public AIVision(AISystem system) : base(system)
    {
    }
    public override void DetectarJogadores()
    {
        GameObject j;
        int index = 0;
        foreach (GameObject a in _system.jogadorAmigo_MaisPerto)
        {
            if ((_system.golPos - a.transform.position).magnitude < _system.menorDistanciaAoGol_JogadoresAmigos)
            {
                index = _system.jogadorAmigo_MaisPerto.IndexOf(a);
                _system.menorDistanciaAoGol_JogadoresAmigos = (_system.golPos - a.transform.position).magnitude;
            }
        }
        j = _system.jogadorAmigo_MaisPerto[index];
        _system.jogadorAmigo_MaisPerto.RemoveAt(index);
        _system.jogadorAmigo_MaisPerto.Insert(0, j);

        foreach (GameObject i in _system.jogadorInimigo_MaisPerto)
        {
            if ((ai.transform.position - i.transform.position).magnitude < _system.menorDistancia_JogadoresInimigos)
            {
                index = _system.jogadorInimigo_MaisPerto.IndexOf(i);
                _system.menorDistancia_JogadoresInimigos = (ai.transform.position - i.transform.position).magnitude;
            }
        }
        j = _system.jogadorInimigo_MaisPerto[index];
        _system.jogadorInimigo_MaisPerto.RemoveAt(index);
        _system.jogadorInimigo_MaisPerto.Insert(0, j);
    }
    public override void VerificarObstaculos()
    {
        bool esq, dir, frente;

        HaObstaculos(out esq, out dir, out frente);

        if (esq && dir && frente || !esq && !dir && !frente)
        {
            //print("Seguir em Frente");
            _system.MoverParaPosicao();//-ai.transform.up * _system.magnitudeChute));
        }
        else
        {
            int random = Random.Range(0, 11);
            Debug.Log("Random: " + random);
            if (random < 8) _system.RotacionarAteFicarLivre(PreferenciaParaRotacionar());
            else { _system.fatorExtraChute = 1.75f; _system.MoverParaPosicao(); }
        }
    }
    public static bool HaObstaculos(out bool Esq, out bool Dir, out bool Frente)
    {
        Vector3 lateralD = new Vector3(ai.transform.position.x + (1.7f * -ai.transform.up.z), 0.1f, ai.transform.position.z - (1.7f * -ai.transform.up.x));
        Vector3 lateralE = new Vector3(ai.transform.position.x - (1.7f * -ai.transform.up.z), 0.1f, ai.transform.position.z + (1.7f * -ai.transform.up.x));
        bool obstaculoEsq = false, obstaculoDir = false, obstaculoFrente = false;

        RaycastHit hit;
        if (Physics.Raycast(lateralD, -ai.transform.up, out hit, _system.magnitudeChute, _system.layerMask))
        {
            if (hit.collider.gameObject.layer != ai.layer) obstaculoDir = true;
        }
        if (Physics.Raycast(lateralE, -ai.transform.up, out hit, _system.magnitudeChute, _system.layerMask))
        {
            if (hit.collider.gameObject.layer != ai.gameObject.layer) obstaculoEsq = true;
        }
        if (Physics.Raycast(ai.transform.position, -ai.transform.up, out hit, _system.magnitudeChute, _system.layerMask))
        {
            if (hit.collider.gameObject.layer != ai.layer) obstaculoFrente = true;
        }

        Esq = obstaculoEsq;
        Dir = obstaculoDir;
        Frente = obstaculoFrente;

        return (obstaculoFrente || obstaculoEsq || obstaculoDir);
    }
    public float PreferenciaParaRotacionar()
    {
        Vector3 ai = LogisticaVars.m_jogadorAi.transform.position;

        if (_system.posParaChute.x < ai.x)
        {
            if (_system.posParaChute.z < ai.z)
            {
                if ((ai.z - _system.posParaChute.z) < (ai.x - _system.posParaChute.x)) return 1; //print("Direita");
                else return -1; //print("Esquerda");
            }
            else
            {
                if ((_system.posParaChute.z - ai.z) > (ai.x - _system.posParaChute.x)) return 1; //print("Direita");
                else return -1; // print("Esquerda");
            }
        }
        else
        {
            if (_system.posParaChute.z < ai.z)
            {
                if ((ai.z - _system.posParaChute.z) > (_system.posParaChute.x - ai.x)) return 1; //print("Direita");
                else return -1; //print("Esquerda");
            }
            else
            {
                if ((_system.posParaChute.z - ai.z) < (_system.posParaChute.x - ai.x)) return 1; //print("Direita");
                else return -1;//print("Esquerda");
            }
        }
    }
}
