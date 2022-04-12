using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguirBola : MonoBehaviour
{
    Vector3 offset;
    public ForcaAtrito bola;
    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(transform.position.x - bola.transform.position.x , transform.position.y, transform.position.z - bola.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(bola.transform.position.x, 0, bola.transform.position.z) + offset;
    }
}
