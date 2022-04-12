using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Estrelas : MonoBehaviour
{
    [SerializeField] int nivelNecessario;
    public void EstadoEstrelas()
    {
        if (GameManager.Instance.m_usuario.m_usarEstrelas && GameManager.Instance.m_usuario.m_level >= nivelNecessario) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }
}
