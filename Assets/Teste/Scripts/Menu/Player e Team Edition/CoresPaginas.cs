using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoresPaginas : MonoBehaviour
{
    /*public GameObject slot, pai;
    public int quantidadeX, quantidadeY, tipoCor, codigoCor;*/

    [SerializeField] List<int> numeroDisponiveis;
    [SerializeField] List<NivelCores> m_cores;


    public void ListarNumeros()
    {
        int j = 0;
        while(j < 3)
        {
            for (int i = 1; i < 75; i++)
            {
                if (i == 10 || i == 15 || i == 20 || i == 30 || i == 35 || i == 40 || i == 50 || i == 60 || i == 75) continue;
                else numeroDisponiveis.Add(i);
            }
            j++;
        }
    }
   
    [ContextMenu("Niveis das Cores")]
    public void NivelDasCores()
    {
        ListarNumeros();

        foreach(NivelCores cor in m_cores)
        {
            if (numeroDisponiveis.Count != 0)
            {
                int random2 = Random.Range(0, numeroDisponiveis.Count);
                cor.m_levelDesbloquear = numeroDisponiveis[random2];
                numeroDisponiveis.RemoveAt(random2);
            }
            else cor.m_levelDesbloquear = 1;
        }
    }

    /*
    [ContextMenu("Instanciar Slots")]
    public void InstanciarCores()
    {
        for(int i = 0; i < quantidadeY; i++)
        {
            for(int j = 0; j < quantidadeX; j++)
            {
                codigoCor++;
                Instantiate(slot, new Vector3(480 - 187.9f + (72f * j), 540 + 183 - (72 * i), 0), Quaternion.identity, pai.transform);
                slot.GetComponent<NivelCores>().m_codigoCor = codigoCor;
                slot.GetComponent<NivelCores>().m_tipoCor = tipoCor;
                slot.name = "Slot";
            }
        }
        
    }*/

}
