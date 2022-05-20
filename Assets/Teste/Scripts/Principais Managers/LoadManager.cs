using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{
    public GameObject m_loadingScreen;
    public Slider m_barraLoad;
    public TextMeshProUGUI m_loadingText;
    public int m_cenaAtual;

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    private static LoadManager m_Instance;
    public static LoadManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                if (FindObjectOfType<LoadManager>() == null)
                {
                    GameObject loadManager = Instantiate(Resources.Load<GameObject>("LoadManager"));
                    m_Instance = loadManager.GetComponent<LoadManager>();
                }
                else
                {
                    m_Instance = FindObjectOfType<LoadManager>();
                }
            }
            return m_Instance;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this);
        m_loadingScreen.gameObject.SetActive(true);
        scenesLoading.Add(SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive));
        StartCoroutine(GetSceneLoadProgress());
    }

    public void CenaEstadio()
    {
        m_loadingScreen.gameObject.SetActive(true);
        m_cenaAtual = 2;
        scenesLoading.Add(SceneManager.UnloadSceneAsync(1));
        scenesLoading.Add(SceneManager.LoadSceneAsync(m_cenaAtual, LoadSceneMode.Additive));
        StartCoroutine(GetSceneLoadProgress());
    }

    public void CenaMenu()
    {
        m_loadingScreen.gameObject.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync(m_cenaAtual, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects));
        scenesLoading.Add(SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive));
        m_cenaAtual = 1;
        StartCoroutine(GetSceneLoadProgress());
        GameManager.Instance.m_menu = true;
    }

    float totalSceneProgress;
    public IEnumerator GetSceneLoadProgress()
    {
        for(int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                totalSceneProgress = 0;
                foreach(AsyncOperation operation in scenesLoading)
                {
                    totalSceneProgress += operation.progress;
                }

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100f;

                m_barraLoad.value = Mathf.RoundToInt(totalSceneProgress);

                m_loadingText.text = "Loading " + Mathf.RoundToInt(totalSceneProgress) ;

                yield return null;
            }
        }


        if(m_cenaAtual == 1) GameManager.Instance.Menu();
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(m_cenaAtual));
        m_loadingScreen.gameObject.SetActive(false);
        scenesLoading.RemoveRange(0,scenesLoading.Count);

        if (GameManager.Instance.getOnAumentarXP())
        {
            print("ANIMACAO BARRA XP");
            float xpAnterior = GameManager.Instance.m_usuario.m_xp;
            GameManager.Instance.AumentarBarraXP(xpAnterior, GameManager.Instance.getXP_ParaAumentar());
            GameManager.Instance.m_sceneManager.BotoesMenuInterativos(false);
        }
    }
}
