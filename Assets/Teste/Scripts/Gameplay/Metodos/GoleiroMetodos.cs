using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GoleiroMetodos: MonoBehaviour
{
    public static UIMetodosGameplay ui;
    static EventsManager events;

    void Start()
    {
        ui = FindObjectOfType<UIMetodosGameplay>();
        events = EventsManager.current;
        events.onAplicarMetodosUiComBotao += BotoesGoleiro;
    }

    private void BotoesGoleiro(string s)
    {
        switch (s)
        {
            case "goleiro posicionado":
                GoleiroPosicionadoParaDefender();
                break;
        }
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
            EncherBarraChuteGoleiro(JogadorVars.m_forca, JogadorVars.m_maxForca);
        }
        //LogisticaVars.m_goleiroGameObject.GetComponentInChildren<AudioListener>().enabled = situacao;
        if (situacao == false) ui.barraChuteGoleiro.SetActive(situacao);

        events.OnAplicarMetodosUiComBotao("bola rasteira");
    }
    public static void ChuteAutomatico(bool bolaRasteira)
    {
        Debug.Log("Chute Automatico");
        Rigidbody bola = GameObject.FindGameObjectWithTag("Bola").GetComponent<Rigidbody>();
        bola.constraints = RigidbodyConstraints.None;

        if (bolaRasteira)
            bola.AddForce(new Vector3(GoleiroVars.m_cosGoleiro, 0.0f, GoleiroVars.m_senoGoleiro) * 15, ForceMode.Impulse);
        else
            bola.AddForce(new Vector3(GoleiroVars.m_cosGoleiro, 0.8f, GoleiroVars.m_senoGoleiro) * 15, ForceMode.Impulse);

        ui.barraChuteGoleiro.transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
        GoleiroVars.m_forcaGoleiro = 0;

        events.OnAplicarRotinas("rotina pos chute goleiro");
    }
    public static void ChuteNormal(bool bolaRasteira)
    {
        Rigidbody bola = GameObject.FindGameObjectWithTag("Bola").GetComponent<Rigidbody>();
        bola.constraints = RigidbodyConstraints.None;

        if (bolaRasteira)
            bola.AddForce(new Vector3(GoleiroVars.m_cosGoleiro, 0.0f, GoleiroVars.m_senoGoleiro) * GoleiroVars.m_forcaGoleiro, ForceMode.Impulse);
        else
            bola.AddForce(new Vector3(GoleiroVars.m_cosGoleiro, 0.8f, GoleiroVars.m_senoGoleiro) * GoleiroVars.m_forcaGoleiro, ForceMode.Impulse);

        ui.barraChuteGoleiro.transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
        GoleiroVars.m_forcaGoleiro = 0;

        if (LogisticaVars.goleiroT1) LogisticaVars.ultimoToque = 1;
        else LogisticaVars.ultimoToque = 2;

        events.OnAplicarRotinas("rotina pos chute goleiro");
    }
    private void GoleiroPosicionadoParaDefender()
    {
        Debug.Log("Hora de CHUTAR!!");
        ComponentesParaGoleiro(false);
        events.SituacaoGameplay("jogo normal");
        events.OnAplicarMetodosUiSemBotao("estados dos botoes", "chute ao gol");

        LogisticaVars.goleiroT1 = LogisticaVars.goleiroT2 = false;
        LogisticaVars.m_goleiroGameObject = null;

        LogisticaVars.defenderGoleiro = false;
        LogisticaVars.auxChuteAoGol = true;
        JogadorVars.m_chuteAoGol = true;

        JogadorMetodos.ResetarValoresChute();

        events.SituacaoGameplay("jogo parado");
        //LogisticaVars.jogoParado = false;
    }
}
