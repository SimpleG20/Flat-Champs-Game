using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaginasManager : MonoBehaviour
{
    [SerializeField] int paginaAtual = 1;
    [SerializeField] bool linkarComSlotManager;
    [SerializeField] GameObject setaDir, setaEsq;
    [SerializeField] List<GameObject> m_paginas;


    public void ProximaPagina()
    {
        paginaAtual++;
        if (paginaAtual > m_paginas.Count) paginaAtual = m_paginas.Count;
        SituacaoSetas();
        SituacaoPaginas();
    }
    public void PaginaAnterior()
    {
        paginaAtual--;
        if (paginaAtual < 1) paginaAtual = 1;
        SituacaoSetas();
        SituacaoPaginas();
    }
    public void VoltarPaginaUm()
    {
        paginaAtual = 1;
        //SituacaoPaginas();
        //SituacaoSetas();
    }
    private void SituacaoPaginas()
    {
        foreach (GameObject p in m_paginas)
        {
            if (m_paginas[paginaAtual - 1] == p) p.SetActive(true);
            else p.SetActive(false);
        }
        if(FindObjectOfType<SlotsManager>() != null && linkarComSlotManager)
        {
            foreach (SlotsManager s in FindObjectsOfType<SlotsManager>())
            {
                s.InicializarSlots();
            }
        }
    }
    private void SituacaoSetas()
    {
        if (paginaAtual == 1) { setaDir.SetActive(true); setaEsq.SetActive(false); }
        else if (paginaAtual > 1 && paginaAtual < m_paginas.Count) { setaDir.SetActive(true); setaEsq.SetActive(true); }
        else { setaEsq.SetActive(true); setaDir.SetActive(false); }
    }

}
