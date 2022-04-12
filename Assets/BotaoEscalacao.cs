using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotaoEscalacao : MonoBehaviour
{
    [SerializeField] public float tipo;
    [SerializeField] float maxDistancia;
    [SerializeField] public float porcentagemX, porcentagemY;
    [SerializeField] bool movimentar;
    Vector3 posInicial;

    // Start is called before the first frame update
    void Start()
    {
        posInicial = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (movimentar)
        {
            transform.position = Input.mousePosition;
            LimiteMovimentacao();
        }
    }

    public Vector2 Posicao()
    {
        Vector2 v = Vector2.zero;

        v.x = transform.localPosition.x / (478 / 2);
        v.y = transform.localPosition.y / (737 / 2);

        return v;
    }

    public void AtivarMovimento()
    {
        movimentar = true;
    }

    void LimiteMovimentacao()
    {
        if(tipo == 1)
        {
            if (transform.position.y > 505) transform.position = new Vector3(transform.position.x, 505);
            if (transform.position.y < 445) transform.position = new Vector3(transform.position.x, 445);

        }
        else if(tipo == 2)
        {
            if (transform.position.y > 410) transform.position = new Vector3(transform.position.x, 410);
            if (transform.position.y < 320) transform.position = new Vector3(transform.position.x, 320);
        }
        else
        {
            if (transform.position.y > 308) transform.position = new Vector3(transform.position.x, 308);
            if (transform.position.y < 240) transform.position = new Vector3(transform.position.x, 240);
        }
        if (transform.position.x < 360) transform.position = new Vector3(360, transform.position.y);
        else if (transform.position.x > 798) transform.position = new Vector3(798, transform.position.y);
    }

    public void AplicarPosicaoNova()
    {
        movimentar = false;
        if(BotaoMaisPerto() < maxDistancia)
        {
            VoltarPosicaoInicial();
        }
        else
        {
            porcentagemX = transform.localPosition.x / 239; 
            porcentagemY = transform.localPosition.y / 318.5f;
            posInicial = transform.position;
        }
    }
    
    float BotaoMaisPerto()
    {
        float distancia = 10000;
        foreach(GameObject b in GameObject.FindGameObjectsWithTag("Botao Lousa"))
        {
            if ((b.transform.position - transform.position).magnitude <= distancia && b != this.gameObject)
            {
                distancia = (b.transform.position - transform.position).magnitude;
            }
            else continue;
        }
        return distancia;
    }

    void VoltarPosicaoInicial()
    {
        transform.position = posInicial;
    }
}
