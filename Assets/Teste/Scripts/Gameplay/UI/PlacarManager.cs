using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlacarManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI siglaTime1, siglaTime2, tempo;
    public TextMeshProUGUI golTime1, golTime2;
    [SerializeField] Image primeiraCorTime1, segundaCorTime1, terceiraCorTime1, primeiraCorTime2, segundaCorTime2, terceiraCorTime2;

    private void Start()
    {
        EventsManager.current.onAtualizarNumeros += AtualizarNumeros;

    }
    void AtualizarNumeros()
    {
        if (LogisticaVars.placarT1 < 10) golTime1.text = "0" + LogisticaVars.placarT1.ToString();
        else golTime1.text = LogisticaVars.placarT1.ToString();

        if (LogisticaVars.placarT2 < 10) golTime2.text = "0" + LogisticaVars.placarT2.ToString();
        else golTime2.text = LogisticaVars.placarT2.ToString();

        if (LogisticaVars.minutosCorridos < 10)
        {
            if (LogisticaVars.segundosCorridos < 10) tempo.text = "0" + LogisticaVars.minutosCorridos.ToString() + ":0" + LogisticaVars.segundosCorridos.ToString();
            else tempo.text = "0" + LogisticaVars.minutosCorridos.ToString() + ":" + LogisticaVars.segundosCorridos.ToString();
        }
        else
        {
            if (LogisticaVars.segundosCorridos < 10) tempo.text = LogisticaVars.minutosCorridos.ToString() + ":0" + LogisticaVars.segundosCorridos.ToString();
            else tempo.text = LogisticaVars.minutosCorridos.ToString() + ":" + LogisticaVars.segundosCorridos.ToString();
        }
    }

    public void SetarPlacarConfigOff()
    {
        Player a = GameManager.Instance.m_usuario;
        Teams b = GameManager.Instance.GetTimeOff();

        siglaTime1.text = a.m_abreviacao;
        siglaTime2.text = b.m_abreviacao;

        if (primeiraCorTime1 != null) primeiraCorTime1.color = a.m_corPrimaria;
        if (segundaCorTime1 != null) segundaCorTime1.color = a.m_corSecundaria;
        if (terceiraCorTime1 != null) terceiraCorTime1.color = a.m_corTerciaria;

        if (primeiraCorTime2 != null) primeiraCorTime2.color = b.m_corPrimaria;
        if (segundaCorTime2 != null) segundaCorTime2.color = b.m_corSecundaria;
        if (terceiraCorTime2 != null) terceiraCorTime2.color = b.m_corTerciaria;
    }

    public void SetarPlacarOn(bool individual)
    {

    }
}
