using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JogadorMetodos: MonoBehaviour
{
    static MovimentacaoDoJogador mJ;
    static UIMetodosGameplay ui;
    //static float velocidadeBarra;

    private void Start()
    {
        mJ = FindObjectOfType<MovimentacaoDoJogador>();
        ui = FindObjectOfType<UIMetodosGameplay>();
        //velocidadeBarra = GameManager.Instance.m_config.m_velocidadeBarraChute;
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
            JogadorVars.m_maxForcaAtual = JogadorVars.m_maxForcaFora;
        }

        if (JogadorVars.m_chuteAoGol == true)
        {
            JogadorVars.m_forca = 50;
            JogadorVars.m_maxForcaAtual = JogadorVars.m_maxForcaChuteAoGol;
        }

        if(!JogadorVars.m_chuteAoGol && !LogisticaVars.continuaSendoFora)
        {
            JogadorVars.m_forca = 50;
            JogadorVars.m_maxForcaAtual = JogadorVars.m_maxForcaNormal;
        }

        EncherBarraChuteJogador(JogadorVars.m_forca, JogadorVars.m_maxForcaAtual);
        //Debug.Log("Forca: " + JogadorVars.m_forca + " Max: " + JogadorVars.m_maxForca);
    }
    public static void ChuteNormal(Vector3 direcaoChute)
    {
        LogisticaVars.m_jogadorEscolhido_Atual.GetComponent<Rigidbody>().AddForce(direcaoChute * JogadorVars.m_forca, ForceMode.Impulse);
        //JogadorVars.m_esperandoContato = true;

        /*if (JogadorVars.m_forca <= 200) { JogadorVars.m_fatorAtritoBola = 1.8f; JogadorVars.m_fatorAtritoJogador = 0.8f; }
        if (JogadorVars.m_forca > 200 && JogadorVars.m_forca <= 300) { JogadorVars.m_fatorAtritoBola = 2.6f; JogadorVars.m_fatorAtritoJogador = 1.1f; }
        if (JogadorVars.m_forca > 300 && JogadorVars.m_forca <= 400) { JogadorVars.m_fatorAtritoBola = 3.2f; JogadorVars.m_fatorAtritoJogador = 3.4f; }
        if (JogadorVars.m_forca > 400) { JogadorVars.m_fatorAtritoBola = 4f; JogadorVars.m_fatorAtritoJogador = 4.7f; }*/
    }
    public static void ChuteMalSucedido()
    {
        Debug.Log("Chute Fraco");
        JogadorVars.m_esperandoContato = false;
        //Debug.Log("Forca: " + JogadorVars.m_forca + " Max: " + JogadorVars.m_maxForca);
        /*JogadorVars.m_fatorAtritoBola = 1.8f;
        JogadorVars.m_fatorAtritoJogador = 1;*/
    }
    public static void PosChute()
    {
        if (LogisticaVars.auxChuteAoGol)
        {
            Debug.Log("MovimentacaoDoJogador: AposChuteAoGol()");
            LogisticaVars.jogadas = 3;
            JogadorVars.m_chuteAoGol = LogisticaVars.auxChuteAoGol = false;
            //ResetarValoresChute();
        }
        else LogisticaVars.jogadas++;
        JogadorVars.m_aplicarChute = false;

        ResetarValoresChute();
        if (LogisticaVars.jogadas == 3) Gameplay._current.SetSituacao(new TrocarVez(Gameplay._current, VariaveisUIsGameplay._current, CamerasSettings._current));
        //EventsManager.current.OnAplicarRotinas("rotina 3 jogadas");
    }

    public static void AumentarEspecial(float qnt, bool trocou)
    {
        if (!trocou)
        {
            if (LogisticaVars.vezJ1) LogisticaVars.m_especialAtualT1 += (qnt/ 2);
            else LogisticaVars.m_especialAtualT2 += (qnt / 2);
        }
        else
        {
            if (!LogisticaVars.vezJ1) { LogisticaVars.m_especialAtualT1 += (qnt / 2); LogisticaVars.m_especialAtualT2 -= (qnt / 2); }
            else { LogisticaVars.m_especialAtualT2 += (qnt / 2); LogisticaVars.m_especialAtualT1 -= (qnt / 2); }
        }
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
        AplicarChuteEscanteio();
        LogisticaVars.jogoParado = false;
    }
    public static void AutoChuteLateral()
    {
        AplicarChuteLateral();
        LogisticaVars.jogoParado = false;
    }

    public static void AplicarChuteEscanteio()
    {
        Rigidbody bola = GameObject.FindGameObjectWithTag("Bola").GetComponent<Rigidbody>();
        bola.constraints = RigidbodyConstraints.None;

        bola.AddForce(mJ.GetUltimaDirecao() * JogadorVars.m_forca, ForceMode.Impulse);

        /*JogadorVars.m_forca = 50;
        JogadorVars.m_maxForca = 360;
        EncherBarraChuteJogador(JogadorVars.m_forca, JogadorVars.m_maxForca);*/

        EventsManager.current.OnFora("rotina sair escanteio");
    }
    public static void AplicarChuteLateral()
    {
        Rigidbody bola = GameObject.FindGameObjectWithTag("Bola").GetComponent<Rigidbody>();
        bola.constraints = RigidbodyConstraints.None;

        bola.AddForce(mJ.GetUltimaDirecao() * 27.5f, ForceMode.Impulse);

        EventsManager.current.OnFora("rotina sair lateral");
    }


}
