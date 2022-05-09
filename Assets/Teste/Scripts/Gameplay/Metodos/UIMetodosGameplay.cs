using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class UIMetodosGameplay : VariaveisUIsGameplay
{
    /*private void MetodosUISemBotao(string metodo, string situacao, bool b, float valorAtualBarra, float maxBarra)
    {
        switch (metodo)
        {
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
                especialBt.gameObject.SetActive(b);
                if(LogisticaVars.especial) { chuteEspecialBt.gameObject.SetActive(b); }
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
    }*/
    /*void MetodosUIComBotao(string s)
    {
        switch (s)
        {
            case "UI: bola rasteira":
                UI_BolaRasteira();
                break;
            case "click":
                clicouUi = true;
                //print("Clicou UI");
                break;
            case "unclick":
                clicouUi = false;
                //print("Deixou de clicar UI");
                break;
        }
    }*/

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
                botaoDiagonal.SetActive(false);
                botaoLivre2.SetActive(false);
                botaoLivre1.SetActive(false);
                botaoMeio.SetActive(true);

                moverJogadorBt.SetActive(true);
                joystick.SetActive(true);
                barraChuteJogador.SetActive(true);
                break;
            #endregion
            #region Situacoes
            case "bola pequena area":
                //print("Ui: Bola na Pequena Area");
                //LogisticaVars.m_jogadorEscolhido.GetComponentInChildren<AudioListener>().enabled = false;
                StartCoroutine(TransicaoBolaPequenaArea());
                
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
        barraChuteJogador.SetActive(false);
        travarMiraBt.gameObject.SetActive(true);
        joystick.SetActive(true);
    }
    void FimEspecial()
    {
        //MetodosUISemBotao("estado jogador e goleiro", "", false, 0, 0);
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
    
}
