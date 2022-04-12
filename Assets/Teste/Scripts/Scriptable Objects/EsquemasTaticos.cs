using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Esquema Novo", menuName = "ScriptableObjects/Esquemas")]
public class EsquemasTaticos : ScriptableObject
{
    public enum ModoJogo { Classico, Rapido, TresContra}
    public string nome;
    public int numAta;
    public int numMeia;
    public int numDef;
    public ModoJogo modoJogo;

}
