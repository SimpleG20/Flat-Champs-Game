using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoCameraTorcida : MonoBehaviour
{
    public bool golT1, golT2;
    public float movimentoVertical;

    [Header("Movimento Eliptico")]
    public float a;
    public float b, x, xi, z, zi;
    public float variacaoX, variacaoZ, tempoCamera, tempoMax;
    public bool comecar;

    void Start()
    {
        tempoMax = 14;
    }

    void FixedUpdate()
    {
        if (golT1) 
        { 
            comecar = true;
            tempoCamera = 0;
            z = a = 40;
            b = 20;
            zi = 0;
            x = xi = transform.position.x;
            transform.eulerAngles = new Vector3(10, 0, 0);
            golT1 = false; 
        }
        else if (golT2) 
        { 
            comecar = true;
            tempoCamera = 0;
            z = a = -40;
            b = 20;
            zi = 0;
            x = xi = transform.position.x;
            transform.eulerAngles = new Vector3(10, 180, 0);
            golT2 = false; 
        }
        if (comecar && tempoCamera <= tempoMax)
        {
            tempoCamera += Time.deltaTime;
            if(variacaoZ >= Mathf.PI * 2) variacaoZ = 0;
            variacaoZ += Time.deltaTime * 0.25f;
            
            z = Mathf.Cos(variacaoZ) * a;

            if (variacaoZ >= Mathf.PI && variacaoZ <= 2 * Mathf.PI) x = -MovimentoElipticoEixoX(a, b, z, xi, zi);
            else x = MovimentoElipticoEixoX(a, b, z, xi, zi);

            movimentoVertical += Time.deltaTime * 5;

            transform.position = new Vector3(x, 20, z);
            transform.Rotate(Vector3.up * Time.deltaTime * 25 * a / Mathf.Sqrt(Mathf.Pow(a,2)), Space.World);
        }
    }

    public float MovimentoElipticoEixoX(float a, float b, float z, float xi, float zi)
    {
        float x = Mathf.Sqrt(Mathf.Pow(b, 2) * (1 - (Mathf.Pow(z - zi, 2) / Mathf.Pow(a, 2)))) + xi;
        return x;
    }
}
