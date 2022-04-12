using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/saves.AGSD";

        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/saves.AGSD";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;

            /*if (stream.Length == 0) return null;
            else
            {
                
            }*/
            
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveConfigurations(Configuration config)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/configuration.saves";

        FileStream stream = new FileStream(path, FileMode.Create);

        ConfigurationData data = new ConfigurationData(config);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static ConfigurationData LoadConfiguration()
    {
        string path = Application.persistentDataPath + "/configuration.saves";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ConfigurationData data = formatter.Deserialize(stream) as ConfigurationData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void CarregarData()
    {
        PlayerData data = LoadPlayer();

        if (data != null) GameManager.Instance.m_usuario.AtualizarData(data);
        else { SavePlayer(GameManager.Instance.m_usuario); Debug.Log("Novo Salvamento Data"); }

        if (GameManager.Instance.m_menu)
        {
            GameObject botao = GameObject.Find("Player Botao");
            GameObject goleiro = GameObject.Find("Player Goleiro");

            GameManager.Instance.SetarScripts();

            GameManager.Instance.m_playerEditionManager.SetarVariaveis(); //player custom
            GameManager.Instance.m_teamEditionManager.SetarVariaveis(); //team custom
            GameManager.Instance.m_configurationManager.SetarVariaveis(); //config 
            GameManager.Instance.m_levelManager.SetarVariaveis(); //level

            GameManager.Instance.m_playerEditionManager.InicializarBotoesSpritesDoMenu(goleiro);
            //Debug.Log("Data: goleiro");

            GameManager.Instance.m_playerEditionManager.InicializarBotoesSpritesDoMenu(botao);
            botao.transform.GetChild(0).gameObject.SetActive(GameManager.Instance.m_usuario.m_usarLogo);
            //Debug.Log("Data: botão");

            GameManager.Instance.m_playerEditionManager.AtualizarShapeMenuBotoes();
            //Debug.Log("Data: shapes");

            PlayerButton p = Resources.Load("Testes/Scriptable Objects/Botoes/Tipo " + GameManager.Instance.m_usuario.m_tipoPlayerBotaoOnline.ToString()) as PlayerButton;
            //Debug.Log("Data: adesivo");

            GameManager.Instance.m_playerEditionManager.m_adesivoEscolhido = GameManager.Instance.m_usuario.m_codigoAdesivo;
            GameManager.Instance.m_playerEditionManager.MudarMateriais(botao.GetComponent<Renderer>(), p);
            //Debug.Log("Data: adesivo material");


            GameManager.Instance.m_statsManager.AtualizarStats(GameManager.Instance.m_usuario.m_vitorias,
                GameManager.Instance.m_usuario.m_derrotas, GameManager.Instance.m_usuario.m_empates);
            //Debug.Log("Data: stats");
            GameManager.Instance.m_statsManager.AtualizarLevel(GameManager.Instance.m_usuario.m_xp);
            //Debug.Log("Data: xp");

            GameManager.Instance.m_logoManager.m_tipoLogo = GameManager.Instance.m_usuario.m_tipoBaseLogo;
            GameManager.Instance.m_logoManager.MudarSpriteStats(GameManager.Instance.m_usuario.m_baseLogo, 
                GameManager.Instance.m_usuario.m_fundoLogo, GameManager.Instance.m_usuario.m_simboloLogo, GameManager.Instance.m_usuario.m_bordaLogo);
            //Debug.Log("Data: Sprite Stats");

            GameManager.Instance.m_logoManager.AtualizarCor();
            
            Debug.Log("Data Carregada com Sucesso");
        }
    }

    public static void CarregarConfiguration()
    {
        ConfigurationData configData = SaveSystem.LoadConfiguration();

        if (configData != null) GameManager.Instance.m_config.AtualizarConfigs(configData);
        else { SaveConfigurations(GameManager.Instance.m_config); Debug.Log("Novo Salvamento Configuracoes"); }
    }
     
}
