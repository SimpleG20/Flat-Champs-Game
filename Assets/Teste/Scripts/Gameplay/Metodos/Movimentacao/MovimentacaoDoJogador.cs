using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovimentacaoDoJogador : MovimentacaoJogadores
{
    float step, velocidadeBarraChute;

    public static bool pc;
    [SerializeField] Toggle dispositivo;

    UIMetodosGameplay ui;
    FisicaJogador fisica;

    void Start()
    {
        ui = FindObjectOfType<UIMetodosGameplay>();

        JogadorVars.m_maxForca = 360;
        JogadorVars.m_forca = 50;

        JogadorVars.m_sensibilidadeChute = 10;
        JogadorVars.m_sensibilidade = GameManager.Instance.m_config.m_camSensibilidade;
        JogadorVars.sensibilidadeEscolha = 80;

        JogadorVars.m_forcaMin = 20 * AtributosFisicos.coefAtritoEsJogador * AtributosFisicos.gravidade;

        velocidadeBarraChute = GameManager.Instance.m_config.m_velocidadeBarraChute;
        if (velocidadeBarraChute == 0) velocidadeBarraChute = 1;
        EventsManager.current.onAplicarMetodosUiComBotao += BotoesJogador;
    }

    void Update()
    {
        #region Direcao Jogador
        if(!LogisticaVars.goleiroT1 && !LogisticaVars.goleiroT2)
        {
            if (LogisticaVars.jogadorSelecionado) SetDirecaoChute(LogisticaVars.m_jogadorEscolhido);
        }
        #endregion

        #region Movimentacao
        if (LogisticaVars.jogadorSelecionado && !LogisticaVars.goleiroT1 && !LogisticaVars.goleiroT2 && !LogisticaVars.especial || LogisticaVars.escolherOutroJogador)
        {
            fisica = LogisticaVars.m_jogadorEscolhido.GetComponent<FisicaJogador>();
            if (fisica.m_podeVirar)
            {
                JogadorVars.jCorrendo = false;
                if (pc)
                {
                    float h = Input.GetAxis("Horizontal");
                    if(h == 0) JogadorVars.m_rotacionar = false;
                    else JogadorVars.m_rotacionar = true;

                    if (!JogadorVars.m_medirChute)
                    {
                        if (LogisticaVars.escolherOutroJogador)
                            LogisticaVars.m_jogadorEscolhido.transform.Rotate(Vector3.forward * h * JogadorVars.sensibilidadeEscolha * Time.deltaTime);
                        else
                            LogisticaVars.m_jogadorEscolhido.transform.Rotate(Vector3.forward * h * JogadorVars.m_sensibilidade * Time.deltaTime);
                    }
                    else
                    {
                        LogisticaVars.m_jogadorEscolhido.transform.Rotate(Vector3.forward * h * JogadorVars.m_sensibilidadeChute * Time.deltaTime);
                    }
                }
                else
                {
                    if (JogadorVars.m_rotacionar)
                    {
                        if (!JogadorVars.m_medirChute)
                        {
                            if (LogisticaVars.escolherOutroJogador)
                                LogisticaVars.m_jogadorEscolhido.transform.Rotate(Vector3.forward * joystickManager.vX * JogadorVars.sensibilidadeEscolha * Time.deltaTime);
                            else
                                LogisticaVars.m_jogadorEscolhido.transform.Rotate(Vector3.forward * joystickManager.vX * JogadorVars.m_sensibilidade * Time.deltaTime);
                        }
                        else
                        {
                            LogisticaVars.m_jogadorEscolhido.transform.Rotate(Vector3.forward * joystickManager.vX * JogadorVars.m_sensibilidadeChute * Time.deltaTime);
                        }
                    }
                }
            }
            else
            {
                JogadorVars.jCorrendo = true;
            }
        }
        #endregion

        #region Medir Chute
        if (JogadorVars.m_medirChute && LogisticaVars.jogadorSelecionado && !LogisticaVars.jogoParado || 
            JogadorVars.m_medirChute && LogisticaVars.continuaSendoFora || JogadorVars.m_medirChute && LogisticaVars.jogadorSelecionado && !LogisticaVars.aplicouPrimeiroToque)
        {
            float parametro = JogadorMetodos.MedirChute();

            if (JogadorVars.m_forca >= JogadorVars.m_maxForca) JogadorVars.m_forca = JogadorVars.m_maxForca;
            else JogadorVars.m_forca += (parametro * velocidadeBarraChute);

            JogadorMetodos.EncherBarraChuteJogador(JogadorVars.m_forca, JogadorVars.m_maxForca);
        }
        else
        {
            // print("JogoParado: " + LogisticaVars.jogoParado + "Continua Fora: " + LogisticaVars.continuaSendoFora);
            if (JogadorVars.m_aplicarChute && LogisticaVars.jogadorSelecionado)
            {
                if (!LogisticaVars.aplicouPrimeiroToque)
                {
                    LogisticaVars.jogoComecou = true;
                    JogadorVars.m_aplicarChute = false;
                    EventsManager.current.OnAplicarMetodosUiSemBotao("estado jogador", "", true);
                    LogisticaVars.jogoParado = false;
                    LogisticaVars.aplicouPrimeiroToque = true;
                    EventsManager.current.SituacaoGameplay("jogo normal");
                }
                if (JogadorVars.m_forca > JogadorVars.m_forcaMin)
                    JogadorMetodos.ChuteNormal(GetUltimaDirecao());
                else JogadorMetodos.ChuteMalSucedido();

                JogadorMetodos.PosChute();
            }
        }
        #endregion

    }

    void FixedUpdate()
    {
        if (LogisticaVars.jogadorSelecionado && !LogisticaVars.goleiroT1 && !LogisticaVars.goleiroT2)
        {
            if (LogisticaVars.redirecionamentoAutomatico && !JogadorVars.m_rotacionar && !JogadorVars.m_medirChute && JogadorVars.jCorrendo)
            {
                if (!LogisticaVars.continuaSendoFora && !LogisticaVars.escolherOutroJogador)
                {
                    step += 1.5f * Time.deltaTime;
                    GameObject.Find("RotacaoCamera").transform.position =
                        Vector3.MoveTowards((LogisticaVars.m_jogadorEscolhido.transform.position - LogisticaVars.m_jogadorEscolhido.transform.up), GameObject.FindGameObjectWithTag("Bola").transform.position, step);

                    Vector3 direction = GameObject.Find("RotacaoCamera").transform.position - LogisticaVars.m_jogadorEscolhido.transform.position;
                    Quaternion rotation = Quaternion.LookRotation(direction);
                    LogisticaVars.m_jogadorEscolhido.transform.rotation = rotation;
                    LogisticaVars.m_jogadorEscolhido.transform.eulerAngles = new Vector3(-90, LogisticaVars.m_jogadorEscolhido.transform.eulerAngles.y, LogisticaVars.m_jogadorEscolhido.transform.eulerAngles.z);
                }
            }
            else
            {
                step = 0;
                GameObject.Find("RotacaoCamera").transform.position =
                    -LogisticaVars.m_jogadorEscolhido.transform.up + LogisticaVars.m_jogadorEscolhido.transform.position;
            }
        }
    }


    #region Metodos usados para os Botoes do Jogador
    private void BotoesJogador(string s)
    {
        switch (s)
        {
            case "MJ: aplicar chute do jogador":
                AplicarChute();
                break;
            case "MJ: acionar lateral":
                AcionarChuteLateral();
                break;
            case "MJ: acionar escanteio":
                AcionarChuteEscanteio();
                break;
            case "MJ: aplicar especial":
                AplicarChuteEspecial();
                break;
            case "direcionar chute":
                MostrarDirecaoChute();
                break;
        }
    }
    public void EstadoMedirForcaDoChute(bool b)
    {
        JogadorVars.m_medirChute = b;
    }
    private void AplicarChute()
    {
        JogadorVars.m_aplicarChute = true;
        //ultimaDirecao = direcaoBola;
    }
    private void AcionarChuteEscanteio()
    {
        JogadorMetodos.AplicarChuteEscanteio();
        LogisticaVars.jogoParado = false;
        EventsManager.current.OnAplicarRotinas("rotina sair escanteio");
    }
    private void AcionarChuteLateral()
    {
        JogadorMetodos.AplicarChuteLateral();
        LogisticaVars.jogoParado = false;
        EventsManager.current.OnAplicarRotinas("rotina sair lateral");
    }
    private void AplicarChuteEspecial()
    {
        print("Aplicar Especial");
        bola.GetComponent<Rigidbody>().useGravity = false;
        Destroy(GameObject.FindGameObjectWithTag("Mira Especial"));
        EventsManager.current.OnAplicarMetodosUiSemBotao("estado jogador e goleiro", "", false);

        LogisticaVars.aplicouEspecial = true;

        if (LogisticaVars.vezJ1) LogisticaVars.ultimoToque = 1;
        else LogisticaVars.ultimoToque = 2;

        //EventsManager.current.SituacaoGameplay("fim especial");
    }
    void MostrarDirecaoChute()
    {
        if (!ui.mostrarDirecionalBolaBt.isOn)
        {
            direcional.SetActive(true);
            LogisticaVars.mostrarDirecaoBola = true;
        }
        else
        {
            direcional.SetActive(false);
            LogisticaVars.mostrarDirecaoBola = false;
        }
    }
    #endregion

    public void PcOuMobile()
    {
        if (dispositivo.isOn) pc = true;
        else pc = false;
    }
}
