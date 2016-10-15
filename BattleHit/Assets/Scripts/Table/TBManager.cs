using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TBManager : MonoBehaviour
{
	private static TBManager instance;  
	private static GameObject container;
	public static TBManager Instance()  
	{  
		if( !instance )  
		{  
			container = new GameObject();
			container.name = "TBManager";
			instance = container.AddComponent(typeof(TBManager)) as TBManager;  
		}  
		return instance;  
	}  

    // ------------------------------------//

    public Dictionary<int, TB_Hero> cont_Hero = null;
    public Dictionary<int, TB_MapInfo> cont_MapInfo = null;
    public Dictionary<string, TB_Conversation> cont_String = null;

    void Awake()
	{
		DontDestroyOnLoad (this);
	}

    void LoadHeroTable()
    {
        cont_Hero = new Dictionary<int, TB_Hero>();

        StringTable st = new StringTable();

        if (false == st.Build("Table/TB_Hero")) { return; }

        int iRowCount = st.row;

        for (int x = 0; x < iRowCount; ++x)
        {
            TB_Hero tbHero = new TB_Hero();

            tbHero.mHeroNo = st.GetValueAsInt(x, "HeroNo");
            tbHero.mHP = st.GetValueAsInt(x, "HP");
            tbHero.mAtk = st.GetValueAsInt(x, "Atk");
            tbHero.mDef = st.GetValueAsInt(x, "Def");
            int iSpeed = st.GetValueAsInt(x, "Speed");
            tbHero.mSpeed = (float)iSpeed * 0.001f;
            int iBlowPower = st.GetValueAsInt(x, "BlowPower");
            tbHero.mBlowPower = (float)iBlowPower * 0.001f;
            int iBlowTolerance = st.GetValueAsInt(x, "BlowTolerance");
            tbHero.mBlowTolerance = (float)iBlowTolerance * 0.001f;
            tbHero.stResPath = st.GetValue(x, "ResPath");
            int iScale = st.GetValueAsInt(x, "Scale");
            tbHero.mScale = (float)iScale * 0.001f;

            int key = tbHero.mHeroNo;
            if (cont_Hero.ContainsKey(key))
            {
                Debug.LogError("Already exist key. " + key.ToString() );
            }

            cont_Hero.Add(key, tbHero);
        }
    }

    void LoadMapInfoTable()
    {
        cont_MapInfo = new Dictionary<int, TB_MapInfo>();

        StringTable st = new StringTable();

        if (false == st.Build("Table/TB_MapInfo")) { return; }

        int iRowCount = st.row;

        for (int x = 0; x < iRowCount; ++x)
        {
            TB_MapInfo tbMapInfo = new TB_MapInfo();

            tbMapInfo.mMapNo = st.GetValueAsInt(x, "MapNo");
            tbMapInfo.mEnableBattle = st.GetValueAsInt(x, "EnableBattleScene");

            for (int i = 0; i < 5; ++i)
            {
                string stRegenMon = "RegenMon" + i.ToString();
                tbMapInfo.mArrRegenMosters[i] = st.GetValueAsInt(x, stRegenMon);
            }

            int key = tbMapInfo.mMapNo;
            if (cont_MapInfo.ContainsKey(key))
            {
                Debug.LogError("Already exist key. " + key.ToString());
            }

            cont_MapInfo.Add(key, tbMapInfo);
        }
    }

    void LoadConverTable()
    {
        cont_String = new Dictionary<string, TB_Conversation>();

        StringTable st = new StringTable();

        if (false == st.Build("Table/TB_Conversation")) { return; }

        int iRowCount = st.row;

        for (int x = 0; x < iRowCount; ++x)
        {
            TB_Conversation tbString = new TB_Conversation();

            tbString.mStringNo = st.GetValueAsInt(x, "NPCNo");
            tbString.mScenarioNo= st.GetValueAsInt(x, "ScenarioNo");
            tbString.stString = st.GetValue(x, "String");

            string key = tbString.mStringNo.ToString() + "_" + tbString.mScenarioNo.ToString();
            if (cont_String.ContainsKey(key))
            {
                Debug.LogError("Already exist key. " + key.ToString());
            }

            cont_String.Add(key, tbString);
        }
    }

    public void LoadTableAll()
    {
        LoadHeroTable();
        LoadMapInfoTable();
        LoadConverTable();
    }

    public string GetConverText(int iStringNo)
    {
        int iStep = GameDataManager.Instance().iScenarioStep;
        string key = iStringNo.ToString() + "_" + iStep.ToString();
        TB_Conversation tableString = null;
        if (TBManager.Instance().cont_String.TryGetValue(key, out tableString))
        {
            return tableString.stString;
        }

        return string.Empty;
    }
}
