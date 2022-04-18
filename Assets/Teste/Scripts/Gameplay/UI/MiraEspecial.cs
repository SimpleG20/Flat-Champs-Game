using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiraEspecial : MonoBehaviour
{
    [Header("Movimento Interno")]
    [SerializeField] Image tempoGO;
    [SerializeField] Image miraGO;
    float tempoEspecial, tempoScale, tamanho, cos;

    [Header("Movimentacao")]
    [SerializeField] GameObject trajetoria;
    GameObject direcaoEspecial;
    Vector3 startPos;

    float speedMira, distancia;
    bool travouMira;

    InputManager joystick;
    EventsManager events;
    UIMetodosGameplay ui;

    void Start()
    {
        events = EventsManager.current;
        joystick = FindObjectOfType<InputManager>();
        ui = FindObjectOfType<UIMetodosGameplay>();

        events.onAplicarMetodosUiComBotao += UiMetodos;

        direcaoEspecial = GameObject.FindGameObjectWithTag("Direcao Especial");

        startPos = transform.position;
        tamanho = 175;

        Instantiate(trajetoria);
    }

    void FixedUpdate()
    {
        #region Scale e Rotation
        tempoEspecial += Time.deltaTime;
        tempoScale += Time.deltaTime / 2;

        tempoGO.fillAmount = tempoEspecial / LogisticaVars.m_maxEspecial;

        if (tempoScale > Mathf.PI / 2) tempoScale = 0;

        cos = Mathf.Cos(tempoScale) * 175;

        if (cos < 175 / 2) tamanho = 175 - cos;
        else tamanho = cos;

        miraGO.transform.Rotate(Vector3.forward * Time.deltaTime * 80);
        miraGO.rectTransform.sizeDelta = Vector2.one * tamanho;
        #endregion

        #region Movimentacao
        distancia = (direcaoEspecial.transform.position - startPos).magnitude;

        if (distancia < 3) speedMira = 1.2f;
        else speedMira = 2.5f;

        if (MovimentacaoDoJogador.pc)
        {
            if (!travouMira)
            {
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");

                direcaoEspecial.transform.Translate(new Vector3(h, v, 0) * Time.deltaTime * speedMira);

                if (new Vector2(h, v).magnitude == 0 && direcaoEspecial.transform.position != startPos)
                {
                    StartCoroutine(Voltar());
                }
            }
            
        }
        else
        {
            if (!travouMira)
            {
                direcaoEspecial.transform.Translate(new Vector3(joystick.valorX_Esq, joystick.valorY_Esq, 0) * Time.deltaTime * speedMira);

                if (new Vector2(joystick.valorX_Esq, joystick.valorY_Esq).magnitude == 0 && direcaoEspecial.transform.position != startPos)
                {
                    StartCoroutine(Voltar());
                }
            }
        }

        transform.position = FindObjectOfType<Camera>().WorldToScreenPoint(direcaoEspecial.transform.position, Camera.MonoOrStereoscopicEye.Mono);
        #endregion
    }

    void UiMetodos(string metodo)
    {
        switch (metodo)
        {
            case "travar mira especial":
                travouMira = true;
                ui.travarMiraBt.gameObject.SetActive(false);
                ui.chuteEspecialBt.gameObject.SetActive(true);
                break;
        }
    }

    public bool MiraTravada()
    {
        return travouMira;
    }

    IEnumerator Voltar()
    {
        float step = 0;

        yield return new WaitForSeconds(0.01f);
        step += 0.1f;
        if(LogisticaVars.vezJ1) direcaoEspecial.transform.position = Vector3.MoveTowards(direcaoEspecial.transform.position, GameObject.FindGameObjectWithTag("Gol2").transform.position, step);
        else direcaoEspecial.transform.position = Vector3.MoveTowards(direcaoEspecial.transform.position, GameObject.FindGameObjectWithTag("Gol1").transform.position, step);
    }
}
