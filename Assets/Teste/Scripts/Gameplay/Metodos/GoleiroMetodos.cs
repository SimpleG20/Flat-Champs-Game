using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GoleiroMetodos: MonoBehaviour
{
    static MovimentacaoDoJogador mJ;

    void Start()
    {
        mJ = FindObjectOfType<MovimentacaoDoJogador>();
    }


    public static void EncherBarraChuteGoleiro(float forca, float maxForca)
    {
        VariaveisUIsGameplay._current.barraChuteGoleiro.transform.GetChild(1).GetComponent<Image>().fillAmount = (forca / maxForca);
        VariaveisUIsGameplay._current.barraChuteGoleiro.transform.GetChild(1).GetComponent<Image>().color = 
            VariaveisUIsGameplay._current.gradienteChute.Evaluate(forca / maxForca);
    }
    public static void ComponentesParaGoleiro(bool situacao)
    {
        if (situacao == true)
        {
            LogisticaVars.m_goleiroGameObject.transform.GetChild(2).GetComponent<CinemachineVirtualCamera>().m_Priority = 102;
            EncherBarraChuteGoleiro(GoleiroVars.m_forcaGoleiro, GoleiroVars.m_maxForca);
        }
        else 
        {
            LogisticaVars.m_goleiroGameObject.transform.GetChild(2).GetComponent<CinemachineVirtualCamera>().m_Priority = 0;
            EncherBarraChuteGoleiro(JogadorVars.m_forca, JogadorVars.m_maxForcaAtual);
        }
        //LogisticaVars.m_goleiroGameObject.GetComponentInChildren<AudioListener>().enabled = situacao;
        if (situacao == false) VariaveisUIsGameplay._current.barraChuteGoleiro.SetActive(situacao);

        Gameplay._current.Situacao_BolaRasteira();
    }
    public static void ChuteAutomatico()
    {
        Debug.Log("Chute Automatico");
        Rigidbody bola = Gameplay._current._bola.m_rbBola;
        bola.constraints = RigidbodyConstraints.None;

        bola.AddForce(mJ.GetUltimaDirecao() * GoleiroVars.m_forcaGoleiro, ForceMode.Impulse);

        VariaveisUIsGameplay._current.barraChuteGoleiro.transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
        GoleiroVars.m_forcaGoleiro = 0;

        if (LogisticaVars.goleiroT1) LogisticaVars.ultimoToque = 1;
        else LogisticaVars.ultimoToque = 2;
        
        GoleiroVars.chutou = true;
        EventsManager.current.OnGoleiro("rotina pos chute goleiro");
    }
    public static void ChuteNormal()
    {
        Rigidbody bola = Gameplay._current._bola.m_rbBola;
        bola.constraints = RigidbodyConstraints.None;

        bola.AddForce(mJ.GetUltimaDirecao() * GoleiroVars.m_forcaGoleiro, ForceMode.Impulse);

        VariaveisUIsGameplay._current.barraChuteGoleiro.transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
        GoleiroVars.m_forcaGoleiro = 0;

        if (LogisticaVars.goleiroT1) LogisticaVars.ultimoToque = 1;
        else LogisticaVars.ultimoToque = 2;

        GoleiroVars.chutou = true;
        EventsManager.current.OnGoleiro("rotina pos chute goleiro");
    }
}
