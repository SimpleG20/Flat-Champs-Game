using UnityEngine;

public class TabelaLevel : MonoBehaviour
{
    const float inicio = 1500;

    public static float getMinXPLevel(int pos)
    {
        float min = inicio * Mathf.Pow(pos - 1, 2) * Mathf.Sqrt(pos - 1);
        return min;
    }
    public static float getMaxXPLevel(int pos)
    {
        float max = inicio * Mathf.Pow(pos, 2) * Mathf.Sqrt(pos);
        return max;
    }
}
