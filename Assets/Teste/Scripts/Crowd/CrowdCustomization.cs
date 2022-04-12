using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdCustomization : MonoBehaviour
{
    [SerializeField] GameObject m_bone, m_bandeirinha;

    void Start()
    {
        ColocarBoneBandeira();
    }

    void ColocarBoneBandeira()
    {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("TorcidaAleatoria"))
        {
            SpawnarBone(g);
            SpawnarBandeirinha(g);
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Torcida1"))
        {
            SpawnarBone(g);
            SpawnarBandeirinha(g);
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Torcida2"))
        {
            SpawnarBone(g);
            SpawnarBandeirinha(g);
        }
    }
    void SpawnarBone(GameObject g)
    {
        int i = Random.Range(0, 6);
        if (i == 0)
        {
            GameObject b = m_bone;
            Instantiate(b, g.transform);
            b.transform.localPosition = Vector3.forward * 0.032f;
            b.transform.localScale = new Vector3(0.73f, 0.73f, 0.73f);
            b.transform.localEulerAngles = Vector3.zero;
        }
    }
    void SpawnarBandeirinha(GameObject g)
    {
        int i = Random.Range(0, 10);

        if (i == 0)
        {
            GameObject b = m_bandeirinha;
            Instantiate(b, g.transform);
            b.transform.localPosition = new Vector3(0.0079f, -0.00729f, 0.021f);
            b.transform.localScale = new Vector3(0.085f, 0.085f, 0.88f);
            b.transform.localEulerAngles = new Vector3(8.43f, 18.14f, 17.76f);
        }
    }
}


