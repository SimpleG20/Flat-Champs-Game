using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SituacaoToggle : MonoBehaviour
{
    public enum Situacao { Selecionado, NaoSelecionado}
    public Situacao tipo;
    bool estado;


    public void AtualizarEstadoToggle()
    {
        switch (tipo)
        {
            case Situacao.Selecionado:
                estado = true;
                break;
            case Situacao.NaoSelecionado:
                estado = false;
                break;
        }
        Estado(estado);
    }

    void Estado(bool b)
    {
        if (GetComponent<Toggle>() != null) GetComponent<Toggle>().isOn = b;
        else transform.GetChild(0).gameObject.SetActive(b); //Caso nao tenha toggle ele busca o check para ativar ou desativar
    }
    
}
