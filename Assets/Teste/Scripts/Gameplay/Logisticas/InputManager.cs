using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [Header("Joystick")]
    Vector2 touchPosA;
    Vector2 touchPosB, posInicialTouchB;
    public Vector2 direcaoLeft, direcaoRight;
    [SerializeField] Transform center;
    [SerializeField] Transform handle;

    public float valorX_Esq, valorY_Esq;
    [SerializeField] float numeroDeToques;

    bool printR, printL, movimentoHandle;
    private int leftTouchId = 99, rightTouchId = 99;

    UIMetodosGameplay ui;

    private void Start()
    {
        ui = FindObjectOfType<UIMetodosGameplay>();
    }

    void Update()
    {
        for (int i = 0; i < Input.touches.Length; i++)
        {
            if (Input.touches[i].position.x > Screen.width / 2)
            {

                touchPosB = Input.touches[i].position;
                if (Input.touches[i].phase == TouchPhase.Began)
                {
                    printR = true;

                    rightTouchId = Input.touches[i].fingerId;
                    posInicialTouchB = touchPosB;
                }
                if(Input.touches[i].phase == TouchPhase.Moved && rightTouchId == Input.touches[i].fingerId)
                {
                    if (!ui.clicouUi) direcaoRight = Vector2.ClampMagnitude(touchPosB - posInicialTouchB, 2);
                    else direcaoRight = Vector2.zero;
                    if (LogisticaVars.goleiroT1 || LogisticaVars.goleiroT2 || LogisticaVars.auxChuteAoGol || LogisticaVars.especial)
                    {
                        //direcaoRight = Vector2.ClampMagnitude(touchPosB - posInicialTouchB, 1);
                        /*if(LogisticaVars.auxChuteAoGol){
                            FindObjectOfType<TesteDirecaoBola>().alturaChute = FindObjectOfType<TesteDirecaoBola>().alturaChute + direcaoRight.y *  Time.deltaTime;
                        }*/
                    }
                }

                if (Input.touches[i].phase == TouchPhase.Ended && rightTouchId == Input.touches[i].fingerId)
                {
                    if (printR) print("Dedo Direito saiu da Tela");
                    printR = false;
                    direcaoRight = Vector2.zero;
                }
            }
            else
            {
                touchPosA = Input.touches[i].position;
                if (Input.touches[i].phase == TouchPhase.Began)
                {
                    printL = true;
                    leftTouchId = Input.touches[i].fingerId;
                    if((touchPosA - new Vector2(center.position.x, center.position.y)).magnitude <= 288)
                    {
                        if (!LogisticaVars.goleiroT1 && !LogisticaVars.goleiroT2)
                        {
                            if (!LogisticaVars.especial) JogadorVars.m_rotacionar = true;
                            GoleiroVars.m_movimentar = false;
                        }
                        else if (LogisticaVars.goleiroT1 || LogisticaVars.goleiroT2)
                        {
                            GoleiroVars.m_movimentar = true;
                            JogadorVars.m_rotacionar = false;
                        }
                        movimentoHandle = true;
                    }
                }
                if (Input.touches[i].phase == TouchPhase.Moved && leftTouchId == Input.touches[i].fingerId)
                {
                    if (movimentoHandle)
                    {
                        if (center.gameObject.GetComponent<Image>().color.a < 0.7) center.gameObject.GetComponent<Image>().color = new Vector4(1, 1, 1, 1f);
                        if (handle.gameObject.GetComponent<Image>().color.a < 0.7) handle.gameObject.GetComponent<Image>().color = new Vector4(1, 1, 1, 1f);

                        direcaoLeft = Vector2.ClampMagnitude(touchPosA - new Vector2(center.position.x, center.position.y), 200);
                        //print(direcaoLeft);
                        valorX_Esq = Mathf.Clamp(direcaoLeft.x / 200, -1, 1);
                        valorY_Esq = Mathf.Clamp(direcaoLeft.y / 200, -1, 1);

                        /*if (valorX_Esq > 1) valorX_Esq = 1;
                        else if (valorX_Esq < -1) valorX_Esq = -1;

                        if (valorY_Esq > 1) valorY_Esq = 1;
                        else if (valorY_Esq < -1) valorY_Esq = -1;*/

                        handle.localPosition = new Vector3(valorX_Esq * 200, valorY_Esq * 200, handle.position.z);
                    }
                }
                else if (Input.touches[i].phase == TouchPhase.Ended && leftTouchId == Input.touches[i].fingerId)
                {
                    if (movimentoHandle)
                    {
                        Color c = Color.white;
                        c.a = 0.25f;
                        if (center.gameObject.GetComponent<Image>().color.a > 0.7) center.gameObject.GetComponent<Image>().color = c;
                        if (handle.gameObject.GetComponent<Image>().color.a < 0.7) handle.gameObject.GetComponent<Image>().color = c;
                        movimentoHandle = false;
                    }

                    if (printL) print("Dedo esquerdo saiu da tela");
                    printL = false;
                    GoleiroVars.m_movimentar = JogadorVars.m_rotacionar = false;
                    direcaoLeft = Vector2.zero;
                    valorX_Esq = 0;
                    valorY_Esq = 0;
                    handle.localPosition = new Vector3(0, 0, 0);
                    
                }
            }
        }
        numeroDeToques = Input.touchCount;
    }
    
}
