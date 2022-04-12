using UnityEngine;

public class CrowdMovement : MonoBehaviour
{
    public bool golT1, golT2, acionouTempo;
    public float tempo, seno, modulo;
    public Vector3 posInicial;

    private void Start()
    {
        posInicial = gameObject.transform.position;
    }

    void Update()
    {
        if (golT1)
        {
            seno = Mathf.Sin(tempo);
            modulo = Mathf.Sqrt(Mathf.Pow(seno, 2));

            if (tempo > Mathf.PI) tempo = 0;
            tempo += Time.deltaTime * 5;

            if (tempo > (Mathf.PI / 2)) seno = -seno;
            else seno = modulo;

            if(this.gameObject.tag == "Torcida1")
            {
                transform.position = new Vector3(posInicial.x, posInicial.y + seno, posInicial.z);
            }
            if(this.gameObject.tag == "TorcidaAleatoria")
            {
                int i = Random.Range(0, 9);
                if (i == 1) transform.position = new Vector3(posInicial.x, posInicial.y + seno, posInicial.z);
            }
        }
        else
        {
            transform.position = posInicial;
            tempo = 0;
            seno = modulo = 0;
        }
        

    }
}
