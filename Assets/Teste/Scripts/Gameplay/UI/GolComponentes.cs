using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GolComponentes : MonoBehaviour
{
    Color corFundo;
    [SerializeField] Image back, sombraLogo, baseLogo, segundaBaseLogo, simboloLogo;
    [SerializeField] TextMeshProUGUI numGolT1, numGolT2;

    // Start is called before the first frame update
    void Start()
    {
        EventsManager.current.onAtualizarNumeros += Atualizar;
    }

    public void SpriteTimeMarcou(Sprite b, Sprite b2, Sprite s, Color primeira, Color segunda, Color terceira)
    {
        sombraLogo.sprite = b;
        baseLogo.sprite = b;
        baseLogo.color = primeira;
        segundaBaseLogo.sprite = b2;
        segundaBaseLogo.color = segunda;
        simboloLogo.sprite = s;
        simboloLogo.color = terceira;
    }

    public void Atualizar()
    {
        if (LogisticaVars.placarT1 < 10) numGolT1.text = "0" + LogisticaVars.placarT1.ToString();
        else numGolT1.text = LogisticaVars.placarT1.ToString();

        if (LogisticaVars.placarT2 < 10) numGolT2.text = "0" + LogisticaVars.placarT2.ToString();
        else numGolT2.text = LogisticaVars.placarT2.ToString();
    }

    public void SetarCor(Color cor)
    {
        corFundo = cor;
        back.color = corFundo;
    }
}
