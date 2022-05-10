using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GoleiroMetodos: MonoBehaviour
{
    static MovimentacaoDoJogador mJ;
    static UIMetodosGameplay ui;
    static EventsManager events;

    void Start()
    {
        mJ = FindObjectOfType<MovimentacaoDoJogador>();
        ui = FindObjectOfType<UIMetodosGameplay>();
        events = EventsManager.current;
    }


    public static void EncherBarraChuteGoleiro(float forca, float maxForca)
    {
        ui.barraChuteGoleiro.transform.GetChild(1).GetComponent<Image>().fillAmount = (forca / maxForca);
        ui.barraChuteGoleiro.transform.GetChild(1).GetComponent<Image>().color = ui.gradienteChute.Evaluate(forca / maxForca);
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
        if (situacao == false) ui.barraChuteGoleiro.SetActive(situacao);

        Gameplay._current.Situacao_BolaRasteira();
    }
    public static void ChuteAutomatico()
    {
        Debug.Log("Chute Automatico");
        Rigidbody bola = GameObject.FindGameObjectWithTag("Bola").GetComponent<Rigidbody>();
        bola.constraints = RigidbodyConstraints.None;

        bola.AddForce(mJ.GetUltimaDirecao() * GoleiroVars.m_forcaGoleiro, ForceMode.Impulse);

        ui.barraChuteGoleiro.transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
        GoleiroVars.m_forcaGoleiro = 0;
        GoleiroVars.chutou = true;
        //events.OnAplicarRotinas("rotina pos chute goleiro");
    }
    public static void ChuteNormal()
    {
        Rigidbody bola = GameObject.FindGameObjectWithTag("Bola").GetComponent<Rigidbody>();
        bola.constraints = RigidbodyConstraints.None;

        bola.AddForce(mJ.GetUltimaDirecao() * GoleiroVars.m_forcaGoleiro, ForceMode.Impulse);

        ui.barraChuteGoleiro.transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
        GoleiroVars.m_forcaGoleiro = 0;

        if (LogisticaVars.goleiroT1) LogisticaVars.ultimoToque = 1;
        else LogisticaVars.ultimoToque = 2;

        GoleiroVars.chutou = true;
        //events.OnAplicarRotinas("rotina pos chute goleiro");
    }
}
