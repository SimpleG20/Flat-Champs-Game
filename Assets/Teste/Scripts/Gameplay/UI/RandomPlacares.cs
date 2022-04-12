using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlacares : MonoBehaviour
{
    [SerializeField] List<GameObject> m_placares;

    public void InstanciarPlacar()
    {
        Instantiate(m_placares[Random.Range(0, m_placares.Count)].gameObject, GameObject.Find("Canvas").transform.GetChild(2));
    }

}
