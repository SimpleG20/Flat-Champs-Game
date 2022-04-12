using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListaDeEsquemas : MonoBehaviour
{
    public EsquemasTaticos esquemaDisponivel;
    public List<EsquemasTaticos> esquemas;


    public void SetarInfoBotoes(out float[,] locais, float x, float y)
    {
        locais = new float[transform.childCount, 3];
        for(int i = 0; i < transform.childCount; i++)
        {
            locais[i, 0] = transform.GetChild(i).GetComponent<BotaoEscalacao>().Posicao().x * x;
            //print(locais[i, 0]);
            locais[i, 1] = transform.GetChild(i).GetComponent<BotaoEscalacao>().Posicao().y * y;
            //print(locais[i, 1]);
            locais[i, 2] = transform.GetChild(i).GetComponent<BotaoEscalacao>().tipo;
            //print(locais[i, 2]);
        }
    }
}
