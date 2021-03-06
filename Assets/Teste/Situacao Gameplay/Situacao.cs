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
    public virtual IEnumerator RotacionarParaJogadorMaisPerto(Vector3 pos)
    {
        yield break;
    }
    public virtual void UI_Situacao(string s)
    {

    }
    public void UI_Normal()
    {
        if(!LogisticaVars.vezAI)
        {
            _ui.EstadoBotoesGoleiro(false);
            _ui.EstadoBotoesJogador(true);
            _ui.cameraEsperaBt.gameObject.SetActive(false);
            _ui.sairSelecaoBt.gameObject.SetActive(false);

            _ui.especialBt.gameObject.SetActive(true);
            _ui.barraChuteJogador.SetActive(true);
            _ui.barraEspecial.SetActive(true);
            _ui.centralBotoes.SetActive(true);

            _ui.numeroJogadasGO.SetActive(true);
            _ui.tempoEscolhaGO.SetActive(true);
            _ui.tempoJogadaGO.SetActive(true);
            _ui.pausarBt.gameObject.SetActive(true);
            _ui.especialBt.interactable = true;
            _ui.m_placar.SetActive(true);

            _gameplay.AjeitarBarraChute();
        }
        else
        {
            _ui.UI_Espera();
        }
    }
    public virtual void Camera_Situacao(string s)
    {

    }
    public virtual IEnumerator Fim()
    {
        LogisticaVars.fimSituacao = true;
        _gameplay._atual = Gameplay.Situacoes.JOGO_NORMAL;
        yield break;
    }
}
