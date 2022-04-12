using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogisticaJogo : MonoBehaviour
{
    public bool autoDirT1, autoDirT2;

    #region Outros
    public LayerMask layerGol;
    private Vector3 posicaoGol1, posicaoGol2;
    public GameObject camAbertura, camTorcida, camTMD, camTME, canvas;

    public FisicaBola bola;
    public VariaveisUIsGameplay ui;
    public InputManager joystickManager;
    EventsManager events;
    #endregion

    private void Awake()
    {

    }

    void Start()
    {

    }

    
    void Update()
    {
        if (LogisticaVars.jogoComecou)
        {

            #region Pos chute ao Gol
            if (LogisticaVars.posChuteAoGol)
            {
                if (bola.m_bolaNoChao && !LogisticaVars.posGol)
                {
                    LogisticaVars.posChuteAoGol = false;
                    LogisticaVars.jogadas = 3;
                }
                else if(LogisticaVars.posGol || LogisticaVars.continuaSendoFora)
                {
                    LogisticaVars.posChuteAoGol = false;
                }
            } //Em desenvolvimento
            #endregion

            #region Se o jogador foi Selecionado ou Nao

            if (!LogisticaVars.jogadorSelecionado && !LogisticaVars.jogoParado)
            {
                LogisticaVars.tempoJogada += Time.deltaTime;
                LogisticaVars.tempoEscolherJogador += Time.deltaTime;

                events.OnAplicarMetodosUiSemBotao("estado jogador e goleiro", "", false);
                //UIMetodosGameplay.EstadoBotoesJogador(false);
                //UIMetodosGameplay.EstadoBotoesGoleiro(false);

                ui.joystick.SetActive(true);

            }
            #endregion
        }
    }


    #region Inicio do Jogo e Fim
    private void SairDoJogo()
    {
        LoadManager.Instance.CenaMenu();
    }
    #endregion

    #region Situacoes

    #region Usados nos Botoes UI
    private void MetodoParaBotoesUI(string s)
    {
        switch (s)
        {
            case "mostrar direcional":
                MostrarDirecaoBola();
                break;
        }
    }
    
    
    
    private void MostrarDirecaoBola()
    {
        Toggle t = ui.mostrarDirecionalBolaBt.GetComponent<Toggle>();
        if (t.isOn) LogisticaVars.mostrarDirecaoBola = true;
        else LogisticaVars.mostrarDirecaoBola = false; 
    }
    #endregion

    #endregion

}
