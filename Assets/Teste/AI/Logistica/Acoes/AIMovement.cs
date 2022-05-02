using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : AIAction
{
    public AIMovement(AISystem system) : base(system)
    {
    }

    public override void DecidirMovimento()
    {
        Vector3 aiPos = ai.transform.position;

        _system.decisao = Random.Range(0, 2);
        if (Vector3.Distance(_system.golPos, _system.jogadorAmigo_MaisPerto[0].transform.position) < Vector3.Distance(_system.golPos, aiPos))
        {
            Debug.Log("Mais longe do gol que seu amigo");

            if (_system.decisao < 2) Debug.Log("Posicionar-se para passar a bola para o amigo");
            else Debug.Log("Eu me garanto");
        }
        else Debug.Log("Mais perto do gol que seu amigo");

        _system.posParaChute = Vector3.zero;
        _system.posParaChute = ObterPosicao(_system.decisao);

        if (_system.decisao < 2) _system.magnitudeChute = Vector3.Distance(_system.jogadorAmigo_MaisPerto[0].transform.position, _system.posParaChute);
        else _system.magnitudeChute = Vector3.Distance(_system.posParaChute, _system.golPos);
        //print(posicaoParaChute);

        _system.rotacaoCamera.transform.position = aiPos - ai.transform.up;

        _system.RotacionarParaPosicao();
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
    
    public override IEnumerator Mover_Posicao()
    {
        float distanciaDaBola = Vector3.Distance(ai.transform.position, _system.posDestino);
        float vel_I = Mathf.Sqrt(distanciaDaBola * AtributosFisicos.coefAtritoDiBola * AtributosFisicos.gravidade * 2);

        float forca = ai.GetComponent<Rigidbody>().mass * vel_I * _system.fatorExtraChute;
        forca = Random.Range(forca - 20, forca + 20);
        forca = forca / Random.Range(0.65f, 0.75f);
        //Debug.Log(forca);

        //Verifica se a forca eh menor ou igual a forca maxima possivel
        
        ai.GetComponent<Rigidbody>().AddForce(-ai.transform.up * forca, ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !ai.GetComponent<FisicaJogador>().m_correndo);

        _system.fatorExtraChute = 1;
        LogisticaVars.jogadas++;
        _system.rotacaoCamera.transform.position = ai.transform.position - ai.transform.up;

        //Verificar qual jogador ai esta mais perto
        //StartCoroutine(RotacionarParaAlvo(bola.m_pos, true));
    }

    Vector3 ObterPosicao(int i)
    {
        _system.posDestino = _system.bola.m_pos;
        if (_system.golPos.z > 0) _system.posDestino.z = _system.bola.m_pos.z - 2;
        else _system.posDestino.z = _system.bola.m_pos.z + 2;

        if (i < 2)
        {
            float x = _system.bola.m_pos.x, x1 = _system.jogadorAmigo_MaisPerto[0].transform.position.x;
            float z = _system.bola.m_pos.z, z1 = _system.golPos.z > 0 ? _system.jogadorAmigo_MaisPerto[0].transform.position.z + 5 : 
                                                                        _system.jogadorAmigo_MaisPerto[0].transform.position.z - 5;

            LinhaDirecaoChute(z, x, z1, x1, _system.posDestino.z, out _system.posDestino.x);
        }
        else
        {
            float x = _system.bola.m_pos.x, x2 = _system.golPos.x;
            float z = _system.bola.m_pos.z, z2 = _system.golPos.z;

            LinhaDirecaoChute(z, x, z2, x2, _system.posDestino.z, out _system.posDestino.x);
        }
        return _system.posDestino;
    }

    public override void AcaoFinalizada()
    {
        base.AcaoFinalizada();
        _system.FimMovimento();
    }
}
