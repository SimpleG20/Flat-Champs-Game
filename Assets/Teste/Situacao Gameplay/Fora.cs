using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fora : Situacao
{
    public Fora(Gameplay gameplay) : base(gameplay)
    { }

    public override IEnumerator Inicio()
    {
        //  Verificar se nao eh gol
        //  Modificar os valores do Chute, maxForca etc..
        //
        //  Pausar o tempo de jogada, e o tempo de Jogo
        //
        //  Verificar quem foi o ultimo a tocar na bola, e se o numero de jogadas eh zerado ou nao (caso troque de Vez)
        //  Caso nao troque se o numero de jogadas igual 2 subtrair 1 e caso o tempo de jogada maior que 15s por 14s
        //  Trocar a vez caso necessario
        //  Desabilitar Dados do Jogador
        //  Mudar UI
        //  
        //
        Debug.Log("Fora");
        return base.Inicio();
    }

    public override void UI_Situacao()
    {
        //UIs setActive(false)
        base.UI_Situacao();
    }

    public virtual IEnumerator Spawnar(string lado)
    {
        yield break;
    }
}
