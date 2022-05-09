using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : AIAction
{
    public AIMovement(AISystem AiSystem, GameObject ai_player) : base(AiSystem, ai_player)
    { }

    public override void IniciarAction()
    {
        if(ai_System.GetStateSystem()._estadoAtual == StateSystem.Estado.GOLEIRO)
        {
            ai_System.MoverGoleiroDefender();
            return;
        }
        Vector3 aiPos = ai_player.transform.position;

        if (Vector3.Distance(ai_System.posParaChute, aiPos) < 1.7f && aiPos.z >= ai_System.bola.m_pos.z && ai_System.posParaChute != ai_System.bola.m_pos)
        {
            if (ai_System._novaDecisao) { Debug.Log("NOVA DECISAO"); DecidirPosicao(); }
            ai_System._novaDecisao = false;
            //ai_System.RotacionarParaAlvo(ai_System.bola.m_pos);

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
        ai_System._novaDecisao = false;
    }
    public override void DecidirPosicao()
    {
        Vector3 aiPos = ai_player.transform.position;
        Vector3 aux;

        if (ai_System.GetDecisao() == AISystem.Decisao.PASSAR_AMIGO)
        {
            aux = ai_System.jogadorAmigo_MaisPerto[0].transform.position;
            Debug.Log("POS TARGET: AMIGO");
        }
        else
        {
            aux = ai_System.golPos;
            Debug.Log("POS TARGET: GOL");
        }
        ai_System.posParaChute = Vector3.zero;
        ai_System.posParaChute = ObterPosicao(ai_System.GetDecisao(), aux);
        Debug.Log("POS para Chutar: " + ai_System.posParaChute);

        if (ai_System.GetDecisao() == AISystem.Decisao.PASSAR_AMIGO)
            ai_System.alcanceChute = Vector3.Distance(ai_System.jogadorAmigo_MaisPerto[0].transform.position, ai_System.posParaChute);
        else
            ai_System.alcanceChute = Vector3.Distance(ai_System.posParaChute, ai_System.golPos);

        ai_System.rotacaoCamera.transform.position = aiPos - ai_player.transform.up;
        ai_System.RotacionarParaPosicao();
    }
    public override IEnumerator Mover_Posicao()
    {
        float distancia_posChute = Vector3.Distance(ai_player.transform.position, ai_System.posParaChute);
        //Debug.Log("Distancia Jogador da Posicao Para Chute: " + distancia_posChute);
        float vel_I = Mathf.Sqrt(distancia_posChute * AtributosFisicos.aceleracao_atritoJogador * 2);

        float forca = vel_I * 360 * ai_System.fatorExtraChute / 16.5f;
        forca = Random.Range(forca - 20, forca + 1);

        //Verifica se a forca eh menor ou igual a forca maxima possivel
        if (forca > JogadorVars.m_maxForcaNormal) forca = JogadorVars.m_maxForcaNormal;
        Debug.Log("MOVIMENTO: Forca No Ai_player: " + forca + "N");

        ai_player.GetComponent<Rigidbody>().AddForce(-ai_player.transform.up * forca, ForceMode.Impulse);
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => !ai_player.GetComponent<FisicaJogador>().m_correndo);

        ai_System.fatorExtraChute = 1;
        ai_System.GetStateSystem().jogadas++;
        //LogisticaVars.jogadas++;

        ai_System.RotacionarParaAlvo(ai_System.bola.m_pos);
    }
    public override IEnumerator Movimentar_Defender()
    {
        return base.Movimentar_Defender();
    }

    Vector3 ObterPosicao(AISystem.Decisao decisao, Vector3 target)
    {
        Vector3 posicionamento;
        posicionamento = ai_System.bola.m_pos;
        if (target.z - posicionamento.z > 0) posicionamento.z = ai_System.bola.m_pos.z - 2;
        else posicionamento.z = ai_System.bola.m_pos.z + 2;

        if (decisao == AISystem.Decisao.PASSAR_AMIGO)
        {
            float aux = ai_System.golPos.x - ai_System.jogadorAmigo_MaisPerto[0].transform.position.x;
            aux = aux / Mathf.Sqrt(Mathf.Pow(aux, 2));

            float x = ai_System.bola.m_pos.x, x1 = ai_System.jogadorAmigo_MaisPerto[0].transform.position.x + (aux * 1);
            float z = ai_System.bola.m_pos.z, z1 = ai_System.jogadorAmigo_MaisPerto[0].transform.position.z - 4;

            ai_System.posTarget = new Vector3(x1, 0, z1);

            LinhaDirecaoChute(z, x, z1, x1, posicionamento.z, out posicionamento.x);
        }
        else
        {
            float random = 0;

            if(decisao != AISystem.Decisao.ESPECIAL)
            {
                random = decisao == AISystem.Decisao.CHUTAR_GOL ? Mathf.Pow(-1, Random.Range(0, 2)) * Random.value * 3 :
                                                                    Mathf.Pow(-1, Random.Range(0, 2)) * Random.value * 5;
            }

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
