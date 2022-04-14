using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Joystick")]
    Vector2 touchPosA;
    Vector2 touchPosB, posInicialTouchB;
    public Vector2 direcaoLeft, direcaoRight;
    public Transform center;
    public Transform handle;

    public float vX, vY;
    public float numeroDeToques;

    public bool voltar;

    private int leftTouch = 99, rightTouch = 99;


    void FixedUpdate()
    {
        for (int i = 0; i < Input.touches.Length; i++)
        {
            if (Input.touches[i].position.x > Screen.width / 2)
            {
                touchPosB = Input.touches[i].position;
                if(Input.touches[i].phase == TouchPhase.Began)
                {
                    rightTouch = Input.touches[i].fingerId;
                    posInicialTouchB = touchPosB;
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
                    direcaoRight = Vector2.zero;
                }
            }
            else
            {
                touchPosA = Input.touches[i].position;
                if (Input.touches[i].phase == TouchPhase.Began)
                {
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
                    GoleiroVars.m_movimentar = JogadorVars.m_rotacionar = false;
                    vX = 0;
                    vY = 0;
                    handle.localPosition = new Vector3(0, 0, 0);
                }
            }
        }
        numeroDeToques = Input.touchCount;
    }
    
}
