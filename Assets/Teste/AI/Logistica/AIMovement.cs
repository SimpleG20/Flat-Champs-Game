using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public float anguloJogador, alcanceVisao, campoVisao;

    public GameObject povAI, bola, gol, rotacaoCamera;
    Vector3 bolaPos, golPos, direcaoParaBola, chutePos;
    public LayerMask layerMask;


    int zGol;
    public float magnitudeChute;
    Quaternion rotacaoAnt;

    public GameObject jogadorAmigo_MaisPerto, jogadorInimigo_MaisPerto;
    float menorDistanciaAoGol_JogadoresAmigos = 1000, menorDistancia_JogadoresInimigos = 1000;

    public void SetMovement()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        //direcaoParaBola = (bola.transform.position - transform.position).normalized;
        bolaPos = bola.transform.position;
        golPos = gol.transform.position;
        zGol = LadoGol();
    }

    private void Update()
    {
        float dir = transform.eulerAngles.y;
        //if (transform.up.x > 0) dir = Mathf.Acos(-transform.up.z / transform.up.magnitude) * Mathf.Rad2Deg - 90;
        //else dir = Mathf.Acos(-transform.up.z / transform.up.magnitude) * Mathf.Rad2Deg + 90;
        float novoAngulo = dir + campoVisao;
        float ang = dir - campoVisao;

        Vector3 visao2 = new Vector3(Mathf.Sin(novoAngulo * Mathf.Deg2Rad), 0, Mathf.Cos(novoAngulo * Mathf.Deg2Rad));
        Vector3 visao3 = new Vector3(Mathf.Sin(ang * Mathf.Deg2Rad), 0, Mathf.Cos(ang * Mathf.Deg2Rad));
        Vector3 lateralD = new Vector3(transform.position.x - 1.7f, 0, transform.position.z);
        Vector3 lateralE = new Vector3(transform.position.x + 1.7f, 0, transform.position.z);

        Debug.DrawRay(transform.position, (visao2) * alcanceVisao, Color.green);
        Debug.DrawRay(transform.position, (visao3) * alcanceVisao, Color.green);
        Debug.DrawRay(lateralE, -transform.up * alcanceVisao, Color.green);
        Debug.DrawRay(lateralD, -transform.up * alcanceVisao, Color.green);
    }

    [ContextMenu("olhar ao redor")]
    public void OlharAoRedor()
    {
        StartCoroutine(RotacionarPOV());
    }
    [ContextMenu("Chutar")]
    void ChutarBola()
    {
        direcaoParaBola = (bolaPos - transform.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, magnitudeChute, layerMask))
        {
            //Debug.DrawRay(bolaPos, transform.forward * magnitudeChute, Color.green);
            bola.GetComponent<Rigidbody>().AddForce(new Vector3(direcaoParaBola.x, Random.Range(1f, 2f), direcaoParaBola.z) * (magnitudeChute * 3 / 2), ForceMode.Impulse);
        }
        else
        {
            bola.GetComponent<Rigidbody>().AddForce(direcaoParaBola * magnitudeChute, ForceMode.Impulse);
        }
    }


    void DecisaoAcao()
    {
        int decisao = Random.Range(0, 2);
        if (Vector3.Distance(golPos, jogadorAmigo_MaisPerto.transform.position) < Vector3.Distance(golPos, transform.position))
        {
            print("Mais longe do gol que seu amigo");

            if (decisao < 2)
            {
                print("Posicionar-se para passar a bola para o amigo");
            }
            else
            {
                print("Eu me garanto");
            }
        }
        else
        {
            print("Mais perto do gol que seu amigo");
        }

        chutePos = ObterPosicao(decisao);

        if (decisao < 2) magnitudeChute = Vector3.Distance(jogadorAmigo_MaisPerto.transform.position, chutePos);
        else magnitudeChute = Vector3.Distance(chutePos, golPos);
        print(chutePos);
        StartCoroutine(RotacionarParaPosicao(chutePos));
    }
    void LinhaDirecaoChute(float z0, float x0, float z1, float x1, float zAtual, out float xAtual)
    {
        float a = (z1 - z0) / (x1 - x0);
        print("A: " + a);
        float b = (-a * x1) + z1;
        print("B: " + b);

        if (a == Mathf.Infinity) xAtual = 0;
        else xAtual = (zAtual - b) / a;
    }


    IEnumerator RotacionarParaAlvo(Vector3 alvo)
    {
        yield return new WaitForSeconds(0.01f);
        float step = 0.8f;
        rotacaoCamera.transform.position = Vector3.MoveTowards(transform.position + transform.forward, alvo, step);

        Vector3 direction = rotacaoCamera.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
        transform.eulerAngles = new Vector3(-90, transform.eulerAngles.y, transform.eulerAngles.z);

        if (rotacaoAnt != rotation) { rotacaoAnt = rotation; StartCoroutine(RotacionarParaAlvo(alvo)); }
    }
    IEnumerator RotacionarPOV()
    {
        float step = 0;

        yield return new WaitForSeconds(0.01f);
        step += 8f;
        povAI.transform.eulerAngles += Vector3.up * step;
        anguloJogador = povAI.transform.eulerAngles.y;

        DetectarJogadores();

        if (anguloJogador > 350)
        {
            povAI.transform.localEulerAngles = Vector3.zero;
            menorDistanciaAoGol_JogadoresAmigos = 1000;
            menorDistancia_JogadoresInimigos = 1000;
            DecisaoAcao();
        }
        else StartCoroutine(RotacionarPOV());
    }
    IEnumerator RotacionarParaPosicao(Vector3 posDestino)
    {
        yield return new WaitForSeconds(0.01f);
        float step = 0.8f;
        rotacaoCamera.transform.position = Vector3.MoveTowards(transform.position + transform.forward, posDestino, step);

        Vector3 direction = rotacaoCamera.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
        transform.eulerAngles = new Vector3(-90, transform.eulerAngles.y, transform.eulerAngles.z);

        if (rotacaoAnt != rotation) { rotacaoAnt = rotation; StartCoroutine(RotacionarParaPosicao(posDestino)); }
        else
        {
            rotacaoAnt = Quaternion.identity;
            yield return new WaitForSeconds(1);
            Vector3 novo = DetectarObstaculos(posDestino);
            StartCoroutine(MoverParaPosicao(novo));
        }
    }
    IEnumerator MoverParaPosicao(Vector3 posDestino)
    {
        float distanciaDaBola = Vector3.Distance(transform.position, bolaPos);
        float vel_I = Mathf.Sqrt(distanciaDaBola * AtributosFisicos.coefAtritoDiBola * AtributosFisicos.gravidade * 2);
        float forca = GetComponent<Rigidbody>().mass * vel_I / Random.Range(0.65f, 0.75f);
        print(forca);

        //Verifica se a forca eh menor ou igual a forca maxima possivel
        
        Vector3 direcao = posDestino - transform.position;

        GetComponent<Rigidbody>().AddForce(direcao.normalized * forca, ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !GetComponent<FisicaJogador>().m_correndo);
        StartCoroutine(RotacionarParaAlvo(bolaPos));
    }


    Vector3 ObterPosicao(int i)
    {
        Vector3 posDestino = Vector3.zero;

        if (zGol > 0) posDestino.z = bolaPos.z - 2;
        else posDestino.z = bolaPos.z + 2;

        if (i < 2)
        {
            float x = bolaPos.x, x1 = jogadorAmigo_MaisPerto.transform.position.x;
            float z = bolaPos.z, z1 = zGol > 0 ? jogadorAmigo_MaisPerto.transform.position.z + 5 : jogadorAmigo_MaisPerto.transform.position.z - 5;
            //direcaoChute = (jogadorAmigo_MaisPerto.transform.position + Vector3.forward * 3) - bolaPos;

            LinhaDirecaoChute(z, x, z1, x1, posDestino.z, out posDestino.x);
        }
        else
        {
            float x = bolaPos.x, x2 = gol.transform.position.x;
            float z = bolaPos.z, z2 = gol.transform.position.z;

            LinhaDirecaoChute(z, x, z2, x2, posDestino.z, out posDestino.x);
            //direcaoChute = gol.transform.position - bolaPos;
        }
        return posDestino;
    }
    Vector3 DetectarObstaculos(Vector3 posDestino)
    {
        anguloJogador = transform.eulerAngles.y;
        float angulo1 = anguloJogador + campoVisao;
        float angulo2 = anguloJogador - campoVisao;

        Vector3 visao2 = new Vector3(Mathf.Sin(angulo1 * Mathf.Deg2Rad), 0, Mathf.Cos(angulo1 * Mathf.Deg2Rad));
        Vector3 visao3 = new Vector3(Mathf.Sin(angulo2 * Mathf.Deg2Rad), 0, Mathf.Cos(angulo2 * Mathf.Deg2Rad));
        Vector3 lateralD = new Vector3(transform.position.x - 1.7f, 0, transform.position.z);
        Vector3 lateralE = new Vector3(transform.position.x + 1.7f, 0, transform.position.z);
        Vector3 novoDestino = posDestino;

        UsarRaycasts(visao2, visao3, lateralD, lateralE);

        return novoDestino;
    }

    private void UsarRaycasts(Vector3 visao2, Vector3 visao3, Vector3 lateralD, Vector3 lateralE)
    {
        RaycastHit hit, hit2, hit3;

        if (Physics.Raycast(transform.position, -transform.up, out hit, alcanceVisao, layerMask))
        {
            print("Obstaculo");
            if (Physics.Raycast(transform.position, visao2, out hit2, alcanceVisao, layerMask))
            {
                //print("Obstaculo");
                if (Physics.Raycast(transform.position, visao3, out hit3, alcanceVisao, layerMask))
                {

                }
                else
                {
                    //print("Esquerda Livre");
                    print("Virou esquerda");
                    StartCoroutine(RotacionarParaAlvo(visao3));
                }
            }
            else
            {
                //print("Direita Livre");
                print("Virou Direita");
                StartCoroutine(RotacionarParaAlvo(visao2));
            }
        }
        else
        {
            print("Livre");
            //Se nao for chute ao gol entao mantenha a direcao
            if (LogisticaVars.auxChuteAoGol)
            {
                RaycastHit hit4;
                if (Physics.Raycast(lateralD, -transform.up, out hit4, alcanceVisao, layerMask))
                {
                    //Virar para a esquerda ate conseguir um espaco razoavel
                }
                else
                {
                    if (Physics.Raycast(lateralE, -transform.up, out hit4, alcanceVisao, layerMask))
                    {
                        //Virar para a Direita ate conseguir um espaco razoavel
                    }
                }
            }
        }
    }

    void DetectarJogadores()
    {
        Ray raioPrincipal = new Ray(povAI.transform.position, povAI.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(raioPrincipal, out hit, alcanceVisao, layerMask))
        {
            if (hit.collider.gameObject.layer == gameObject.layer)
            {
                if ((golPos - hit.collider.gameObject.transform.position).magnitude < menorDistanciaAoGol_JogadoresAmigos)
                {
                    menorDistanciaAoGol_JogadoresAmigos = (golPos - hit.collider.gameObject.transform.position).magnitude;
                    jogadorAmigo_MaisPerto = hit.collider.gameObject;
                }
            }
            else if (hit.collider.gameObject.layer == gameObject.layer - 1)
            {
                if ((hit.collider.gameObject.transform.position - transform.position).magnitude < menorDistancia_JogadoresInimigos)
                {
                    menorDistancia_JogadoresInimigos = (hit.collider.gameObject.transform.position - transform.position).magnitude;
                    jogadorInimigo_MaisPerto = hit.collider.gameObject;
                }
            }
        }
    }
    int LadoGol()
    {
        if (gol.transform.position.z > 0) return 1;
        else return -1;
    }
}
