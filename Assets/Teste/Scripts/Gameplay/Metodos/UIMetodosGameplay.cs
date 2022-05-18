using System.Collections;
using UnityEngine;
using Cinemachine;

public class UIMetodosGameplay : VariaveisUIsGameplay
{
    IEnumerator TransicaoBolaPequenaArea()
    {
        FindObjectOfType<CamerasSettings>().MudarBlendCamera(CinemachineBlendDefinition.Style.EaseInOut);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => !FindObjectOfType<CinemachineBrain>().IsBlending);
        EstadoBotoesJogador(false);
        EstadoBotoesGoleiro(true);
        selecionarJogadorBt.gameObject.SetActive(false);
    }
    
}
