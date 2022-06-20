using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovimentacaoDoGoleiro : MovimentacaoJogadores
{
    GameObject goleiro1, goleiro2;

    void Start()
    {
        GoleiroVars.m_forcaGoleiro = 0;
        StartCoroutine(EsperarParaSetGoleiros());
    }

    void Update()
    {
        if(LogisticaVars.m_goleiroGameObject != null)
        {
            #region Chute
            if (GoleiroVars.m_medirChute)
            {
                if (GoleiroVars.m_forcaGoleiro >= GoleiroVars.m_maxForca) GoleiroVars.m_forcaGoleiro = GoleiroVars.m_maxForca;
                else GoleiroVars.m_forcaGoleiro +=2;

                GoleiroMetodos.EncherBarraChuteGoleiro(GoleiroVars.m_forcaGoleiro, GoleiroVars.m_maxForca);
            }
            else
            {
                if (GoleiroVars.m_aplicarChute)
                {
                    GoleiroMetodos.ChuteNormal();
                    GoleiroVars.m_aplicarChute = false;
                }
            }
            #endregion

            #region Movimentacao
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector3 dir = Vector3.zero;

            if (LogisticaVars.goleiroT1 || LogisticaVars.goleiroT2 && Gameplay._current.modoPartida == Partida.Modo.JOGADO_VERSUS_JOGADOR)
            {
                if (GoleiroVars.m_medirChute) LogisticaVars.m_goleiroGameObject.transform.Rotate(Vector3.up * GoleiroVars.m_sensibilidadeChute * Time.deltaTime * joystickManager.direcaoRight.x, Space.World);
                else LogisticaVars.m_goleiroGameObject.transform.Rotate(Vector3.up * GoleiroVars.m_sensibilidade * Time.deltaTime * joystickManager.direcaoRight.x, Space.World);
            }

            if (!LogisticaVars.tiroDeMeta)
            {
                if(LogisticaVars.goleiroT1 || LogisticaVars.goleiroT2 && Gameplay._current.modoPartida == Partida.Modo.JOGADO_VERSUS_JOGADOR)
                {
                    if (MovimentacaoDoJogador.pc) dir = new Vector3(h * GoleiroVars.m_speed * Time.deltaTime, -v * GoleiroVars.m_speed * Time.deltaTime, 0);
                    else
                    {
                        if (GoleiroVars.m_movimentar)
                            dir = new Vector3(joystickManager.valorX_Esq * GoleiroVars.m_speed * Time.deltaTime, -joystickManager.valorY_Esq * GoleiroVars.m_speed * Time.deltaTime, 0);
                    }
                    LogisticaVars.m_goleiroGameObject.transform.Translate(dir, Space.Self);
                }
            }
            #endregion
        }
        else
        {
            if (LogisticaVars.jogoComecou && !LogisticaVars.auxChuteAoGol && !LogisticaVars.especial)
            {
                if (goleiro1 != null) goleiro1.transform.position = bola.transform.position.x > 7 || bola.transform.position.x < -7 ? goleiro1.transform.position :
                        Vector3.MoveTowards(goleiro1.transform.position, new Vector3(bola.transform.position.x, goleiro1.transform.position.y, goleiro1.transform.position.z), 0.25f);

                if(goleiro2 != null) goleiro2.transform.position = bola.transform.position.x > 7 || bola.transform.position.x < -7 ? goleiro2.transform.position : 
                        Vector3.MoveTowards(goleiro2.transform.position, new Vector3(bola.transform.position.x, goleiro2.transform.position.y, goleiro2.transform.position.z), 0.25f);
            }
        }
    }

    #region Metodos para os Botoes do Goleiro
    public void BotoesGoleiro(string s)
    {
        switch (s)
        {
            case "MG: aplicar chute do goleiro":
                AplicarChuteGoleiro();
                break;
        }
    }
    public void MedirForcaDoChuteGoleiro(bool b)
    {
        GoleiroVars.m_medirChute = b;
    }
    private void AplicarChuteGoleiro()
    {
        GoleiroVars.m_aplicarChute = true;
    }
    #endregion

    IEnumerator EsperarParaSetGoleiros()
    {
        yield return new WaitUntil(() => LogisticaVars.jogoComecou);
        goleiro1 = GameObject.FindGameObjectWithTag("Goleiro1");
        goleiro2 = GameObject.FindGameObjectWithTag("Goleiro2");
        Gameplay._current.posGol1.z = goleiro1.transform.position.z;
        Gameplay._current.posGol2.z = goleiro2.transform.position.z;
    }
}
