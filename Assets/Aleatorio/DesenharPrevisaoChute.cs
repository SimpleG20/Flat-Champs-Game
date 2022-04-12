using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesenharPrevisaoChute : MonoBehaviour
{
    public GameObject bola, direcaoEspecial;

    public Transform Point1;
    public Transform Point2;
    public Transform Point3;
    public LineRenderer linerenderer;
    public float vertexCount;
    public int vetor;
    public bool chutar;

    public List<Vector3> caminhoLivre;

    private void Start()
    {
        Point1 = bola.transform;
        Point3 = direcaoEspecial.transform;
        Point2.position = (Point3.position - Point1.position) / 2;

    }

    void Update()
    {
        if (!chutar)
        {
            var pointList = new List<Vector3>();
            for (float ratio = 0; ratio <= 1; ratio += 1 / vertexCount)
            {
                var tangent1 = Vector3.Lerp(Point1.position, Point2.position, ratio);
                var tangent2 = Vector3.Lerp(Point2.position, Point3.position, ratio);
                var curve = Vector3.Lerp(tangent1, tangent2, ratio);

                pointList.Add(curve);
            }

            linerenderer.positionCount = pointList.Count;
            linerenderer.SetPositions(pointList.ToArray());

            caminhoLivre = pointList;
            ArrumarTrajetoria();
        }
        else
        {
            bola.GetComponent<Rigidbody>().useGravity = false;
            StartCoroutine(PercorrerCaminho(caminhoLivre));
            if (Point2.position.x > 1) bola.transform.Rotate(Vector3.down * 270 * Time.deltaTime);
            else if (Point2.position.x < -1) bola.transform.Rotate(Vector3.up * 270 * Time.deltaTime);
        }

        
    }

    void ArrumarTrajetoria()
    {
        for(int i = 0; i < caminhoLivre.Count;i++)
        {
            Vector3 erro;
            float erroX = Random.value * Mathf.Pow(-1, Random.Range(1,3));
            float erroY = Random.value * Mathf.Pow(-1, Random.Range(1, 3));

            if (Point2.position.y < 2) erroY = 0;

            if (i < 2 * caminhoLivre.Count / 3) erro = new Vector3(0, 0, 0);
            else erro = new Vector3(erroX / 2, erroY, 0);

            caminhoLivre[i] += erro;
        }
    }

    IEnumerator PercorrerCaminho(List<Vector3> caminho)
    {
        float step = 0;

        yield return new WaitForSeconds(0.01f);
        step += 1.25f;
        bola.transform.position = Vector3.MoveTowards(bola.transform.position, caminho[vetor], step);
        if (bola.transform.position == caminho[caminho.Count - 1]) chutar = false;
        else
        {
            if (bola.transform.position == caminho[vetor]) vetor++;
        }
    }
}
