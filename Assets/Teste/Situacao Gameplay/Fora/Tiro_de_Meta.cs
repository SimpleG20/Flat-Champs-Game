using System.Collections;
using Cinemachine;
using UnityEngine;

public class Tiro_de_Meta : Fora
{
    public Tiro_de_Meta(Gameplay gameplay, VariaveisUIsGameplay ui, CamerasSettings camera) : base(gameplay, ui, camera)
    {
    }

    public override IEnumerator Inicio()
    {
        base.Inicio();
        if(LogisticaVars.fundo1 && LogisticaVars.ultimoToque != 1)
        {
            LogisticaVars.vezJ1 = true;
            LogisticaVars.vezJ2 = false;
        }
        if (LogisticaVars.fundo2 && LogisticaVars.ultimoToque != 2)
        {
            LogisticaVars.vezJ2 = true;
            LogisticaVars.vezJ1 = false;
        }

        Camera_Situacao("habilitar cam lateral");

        yield return new WaitForSeconds(1);
        _gameplay.Spawnar("tiro de meta");
    }

    public override IEnumerator Spawnar(string lado)
    {
        LogisticaVars.tiroDeMeta = true;
        yield return new WaitForSeconds(0.75f);
        if (lado == "fundo 2")
        {
            Debug.Log("TIRO DE META: Goleiro 2");

            LogisticaVars.goleiroT2 = true;
            LogisticaVars.m_goleiroGameObject = GameObject.FindGameObjectWithTag("Goleiro2");

            LogisticaVars.m_goleiroGameObject.transform.position = 
                new Vector3(_gameplay._bola.m_posicaoFundo.x, LogisticaVars.m_goleiroGameObject.transform.position.y, _gameplay._bola.m_posicaoFundo.z + 3);
            LogisticaVars.fundo2 = true;
        }
        if (lado == "fundo 1")
        {
            Debug.Log("TIRO DE META: Goleiro 1");
            
            LogisticaVars.goleiroT1 = true;
            LogisticaVars.m_goleiroGameObject = GameObject.FindGameObjectWithTag("Goleiro1");

            LogisticaVars.m_goleiroGameObject.transform.position = 
                new Vector3(_gameplay._bola.m_posicaoFundo.x, LogisticaVars.m_goleiroGameObject.transform.position.y, _gameplay._bola.m_posicaoFundo.z - 3);

            LogisticaVars.fundo1 = true;
        }

        yield return new WaitForSeconds(1.25f);
        _gameplay._bola.m_toqueChao = false;
        Camera_Situacao("desabilitar cam lateral");
        GoleiroMetodos.ComponentesParaGoleiro(true);
        _gameplay._bola.RedirecionarGoleiros();

        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !_camera.GetPrincipal().IsBlending);
        EstadoJogo.JogoNormal();
        EventsManager.current.OnFora("rotina tempo tiro de meta"); //RotinasGameplay
        UI_Meio();
        _camera.MudarBlendCamera(CinemachineBlendDefinition.Style.EaseInOut);

        yield return new WaitUntil(() => /*esperar chute do goleiro*/false);
        Fim();
    }
    public override void UI_Meio()
    {
        _ui.EstadoBotoesJogador(false);
        _ui.EstadoBotoesGoleiro(true);
        _ui.selecionarJogadorBt.gameObject.SetActive(false);
        _ui.barraChuteGoleiro.SetActive(true);
    }

}
