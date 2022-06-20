using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using Cinemachine.PostFX;

public class CamerasSettings : MonoBehaviour
{
    bool camEspera;
    [SerializeField] CinemachineBrain camPrincipal;
    [SerializeField] GameObject lateralEsq, lateralDir, tiroDeMeta, torcida, espera, follow;
    [SerializeField] VolumeProfile blur;

    public static CamerasSettings _current;

    private void Awake()
    {
        _current = this;
    }


    public CinemachineBrain GetPrincipal()
    {
        return camPrincipal;
    }

    public void SituacoesCameras(string situacao)
    {
        switch (situacao)
        {
            case "acionar camera espera":
                AcionarCameraDeEspera();
                break;
            case "gol marcado":
                AcionarCameraTorcida();
                break;
            case "habilitar cam lateral":
                AcionarCameraLateral();
                break;
            case "desabilitar cam lateral":
                DesabilitarCamerasMenosJogador();
                break;
            case "habilitar camera tiro de meta":
                AcionarCameraTiroDeMeta();
                break;
            case "desabilitar camera tiro de meta":
                DesabilitarCamerasMenosJogador();
                break;
            case "acionar camera especial":
                AcionarCameraEspecial();
                break;
            case "fim especial":
                RetirarNoise(LogisticaVars.cameraJogador);
                break;
            case "desabilitar camera espera":
                DesabilitarCameraEspera();
                break;
            case "desabilitar camera torcida":
                torcida.GetComponent<CinemachineVirtualCamera>().m_Priority = 0;
                break;
            case "somente camera jogador":
                DesabilitarCamerasMenosJogador();
                break;
        }
    }

    public void MudarBlendCamera(CinemachineBlendDefinition.Style blend)
    {
        camPrincipal.m_DefaultBlend.m_Style = blend;
    }

    #region Acrescentar ou Retirar Modificacoes
    
    void RetirarNoise(CinemachineVirtualCamera cam)
    {
        cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
    }
    void EditarNoise(CinemachineVirtualCamera cam, float amplitude = default, float frequency = default)
    {
        cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;
        cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequency;
    }
    public void AplicarBlur(CinemachineVirtualCamera cam)
    {
        if (cam.gameObject.GetComponent<CinemachineVolumeSettings>() == null) cam.gameObject.AddComponent<CinemachineVolumeSettings>().m_Profile = blur;
        else cam.gameObject.GetComponent<CinemachineVolumeSettings>().m_Profile = blur;
    }
    public void RetirarBlur(CinemachineVirtualCamera cam)
    {
        cam.gameObject.GetComponent<CinemachineVolumeSettings>().m_Profile = null;
    }
    #endregion

