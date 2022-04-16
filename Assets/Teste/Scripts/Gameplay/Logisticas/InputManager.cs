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

    public float vX, vY;
    [SerializeField] float numeroDeToques;

    bool printR, printL;

    private int leftTouch = 99, rightTouch = 99;

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
        m_PointerEventData = new PointerEventData(m_EventSystem);
        m_PointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        m_Raycaster.Raycast(m_PointerEventData, results);

        for (int i = 0; i < Input.touches.Length; i++)
        {
            if (Input.touches[i].position.x > Screen.width / 2)
            {
                touchPosB = Input.touches[i].position;
                
                if(Input.touches[i].phase == TouchPhase.Began)
                {
                    printR = true;
                    rightTouch = Input.touches[i].fingerId;
                    foreach (RaycastResult result in results)
                    {
                        if (result.gameObject.layer == 5 && touchPosB == new Vector2(result.gameObject.transform.position.x, result.gameObject.transform.position.y))
                        {
                            print("UI");
                        }
                        else if(result.gameObject.layer != 5 && touchPosB != new Vector2(result.gameObject.transform.position.x, result.gameObject.transform.position.y))
                        {
                            posInicialTouchB = touchPosB;
                        }
                    }
                    
                }

                if(Input.touches[i].phase == TouchPhase.Moved && rightTouch == Input.touches[i].fingerId)
                {
                    direcaoRight = Vector2.ClampMagnitude(touchPosB - posInicialTouchB, 1);
                    if (LogisticaVars.goleiroT1 || LogisticaVars.goleiroT2 || LogisticaVars.auxChuteAoGol || LogisticaVars.especial)
                    {
                        //direcaoRight = Vector2.ClampMagnitude(touchPosB - posInicialTouchB, 1);
                        /*if(LogisticaVars.auxChuteAoGol){
                            FindObjectOfType<TesteDirecaoBola>().alturaChute = FindObjectOfType<TesteDirecaoBola>().alturaChute + direcaoRight.y *  Time.deltaTime;
                        }*/
                    }
                }

                if (Input.touches[i].phase == TouchPhase.Ended && rightTouch == Input.touches[i].fingerId)
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
                    leftTouch = Input.touches[i].fingerId;
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
                if (Input.touches[i].phase == TouchPhase.Moved && leftTouch == Input.touches[i].fingerId)
                {
                    direcaoLeft = Vector2.ClampMagnitude(touchPosA - new Vector2(center.position.x, center.position.y), 150);
                    vX = direcaoLeft.x / 150;
                    vY = direcaoLeft.y / 150;

                    if (vX > 1) vX = 1;
                    else if (vX < -1) vX = -1;

                    if (vY > 1) vY = 1;
                    else if (vY < -1) vY = -1;
                    
                    handle.localPosition = new Vector3(vX * 150, vY * 150, handle.position.z);
                }
                else if (Input.touches[i].phase == TouchPhase.Ended && leftTouch == Input.touches[i].fingerId)
                {
                    if (printL) print("Dedo esquerdo saiu da tela");
                    printL = false;
                    GoleiroVars.m_movimentar = JogadorVars.m_rotacionar = false;
                    direcaoLeft = Vector2.zero;
                    vX = 0;
                    vY = 0;
                    handle.localPosition = new Vector3(0, 0, 0);
                }
            }
        }
        numeroDeToques = Input.touchCount;
    }
    
}
