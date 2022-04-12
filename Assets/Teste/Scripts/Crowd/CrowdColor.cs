using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdColor : MonoBehaviour
{
    [SerializeField] private List<Material> _bodyColors;
    [SerializeField] private List<Material> _headColors;

    void Awake()
    {
        //Set Randomly colors to crowd 
        SetColors();
    }

    void SetColors()
    {
        float a = Random.Range(100, 126);
        foreach (GameObject t in GameObject.FindGameObjectsWithTag("Torcida1"))
        {
            ColorirAjustarRotacao(a, t, 1);
        }
        foreach (GameObject t in GameObject.FindGameObjectsWithTag("Torcida2"))
        {
            ColorirAjustarRotacao(a, t, 2);
        }
        foreach (GameObject t in GameObject.FindGameObjectsWithTag("TorcidaAleatoria"))
        {
            ColorirAjustarRotacao(a, t, 0);
        }
    }

    private void ColorirAjustarRotacao(float a, GameObject t, int ale)
    {
        int i = Random.Range(0, _bodyColors.Count);
        int j = Random.Range(0, _headColors.Count);
        switch (ale)
        {
            case 0:
                t.GetComponent<MeshRenderer>().materials[0].color = _bodyColors[i].color;
                t.GetComponent<MeshRenderer>().materials[1].color = _headColors[j].color;
                break;
            case 1:
                t.GetComponent<MeshRenderer>().materials[0].color = _bodyColors[7].color;
                t.GetComponent<MeshRenderer>().materials[1].color = _headColors[j].color;
                break;
            case 2:
                t.GetComponent<MeshRenderer>().materials[0].color = _bodyColors[8].color;
                t.GetComponent<MeshRenderer>().materials[1].color = _headColors[j].color;
                break;
        }
        
        t.transform.localScale = new Vector3(a, a, a);
        Vector3 vetorPosicao = (Vector3.zero - t.transform.position).normalized;
        float angulo;

        if (t.transform.position.z < 0) angulo = 360 - Mathf.Rad2Deg * Mathf.Acos(vetorPosicao.x / vetorPosicao.magnitude);
        else angulo = Mathf.Rad2Deg * Mathf.Acos(vetorPosicao.x / vetorPosicao.magnitude);

        t.transform.localEulerAngles = new Vector3(-90, 0, angulo + 90);
    }
}
