using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiraEspecial : MonoBehaviour
{
    [Header("Teste")]
    public GameObject gol;
    public GameObject bola, direcaoEspecial;
    public float distanciaGol, forca;
    public bool chutar, pc;

    [Header("Movimento Interno")]
    [SerializeField] Image tempoGO;
    [SerializeField] Image miraGO;
    float tempoEspecial, tempoScale, tamanho, cos;

    [Header("Movimentacao")]
    public Vector2 startPos;

    public float speedMira, distancia, velocidadeVolta;
    public bool voltando, travouMira;

    //private FisicaBola bola;
    private InputManager joystick;

    void Start()
    {
        joystick = FindObjectOfType<InputManager>();
        //bola = FindObjectOfType<FisicaBola>();

        direcaoEspecial.transform.position = GameObject.Find("Gol Tipo 1").transform.position;
        startPos = FindObjectOfType<Camera>().WorldToScreenPoint(direcaoEspecial.transform.position, Camera.MonoOrStereoscopicEye.Mono);

        transform.position = startPos;
        velocidadeVolta = 3.5f;
        tamanho = 175;

        distanciaGol = (gol.transform.position - bola.transform.position).magnitude;
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
        distancia = (direcaoEspecial.transform.position - gol.transform.position).magnitude;

        if (distancia < 3) speedMira = 1.5f;
        else speedMira = 2f;

        if (pc)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            direcaoEspecial.transform.Translate(new Vector3(h, v, 0) * Time.deltaTime * speedMira);

            if (new Vector2(h, v).magnitude == 0 && !travouMira)
            {
                StartCoroutine(Voltar());
            }
        }
        else
        {
            direcaoEspecial.transform.Translate(new Vector3(-joystick.vX, joystick.vY, 0) * Time.deltaTime * speedMira);

            if (new Vector2(joystick.vX, joystick.vY).magnitude == 0 && !travouMira && direcaoEspecial.transform.position != gol.transform.position)
            {
                StartCoroutine(Voltar());
            }
        }

        transform.position = FindObjectOfType<Camera>().WorldToScreenPoint(direcaoEspecial.transform.position, Camera.MonoOrStereoscopicEye.Mono);
        #endregion


    }

    private void Update()
    {

        if (chutar)
        {
            bola.GetComponent<Rigidbody>().AddForce((direcaoEspecial.transform.position - bola.transform.position).normalized * forca, ForceMode.Impulse);
            chutar = false;
        }
    }


    IEnumerator Voltar()
    {
        float step = 0;

        yield return new WaitForSeconds(0.01f);
        step += 0.05f;
        direcaoEspecial.transform.position = Vector3.MoveTowards(direcaoEspecial.transform.position, gol.transform.position, step);
    }
}
