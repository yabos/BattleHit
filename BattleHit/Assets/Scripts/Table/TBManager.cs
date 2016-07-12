using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TBManager 
{
    protected static TBManager m_Instance = null;
    public static TBManager Instance()
    {
        if (m_Instance == null)
        {
            m_Instance = new TBManager();
        }

        return m_Instance;
    }

    protected TBManager()
    { }


    // ------------------------------------//

    public Dictionary<int, TB_Hero> cont_Hero = null;

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

    public void LoadTableAll()
    {
        LoadHeroTable();
    }
}
