using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class UIMetodosGameplay : VariaveisUIsGameplay
{
    private void Start()
    {
        EventsManager.current.onAplicarMetodosUiSemBotao += MetodosUISemBotao;
        EventsManager.current.onAplicarMetodosUiComBotao += MetodosUIComBotao;
    }

    private void MetodosUISemBotao(string metodo, string situacao, bool b, float valorAtualBarra, float maxBarra)
    {
        switch (metodo)
        {
            case "estados dos botoes":
                EstadoBotoesSituacoeGameplay(situacao);
                break;
            case "estado todo botoes":
                EstadoTodosOsBotoes(b);
                break;
            case "estado jogador":
                EstadoBotoesJogador(b);
                break;
            case "estado goleiro":
                EstadoBotoesGoleiro(b);
                break;
            case "estado jogador e goleiro":
                EstadoBotoesGoleiro(b);
                EstadoBotoesJogador(b);
                EstadoBotoesCentral(b);
                barraChuteJogador.SetActive(b);
                break; 
            case "especial":
                Especial();
                break;
            case "fim especial":
                FimEspecial();
                break;
            case "pause":
                BotoesNoPause();
                break;
            case "unpause":
                BotoesNoUnPause();
                break;
        }
    }
    void MetodosUIComBotao(string s)
    {
        switch (s)
        {
            case "bola rasteira":
                BolaRasteira();
                break;
        }
    }

    void EstadoBotoesSituacoeGameplay(string situacao)
    {
        switch (situacao)
        {
            #region Primeiro Toque
            case "primeiro toque":
                //print("Ui: primeiro Toque");
                m_placar.SetActive(true);
                pausarBt.gameObject.SetActive(true);
                tempoEscolhaGO.SetActive(true);
                numeroJogadasGO.SetActive(true);
                tempoJogadaGO.SetActive(true);

                centralBotoes.SetActive(true);
                barraEspecial.SetActive(true);
                especialBt.gameObject.SetActive(true);
                botaoBaixo.SetActive(false);
                botaoCima.SetActive(false);
                botaoLivre.SetActive(false);
                botaoLivre2.SetActive(false);
                botaoLivre3.SetActive(false);
                botaoMeio.SetActive(true);

                moverJogadorBt.SetActive(true);
                joystick.SetActive(true);
                barraChuteJogador.SetActive(true);
                break;
            #endregion

            #region Selecao
            case "selecao":
                //print("Ui: Selecionar Outro Jogador");
                EstadoBotoesJogador(false);
                EstadoBotoesGoleiro(false);
                botaoCima.SetActive(true);
                botaoMeio.SetActive(false);
                botaoBaixo.SetActive(false);
                botaoLivre.SetActive(false);
                botaoLivre2.SetActive(false);
                botaoLivre3.SetActive(false);

                sairSelecaoBt.gameObject.SetActive(true);
                joystick.SetActive(true);
                break;
            #endregion

            #region Situacoes
            case "bola pequena area":
                //print("Ui: Bola na Pequena Area");
                //LogisticaVars.m_jogadorEscolhido.GetComponentInChildren<AudioListener>().enabled = false;
                StartCoroutine(TransicaoBolaPequenaArea());
                
                break;
            case "tiro de meta":
                //print("Ui: Chute Tiro de Meta Goleiro");
                //LogisticaVars.m_jogadorEscolhido.GetComponentInChildren<AudioListener>().enabled = false;
                EstadoBotoesJogador(false);
                EstadoBotoesGoleiro(true);
                selecionarJogadorBt.gameObject.SetActive(false);
                barraChuteGoleiro.SetActive(true);
                FindObjectOfType<CamerasSettings>().MudarBlendCamera(CinemachineBlendDefinition.Style.EaseInOut);
                break;
            case "chute ao gol":
                //print("Ui: Chute ao Gol");
                EstadoBotoesJogador(false);
                EstadoBotoesGoleiro(false); 
                goleiroPosicionadoBt.gameObject.SetActive(false);
                especialBt.gameObject.SetActive(false);
                botaoLivre.SetActive(false);

                botaoMeio.SetActive(true);
                botaoLivre2.SetActive(true);
                moverJogadorBt.SetActive(true);
                mostrarDirecionalBolaBt.gameObject.SetActive(true);

                barraChuteJogador.SetActive(true);
                joystick.SetActive(true);
                break;
            case "defender chute ao gol":
                //print("Ui: Posicionar Goleiro");
                //LogisticaVars.m_jogadorEscolhido.GetComponentInChildren<AudioListener>().enabled = false;
                EstadoBotoesJogador(false);
                EstadoBotoesGoleiro(false);
                EstadoBotoesCentral(false);
                especialBt.gameObject.SetActive(false);

                //barraChuteGoleiro.SetActive(false);
                goleiroPosicionadoBt.gameObject.SetActive(true);
                joystick.SetActive(true);
                break;
            case "fora":
                //print("Ui: Chute Fora");
                //LogisticaVars.m_jogadorEscolhido.GetComponentInChildren<AudioListener>().enabled = true;
                EstadoBotoesGoleiro(false);
                EstadoBotoesJogador(false);
                EstadoBotoesCentral(false);

                centralBotoes.SetActive(true);
                barraEspecial.SetActive(true);
                direcaoBolaBt.gameObject.SetActive(true);
                botaoBaixo.SetActive(true);
                botaoMeio.SetActive(true);
                joystick.SetActive(true);

                if (LogisticaVars.lateral) lateralBt.gameObject.SetActive(true);
                else { escanteioBt.gameObject.SetActive(true); barraChuteJogador.SetActive(true); }
                break;
            case "camera tiro de meta":
                //print("Ui: Cameras Laterais ou Tiro de Meta");
                EstadoTodosOsBotoes(false);
                m_placar.SetActive(true);
                pausarBt.gameObject.SetActive(true);
                break;
            #endregion

            default: // Normal, Jogador
                //print("Ui: Normal");
                //LogisticaVars.m_jogadorEscolhido.GetComponentInChildren<AudioListener>().enabled = true;
                EstadoBotoesGoleiro(false);
                EstadoBotoesJogador(true);
                especialBt.gameObject.SetActive(true);
                barraChuteJogador.SetActive(true);
                barraEspecial.SetActive(true);
                centralBotoes.SetActive(true);

                numeroJogadasGO.SetActive(true);
                tempoEscolhaGO.SetActive(true);
                tempoJogadaGO.SetActive(true);
                pausarBt.gameObject.SetActive(true);
                sairSelecaoBt.gameObject.SetActive(false);
                especialBt.interactable = true;
                break;
        }
    }

    void EstadoTodosOsBotoes(bool b)
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
    void EstadoBotoesCentral(bool b)
    {
        botaoBaixo.SetActive(b);
        botaoMeio.SetActive(b);
        botaoCima.SetActive(b);
        botaoLivre.SetActive(b);
        botaoLivre2.SetActive(b);
        botaoLivre3.SetActive(b);
    }
    void EstadoBotoesJogador(bool b)
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
    void EstadoBotoesGoleiro(bool b)
    {
        barraChuteGoleiro.SetActive(b);
        chuteGoleiroBt.gameObject.SetActive(b);
        joystick.SetActive(b);
        direcaoBolaBt.gameObject.SetActive(b);
        selecionarJogadorBt.gameObject.SetActive(b);

        if(b == true)
        {
            centralBotoes.SetActive(true);
            EstadoBotoesCentral(false);
            botaoMeio.SetActive(true);
            botaoBaixo.SetActive(true);
        }
    }

    IEnumerator TransicaoBolaPequenaArea()
    {
        FindObjectOfType<CamerasSettings>().MudarBlendCamera(CinemachineBlendDefinition.Style.EaseInOut);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !FindObjectOfType<CinemachineBrain>().IsBlending);
        EstadoBotoesJogador(false);
        EstadoBotoesGoleiro(true);
        selecionarJogadorBt.gameObject.SetActive(false);
    }

    void Especial()
    {
        EstadoBotoesJogador(false);
        EstadoBotoesGoleiro(false);
        EstadoBotoesCentral(false);
        centralBotoes.SetActive(true);
        //chuteEspecialBt.gameObject.SetActive(true);
        travarMiraBt.gameObject.SetActive(true);
        joystick.SetActive(true);
    }
    void FimEspecial()
    {
        MetodosUISemBotao("estado jogador e goleiro", "", false, 0, 0);
        chuteEspecialBt.gameObject.SetActive(false);
    }
    void BotoesNoPause()
    {
        if(FindObjectsOfType<LinkarBotaoComIcone>() != null)
        {
            foreach (LinkarBotaoComIcone l in FindObjectsOfType<LinkarBotaoComIcone>())
            {
                l.gameObject.GetComponent<Button>().interactable = false;
            }
        }

        GameObject.Find("Canvas").transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 0;
    }
    void BotoesNoUnPause()
    { 
        if (FindObjectsOfType<LinkarBotaoComIcone>() != null)
        {
            foreach (LinkarBotaoComIcone l in FindObjectsOfType<LinkarBotaoComIcone>())
            {
                l.gameObject.GetComponent<Button>().interactable = true;
            }
        }

        GameObject.Find("Canvas").transform.GetChild(2).GetComponent<CanvasGroup>().alpha = 1;
    }
    void BolaRasteira()
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
