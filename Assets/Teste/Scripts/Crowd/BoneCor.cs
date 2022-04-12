using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneCor : MonoBehaviour
{
    public GameObject m_torcedor;
    // Start is called before the first frame update
    void Start()
    {
        m_torcedor = transform.parent.gameObject;
        this.GetComponent<MeshRenderer>().materials = new Material[1];
        this.GetComponent<MeshRenderer>().materials[0].color = m_torcedor.GetComponent<MeshRenderer>().materials[0].color;
    }
}
