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
}
