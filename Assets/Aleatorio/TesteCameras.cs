using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TesteCameras : MonoBehaviour
{
    Quaternion rotationAnt;
    public GameObject bola, jogador, target, direcional;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bola.GetComponent<Rigidbody>().AddForce(new Vector3(1 * Random.Range(-20, 20), 1 * Random.Range(0, 20), 1 * Random.Range(-20, 20)), ForceMode.Impulse);
        }
        
    }

    [ContextMenu("Mover")]
    void MoverJogador()
    {
        StartCoroutine(RotacionarJogadorMaisPerto(target));
    }

    IEnumerator RotacionarJogadorMaisPerto(GameObject jogadorPerto)
    {
        float step = 0;
        yield return new WaitForSeconds(0.01f);
        step += 0.05f;
        direcional.transform.position = Vector3.MoveTowards((jogador.transform.position - jogador.transform.up), jogadorPerto.transform.position, step);

        Vector3 direction = direcional.transform.position - jogador.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        jogador.transform.rotation = rotation;
        jogador.transform.eulerAngles = new Vector3(-90, jogador.transform.eulerAngles.y, jogador.transform.eulerAngles.z);

        if (rotation != rotationAnt) { rotationAnt = rotation; StartCoroutine(RotacionarJogadorMaisPerto(jogadorPerto)); }
    }
}
