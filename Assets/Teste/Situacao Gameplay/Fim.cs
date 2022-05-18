using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fim : Situacao
{
    public Fim(Gameplay gameplay, VariaveisUIsGameplay ui, CamerasSettings camera) : base(gameplay, ui, camera)
    {
    }

    public override IEnumerator Inicio()
    {
        VerQuemGanhou();
        DeterminarXP();

        LoadManager.Instance.CenaMenu();
        yield break;
    }

    void DeterminarXP()
    {
        if (_gameplay.conexaoPartida == Partida.Conexao.OFFLINE)
        {
            if (_gameplay.quitou)
            {

            }
            else
            {
                GameManager.Instance.setAumentarXP(true);
                if (_gameplay.modoPartida == Partida.Modo.JOGADO_VERSUS_JOGADOR)
                {
                    GameManager.Instance.setQntXP(200);
                }
                else
                {
                    if (LogisticaVars.empate) GameManager.Instance.setQntXP(GameManager.Instance.m_partida.getXP() / 1.25f);
                    else
                    {
                        if (LogisticaVars.j1Ganhou) GameManager.Instance.setQntXP(GameManager.Instance.m_partida.getXP());
                        else GameManager.Instance.setQntXP(GameManager.Instance.m_partida.getXP() / 1.75f);
                    }
                }
            }
        }
        else
        {
            //ver se alguem quitou, e se quitou puni-lo e recompensar quem estava jogando com metade do xp
            //case ninguem quitou, distribuir os xp para os jogadores
            //quem ganhou adquire mais xp que o normal 
        }
    }
    void VerQuemGanhou()
    {
        LogisticaVars.j1Ganhou = LogisticaVars.j2Ganhou = LogisticaVars.empate = false;

        if(LogisticaVars.placarT1 > LogisticaVars.placarT2)
        {
            LogisticaVars.j1Ganhou = true;
        }
        else if(LogisticaVars.placarT1 == LogisticaVars.placarT2)
        {
            LogisticaVars.empate = true;
        }
        else
        {
            LogisticaVars.j2Ganhou = true;
        }
    }
}
