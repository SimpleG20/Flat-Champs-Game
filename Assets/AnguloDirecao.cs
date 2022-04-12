using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AnguloDirecao : MonoBehaviour
{
    public Vector3 forward;
    public float speed;


    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.localEulerAngles += Vector3.forward * h * speed * Time.deltaTime;
        transform.Translate(-transform.up * v * 2 * Time.deltaTime, Space.World);

        forward = -transform.up;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(-transform.up.x, 0, -transform.up.z) * 150, ForceMode.Impulse);
        }
    }

}
