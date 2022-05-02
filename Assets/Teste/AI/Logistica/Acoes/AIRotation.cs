using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRotation : AIAction
{
    public AIRotation(AISystem system) : base(system)
    {
    }


    public override IEnumerator Rotacionar_Alvo(Vector3 alvo, bool proximaJogada)
    {
        yield return new WaitForSeconds(0.01f);
        float step = 0.5f;
        _system.rotacaoCamera.transform.position = Vector3.MoveTowards(_system.rotacaoCamera.transform.position, alvo, step);

        Vector3 direction = _system.rotacaoCamera.transform.position - ai.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        ai.transform.rotation = rotation;
        ai.transform.eulerAngles = new Vector3(-90, ai.transform.eulerAngles.y, ai.transform.eulerAngles.z);

        if (_system.rotacaoAnt != rotation) 
        { _system.rotacaoAnt = rotation; _system.RotacionarParaAlvo(alvo, proximaJogada); }
    }
    public override IEnumerator Rotacionar_POV()
    {
        _system.DetectarJogadores();
        _system.menorDistanciaAoGol_JogadoresAmigos = 1000;
        _system.menorDistancia_JogadoresInimigos = 1000;

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !_system.bola.GetComponent<FisicaBola>().m_bolaCorrendo);
        _system.DecidirMovimento();
    }
    public override IEnumerator Rotacionar_Posicao()
    {
        yield return new WaitForSeconds(0.01f);
        float step = 0.5f;
        _system.rotacaoCamera.transform.position = Vector3.MoveTowards(_system.rotacaoCamera.transform.position, _system.posDestino, step);

        Vector3 direction = _system.rotacaoCamera.transform.position - ai.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        ai.transform.rotation = rotation;
        ai.transform.eulerAngles = new Vector3(-90, ai.transform.eulerAngles.y, ai.transform.eulerAngles.z);

        if (_system.rotacaoAnt != rotation) { _system.rotacaoAnt = rotation; _system.RotacionarParaPosicao(); }
        else
        {
            _system.rotacaoAnt = Quaternion.identity;
            yield return new WaitForSeconds(1);
            _system.VerificarObstaculos();
            //StartCoroutine(MoverParaPosicao(novo));
        }
    }
    public override IEnumerator Rotacionar_FicarLivre(float direcao)
    {
        Debug.Log("Rotacionar");
        ai.transform.eulerAngles += Vector3.up * (360f * 0.01f) * direcao;

        yield return new WaitForSeconds(0.01f);

        bool esq, dir, frente;

        if (AIVision.HaObstaculos(out esq, out dir, out frente)) _system.RotacionarAteFicarLivre(direcao);
        else _system.MoverParaPosicao();
    }
    
}
