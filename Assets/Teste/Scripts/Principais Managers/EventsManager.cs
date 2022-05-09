using System;
using System.Collections;
using System.Collections.Generic;
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

    public event Action<string> onChuteAoGol, onEscolherOutro, onEspecial, onGol, onFora, onPequeunaArea, onComecar;
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
    public void OnComecar(string s)
    {
        if (onComecar != null) onComecar(s);
    }

    #region UI
    public event Action<string> onAplicarRotinas;
    /*public void OnAplicarRotinas(string s)
    {
        if (onAplicarRotinas != null) onAplicarRotinas(s);
    }*/

    public event Action<string> onAplicarMetodosUiComBotao;
    /*public void OnAplicarMetodosUiComBotao(string s)
    {
        if (onAplicarMetodosUiComBotao != null) onAplicarMetodosUiComBotao(s);
    }*/

    public event Action<string, string, bool, float, float> onAplicarMetodosUiSemBotao;
    /*public void OnAplicarMetodosUiSemBotao(string s, string s2 = default, bool b = default, float f = default, float f2 = default)
    {
        if (onAplicarMetodosUiSemBotao != null) onAplicarMetodosUiSemBotao(s, s2, b, f, f2);
    }*/

    public event Action onAtualizarNumeros;
    public void OnAtualizarNumeros()
    {
        if (onAtualizarNumeros != null) onAtualizarNumeros();
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

    public event Action onTrocarVez;
    public void OnTrocarVez()
    {
        if (onTrocarVez != null) onTrocarVez();
    }

    public event Action<float> onAjeitarCamera;
    public void AjeitarCamera(float y)
    {
        if (onAjeitarCamera != null) onAjeitarCamera(y);
    }
    #endregion

    public event Action<string> onSituacaoGameplay;
    /*public void SituacaoGameplay(string s)
    {
        if (onSituacaoGameplay != null) onSituacaoGameplay(s);
    }*/

    public event Action onAtualizarPlacar;
    public void AtualizarPlacar()
    {
        if (onAtualizarPlacar != null) onAtualizarPlacar();
    }
    #endregion


}
