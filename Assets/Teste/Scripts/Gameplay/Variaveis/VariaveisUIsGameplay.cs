using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VariaveisUIsGameplay : MonoBehaviour
{
    [Header("UI's")]
    public GameObject m_placar;
    public GameObject aberturaGO, golMarcadoGO;
    public GameObject numeroJogadasGO, tempoJogadaGO, tempoEscolhaGO;
    public GameObject centralBotoes, botaoBaixo, botaoMeio, botaoCima, botaoDiagonal, botaoLivre1, botaoLivre2;
    public GameObject joystick, miraEspecial, iconePequeno, iconeMedio, iconeGrande, inibidorMoverJogadorGO;
    public Button pausarBt;
    public Gradient gradienteChute, gradEscpecial;
    public bool clicouUi;

    [Header("UI jogador")]
    public GameObject moverJogadorBt;
    public Button selecionarJogadorBt, sairSelecaoBt, chuteGolBt, especialBt, chuteEspecialBt, travarMiraBt;
    public Button escanteioBt, lateralBt;
    public Toggle direcaoBolaBt, rotacaoAutoBt, mostrarDirecionalBolaBt;
    public GameObject barraChuteJogador, barraEspecial;

    [Header("UI Goleiro")]
    public GameObject barraChuteGoleiro;
    public GameObject chuteGoleiroBt;
    public Button goleiroPosicionadoBt;
    //public Gradient m_gradienteGoleiro;

    public static VariaveisUIsGameplay _current;

    private void Awake()
    {
        _current = this;
    }

    public void EstadoTodosOsBotoes(bool b)
    {
        EstadoBotoesGoleiro(b);
        EstadoBotoesJogador(b);
        centralBotoes.SetActive(b);
        barraEspecial.SetActive(b);
        barraChuteJogador.SetActive(b);
        barraChuteGoleiro.SetActive(b);
        especialBt.gameObject.SetActive(b);
        pausarBt.gameObject.SetActive(b);
        tempoEscolhaGO.SetActive(b);
        tempoJogadaGO.SetActive(b);
        numeroJogadasGO.SetActive(b);
        m_placar.SetActive(b);
    }
    public void EstadoBotoesCentral(bool b)
    {
        botaoBaixo.SetActive(b);
        botaoMeio.SetActive(b);
        botaoCima.SetActive(b);
        botaoDiagonal.SetActive(b);
        botaoLivre2.SetActive(b);
        botaoLivre1.SetActive(b);
    }
    public void EstadoBotoesJogador(bool b)
    {
        barraChuteJogador.SetActive(b);
        joystick.SetActive(b);
        moverJogadorBt.gameObject.SetActive(b);
        selecionarJogadorBt.gameObject.SetActive(b);
        direcaoBolaBt.gameObject.SetActive(b);
        chuteGolBt.gameObject.SetActive(b);
        rotacaoAutoBt.gameObject.SetActive(b);
        mostrarDirecionalBolaBt.gameObject.SetActive(b);

        if (b == true) { centralBotoes.SetActive(true); EstadoBotoesCentral(true); }
        else especialBt.interactable = false;
    }
    public void EstadoBotoesGoleiro(bool b)
    {
        barraChuteGoleiro.SetActive(b);
        chuteGoleiroBt.gameObject.SetActive(b);
        joystick.SetActive(b);
        direcaoBolaBt.gameObject.SetActive(b);
        selecionarJogadorBt.gameObject.SetActive(b);

        if (b == true)
        {
            centralBotoes.SetActive(true);
            EstadoBotoesCentral(false);
            botaoMeio.SetActive(true);
            botaoBaixo.SetActive(true);
        }
    }

    public void UI_BolaRasteira()
    {
        if (direcaoBolaBt.isOn)
        {
            if (LogisticaVars.vezJ1 || LogisticaVars.goleiroT1)
            {
                LogisticaVars.bolaRasteiraT1 = true;
                direcaoBolaBt.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            }
            if (LogisticaVars.vezJ2 || LogisticaVars.goleiroT2)
            {
                LogisticaVars.bolaRasteiraT2 = true;
                direcaoBolaBt.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            }
        }
        else
        {
            if (LogisticaVars.vezJ1 || LogisticaVars.goleiroT1)
            {
                LogisticaVars.bolaRasteiraT1 = false;
                direcaoBolaBt.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            }
            if (LogisticaVars.vezJ2 || LogisticaVars.goleiroT2)
            {
                LogisticaVars.bolaRasteiraT2 = false;
                direcaoBolaBt.gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}
