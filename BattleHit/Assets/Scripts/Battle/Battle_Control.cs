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

    eBattleState mBattleState = eBattleState.eBattle_Ing;

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
        mBattleState = eBattleState.eBattle_Ing;

        CreativeSpore.RpgMapEditor.Camera2DController cam2d =Camera.main.transform.GetComponent<CreativeSpore.RpgMapEditor.Camera2DController>();
        cam2d.PixelToUnits = 101;
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
                InitBattlePos();
                break;

			case 4:
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
                Map.transform.localScale = new Vector3(1f, 0.8f, 1f);

                // 전투 맵 테이블 읽어서 해당 전투에 맞는   BGM 틀어주자.
                // 일단은 무조건  battlebgm
                SoundManager.Instance().PlayBattleBGM(SoundManager.eBattleBGM.eBattleBGM_Normal);
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
            Hero_Control hero = UtilFunc.CreateHero(tTeam, 1001, 1, true);
            if (hero != null)
            {
                Transform tSPos = transform.FindChild("Map/RegenPos/MyTeam/0");
                if (tSPos != null)
                {
                    hero.transform.position = tSPos.position;
                    hero.transform.rotation = Quaternion.identity;
                    hero.transform.localScale = Vector3.one;
                }


                mListMyHeroes.Add(hero);
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
            int iMonIndex = 0;
            int iMonLv = 0;
            GameMain.Instance().GetEncountMonsterInfo(ref iMonIndex, ref iMonLv);
            Hero_Control hero = UtilFunc.CreateHero(tTeam, iMonIndex, iMonLv, false);
            if (hero != null)
            {
                Transform tSPos = transform.FindChild("Map/RegenPos/EnemyTeam/0");
                if (tSPos != null)
                {
                    hero.transform.position = tSPos.position;
                    hero.transform.rotation = Quaternion.identity;
                    hero.transform.localScale = Vector3.one;
                }

                mListEnemyHeroes.Add(hero);
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
		bool bAliveHeroes = false;
		for (int i = 0; i < mListMyHeroes.Count; ++i)
		{
			if (!mListMyHeroes[i].IsDie)
			{
				bAliveHeroes = true;
			}
		}
		if (!bAliveHeroes)
		{
			mBattleState = eBattleState.eBattle_Lose;
            UtilFunc.FadeInOut(true);

            StartCoroutine(EndBattle(2));
			return;
		}

		bAliveHeroes = false;
        for (int i = 0; i < mListEnemyHeroes.Count; ++i)
        {
            if (!mListEnemyHeroes[i].IsDie)
            {
                bAliveHeroes = true;
            }
        }

        if (!bAliveHeroes)
        {
            mBattleState = eBattleState.eBattle_Win;

            StartCoroutine(EndBattle(2));
        }
    }

	IEnumerator EndBattle(float fTime)
	{
        yield return new WaitForSeconds(fTime);

        UtilFunc.FadeInOut(true);

        UIManager.Instance ().ActiveUI(UIManager.eUIState.UIState_Field);
		GameMain.Instance ().SetCameraPixelToUnit (100);
		GameMain.Instance ().SetCameraFloowObjBehaviour (0.15f);
		GameMain.Instance ().FieldPlayer.SetActive (true);

        SoundManager.Instance().PlayCurrentBGM();

        NGUITools.Destroy (gameObject);
	}
}