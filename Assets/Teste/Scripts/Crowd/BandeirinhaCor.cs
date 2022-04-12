using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandeirinhaCor : MonoBehaviour
{
    [SerializeField] Material m_time1, m_time2, m_marrom;
    void Start()
    {
        GameObject m_torcedor = transform.parent.gameObject;

        GetComponent<MeshRenderer>().materials = new Material[2];
        GetComponent<MeshRenderer>().materials[0].color = m_marrom.color;

        if (m_torcedor.CompareTag("Torcida1"))
        {
            GetComponent<MeshRenderer>().materials[1].color = m_time1.color;
        }
        else if (m_torcedor.CompareTag("Torcida2"))
        {
            GetComponent<MeshRenderer>().materials[1].color = m_time2.color;
        }
        else
        {
            int i = Random.Range(0, 6);
            if (i <= 3) GetComponent<MeshRenderer>().materials[1].color = m_time1.color;
            else GetComponent<MeshRenderer>().materials[1].color = m_time2.color;
        }
        
    }
}
