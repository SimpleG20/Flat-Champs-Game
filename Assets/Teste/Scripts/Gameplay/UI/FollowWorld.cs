using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowWorld : MonoBehaviour
{
    [Header("Tweaks")]
    public bool ajustou;
    public Transform lookAt;
    public Vector3 offset;

    public void PosicionarBarra()
    {
        if (!ajustou)
        {
            if (lookAt != null)
            {
                Vector3 pos = FindObjectOfType<Camera>().WorldToScreenPoint(lookAt.position) + offset;

                if (transform.position != pos) { transform.position = pos; ajustou = true; }
            }
            else
            {
                return;
            }
        }
        
    }
}
