using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotacionarPlayerMenu : MonoBehaviour
{
    [SerializeField] GameObject m_player;
    Vector3 posInicial;
    public float anguloPartida;
    ScrollRect scroll;

    bool rotacionar;
    float angulo;

    // Start is called before the first frame update
    void Start()
    {
        scroll = GetComponent<ScrollRect>();
        posInicial = scroll.transform.position;
        anguloPartida = GameObject.Find("Player Botao").transform.rotation.eulerAngles.y;
    }

    private void Update()
    {
        angulo = m_player.transform.rotation.eulerAngles.y;
        if (transform.rotation.eulerAngles.y > 90) angulo = -m_player.transform.rotation.eulerAngles.y + 90 + 360;
        if (rotacionar) Rotacionar();
    }

    public void AcionarRotacao()
    {
        rotacionar = true;
    }

    public void VoltarPosInicial()
    {
        anguloPartida = angulo;
        rotacionar = false;
        scroll.transform.position = posInicial;
    }

    public void SetarAnguloPartida(float ang)
    {
        anguloPartida = ang;
    }

    void Rotacionar()
    {
        m_player.transform.eulerAngles = Vector3.up * ( anguloPartida - (scroll.content.transform.position.magnitude - posInicial.magnitude)) + Vector3.left * 90;
    }
}
