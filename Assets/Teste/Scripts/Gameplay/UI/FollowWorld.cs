using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowWorld : MonoBehaviour
{
    [Header("Tweaks")]
    public Transform lookAt;
    public Vector3 offset;

    // Update is called once per frame
    void Start()
    {
        PosicionarBarra();
    }

    public void PosicionarBarra()
    {
        if (lookAt != null)
        {
            Vector3 pos = FindObjectOfType<Camera>().WorldToScreenPoint(lookAt.position) + offset;

            if (transform.position != pos) transform.position = pos;
        }
        else
        {
            return;
        }
    }
}
