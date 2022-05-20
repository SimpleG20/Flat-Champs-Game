using System;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public static EventsManager current;

    private void Awake()
    {
        current = this;
    }

    #region Menu
    public event Action<string> onClickMenu;
    public void ClickInFirstScene(string situacao)
    {
        if (onClickMenu != null) onClickMenu(situacao);
    }
    public event Action<int, int, int> onAtualizarStats;
    public void AtualizarStats(int vitoria, int derrota, int empate)
    {
        if (onAtualizarStats != null) onAtualizarStats(vitoria, derrota, empate); 
    }
    #endregion


    #region Gameplay

    #region Situacoes
    public event Action<string> onChuteAoGol, onEscolherOutro, onEspecial, onGol,
        onFora, onGoleiro, onComecar;
    public void OnChuteAoGol(string s)
    {
        if (onChuteAoGol != null) onChuteAoGol(s);
    }
    public void OnEscolherOutro(string s)
    {
        if (onEscolherOutro != null) onEscolherOutro(s);
    }
    public void OnEspecial(string s)
    {
        if (onEspecial != null) onEspecial(s);
    }
    public void OnGol(string s)
    {
        if (onGol != null) onGol(s);
    }
    public void OnFora(string s)
    {
        if (onFora != null) onFora(s);
    }
    public void OnGoleiro(string s)
    {
        if (onGoleiro != null) onGoleiro(s);
    }
    public void OnComecar(string s)
    {
        if (onComecar != null) onComecar(s);
    }
    #endregion

    #region UI
    public event Action onAtualizarNumeros;
    public void OnAtualizarNumeros()
    {
        if (onAtualizarNumeros != null) onAtualizarNumeros();
    }
    public event Action<string> onClickUi;
    public void OnClickUi(string s)
    {
        if (onClickUi != null) onClickUi(s);
    }
    #endregion

    #region Selecao
    public event Action onEscolherJogador, onSelecaoAutomatica;
    public void EscolherJogador()
    {
        if (onEscolherJogador != null) onEscolherJogador();
    }
    public void SelecaoAutomatica()
    {
        if (onSelecaoAutomatica != null) onSelecaoAutomatica();
    }
    public void AcionarCameraEspera()
    {
        VariaveisUIsGameplay._current.EstadoTodosOsBotoes(false);
        VariaveisUIsGameplay._current.m_placar.SetActive(true);
        VariaveisUIsGameplay._current.pausarBt.gameObject.SetActive(true);
        CamerasSettings._current.SituacoesCameras("acionar camera espera");
    }
    #endregion

    public event Action onAtualizarPlacar;
    public void AtualizarPlacar()
    {
        if (onAtualizarPlacar != null) onAtualizarPlacar();
    }
    #endregion
}
