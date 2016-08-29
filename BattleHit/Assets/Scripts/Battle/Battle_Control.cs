using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NearPath
{
    public bool mIsEntered = false;
    public Transform mTran = null;
}

public class Battle_Control : MonoBehaviour
{
    public enum eBattleState
    {
        eBattle_Ready,
        eBattle_Ing,
        eBattle_Win,
        eBattle_Lose,
        eBattle_End,
    }

    public static readonly string stMapLoadPath = "Map/BattleMap/";

    eBattleState mBattleState = eBattleState.eBattle_Ready;

    List<Hero_Control> mListMyHeroes = new List<Hero_Control>();
    List<Hero_Control> mListEnemyHeroes = new List<Hero_Control>();
    List<Hero_Control> mListSortingLayer = new List<Hero_Control>();

    int m_iLoadingState = 0;

    Transform mBattleStartTo = null;
    List<NearPath> mListBattleEndPos = new List<NearPath>();

    public eBattleState BattleState
    {
        set { mBattleState = value; }
        get { return mBattleState; }
    }

    public List<Hero_Control> ListMyHeroes
    {
        get { return mListMyHeroes; }
    }

    public List<Hero_Control> ListEnemyHeroes
    {
        get { return mListEnemyHeroes; }
    }

    public Transform BattleStartTo
    {
        get { return mBattleStartTo; }
    }

    public List<NearPath> ListBattleEndPos
    {
        get { return mListBattleEndPos; }
    }

    void Start()
    {
        mBattleState = eBattleState.eBattle_Ready;

        CreativeSpore.RpgMapEditor.Camera2DController cam2d =Camera.main.transform.GetComponent<CreativeSpore.RpgMapEditor.Camera2DController>();
        cam2d.PixelToUnits = 300;
    }

    void Update()
    {
        LoadingProcess();

        UpdateSortingLayer();
    }   


    void UpdateSortingLayer()
    {
        if (mListSortingLayer == null || mListSortingLayer.Count <= 0) return;

        mListSortingLayer.Sort(delegate (Hero_Control x, Hero_Control y)
        {
            return y.transform.localPosition.y.CompareTo(x.transform.localPosition.y);
        });

        for (int i = 0; i < mListSortingLayer.Count; ++i)
        {
            mListSortingLayer[i].ListSR.Sort( delegate (SpriteRenderer x, SpriteRenderer y)
            {
                return x.sortingOrder.CompareTo(y.sortingOrder);
            });

            for (int j = 0; j < mListSortingLayer[i].ListSR.Count; ++j)
            {
                mListSortingLayer[i].ListSR[j].sortingOrder = ((i+1) * (100 + j));

                if (j == mListSortingLayer[i].ListSR.Count - 1)
                {
                    mListSortingLayer[i].MaxSortingOrderNo = ((i + 1) * (100 + j));
                }
            }
        }
    }

    void LoadingProcess()
    {
        switch (m_iLoadingState)
        {
            case 0:
                StartCoroutine(CreateMap(10101));
                break;

            case 1:
                StartCoroutine(SetMyTeamHero());
                break;

            case 2:
                StartCoroutine(SetEnemyTeamHero());
                break;

            case 3:
                //AggroInit();
                m_iLoadingState++;
                break;

            case 4:
                InitBattlePos();
                break;

			case 5:
				BattleUI_Control ui = UIManager.Instance ().GetUI () as BattleUI_Control;
				ui.ActiveLoadingIMG (false);
				m_iLoadingState++;
				break;
        }
    }

    IEnumerator CreateMap( int iMapNo )
    {
		GameObject goMap = VResources.Load<GameObject>(stMapLoadPath + iMapNo.ToString());
        if (goMap != null)
        { 
            GameObject Map = GameObject.Instantiate( goMap ) as GameObject;
            if (Map != null)
            {
                Map.transform.parent = transform;                

                Map.name = "Map";

                Vector3 vCamPos = Camera.main.transform.position;
                Map.transform.position = new Vector3(vCamPos.x, vCamPos.y, 0);
                Map.transform.rotation = Quaternion.identity;
                Map.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            }
        }

        yield return new WaitForEndOfFrame();

        m_iLoadingState++;
    }

    IEnumerator SetMyTeamHero()
    {
        Transform tTeam = transform.FindChild("Team/MyTeam");
        if (tTeam != null)
        {
            //for (int i = 0; i < 3; ++i)
            {
                Hero_Control hero = UtilFunc.CreateHero(tTeam, 1001, 1, true);
                if (hero != null)
                {
                    Transform tSPos = transform.FindChild("Map/RegenPos/MyTeam/StartFrom");
                    if (tSPos != null)
                    {
                        hero.transform.position = tSPos.position;
                        hero.transform.rotation = Quaternion.identity;
                        hero.transform.localScale = Vector3.one;
                    }

                    mListMyHeroes.Add(hero);
                }
            }

            mListSortingLayer.AddRange(mListMyHeroes);
        }

        yield return new WaitForEndOfFrame();

        m_iLoadingState++;
    }

    IEnumerator SetEnemyTeamHero()
    {
        Transform tTeam = transform.FindChild("Team/EnemyTeam");
        if (tTeam != null)
        {
            for (int i = 0; i < 1; ++i)
            {
                Hero_Control hero = UtilFunc.CreateHero(tTeam, 2001, 1, false);
                if (hero != null)
                {
                    Transform tSPos = transform.FindChild("Map/RegenPos/EnemyTeam/" + i.ToString());
                    if (tSPos != null)
                    {
                        hero.transform.position = tSPos.position;
                        hero.transform.rotation = Quaternion.identity;
                        hero.transform.localScale = Vector3.one;
                    }

                    mListEnemyHeroes.Add(hero);
                }
            }

            mListSortingLayer.AddRange(mListEnemyHeroes);
        }

        yield return new WaitForEndOfFrame();

        m_iLoadingState++;
    }

    //void AggroInit()
    //{
    //    foreach (var nodeMy in mListMyHeroes)
    //    {
    //        foreach (var nodeEnemy in mListEnemyHeroes)
    //        {
    //            nodeMy.DicAggro.Add(nodeEnemy, 1);
    //        }
    //    }

    //    foreach (var nodeEnemy in mListEnemyHeroes)
    //    {
    //        foreach (var nodeMy in mListMyHeroes)
    //        {
    //            nodeEnemy.DicAggro.Add(nodeMy, 1);
    //        }
    //    }

    //    m_iLoadingState++;
    //}

    void InitBattlePos()
    {
        Transform tStartTo = transform.FindChild("Map/RegenPos/MyTeam/StartTo");
        if (tStartTo != null)
        {
            mBattleStartTo = tStartTo;
        }

        for (int i = 0; i < 6; ++i)
        {
            Transform tEndPath = transform.FindChild("Map/RegenPos/MyTeam/EndPath" + i.ToString());
            if (tEndPath == null) continue;
            NearPath np = new NearPath();
            np.mIsEntered = false;
            np.mTran = tEndPath;
            mListBattleEndPos.Add(np);
        }

        m_iLoadingState++;
    }

    public void CheckEndBattle()
    {
        bool bAliveEnemy = false;
        for (int i = 0; i < mListEnemyHeroes.Count; ++i)
        {
            if (!mListEnemyHeroes[i].IsDie)
            {
                bAliveEnemy = true;
            }
        }

        if (!bAliveEnemy)
        {
            mBattleState = eBattleState.eBattle_Win;
        }
    }
}