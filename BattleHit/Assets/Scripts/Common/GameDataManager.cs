using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameDataManager : MonoBehaviour
{
    private static GameDataManager instance;
    private static GameObject container;
    public static GameDataManager Instance()
    {
        if (!instance)
        {
            container = new GameObject();
            container.name = "GameDataManager";
            instance = container.AddComponent(typeof(GameDataManager)) as GameDataManager;
        }
        return instance;
    }

    // variable

    private int m_iScenarioStep = 0;
    public int iScenarioStep
    {
        set { m_iScenarioStep = value; }
        get { return m_iScenarioStep; }
    }


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Duplicate GameMain");
        }
        
        DontDestroyOnLoad(this);
    }

    public void LoadSaveFile()
    {
        // 읽어온 세이브 파일로 셋팅
        m_iScenarioStep = 0;
    }
}