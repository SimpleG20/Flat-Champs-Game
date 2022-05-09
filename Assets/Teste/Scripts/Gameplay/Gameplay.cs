using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    public enum Situacoes { JOGO_NORMAL, FORA, PAUSADO}


    public Quaternion rotacaoAnt;

    public Situacoes _atual;
    Situacao _situacaoAtual;
    public FisicaBola _bola;

    public static Gameplay _current;
    private void Awake()
    {
        _current = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSituacao(Situacao situacao)
    {
        _situacaoAtual = situacao;
        _situacaoAtual.Inicio();
    }
    public Situacao GetSituacao()
    {
        return _situacaoAtual;
    }

    #region Chute ao Gol
    public void OnChuteAoGol() //Botao Chute ao Gol
    {
        SetSituacao(new ChuteAoGol(_current, VariaveisUIsGameplay._current, CamerasSettings._current));
    }
    public void GoleiroPosicionado()
    {
        StartCoroutine(_situacaoAtual.Meio());
    }
    #endregion

    public void OnSelecionarOutro()//Botao
    {
        SetSituacao(new EscolherJogador(_current, VariaveisUIsGameplay._current, CamerasSettings._current));
    }
    public void RotacionarJogadorPerto(GameObject jogadorPerto)
    {
        StartCoroutine(_situacaoAtual.RotacionarParaJogadorMaisPerto(jogadorPerto));
    }
    public void Spawnar(string situacao)
    {
        switch (situacao)
        {
            case "lateral":
                if (_bola.m_pos.x < 0) StartCoroutine(_situacaoAtual.Spawnar("lateral esquerda"));
                else StartCoroutine(_situacaoAtual.Spawnar("lateral direita"));
                break;
            case "escanteio":
                if (_bola.m_pos.z < 0) StartCoroutine(_situacaoAtual.Spawnar("fundo 2"));
                else StartCoroutine(_situacaoAtual.Spawnar("fundo 1"));
                break;
            case "tiro de meta":
                if (_bola.m_pos.z < 0) StartCoroutine(_situacaoAtual.Spawnar("fundo 2"));
                else StartCoroutine(_situacaoAtual.Spawnar("fundo 1"));
                break;
            case "especial":
                break;
        }
    }
}
