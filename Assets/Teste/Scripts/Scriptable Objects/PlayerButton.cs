using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tipo", menuName = "ScriptableObjects/Botao")]
public class PlayerButton : ScriptableObject
{
    public enum TipoBotao { Jogador, Goleiro}
    public enum Raridade { None, Verde, Azul, Roxo, Amarelo, Laranja }
    public string nome;
    public int cod;
    public TipoBotao tipoBotao;

    public Sprite m_locked;
    public Sprite m_imagem;

    public GameObject m_formato;
    public MeshCollider m_collider;

    public Sprite m_cor1;
    public Sprite m_cor2;
    public Sprite m_cor3;
    public bool m_adesivo;
    public bool m_podeAdesivo;
    public bool m_exclusivoMetal;

    public Raridade raridade;
    public int m_tipo;
    public int m_levelDesbloquear;
    public float m_rotacao;
    
}
