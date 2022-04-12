using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MovimentoMenssagens : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float limite, inicio;

    void FixedUpdate()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        if(speed > 0)
        {
            if (transform.position.x > limite) transform.position = new Vector3(inicio, transform.position.y, transform.position.z);
        }
        else
        {
            if (transform.position.x < limite) transform.position = new Vector3(inicio, transform.position.y, transform.position.z);
        }
        
    }
}
