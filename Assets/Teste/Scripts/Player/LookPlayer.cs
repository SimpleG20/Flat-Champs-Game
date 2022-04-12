using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookPlayer : MonoBehaviour
{
    public GameObject m_player;
    void Update()
    {
        transform.LookAt(m_player.transform);
    }
}
