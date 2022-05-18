using UnityEngine;

[CreateAssetMenu(fileName = "Partida", menuName = "ScriptableObjects/Partidas")]
public class Partida : ScriptableObject
{
    public enum Tipo { CLASSICO, QUICK, VERSUS3_TIME, VERSUS3_INDIVIDUAL, VERSUS1}
    public enum Conexao { OFFLINE, ONLINE}
    public enum Modo { JOGADO_VERSUS_JOGADOR, JOGADOR_VERSUS_AI}

    Tipo m_tipo;
    Conexao m_conexao;
    Modo m_modo;

    bool m_partidaConcluida;
    bool m_partidaQuitada;
    bool m_perdaDeConexao;

    float m_XP;

    #region Set e Get
    public Tipo getTipo()
    {
        return m_tipo;
    }
    public Modo getModo()
    {
        return m_modo;
    }
    public Conexao getConexao()
    {
        return m_conexao;
    }
    public bool getPerdaConexao()
    {
        return m_perdaDeConexao;
    }
    public bool getQuitada()
    {
        return m_partidaQuitada;
    }
    public bool getConcluida()
    {
        return m_partidaConcluida;
    }
    public float getXP()
    {
        if (m_XP == 0) setXPEsperado();
        //Debug.Log(m_XP);
        return m_XP;
    }

    public void setTipo(Tipo tipo)
    {
        m_tipo = tipo;
    }
    public void setConexao(Conexao conexao)
    {
        m_conexao = conexao;
    }
    public void setModo(Modo modo)
    {
        m_modo = modo;
    }

    public void setPerdaConexao(bool b)
    {
        m_perdaDeConexao = b;
    }
    public void setQuitada(bool b)
    {
        m_partidaQuitada = b;
    }
    public void setConcluida(bool b)
    {
        m_partidaConcluida = b;
    }
    public void setXPEsperado()
    {
        m_XP = ProjecaoXP(m_tipo, m_conexao);
    }
    #endregion

    float ProjecaoXP(Tipo tipo, Conexao conexao)
    {
        float retorno;
        switch (tipo)
        {
            case Tipo.CLASSICO:
                retorno = 350;
                break;
            case Tipo.QUICK:
                retorno = 315;
                break;
            case Tipo.VERSUS3_TIME:
                retorno = 250;
                break;
            case Tipo.VERSUS3_INDIVIDUAL:
                retorno = 250;
                break;
            case Tipo.VERSUS1:
                retorno = 200;
                break;
            default:
                retorno = 250;
                break;
        }
        if (conexao == Conexao.OFFLINE) retorno *= 1;
        else retorno *= 1.15f;

        return retorno;
    }
}
