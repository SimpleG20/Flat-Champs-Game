using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Situacao
{
    protected readonly Gameplay _gameplay;
    protected readonly VariaveisUIsGameplay _ui;
    protected readonly CamerasSettings _camera;
    public Situacao(Gameplay gameplay, VariaveisUIsGameplay ui, CamerasSettings camera)
    {
        _gameplay = gameplay;
        _ui = ui;
        _camera = camera;
    }

    public virtual IEnumerator Inicio()
    {
        yield break;
    }
    public virtual IEnumerator Meio()
    {
        yield break;
    }
    public virtual IEnumerator Spawnar(string lado)
    {
        yield break;
    }
    public virtual IEnumerator RotacionarParaJogadorMaisPerto(GameObject jogador)
    {
        yield break;
    }
    public virtual void UI_Situacao(string s)
    {

    }
    public virtual void Camera_Situacao(string s)
    {

    }
    public virtual void Fim()
    {
        _gameplay._atual = Gameplay.Situacoes.JOGO_NORMAL;
    }
}
