using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtivosDefault : MonoBehaviour
{
    [SerializeField] List<GameObject> ativos, desativados;

    public void SetarDefault()
    {
        if (ativos != null)
        {
            foreach (GameObject g in ativos)
            {
                if(g != null) g.SetActive(true);
            }
        }

        if (desativados != null)
        {
            foreach (GameObject g in desativados)
            {
                if(g != null) g.SetActive(false);
            }
        }

        if(GetComponent<PaginasManager>() != null ) GetComponent<PaginasManager>().VoltarPaginaUm();

        /*if(FindObjectsOfType<SituacaoToggle>() != null)
        {
            foreach (SituacaoToggle t in FindObjectsOfType<SituacaoToggle>())
            {
                t.AtualizarEstadoToggle();
            }
        }*/
    }
}
