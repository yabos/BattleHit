using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMain : MonoBehaviour  
{
    private static GameMain instance;  
    private static GameObject container;
    public static GameMain Instance()  
    {  
        if( !instance )  
        {  
            container = new GameObject();
            container.name = "GameMain";
            instance = container.AddComponent(typeof(GameMain)) as GameMain;  
        }  
        return instance;  
    }  

    public static readonly string stBattleRootPath = "Battle/Prefabs/Battle_Root";

    //ComDef.GameState mGameState = ComDef.GameState.GAMESTATE_LOBBY;

    public float mGameSpeed = 2f;

    GameObject mUIRoot = null;
    GameObject mBattleRoot = null;
    UIManager mUIManager = null;

    public GameObject UIRoot
    {
        set { mUIRoot = value; }
        get { return mUIRoot; }
    }

    public GameObject BattleRoot
    {
        set { mBattleRoot = value; }
        get { return mBattleRoot; }
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
    }

    // Use this for initialization
    void Start () 
    {
        Init();
    }
	
	// Update is called once per frame
	void Update () 
    {
        Time.timeScale = mGameSpeed;
    }

    void Init()
    {
        TBManager.Instance().LoadTableAll();

        ResourcesLoad();

        TitleUILoad();

        // Load save data.   
    }


    void ResourcesLoad()
    {
        // 3d model lad

        // effect load
        EffectManager.Instance().EffectLoad();
    }

    void TitleUILoad()
    {
        UIManager.Instance().TitleUILoad();
    }

    public void GoLobby()
    {
        UIManager.Instance().LoadUI(UIManager.eUIState.UIState_Lobby);
    }

    public void GoBattle()
    {
        UIManager.Instance().LoadUI(UIManager.eUIState.UIState_Battle);
        StartCoroutine(LoadBattleRoot());
    }

    // test loading code
    bool bisLoading = false;
    IEnumerator LoadBattleRoot()
    {
        bisLoading = true;
        yield return null;

        GameObject goBattleRoot = Resources.Load(stBattleRootPath) as GameObject;
        if (goBattleRoot != null)
        {
            mBattleRoot = GameObject.Instantiate(goBattleRoot);
            if (mBattleRoot != null)
            {
                mBattleRoot.transform.name = "Battle_Root";
                mBattleRoot.transform.position = Vector3.zero;
                mBattleRoot.transform.rotation = Quaternion.identity;
                mBattleRoot.transform.localScale = Vector3.one;
            }
        }

        bisLoading = false;
    }

    public Battle_Control Battle_Control()
    {
        if (mBattleRoot == null) return null;

        Battle_Control bc = mBattleRoot.GetComponent<Battle_Control>();
        if (bc == null) return null;

        return bc;
    }
}
