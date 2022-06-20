using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensaoCampo : MonoBehaviour
{
    [SerializeField] float tamanhoX, tamanhoZ;
    [SerializeField] Vector3 tiroDeMetaPosG1, tiroDeMetaPosG2;
    [SerializeField] float xLateralD, xLateralE;

    public float Lateral(float x)
    {
        if (x > 0) return xLateralD;
        return xLateralE;
    }
    public Vector2 TamanhoCampo()
    {
        return new Vector2(tamanhoX, tamanhoZ);
    }
    public Vector3 PosicaoBolaTiroDeMeta(int i, bool direita)
    {
        if (i == 1)
        {
            if (direita == true) return tiroDeMetaPosG1;
            else return new Vector3(-tiroDeMetaPosG1.x, 0.55f, tiroDeMetaPosG1.z);
        }
        else
        {
            if (direita == true) return tiroDeMetaPosG2;
            else return new Vector3(-tiroDeMetaPosG2.x, 0.55f, tiroDeMetaPosG2.z);
        }
    }
}
