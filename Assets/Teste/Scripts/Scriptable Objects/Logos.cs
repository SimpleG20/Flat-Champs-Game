using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Logo", menuName = "ScriptableObjects/Logos")]
public class Logos : ScriptableObject
{
    public Sprite m_locked;
    public Sprite m_tipo1;
    public Sprite m_tipo2;
    public Sprite m_tipo3;
    public Sprite m_tipo4;

    public int m_tipo;
    public int m_levelDesbloquear;
}
