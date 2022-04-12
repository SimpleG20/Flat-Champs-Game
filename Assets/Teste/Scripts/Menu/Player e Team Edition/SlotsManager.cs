using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotsManager : MonoBehaviour
{
    [SerializeField] List<GameObject> slotsScroll;
    //float maskWidth;
    //float maskHeight, diferencaHeight, diferencaWidth;
    //[SerializeField] GameObject handle;
    //[SerializeField] Image mask;
    //[SerializeField] public ScrollRect scroll;
    //Vector3 posInicialHandle, limite;

    [Header("Listas")]
    public List<GameObject> slotsParaMetodosNoEditor;
    public List<Adesivos> listaAdesivos;
    public List<PlayerButton> listaFormatos;
    public List<Logos> listaLogos;


    [Header("Adicionar Slots")]
    public GameObject pai;
    public GameObject prefab;
    public int numeroDeSlotsDesejadosX, numeroDeSlotsDesejadosY;

    [Header("Posicao")]
    public Vector3 posInicial;
    public Vector3 posFinal;
    public float distanciaAcrescentadaX, distanciaAcrescentadaY;

    /*void Start()
    {
        InicializarSlots();
    }*/

    #region Metodos Editor
    [ContextMenu("SetarPosciaoInicial")]
    public void SetarPosFinal()
    {
        posInicial = prefab.transform.position;
        posFinal = prefab.transform.position;
    }

    [ContextMenu("instaciarSlots")]
    private void InstanciarSlots()
    {

        for (int i = 1; i <= numeroDeSlotsDesejadosY; i++)
        {
            for (int j = 1; j <= numeroDeSlotsDesejadosX; j++)
            {
                Instantiate(prefab, posFinal, Quaternion.identity, pai.transform);
                posFinal = new Vector3(posInicial.x + (distanciaAcrescentadaX * j), posInicial.y + (distanciaAcrescentadaY * i), 0);
            }
        }
    }

    [ContextMenu("Colocar Script Adesivo")]
    private void ColocarScriptAdesivo()
    {
        HeapSortAdesivos(listaAdesivos);
        for (int i = 0; i < slotsParaMetodosNoEditor.Count; i++)
        {
            if (slotsParaMetodosNoEditor[i] == null)
            {
                print("Criar slot para adicionar Scriptable");
            }
            slotsParaMetodosNoEditor[i].GetComponentInChildren<AdesivosImagemBotao>().m_adesivo = null;
            slotsParaMetodosNoEditor[i].GetComponentInChildren<AdesivosImagemBotao>().m_adesivo = listaAdesivos[i];
        }
    }

    [ContextMenu("Colocar Script Botoes")]
    private void ColocarScpritBotao()
    {
        HeapSortFormatos(listaFormatos);
        for(int i = 0; i < slotsParaMetodosNoEditor.Count; i++)
        {
            if (slotsParaMetodosNoEditor[i] == null)
            {
                print("Criar slot para adicionar Scriptable");
            }
            slotsParaMetodosNoEditor[i].GetComponentInChildren<FormatosImagemBotao>().m_botao = null;
            slotsParaMetodosNoEditor[i].GetComponentInChildren<FormatosImagemBotao>().m_botao = listaFormatos[i];
        }
    }
    
    [ContextMenu("Colocar Script Logo")]
    private void ColocarScriptLogo()
    {
        HeapSortLogos(listaLogos);
        for (int i = 0; i < slotsParaMetodosNoEditor.Count; i++)
        {
            if(slotsParaMetodosNoEditor[i] == null)
            {
                print("Criar slot para adicionar Scriptable");
            }
            slotsParaMetodosNoEditor[i].GetComponentInChildren<LogosImagemBrasao>().m_logo = null;
            slotsParaMetodosNoEditor[i].GetComponentInChildren<LogosImagemBrasao>().m_logo = listaLogos[i];
        }
    }
    #endregion

    void HeapSortAdesivos(List<Adesivos> vetor)
    {
        Adesivos t;
        int tamanho = vetor.Count;
        int i = tamanho / 2, pai, filho;
        while (true)
        {
            if (i > 0)
            {
                i--;
                t = vetor[i];
            }
            else
            {
                tamanho--;
                if (tamanho <= 0) { return; }
                t = vetor[tamanho];
                vetor[tamanho] = vetor[0];
            }
            pai = i;
            filho = ((i * 2) + 1);
            while (filho < tamanho)
            {
                if ((filho + 1 < tamanho) && (vetor[filho + 1].m_levelDesbloquear > vetor[filho].m_levelDesbloquear)) { filho++; }
                if (vetor[filho].m_levelDesbloquear > t.m_levelDesbloquear)
                {
                    vetor[pai] = vetor[filho];
                    pai = filho;
                    filho = pai * 2 + 1;
                }
                else { break; }
            }
            vetor[pai] = t;
        }
    }
    void HeapSortFormatos(List<PlayerButton> vetor)
    {
        PlayerButton t;
        int tamanho = vetor.Count;
        int i = tamanho / 2, pai, filho;
        while (true)
        {
            if (i > 0)
            {
                i--;
                t = vetor[i];
            }
            else
            {
                tamanho--;
                if (tamanho <= 0) { return; }
                t = vetor[tamanho];
                vetor[tamanho] = vetor[0];
            }
            pai = i;
            filho = ((i * 2) + 1);
            while (filho < tamanho)
            {
                if ((filho + 1 < tamanho) && (vetor[filho + 1].m_levelDesbloquear > vetor[filho].m_levelDesbloquear)) { filho++; }
                if (vetor[filho].m_levelDesbloquear > t.m_levelDesbloquear)
                {
                    vetor[pai] = vetor[filho];
                    pai = filho;
                    filho = pai * 2 + 1;
                }
                else { break; }
            }
            vetor[pai] = t;
        }
    }
    void HeapSortLogos(List<Logos> vetor)
    {
        Logos t;
        int tamanho = vetor.Count;
        int i = tamanho / 2, pai, filho;
        while (true)
        {
            if (i > 0)
            {
                i--;
                t = vetor[i];
            }
            else
            {
                tamanho--;
                if (tamanho <= 0) { return; }
                t = vetor[tamanho];
                vetor[tamanho] = vetor[0];
            }
            pai = i;
            filho = ((i * 2) + 1);
            while (filho < tamanho)
            {
                if ((filho + 1 < tamanho) && (vetor[filho + 1].m_levelDesbloquear > vetor[filho].m_levelDesbloquear)) { filho++; }
                if (vetor[filho].m_levelDesbloquear > t.m_levelDesbloquear)
                {
                    vetor[pai] = vetor[filho];
                    pai = filho;
                    filho = pai * 2 + 1;
                }
                else { break; }
            }
            vetor[pai] = t;
        }
    }

    public void InicializarSlots()
    {
        foreach (GameObject s in slotsScroll)
        {
            if (s != null)
            {
                if(s.GetComponent<ImagemsInterativas>() != null) s.GetComponent<ImagemsInterativas>().SetarSlots();
            }
        }

        /*medida.AtualizarMedida();
        if (posInicialHandle.magnitude != 0) handle.transform.position = posInicialHandle;

        maskWidth = mask.rectTransform.rect.width;
        maskHeight = mask.rectTransform.rect.height;
        diferencaHeight = medida.height - maskHeight;
        diferencaWidth = medida.width - maskWidth;

        posInicialHandle = handle.transform.position;

        if (diferencaHeight < 0 && diferencaWidth > 0) limite = posInicialHandle + new Vector3(-diferencaWidth, 0, 0);
        else if (diferencaWidth < 0 && diferencaHeight < 0) limite = posInicialHandle;
        else if (diferencaHeight > 0 && diferencaWidth < 0) limite = posInicialHandle + new Vector3(0, +diferencaHeight, 0);
        else limite = posInicialHandle + new Vector3(-diferencaWidth, +diferencaHeight, 0);*/
    }
    public void LimiteScroll()
    {
        /*if (scroll.horizontal)
        {
            if (scroll.content.transform.position.x > posInicialHandle.x) scroll.content.transform.position = posInicialHandle;
            if ((posInicialHandle - scroll.content.transform.position).magnitude > diferencaWidth) scroll.content.transform.position = limite;
        }
        else
        {
            if (scroll.content.transform.position.y < posInicialHandle.y) scroll.content.transform.position = posInicialHandle;
            if ((posInicialHandle - scroll.content.transform.position).magnitude > diferencaHeight) scroll.content.transform.position = limite;
        }*/
    }

}
