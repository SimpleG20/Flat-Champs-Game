using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Adesivo", menuName = "ScriptableObjects/Adesivos")]
public class Adesivos : ScriptableObject
{
    public enum Raridade { None, Verde, Azul, Roxo, Amarelo, Laranja }
    public string nome;
    public Sprite m_locked;
    public Sprite m_imagem;
    public Material m_material;

    public Raridade raridade;
    public float m_tamanhoNoSlot;
    public int m_cod;
    public int m_levelDesbloquear;
    public bool m_colorir;
}
