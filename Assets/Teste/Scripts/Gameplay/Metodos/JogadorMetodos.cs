using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JogadorMetodos: MonoBehaviour
{
    public static UIMetodosGameplay ui;
    public static float velocidadeBarra;

    private void Start()
    {
        ui = FindObjectOfType<UIMetodosGameplay>();
        velocidadeBarra = GameManager.Instance.m_config.m_velocidadeBarraChute;
    }

    public static float MedirChute()
    {
        float parametro = 10;

        if (JogadorVars.m_chuteAoGol == true)
        {
            parametro = 17;
        }

        if (LogisticaVars.continuaSendoFora == true)
        {
            //JogadorVars.m_maxForca = 45;
            parametro = 1;
        }

        if(!LogisticaVars.continuaSendoFora && !JogadorVars.m_chuteAoGol)
        {
            if (JogadorVars.m_forca < JogadorVars.m_forcaMin) parametro = 15;
            else parametro = 12;
        }

        return parametro;
    }

    public static void ResetarValoresChute()
    {
        //Debug.Log("Resetar Valores do Chute");
        EstadoMedirForcaDoChute(false);

        if (LogisticaVars.continuaSendoFora == true)
        {
            JogadorVars.m_forca = 0;
            JogadorVars.m_maxForca = 45;
        }

        if (JogadorVars.m_chuteAoGol == true)
        {
            JogadorVars.m_forca = 50;
            JogadorVars.m_maxForca = 620;
        }

        if(!JogadorVars.m_chuteAoGol && !LogisticaVars.continuaSendoFora)
        {
            JogadorVars.m_forca = 50;
            JogadorVars.m_maxForca = 360;
        }

        EncherBarraChuteJogador(JogadorVars.m_forca, JogadorVars.m_maxForca);
        //Debug.Log("Forca: " + JogadorVars.m_forca + " Max: " + JogadorVars.m_maxForca);
    }
    public static void ChuteNormal(Vector3 direcaoChute)
    {
        LogisticaVars.m_jogadorEscolhido.GetComponent<Rigidbody>().AddForce(direcaoChute * JogadorVars.m_forca, ForceMode.Impulse);

        /*if (JogadorVars.m_forca <= 200) { JogadorVars.m_fatorAtritoBola = 1.8f; JogadorVars.m_fatorAtritoJogador = 0.8f; }
        if (JogadorVars.m_forca > 200 && JogadorVars.m_forca <= 300) { JogadorVars.m_fatorAtritoBola = 2.6f; JogadorVars.m_fatorAtritoJogador = 1.1f; }
        if (JogadorVars.m_forca > 300 && JogadorVars.m_forca <= 400) { JogadorVars.m_fatorAtritoBola = 3.2f; JogadorVars.m_fatorAtritoJogador = 3.4f; }
        if (JogadorVars.m_forca > 400) { JogadorVars.m_fatorAtritoBola = 4f; JogadorVars.m_fatorAtritoJogador = 4.7f; }*/
    }
    public static void ChuteMalSucedido()
    {
        Debug.Log("Chute Fraco");
        //Debug.Log("Forca: " + JogadorVars.m_forca + " Max: " + JogadorVars.m_maxForca);
        /*JogadorVars.m_fatorAtritoBola = 1.8f;
        JogadorVars.m_fatorAtritoJogador = 1;*/
    }
    public static void PosChute()
    {
        JogadorVars.m_forca = 50;
        JogadorVars.m_maxForca = 360;
        EncherBarraChuteJogador(JogadorVars.m_forca, JogadorVars.m_maxForca);

        if (LogisticaVars.auxChuteAoGol)
        {
            Debug.Log("MovimentacaoDoJogador: AposChuteAoGol()");
            EventsManager.current.OnAplicarMetodosUiSemBotao("estado jogador e goleiro", "", false);
            LogisticaVars.jogadas = 3;
            JogadorVars.m_chuteAoGol = LogisticaVars.auxChuteAoGol = false;
            ResetarValoresChute();
        }
        else LogisticaVars.jogadas++;
        JogadorVars.m_aplicarChute = false;

        if (LogisticaVars.jogadas == 3) EventsManager.current.OnAplicarRotinas("rotina 3 jogadas");
    }

    public static void EncherBarraChuteJogador(float forca, float maxForca)
    {
        ui.barraChuteJogador.transform.GetChild(1).GetComponent<Image>().fillAmount = (forca / maxForca);
        ui.barraChuteJogador.transform.GetChild(1).GetComponent<Image>().color = ui.gradienteChute.Evaluate(forca / maxForca);
    }
    private static void EstadoMedirForcaDoChute(bool b)
    {
        JogadorVars.m_medirChute = b;
    }

    public static void AutoChuteEscanteio()
    {
        if (LogisticaVars.vezJ1) AplicarChuteEscanteio(LogisticaVars.bolaRasteiraT1);
        else AplicarChuteEscanteio(LogisticaVars.bolaRasteiraT2);
        LogisticaVars.jogoParado = false;
    }
    public static void AutoChuteLateral()
    {
        if (LogisticaVars.vezJ1) AplicarChuteLateral(LogisticaVars.bolaRasteiraT1);
        else AplicarChuteLateral(LogisticaVars.bolaRasteiraT2);
        LogisticaVars.jogoParado = false;
    }

    public static void AplicarChuteEscanteio(bool bolaRasteira)
    {
        Rigidbody bola = GameObject.FindGameObjectWithTag("Bola").GetComponent<Rigidbody>();
        bola.constraints = RigidbodyConstraints.None;

        if (bolaRasteira)
            bola.AddForce(new Vector3(JogadorVars.m_cosJogadorEscolhido, 0.0f, JogadorVars.m_senoJogadorEscolhido) * JogadorVars.m_forca, ForceMode.Impulse);
        else
            bola.AddForce(new Vector3(JogadorVars.m_cosJogadorEscolhido, 0.75f, JogadorVars.m_senoJogadorEscolhido) * JogadorVars.m_forca, ForceMode.Impulse);

        JogadorVars.m_forca = 50;
        JogadorVars.m_maxForca = 250;
        EncherBarraChuteJogador(JogadorVars.m_forca, JogadorVars.m_maxForca);

        EventsManager.current.OnAplicarRotinas("rotina sair escanteio");
    }
    public static void AplicarChuteLateral(bool bolaRasteira)
    {
        Rigidbody bola = GameObject.FindGameObjectWithTag("Bola").GetComponent<Rigidbody>();
        bola.constraints = RigidbodyConstraints.None;

        if (bolaRasteira)
            bola.AddForce(new Vector3(JogadorVars.m_cosJogadorEscolhido, 0, JogadorVars.m_senoJogadorEscolhido) * 29, ForceMode.Impulse);
        else
            bola.AddForce(new Vector3(JogadorVars.m_cosJogadorEscolhido, 0.6f, JogadorVars.m_senoJogadorEscolhido) * 25, ForceMode.Impulse);

        EventsManager.current.OnAplicarRotinas("rotina sair lateral");
    }


}