    #region Acionar Cameras
    void AcionarCameraDeEspera()
    {
        camEspera = true;
        MudarBlendCamera(CinemachineBlendDefinition.Style.HardOut);

        if(Gameplay._current.conexaoPartida == Partida.Conexao.OFFLINE)
        {
            espera.GetComponent<CinemachineVirtualCamera>().m_LookAt = Gameplay._current._bola.transform;
            espera.GetComponent<CinemachineVirtualCamera>().AddCinemachineComponent<CinemachineHardLookAt>();
            espera.GetComponent<CinemachineVirtualCamera>().m_Priority = 101;
        }
        else
        {
            //Depois
        }

        StartCoroutine(EsperarTransicaoCameraEspera());
    }
    void AcionarCameraTorcida()
    {
        EditarNoise(torcida.GetComponent<CinemachineVirtualCamera>(), 1.5f, 1.5f);
        MudarBlendCamera(CinemachineBlendDefinition.Style.HardIn);
        torcida.GetComponent<CinemachineVirtualCamera>().m_Priority = 101;
    }
    void AcionarCameraLateral()
    {
        float xCampo = Gameplay._current.dimensoesCampo.TamanhoCampo().x / 2;
        float zCampo = Gameplay._current.dimensoesCampo.TamanhoCampo().y;
        Vector3 target = new Vector3(Gameplay._current._bola.transform.position.x * 3 / 4, Gameplay._current._bola.transform.position.y, Gameplay._current._bola.transform.position.z);
        follow.transform.position = target;

        MudarBlendCamera(CinemachineBlendDefinition.Style.HardOut);

        if (Gameplay._current._bola.transform.position.z > zCampo / 4 || Gameplay._current._bola.transform.position.z < -zCampo / 4)
        {
            if (Gameplay._current._bola.transform.position.x > 0)
            {
                lateralDir.transform.GetChild(1).GetComponent<CinemachineVirtualCamera>().m_Priority = 101;

                if (Gameplay._current._bola.transform.position.z > 0) lateralDir.transform.GetChild(1).transform.position = new Vector3(xCampo + 8, 13, 37.5f);
                else lateralDir.transform.GetChild(1).transform.position = new Vector3(xCampo + 8, 13, -37.5f);
            }
            else
            {
                lateralEsq.transform.GetChild(1).GetComponent<CinemachineVirtualCamera>().m_Priority = 101;

                if (Gameplay._current._bola.transform.position.z > 0) lateralEsq.transform.GetChild(1).transform.position = new Vector3(-xCampo - 8, 13, 37.5f);
                else lateralEsq.transform.GetChild(1).transform.position = new Vector3(-xCampo - 8, 13, -37.5f);
            }
        }
        else
        {
            if (Gameplay._current._bola.transform.position.x > 0)
            {
                lateralDir.transform.GetChild(0).transform.position = new Vector3(xCampo + 8, 10, target.z);
                lateralDir.transform.GetChild(0).GetComponent<CinemachineVirtualCamera>().m_Priority = 101;
            }
            else
            {
                lateralEsq.transform.GetChild(0).transform.position = new Vector3(-xCampo - 8, 10, target.z);
                lateralEsq.transform.GetChild(0).GetComponent<CinemachineVirtualCamera>().m_Priority = 101;
            }
        }
    }
    void AcionarCameraTiroDeMeta()
    {
        tiroDeMeta.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = 0;
        tiroDeMeta.GetComponent<CinemachineVirtualCamera>().m_Follow = Gameplay._current._bola.transform;
        tiroDeMeta.GetComponent<CinemachineVirtualCamera>().m_LookAt = Gameplay._current._bola.transform;
        tiroDeMeta.GetComponent<CinemachineVirtualCamera>().m_Priority = 103;

        if (Gameplay._current._bola.transform.position.z > 0 && Gameplay._current._bola.transform.position.x > 0) tiroDeMeta.GetComponent<CinemachineVirtualCamera>().
                GetCinemachineComponent<CinemachineTrackedDolly>().m_Path = GameObject.Find("Caminho Camera TM Esq G2").GetComponent<CinemachinePath>();
        else if(Gameplay._current._bola.transform.position.z > 0 && Gameplay._current._bola.transform.position.x < 0) tiroDeMeta.GetComponent<CinemachineVirtualCamera>().
                GetCinemachineComponent<CinemachineTrackedDolly>().m_Path = GameObject.Find("Caminho Camera TM Dir G2").GetComponent<CinemachinePath>();
        else if (Gameplay._current._bola.transform.position.z < 0 && Gameplay._current._bola.transform.position.x < 0) tiroDeMeta.GetComponent<CinemachineVirtualCamera>().
                 GetCinemachineComponent<CinemachineTrackedDolly>().m_Path = GameObject.Find("Caminho Camera TM Esq G1").GetComponent<CinemachinePath>();
        else tiroDeMeta.GetComponent<CinemachineVirtualCamera>().
                 GetCinemachineComponent<CinemachineTrackedDolly>().m_Path = GameObject.Find("Caminho Camera TM Dir G1").GetComponent<CinemachinePath>();
    }
    void AcionarCameraEspecial()
    {
        //AplicarNoise(LogisticaVars.cameraJogador, Resources.Load("Packages/com.unity.cinemachine/Presets/Noise/6D Shake") as NoiseSettings);
        EditarNoise(LogisticaVars.cameraJogador.GetComponent<CinemachineVirtualCamera>(), 2, 0.06f);
        if (LogisticaVars.vezJ1) LogisticaVars.cameraJogador.m_LookAt = GameObject.FindGameObjectWithTag("Gol2").transform;
        else LogisticaVars.cameraJogador.LookAt = GameObject.FindGameObjectWithTag("Gol1").transform;

        LogisticaVars.cameraJogador.m_Priority = 99;
    }
    #endregion

    void DesabilitarCameraEspera()
    {
        espera.GetComponent<CinemachineVirtualCamera>().m_Priority = 0;
        camEspera = false;
    }
    void DesabilitarCamerasMenosJogador()
    {
        lateralEsq.transform.GetChild(0).GetComponent<CinemachineVirtualCamera>().m_Priority = 0;
        lateralEsq.transform.GetChild(1).GetComponent<CinemachineVirtualCamera>().m_Priority = 0;
        lateralDir.transform.GetChild(0).GetComponent<CinemachineVirtualCamera>().m_Priority = 0;
        lateralDir.transform.GetChild(1).GetComponent<CinemachineVirtualCamera>().m_Priority = 0;
        tiroDeMeta.GetComponent<CinemachineVirtualCamera>().m_Priority = 0;
        torcida.GetComponent<CinemachineVirtualCamera>().m_Priority = 0;
        espera.GetComponent<CinemachineVirtualCamera>().m_Priority = 0;
        StartCoroutine(EsperarTransicaoParaMudarBlend(CinemachineBlendDefinition.Style.Cut));
    }
    public CinemachineVirtualCamera getEspera()
    {
        return espera.GetComponent<CinemachineVirtualCamera>();
    }
    public bool getCameraEspera()
    {
        return camEspera;
    }
    public void setCameraEspera(bool b)
    {
        camEspera = b;
    }

    IEnumerator EsperarTransicaoCameraEspera()
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !camPrincipal.IsBlending);

        VariaveisUIsGameplay._current.UI_Espera();
    }
    public IEnumerator EsperarTransicaoParaMudarBlend(CinemachineBlendDefinition.Style c)
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !camPrincipal.IsBlending);
        if(LogisticaVars.cameraJogador != null) LogisticaVars.cameraJogador.m_Lens.FieldOfView = 60;
        MudarBlendCamera(c);
    }
}
