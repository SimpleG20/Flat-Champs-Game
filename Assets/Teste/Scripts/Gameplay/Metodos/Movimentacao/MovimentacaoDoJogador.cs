using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovimentacaoDoJogador : MonoBehaviour
{
    float step, velocidadeBarraChute;

    [SerializeField] Toggle dispositivo;
    public static bool pc;

    private InputManager joystickManager;
    private FisicaBola bola;
    private FisicaJogador fisica;

    void Start()
    {
        joystickManager = FindObjectOfType<InputManager>();
        bola = FindObjectOfType<FisicaBola>();

        JogadorVars.ajusteDirecao = -40;
        JogadorVars.m_alturaMax = 45;
        JogadorVars.m_maxForca = 360;
        JogadorVars.m_forca = 50;

        JogadorVars.m_sensibilidadeChute = 10;
        JogadorVars.m_sensibilidade = GameManager.Instance.m_config.m_camSensibilidade;
        JogadorVars.sensibilidadeEscolha = 80;

        JogadorVars.m_forcaMin = 20 * AtributosFisicos.coefAtritoEsJogador * AtributosFisicos.gravidade;

        //JogadorVars.m_rotacionar = true;
        velocidadeBarraChute = GameManager.Instance.m_config.m_velocidadeBarraChute;
        if (velocidadeBarraChute == 0) velocidadeBarraChute = 1;
        EventsManager.current.onAplicarMetodosUiComBotao += BotoesJogador;
    }

    void Update()
    {
        #region Angulo Jogador
        if(LogisticaVars.jogadorSelecionado)
        {
            JogadorVars.m_senoJogadorEscolhido = -LogisticaVars.m_jogadorEscolhido.transform.up.z;
            JogadorVars.m_cosJogadorEscolhido = -LogisticaVars.m_jogadorEscolhido.transform.up.x;
            JogadorVars.direcaoChute = new Vector3(JogadorVars.m_cosJogadorEscolhido, 0, JogadorVars.m_senoJogadorEscolhido);
        }
        #endregion

        #region Medir Chute
        if (JogadorVars.m_medirChute && LogisticaVars.jogadorSelecionado && !LogisticaVars.jogoParado || 
            JogadorVars.m_medirChute && LogisticaVars.continuaSendoFora)
        {
            float parametro = JogadorMetodos.MedirChute();

            if (JogadorVars.m_forca >= JogadorVars.m_maxForca) JogadorVars.m_forca = JogadorVars.m_maxForca;
            else JogadorVars.m_forca += (parametro * velocidadeBarraChute);

            JogadorMetodos.EncherBarraChuteJogador(JogadorVars.m_forca, JogadorVars.m_maxForca);
        }
        else
        {
            //print("JogoParado: " + LogisticaVars.jogoParado + "Continua Fora: " + LogisticaVars.continuaSendoFora);
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
                    JogadorMetodos.ChuteNormal(JogadorVars.direcaoChute);
                else JogadorMetodos.ChuteMalSucedido();

                JogadorMetodos.PosChute();
            }
        }
        #endregion

        #region Movimentacao
        if(LogisticaVars.jogadorSelecionado && !LogisticaVars.goleiroT1 && !LogisticaVars.goleiroT2 && !LogisticaVars.especial || LogisticaVars.escolherOutroJogador)
        {
            fisica = LogisticaVars.m_jogadorEscolhido.GetComponent<FisicaJogador>();
            if (JogadorVars.m_rotacionar && !pc)
            {
                if (fisica.m_podeVirar && !JogadorVars.m_medirChute)
                {
                    if (LogisticaVars.escolherOutroJogador)
                    {
                        LogisticaVars.m_jogadorEscolhido.transform.Rotate(Vector3.forward * joystickManager.vX * JogadorVars.sensibilidadeEscolha * Time.deltaTime);
                    }
                    else
                    {
                        LogisticaVars.m_jogadorEscolhido.transform.Rotate(Vector3.forward * joystickManager.vX * JogadorVars.m_sensibilidade * Time.deltaTime);
                    }
                }
                else if (fisica.m_podeVirar && JogadorVars.m_medirChute)
                {
                    LogisticaVars.m_jogadorEscolhido.transform.Rotate(Vector3.forward * joystickManager.vX * JogadorVars.m_sensibilidadeChute * Time.deltaTime);
                }
            }

            float h = Input.GetAxis("Horizontal");
            if (h != 0 && pc)
            {
                JogadorVars.m_rotacionar = true;
                if (fisica.m_podeVirar && !JogadorVars.m_medirChute)
                {
                    if (LogisticaVars.escolherOutroJogador) LogisticaVars.m_jogadorEscolhido.transform.Rotate(Vector3.forward * h * JogadorVars.sensibilidadeEscolha * Time.deltaTime);
                    else LogisticaVars.m_jogadorEscolhido.transform.Rotate(Vector3.forward * h * JogadorVars.m_sensibilidade * Time.deltaTime);
                }
                else if (fisica.m_podeVirar && JogadorVars.m_medirChute)
                    LogisticaVars.m_jogadorEscolhido.transform.Rotate(Vector3.forward * h * JogadorVars.m_sensibilidadeChute * Time.deltaTime);
            }
            else if (h == 0 && pc) JogadorVars.m_rotacionar = false;
        }
        #endregion
    }

    void FixedUpdate()
    {
        if (LogisticaVars.jogadorSelecionado && !LogisticaVars.goleiroT1 && !LogisticaVars.goleiroT2)
        {
            if (LogisticaVars.redirecionamentoAutomatico && !JogadorVars.m_rotacionar && !JogadorVars.m_medirChute)
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
            case "aplicar chute do jogador":
                AplicarChute();
                break;
            case "acionar lateral":
                AcionarChuteLateral();
                break;
            case "acionar escanteio":
                AcionarChuteEscanteio();
                break;
            case "aplicar especial":
                AplicarChuteEspecial();
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
    }
    private void AcionarChuteEscanteio()
    {
        if (LogisticaVars.vezJ1) JogadorMetodos.AplicarChuteEscanteio(LogisticaVars.bolaRasteiraT1);
        else JogadorMetodos.AplicarChuteEscanteio(LogisticaVars.bolaRasteiraT2);
        LogisticaVars.jogoParado = false;
        EventsManager.current.OnAplicarRotinas("rotina sair escanteio");
    }
    private void AcionarChuteLateral()
    {
        if (LogisticaVars.vezJ1) JogadorMetodos.AplicarChuteLateral(LogisticaVars.bolaRasteiraT1);
        else JogadorMetodos.AplicarChuteLateral(LogisticaVars.bolaRasteiraT2);
        LogisticaVars.jogoParado = false;
        EventsManager.current.OnAplicarRotinas("rotina sair lateral");
    }
    private void AplicarChuteEspecial()
    {
        Vector3 chute = new Vector3(bola.direcaoBola.x,
            bola.direcaoBola.y / bola.direcaoBola.magnitude * 350f,
            bola.direcaoBola.z / bola.direcaoBola.magnitude * 135f);
        //print(chute);
        bola.m_rbBola.AddForce(chute, ForceMode.Impulse);
        LogisticaVars.aplicouEspecial = true;

        if (LogisticaVars.vezJ1) LogisticaVars.ultimoToque = 1;
        else LogisticaVars.ultimoToque = 2;

        EventsManager.current.SituacaoGameplay("fim especial");
    }
    #endregion

    public void PcOuMobile()
    {
        if (dispositivo.isOn) pc = true;
        else pc = false;
    }
}
