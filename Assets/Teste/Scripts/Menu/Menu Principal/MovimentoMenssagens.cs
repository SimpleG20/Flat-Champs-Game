using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MovimentoMenssagens : MonoBehaviour
{
    [SerializeField] List<string> mensagens;
    public string mensagem;
    [SerializeField] float tempo, limite, inicio;
    int indice;

    void Start()
    {
        inicio = transform.position.x;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(limite, transform.position.y, 0), tempo);

        if (transform.position.x == limite)
        {
            indice++;
            if (indice >= mensagens.Count) indice = 0;

            GetComponent<TextMeshProUGUI>().text = mensagens[indice];
            float aux = limite;
            limite = inicio;
            inicio = aux;
        }
    }

    [ContextMenu("Mensagem")]
    void Mensagem()
    {
        mensagens.Add(mensagem);
    }
}
