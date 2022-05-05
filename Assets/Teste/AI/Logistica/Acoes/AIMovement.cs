using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : AIAction
{
    public AIMovement(AISystem AiSystem, GameObject ai_player) : base(AiSystem, ai_player)
    { }

    public override void IniciarAction()
    {
        Vector3 aiPos = ai_player.transform.position;
        if (Vector3.Distance(ai_System.bola.m_pos, aiPos) < 4.5f) 
        { 
            Debug.Log("Se Arrumar para Chutar a Bola");
            if(ai_System.GetDecisao() == AISystem.Decisao.AVANCAR || ai_System.GetDecisao() == AISystem.Decisao.CHUTAO || ai_System.GetDecisao() == AISystem.Decisao.PASSAR_AMIGO)
            {
                ai_System.GetStateSystem().OnChutarNormal();
            }
            else if(ai_System.GetDecisao() == AISystem.Decisao.CHUTAR_GOL)
            {
                ai_System.GetStateSystem().OnChutar_ao_Gol();
            }
            else if(ai_System.GetDecisao() == AISystem.Decisao.ESPECIAL)
            {
                ai_System.GetStateSystem().OnEspecial();
            }
            return; 
        }

        if (LogisticaVars.jogadas == 3) 
        { 
            Debug.Log("Trocar Vez");
            AcaoFinalizada();
            return; 
        }

        ai_System.RotacionarVasculharArea();
    }
    public override IEnumerator Mover_Posicao()
    {
        float distanciaDaBola = Vector3.Distance(ai_player.transform.position, ai_System.posParaChute);
        Debug.Log("Distancia Jogador da Posicao Para Chute: " + distanciaDaBola);
        float vel_I = Mathf.Sqrt(distanciaDaBola * AtributosFisicos.coefAtritoDiBola * AtributosFisicos.gravidade * 2);

        float forca = ai_player.GetComponent<Rigidbody>().mass * vel_I * ai_System.fatorExtraChute;
        forca = Random.Range(forca - 20, forca + 20);
        forca = forca / Random.Range(0.65f, 0.75f);

        //Verifica se a forca eh menor ou igual a forca maxima possivel
        if (forca > 500) forca = 500;
        Debug.Log("Forca No Ai_player: " + forca + "N");

        ai_player.GetComponent<Rigidbody>().AddForce(-ai_player.transform.up * forca, ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !ai_player.GetComponent<FisicaJogador>().m_correndo);

        ai_System.fatorExtraChute = 1;
        ai_System.GetStateSystem().jogadas++;
        //LogisticaVars.jogadas++;

        ai_System.RotacionarParaAlvo(ai_System.bola.m_pos);
        ai_System.rotacaoCamera.transform.position = ai_player.transform.position - ai_player.transform.up;
        if(!ai_player.GetComponent<FisicaJogador>().m_correndo) ai_System.VerificarProximaJogada();
        //Verificar qual jogador ai esta mais perto
    }
    public override void DecidirMovimento()
    {
        Vector3 aiPos = ai_player.transform.position;

        ai_System.posParaChute = Vector3.zero;
        ai_System.posParaChute = ObterPosicao(ai_System.GetDecisao());

        if (ai_System.GetDecisao() == AISystem.Decisao.PASSAR_AMIGO) 
            ai_System.magnitudeChute = Vector3.Distance(ai_System.jogadorAmigo_MaisPerto[0].transform.position, ai_System.posParaChute);
        else 
            ai_System.magnitudeChute = Vector3.Distance(ai_System.posParaChute, ai_System.golPos);

        ai_System.rotacaoCamera.transform.position = aiPos - ai_player.transform.up;
        ai_System.RotacionarParaPosicao();
    }

    Vector3 ObterPosicao(AISystem.Decisao decisao)
    {
        Vector3 posicionamento;
        posicionamento = ai_System.bola.m_pos;
        if (ai_System.golPos.z > 0) posicionamento.z = ai_System.bola.m_pos.z - 2;
        else posicionamento.z = ai_System.bola.m_pos.z + 2;

        if (decisao == AISystem.Decisao.PASSAR_AMIGO)
        {
            float aux = ai_System.golPos.x - ai_System.jogadorAmigo_MaisPerto[0].transform.position.x;
            aux = aux / Mathf.Sqrt(Mathf.Pow(aux, 2));

            float x = ai_System.bola.m_pos.x, x1 = ai_System.jogadorAmigo_MaisPerto[0].transform.position.x + (aux * 2);
            float z = ai_System.bola.m_pos.z, z1 = ai_System.jogadorAmigo_MaisPerto[0].transform.position.z - 5;

            ai_System.posTarget = new Vector3(x1, 0, z1);

            LinhaDirecaoChute(z, x, z1, x1, posicionamento.z, out posicionamento.x);
        }
        else
        {
            float random = decisao == AISystem.Decisao.CHUTAR_GOL ? Mathf.Pow(-1, Random.Range(0, 2)) * Random.Range(0, 3f) :
                                                                    Mathf.Pow(-1, Random.Range(0, 2)) * Random.Range(0, 5f);

            float x = ai_System.bola.m_pos.x, x2 = ai_System.golPos.x + random;
            float z = ai_System.bola.m_pos.z, z2 = ai_System.golPos.z;

            ai_System.posTarget = new Vector3(x2, 0, z2);

            LinhaDirecaoChute(z, x, z2, x2, posicionamento.z, out posicionamento.x);
        }
        return posicionamento;
    }
    void LinhaDirecaoChute(float z0, float x0, float z1, float x1, float zAtual, out float xAtual)
    {
        float a = (z1 - z0) / (x1 - x0);
        //print("A: " + a);
        float b = (-a * x1) + z1;
        //print("B: " + b);

        if (a == Mathf.Infinity) xAtual = 0;
        else xAtual = (zAtual - b) / a;
    }
    

    public override void AcaoFinalizada()
    {
        base.AcaoFinalizada();
        ai_System.FimMovimento();
    }
}
