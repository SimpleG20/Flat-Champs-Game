using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcompanharNaLateral : MonoBehaviour
{
    private GameObject bola;

    // Start is called before the first frame update
    void Start()
    {
        bola = GameObject.Find("Bola");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, bola.transform.position.z);
    }
}
