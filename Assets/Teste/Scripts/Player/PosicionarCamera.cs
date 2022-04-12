using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosicionarCamera : MonoBehaviour
{
    public List<Vector3> m_posicoesCamera;

    public void PosicionarCameraParaJogador(GameObject g, int i)
    {
        switch (i)
        {
            case 1:
                g.transform.GetChild(1).localEulerAngles = m_posicoesCamera[0];
                print(i);
                break;
            case 2:
                g.transform.GetChild(1).localEulerAngles = m_posicoesCamera[1];
                print(i);
                break;
            case 3:
                g.transform.GetChild(1).localEulerAngles = m_posicoesCamera[2];
                print(i);
                break;
            case 4:
                g.transform.GetChild(1).localEulerAngles = m_posicoesCamera[3];
                print(i);
                break;
            case 5:
                g.transform.GetChild(1).localEulerAngles = m_posicoesCamera[4];
                print(i);
                break;
            case 6:
                g.transform.GetChild(1).localEulerAngles = m_posicoesCamera[5];
                print(i);
                break;
            case 7:
                g.transform.GetChild(1).localEulerAngles = m_posicoesCamera[6];
                print(i);
                break;
            case 8:
                g.transform.GetChild(1).localEulerAngles = m_posicoesCamera[7];
                print(i);
                break;
            case 9:
                g.transform.GetChild(1).localEulerAngles = m_posicoesCamera[8];
                print(i);
                break;
        }
    }
}
