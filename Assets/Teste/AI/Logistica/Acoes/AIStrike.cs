using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStrike : AIAction
{
    public AIStrike(AISystem AiSystem, GameObject ai) : base(AiSystem, ai)
    {
    }

    float forcaChute;
    Vector3 direcaoChute;

    public override void IniciarAction()
    {
        switch (ai_System.GetDecisao())
        {
            case AISystem.Decisao.AVANCAR:
                ai_System.ChuteAvancar();
                break;
            case AISystem.Decisao.CHUTAO:
                ai_System.ChuteChutao();
                break;
            case AISystem.Decisao.PASSAR_AMIGO:
                ai_System.ChutePasse();
                break;
            case AISystem.Decisao.CHUTAR_GOL:
                ai_System.ChuteGol();
                break;
        }
    }

    public override IEnumerator Chute_Avancar()
    {
        Debug.Log("AI STRIKE: Avancar");
        direcaoChute = CalcularVetorDirecao();
        forcaChute = CalcularForcaNaBola();
        ai_System.bola.GetComponent<Rigidbody>().AddForce(direcaoChute * forcaChute, ForceMode.Impulse);
        ai_System.GetStateSystem().jogadas++;

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !ai_System.bola.m_bolaCorrendo);

        ai_System.GetStateSystem().OnEsperar();
    }
    public override IEnumerator Chute_Passe()
    {
        Debug.Log("AI STRIKE: Passe");
        direcaoChute = CalcularVetorDirecao();
        forcaChute = CalcularForcaNaBola();
        ai_System.bola.GetComponent<Rigidbody>().AddForce(direcaoChute * forcaChute, ForceMode.Impulse);
        ai_System.GetStateSystem().jogadas++;

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !ai_System.bola.m_bolaCorrendo);

        ai_System.GetStateSystem().OnEsperar();
    }
    public override IEnumerator Chute_Chutao()
    {
        Debug.Log("AI STRIKE: Chutao");
        direcaoChute = CalcularVetorDirecao();
        forcaChute = CalcularForcaNaBola();
        ai_System.bola.GetComponent<Rigidbody>().AddForce(direcaoChute * forcaChute, ForceMode.Impulse);
        ai_System.GetStateSystem().jogadas = 3;

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !ai_System.bola.m_bolaCorrendo);

        ai_System.GetStateSystem().OnEnd();
    }
    public override IEnumerator Chute_Gol()
    {
        Debug.Log("AI STRIKE: GOL");
        direcaoChute = CalcularVetorDirecao();
        forcaChute = CalcularForcaNaBola();
        ai_System.bola.GetComponent<Rigidbody>().AddForce(direcaoChute * forcaChute, ForceMode.Impulse);
        ai_System.GetStateSystem().jogadas = 3;

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !ai_System.bola.m_bolaCorrendo);

        ai_System.GetStateSystem().OnEnd();
    }

    float CalcularForcaNaBola()
    {
        float distanciaDaBola = Vector3.Distance(ai_System.bola.m_pos, ai_System.posTarget);
        float vel_I = Mathf.Sqrt(distanciaDaBola * AtributosFisicos.coefAtritoDiBola * AtributosFisicos.gravidade * 2);

        float forca = ai_System.bola.GetComponent<Rigidbody>().mass * vel_I * ai_System.fatorExtraChute;
        Debug.Log("Forca Na Bola: " + forca + "N");
        forca = forca / Random.Range(0.80f, 0.90f);
        return forca;
    }

    public override bool HaObstaculos(out bool esq, out bool dir, out bool frente)
    {
        esq = dir = frente = false;
        RaycastHit hit;
        if (Physics.Raycast(ai_System.bola.m_pos, direcaoChute, out hit, ai_System.magnitudeChute, ai_System.layerMask))
        {
            if (hit.collider.gameObject != ai_player && !hit.collider.gameObject.CompareTag("Bola")) frente = true;
        }

        return frente;
    }

    
    void SituacaoBola(Vector3 aux)
    {
        bool esq, dir, frente;
        if(HaObstaculos(out esq, out dir, out frente))
        {
            Debug.Log("Chute Por Cima");
            aux.y = Random.Range(2, 4.1f);
        }
        else
        {
            if(Random.Range(0, 8) < 2) aux.y = Random.Range(2, 4.1f);
        }
        //verificar se o chute sera rasteriro ou nao
    }
    Vector3 CalcularVetorDirecao()
    {
        Vector3 aux = (ai_System.posTarget - ai_System.bola.m_pos).normalized;
        SituacaoBola(aux);
        return aux;
    }
}
