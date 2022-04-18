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

    bool printR, printL;

    private int leftTouchId = 99, rightTouchId = 99;

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    private void Start()
    {
        m_Raycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
        m_EventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    void FixedUpdate()
    {
        for (int i = 0; i < Input.touches.Length; i++)
        {
            if (Input.touches[i].position.x > Screen.width / 2)
            {
                m_PointerEventData = new PointerEventData(m_EventSystem);
                m_PointerEventData.position = Input.touches[i].position;
                List<RaycastResult> results = new List<RaycastResult>();
                m_Raycaster.Raycast(m_PointerEventData, results);

                touchPosB = Input.touches[i].position;
                if (Input.touches[i].phase == TouchPhase.Began)
                {
                    printR = true;
                    //rightTouchId = Input.touches[i].fingerId;
                    //posInicialTouchB = touchPosB;

                    foreach (RaycastResult result in results)
                    {
                        if (result.gameObject.layer == 5)
                        {
                            //posInicialTouchB = Vector2.zero;
                        }
                        else
                        {
                            rightTouchId = Input.touches[i].fingerId;
                            posInicialTouchB = touchPosB;
                        }
                    }
                }
                if(Input.touches[i].phase == TouchPhase.Moved && rightTouchId == Input.touches[i].fingerId)
                {
                    if (posInicialTouchB.magnitude != 0) direcaoRight = Vector2.ClampMagnitude(touchPosB - posInicialTouchB, 2);
                    else direcaoRight = Vector2.zero;

                    //if (touchPosB.y < posInicialTouchB.y) direcaoRight.y = -direcaoRight.y;

                    //print(direcaoRight);
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
                    //if (printR) print("Dedo Direito saiu da Tela");
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
                    if (!LogisticaVars.goleiroT1 && !LogisticaVars.goleiroT2)
                    {
                        if(!LogisticaVars.especial) JogadorVars.m_rotacionar = true;
                        GoleiroVars.m_movimentar = false;
                    }
                    else if(LogisticaVars.goleiroT1 || LogisticaVars.goleiroT2)
                    {
                        GoleiroVars.m_movimentar = true;
                        JogadorVars.m_rotacionar = false;
                    }
                }
                if (Input.touches[i].phase == TouchPhase.Moved && leftTouchId == Input.touches[i].fingerId)
                {
                    direcaoLeft = Vector2.ClampMagnitude(touchPosA - new Vector2(center.position.x, center.position.y), 150);
                    print(direcaoLeft);
                    valorX_Esq = direcaoLeft.x / 150;
                    valorY_Esq = direcaoLeft.y / 150;

                    if (valorX_Esq > 1) valorX_Esq = 1;
                    else if (valorX_Esq < -1) valorX_Esq = -1;

                    if (valorY_Esq > 1) valorY_Esq = 1;
                    else if (valorY_Esq < -1) valorY_Esq = -1;
                    
                    handle.localPosition = new Vector3(valorX_Esq * 150, valorY_Esq * 150, handle.position.z);
                }
                else if (Input.touches[i].phase == TouchPhase.Ended && leftTouchId == Input.touches[i].fingerId)
                {
                    //if (printL) print("Dedo esquerdo saiu da tela");
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
