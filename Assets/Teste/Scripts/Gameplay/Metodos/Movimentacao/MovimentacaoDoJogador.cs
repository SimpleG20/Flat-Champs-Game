using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovimentacaoDoJogador : MonoBehaviour
{
    const float maxAlturaChute = 4;
    public float senoJogador, cosJogador, distanciaDaBola, maxAnguloParaChute, alturaChute;
    public float anguloBola, anguloJogador, anguloBolaJogador, anguloDirJogadorBola;
    float step, velocidadeBarraChute, erro;

    Vector3 direcaoJogadorBola, direcaoBola, ultimaDirecao;

    GameObject direcional;

    [SerializeField] Toggle dispositivo;
    public static bool pc;

    UIMetodosGameplay ui;
    InputManager joystickManager;
    FisicaBola bola;
    FisicaJogador fisica;

    void Start()
    {
        joystickManager = FindObjectOfType<InputManager>();
        ui = FindObjectOfType<UIMetodosGameplay>();
        bola = FindObjectOfType<FisicaBola>();
        direcional = GameObject.FindGameObjectWithTag("Direcional Chute");
        direcional.SetActive(false);

        JogadorVars.ajusteDirecao = -40;
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
        #region Angulo Jogador
        if(LogisticaVars.jogadorSelecionado)
        {
            senoJogador = -LogisticaVars.m_jogadorEscolhido.transform.up.z;
            cosJogador = -LogisticaVars.m_jogadorEscolhido.transform.up.x;
            JogadorVars.direcaoChute = new Vector3(cosJogador, 0, senoJogador);
            distanciaDaBola = (bola.transform.position - LogisticaVars.m_jogadorEscolhido.transform.position).magnitude;

            erro = Mathf.Clamp(distanciaDaBola, 0, 10);
            maxAnguloParaChute = (360 / Mathf.Pow(distanciaDaBola, 2)) + Mathf.Pow(((distanciaDaBola - 2) / distanciaDaBola) + 1.25f, 2) + erro;
            anguloJogador = Mathf.Acos(JogadorVars.direcaoChute.x) * Mathf.Rad2Deg;
            anguloBolaJogador = Mathf.Acos(bola.m_vetorDistanciaDoJogador.x / bola.m_vetorDistanciaDoJogador.magnitude) * Mathf.Rad2Deg;
            anguloDirJogadorBola = Mathf.Acos((bola.m_vetorDistanciaDoJogador.x * JogadorVars.direcaoChute.x + JogadorVars.direcaoChute.z * bola.m_vetorDistanciaDoJogador.z) /
                (bola.m_vetorDistanciaDoJogador.magnitude * JogadorVars.direcaoChute.magnitude)) * Mathf.Rad2Deg;


            if (!LogisticaVars.bolaRasteiraT1 && LogisticaVars.vezJ1 || !LogisticaVars.bolaRasteiraT2 && LogisticaVars.vezJ2)
                alturaChute += joystickManager.direcaoRight.y * Time.deltaTime;
            else alturaChute = 0;
            if (alturaChute >= maxAlturaChute) alturaChute = maxAlturaChute;
            if (alturaChute <= 0) alturaChute = 0;

            #region Ajustes
            if (maxAnguloParaChute > 90) maxAnguloParaChute = 90;
            if (anguloDirJogadorBola > maxAnguloParaChute) { direcional.SetActive(false); }
            else
            {
                if (bola.m_bolaNoChao && !bola.m_bolaCorrendo && LogisticaVars.mostrarDirecaoBola && !direcional.activeSelf) direcional.SetActive(true);
                else if (!bola.m_bolaNoChao) direcional.SetActive(false);
            }
            if (senoJogador < 0) anguloJogador = 360 - anguloJogador;
            if (bola.m_vetorDistanciaDoJogador.z < 0) anguloBolaJogador = 360 - anguloBolaJogador;
            if (anguloBolaJogador == 360) anguloBolaJogador = 0;

            if (anguloJogador < anguloBolaJogador)
            {
                if (anguloJogador < 0 || anguloJogador > 0) anguloDirJogadorBola = -anguloDirJogadorBola;
            }
            else
            {
                if (anguloJogador > 360 - maxAnguloParaChute) anguloDirJogadorBola = -anguloDirJogadorBola;
            }

            anguloBola = -(anguloDirJogadorBola / maxAnguloParaChute) * 90 + anguloBolaJogador;
            direcaoBola = new Vector3(Mathf.Cos(anguloBola * Mathf.Deg2Rad), Mathf.Tan(alturaChute / maxAlturaChute * 40 * Mathf.Deg2Rad), Mathf.Sin(anguloBola * Mathf.Deg2Rad));
            #endregion

            Debug.DrawRay(LogisticaVars.m_jogadorEscolhido.transform.position, JogadorVars.direcaoChute * 5, Color.red);
            Debug.DrawRay(LogisticaVars.m_jogadorEscolhido.transform.position, bola.m_vetorDistanciaDoJogador, Color.blue);
            Debug.DrawRay(bola.transform.position, direcaoBola, Color.green);
        }
        #endregion

        #region Movimentacao
        if (LogisticaVars.jogadorSelecionado && !LogisticaVars.goleiroT1 && !LogisticaVars.goleiroT2 && !LogisticaVars.especial || LogisticaVars.escolherOutroJogador)
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
                    JogadorMetodos.ChuteNormal(JogadorVars.direcaoChute);
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
        ultimaDirecao = direcaoBola;
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

    public float GetAnguloBola()
    {
        return anguloBola;
    }
    public Vector3 GetUltimaDirecao()
    {
        return ultimaDirecao;
    }
    public Vector3 GetDirecaoChute()
    {

        return direcaoBola;
    }
    public void PcOuMobile()
    {
        if (dispositivo.isOn) pc = true;
        else pc = false;
    }
}
