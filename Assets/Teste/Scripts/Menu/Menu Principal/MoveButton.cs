 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveButton : MonoBehaviour
{
    Vector3 posInicial;

    private void Start()
    {
        posInicial = transform.position;
    }

    public void MoveButtonToRight()
    {
        if(posInicial.x - 30 <= transform.position.x)
            transform.position = new Vector3(transform.position.x + 30, transform.position.y, transform.position.z);
    }

    public void MoveButtonToLeft()
    {
        if(posInicial.x + 30 >= transform.position.x) 
            transform.position = new Vector3(transform.position.x - 30, transform.position.y, transform.position.z);
    }
    public void VoltarPosicaoIncial()
    {
        transform.position = posInicial;
    }
}
