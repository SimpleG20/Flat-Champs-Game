using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesenharPrevisaoChute : MonoBehaviour
{
    [SerializeField] LineRenderer linerenderer;
    float vertexCount;
    int vetor;

    Transform Point1, Point3;
    [SerializeField] public Transform Point2;

    List<Vector3> caminhoLivre;

    FisicaBola bola;
    InputManager input;

    private void Start()
    {
        bola = FindObjectOfType<FisicaBola>();
        input = FindObjectOfType<InputManager>();

        Point1 = bola.transform;
        if (LogisticaVars.especial) Point3 = GameObject.FindGameObjectWithTag("Direcao Especial").transform;
        else print("Trajetoria para outra ocasiao");
        Point2.position = (Point3.position - Point1.position) / 2;
        vertexCount = 12;
    }

    void Update()
    {
        if (!LogisticaVars.aplicouEspecial)
        {
            if (!GameManager.Instance.m_jogadorAi)
            {
                if (FindObjectOfType<MiraEspecial>().MiraTravada())
                {
                    if (MovimentacaoDoJogador.pc) Point2.Translate(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * Time.deltaTime * 2);
                    else Point2.Translate(new Vector3(input.direcaoRight.x, input.direcaoRight.y, 0) * Time.deltaTime * 2);
                }
            }

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
            else erro = new Vector3(erroX / 1.5f, erroY, 0);

            caminhoLivre[i] += erro;
        }
    }

    IEnumerator PercorrerCaminho(List<Vector3> caminho)
    {
        float step = 0;

        yield return new WaitForSeconds(0.01f);
        step += 2.5f;
        bola.transform.position = Vector3.MoveTowards(bola.transform.position, caminho[vetor], step);
        if (bola.transform.position == caminho[caminho.Count - 1]) { /*EventsManager.current.SituacaoGameplay("fim especial");*/ Destroy(gameObject); }
        else
        {
            if (bola.transform.position == caminho[vetor]) vetor++;
        }
    }
}
