using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpecial : AIAction
{
    public AISpecial(AISystem AiSystem, GameObject ai) : base(AiSystem, ai)
    {
    }

    Vector3 direcaoEspecial;

    public override void IniciarAction()
    {
        ai_System.EspecialInstanciarTrajetoria();
        //Mudar LogisticaVars
        //ai_System.GetStateSystem().contagem = false;
    }

    public override IEnumerator Especial_Instanciar()
    {
        Debug.Log("Instaciou trajetoria");
        yield return new WaitForSeconds(2);
        ai_System.EspecialMira();
    }
    public override IEnumerator Especial_Mira()
    {
        yield return new WaitForSeconds(4);

        float randomX = Random.Range(-7f, 7f) + ai_System.golPos.x;
        float randomY = Random.Range(-ai_System.golPos.y / 2, ai_System.golPos.y / 2) + ai_System.golPos.y;

        direcaoEspecial = new Vector3(randomX, randomY, ai_System.golPos.z);
        Debug.Log("ESPECIAL TARGET: " + direcaoEspecial);
        ai_System.EspecialMoverTarget();
        
    }
    public override IEnumerator Especial_MoverTarget()
    {
        yield return new WaitForSeconds(0.01f);
        float step = 0.1f;
        ai_System.especialTarget.transform.position = Vector3.MoveTowards(ai_System.especialTarget.transform.position, direcaoEspecial, step);

        if (ai_System.especialTarget.transform.position != direcaoEspecial) ai_System.EspecialMoverTarget();
        else ai_System.EspecialTrajetoria();
    }
    public override IEnumerator Especial_Trajetoria()
    {
        Vector3 p2 = ai_System.GetTrajetoriaEspecial().Point2.position;
        ai_System.GetTrajetoriaEspecial().Point2.position = new Vector3(p2.x + Random.Range(-5, 5), p2.y + Random.Range(0f, 4f), p2.z);

        yield return new WaitForSeconds(2);
        ai_System.EspecialChute();
    }
    public override IEnumerator Especial_Chute()
    {
        LogisticaVars.aplicouEspecial = true;
        Debug.Log("AI ESPECIAL: Acionado");
        yield break;
    }

}
