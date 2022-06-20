using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStrike : AIAction
{
    public AIStrike(AISystem AiSystem, GameObject ai) : base(AiSystem, ai)
    {
    }

    float forcaChute, forcaNaBola;

    public override void IniciarAction()
    {
        Debug.Log("AI STRIKE: " + ai_System.GetDecisao());
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
            case AISystem.Decisao.LATERAL:
                ai_System.ChuteLateral();
                break;
            case AISystem.Decisao.ESCANTEIO:
                ai_System.ChuteEscanteio();
                break;
            case AISystem.Decisao.CHUTAR_GOL:
                ai_System.ChuteGol();
                break;
            case AISystem.Decisao.CHUTE_GOLEIRO:
                ai_System.ChuteTiroDeMeta();
                break;
        }
    }

    public override IEnumerator Chute_Avancar()
    {
        Debug.Log("AI STRIKE: Avancar");
        ai_System.direcaoChute = CalcularVetorDirecao();
        forcaNaBola = CalcularForcaNaBola(AISystem.Decisao.AVANCAR);
        forcaChute = CalcularForcaChute();

        if (forcaChute > JogadorVars.m_maxForcaNormal) forcaChute = JogadorVars.m_maxForcaNormal;
        ai_player.GetComponent<Rigidbody>().AddForce(-ai_player.transform.up * forcaChute, ForceMode.Impulse);
        JogadorVars.m_esperandoContato = true;

        //Substituir pelo LogisticaVars
        //ai_System.GetStateSystem().jogadas++;
        LogisticaVars.jogadas++;

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !ai_System.bola.m_bolaCorrendo);

        ai_System.posParaChute = ai_System.bola.m_pos;
        ai_System.GetStateSystem().OnEsperar();
    }
    public override IEnumerator Chute_Passe()
    {
        Debug.Log("AI STRIKE: Passe");
        ai_System.direcaoChute = CalcularVetorDirecao();
        forcaNaBola = CalcularForcaNaBola(AISystem.Decisao.PASSAR_AMIGO);
        forcaChute = CalcularForcaChute();

        if (forcaChute > JogadorVars.m_maxForcaNormal) forcaChute = JogadorVars.m_maxForcaNormal;
        ai_player.GetComponent<Rigidbody>().AddForce(-ai_player.transform.up * forcaChute, ForceMode.Impulse);
        JogadorVars.m_esperandoContato = true;
        LogisticaVars.jogadas++;
        ai_System._passouBola = true;

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !ai_System.bola.m_bolaCorrendo);

        ai_System.posParaChute = ai_System.bola.m_pos;
        ai_System.GetStateSystem().OnEsperar();
    }
    public override IEnumerator Chute_Chutao()
    {
        Debug.Log("AI STRIKE: Chutao");
        ai_System.direcaoChute = CalcularVetorDirecao(0.65f, true);
        forcaNaBola = CalcularForcaNaBola(AISystem.Decisao.CHUTAO);
        forcaChute = CalcularForcaChute();

        if (forcaChute > JogadorVars.m_maxForcaNormal) forcaChute = JogadorVars.m_maxForcaNormal;
        ai_player.GetComponent<Rigidbody>().AddForce(-ai_player.transform.up * forcaChute, ForceMode.Impulse);
        JogadorVars.m_esperandoContato = true;
        LogisticaVars.jogadas++;

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !ai_System.bola.m_bolaCorrendo);
        ai_System.posParaChute = ai_System.bola.m_pos;
        //ai_System.GetStateSystem().OnEnd();
    }
    public override IEnumerator Chute_Gol()
    {
        Debug.Log("AI STRIKE: GOL");
        ai_System.direcaoChute = CalcularVetorDirecao();
        forcaNaBola = CalcularForcaNaBola(AISystem.Decisao.CHUTAR_GOL);
        forcaChute = CalcularForcaChute();

        if (forcaChute > JogadorVars.m_maxForcaChuteAoGol) forcaChute = JogadorVars.m_maxForcaChuteAoGol;
        ai_player.GetComponent<Rigidbody>().AddForce(-ai_player.transform.up * forcaChute, ForceMode.Impulse);
        LogisticaVars.jogadas = 3;
        JogadorVars.m_chuteAoGol = false;
        JogadorVars.m_esperandoContato = true;

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !ai_System.bola.m_bolaCorrendo);
        ai_System.posParaChute = ai_System.bola.m_pos;
        Gameplay._current.GetStateSystem().OnEnd();
    }
    public override IEnumerator Chute_Lateral()
    {
        Vector3 aux = ai_System.posTarget;
        //Atualiza quem esta mais perto do gol
        ai_System.DetectarJogadores();
        ai_System.posTarget = ai_System.jogadorAmigo_MaisPerto[0].transform.position;

        bool rasteira = Random.Range(0, 2) == 1 ? true : false;
        if(rasteira) ai_System.direcaoChute = CalcularVetorDirecao();
        else ai_System.direcaoChute = CalcularVetorDirecao(0.5f, true);

        yield return new WaitForSeconds(2);
        ai_System.bola.m_rbBola.AddForce(ai_System.direcaoChute * 30, ForceMode.Impulse);
        LogisticaVars.ultimoToque = 2;
        ai_System.posParaChute = ai_System.bola.m_pos;
        ai_System.posTarget = aux;
        EventsManager.current.OnFora("rotina sair lateral");

        yield return new WaitUntil(() => LogisticaVars.saiuFora);
        ai_System.OnIniciarMovimento();
    }
    public override IEnumerator Chute_Escanteio()
    {
        Vector3 aux = ai_System.posTarget;
        //Atualiza quem esta mais perto do gol
        ai_System.DetectarJogadores();
        ai_System.posTarget = ai_System.jogadorAmigo_MaisPerto[0].transform.position.z <= ai_System.golPos.z + 5 ? ai_System.jogadorAmigo_MaisPerto[0].transform.position :
                                                                                                                   ai_System.golPos + Vector3.forward * Random.Range(4f, 14f);

        ai_System.direcaoChute = CalcularVetorDirecao(0.65f, true);
        forcaNaBola = CalcularForcaNaBola(AISystem.Decisao.ESCANTEIO) / 1.85f;

        if (forcaNaBola > JogadorVars.m_maxForcaFora) forcaNaBola = JogadorVars.m_maxForcaFora;

        yield return new WaitForSeconds(2);
        ai_System.bola.m_rbBola.AddForce(ai_System.direcaoChute * forcaNaBola, ForceMode.Impulse);
        LogisticaVars.ultimoToque = 2;
        EventsManager.current.OnFora("rotina sair escanteio");

        ai_System.posParaChute = ai_System.bola.m_pos;
        ai_System.posTarget = aux;

        yield return new WaitUntil(() => LogisticaVars.saiuFora);
        ai_System.OnIniciarMovimento();
    }
    public override IEnumerator Chute_TiroDeMeta_PequenaArea()
    {
        Vector3 aux = ai_System.posTarget;
        //Atualiza quem esta mais perto do gol
        ai_System.DetectarJogadores();
        ai_System.posTarget = ai_System.jogadorAmigo_MaisPerto[0].transform.position;

        bool rasteira = Random.Range(0, 2) == 1 ? true : false;
        if (rasteira)
        {
            ai_System.direcaoChute = CalcularVetorDirecao();
            forcaNaBola = CalcularForcaNaBola(AISystem.Decisao.CHUTE_GOLEIRO);
        }
        else
        {
            ai_System.direcaoChute = CalcularVetorDirecao(0.5f, true);
            forcaNaBola = CalcularForcaNaBola(AISystem.Decisao.CHUTE_GOLEIRO);
        }
        if (forcaNaBola > GoleiroVars.m_maxForca) forcaNaBola = GoleiroVars.m_maxForca;

        ai_System.bola.m_rbBola.AddForce(ai_System.direcaoChute * forcaNaBola, ForceMode.Impulse);
        GoleiroVars.chutou = true;
        LogisticaVars.ultimoToque = 2;
        ai_System.posParaChute = ai_System.bola.m_pos;
        ai_System.posTarget = aux;
        EventsManager.current.OnGoleiro("rotina pos chute goleiro");
        yield break;
    }

    public override bool HaObstaculos(out bool esq, out bool dir, out bool frente)
    {
        esq = dir = frente = false;
        RaycastHit hit;
        if (Physics.Raycast(ai_System.bola.m_pos, ai_System.direcaoChute, out hit, ai_System.alcanceChute, ai_System.layerMask))
        {
            if (hit.collider.gameObject != ai_player && !hit.collider.gameObject.CompareTag("Bola")) frente = true;
        }

        return frente;
    }
    float CalcularForcaNaBola(AISystem.Decisao decisao)
    {
        float distanciaDaBola;
        float vel_I;

        if (decisao == AISystem.Decisao.AVANCAR)
        {
            distanciaDaBola = Vector3.Distance(ai_System.bola.m_pos, ai_System.posTarget);
            if (distanciaDaBola > 10 && distanciaDaBola < 20) distanciaDaBola /= 1.5f;
            else if (distanciaDaBola >= 20 && distanciaDaBola < 40) distanciaDaBola /= 3.5f;
            else distanciaDaBola /= 5;
        }
        else if(decisao == AISystem.Decisao.CHUTE_GOLEIRO)
        {
            distanciaDaBola = Vector3.Distance(ai_System.bola.m_pos, ai_System.posTarget);
            if (distanciaDaBola >= 40) distanciaDaBola /= 1.5f;
        }
        else if(decisao == AISystem.Decisao.PASSAR_AMIGO)
            distanciaDaBola = Vector3.Distance(ai_System.bola.m_pos, ai_System.posTarget) / 1.5f;
        else
            distanciaDaBola = Vector3.Distance(ai_System.bola.m_pos, ai_System.posTarget);

        vel_I = Mathf.Sqrt(distanciaDaBola * AtributosFisicos.aceleracao_atritoBola * 2);
        //Debug.Log("STRIKE: Velocidade Esperada Bola: " + vel_I);
        float forca = vel_I * 40 / 16;
        //Debug.Log("STRIKE: Forca Na Bola: " + forca + "N");
        return forca;
    }
    float CalcularForcaChute()
    {
        float velocidadeImpacto = forcaNaBola * 16 / 40;
        float distanciaJogador_PosChute = Vector3.Distance(LogisticaVars.m_jogadorEscolhido_Atual.transform.position, ai_System.posParaChute);
        float vel_I = Mathf.Sqrt(Mathf.Pow(velocidadeImpacto, 2) + distanciaJogador_PosChute * AtributosFisicos.aceleracao_atritoJogador * 2);

        float forca = vel_I * 360 / 16.5f;
        //Debug.Log("STRIKE: Forca No Jogador: " + forca + "N");
        //if(ai_System.GetDecisao() == AISystem.Decisao.CHUTAO) Debug.Log("STRIKE: Forca No Jogador: " + forca + "N");

        return forca;
    }
    Vector3 CalcularVetorDirecao(float minAltura = 0.25f, bool porCima = default)
    {
        bool esq, dir, frente;
        Vector3 aux = (ai_System.posTarget - ai_System.bola.m_pos).normalized;

        if (HaObstaculos(out esq, out dir, out frente) || porCima)
        {
            Debug.Log("Chute Por Cima");
            aux.y = Random.Range(minAltura, 1.15f);
        }
        else
        {
            if (Random.Range(0, 8) < 2) { Debug.Log("Chute Por Cima"); aux.y = Random.Range(minAltura, 1f); }
            else aux.y = 0;
        }

        //Debug.Log("AI_STRIKE: " + aux);
        return aux;
    }
}
