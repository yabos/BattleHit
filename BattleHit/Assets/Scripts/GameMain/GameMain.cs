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
    UIManager mUIManager = null;

    GameObject mBattleRoot = null;
    Battle_Control mBattleControl = null;

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

    public Battle_Control BattleControl
    {
        set { mBattleControl = value; }
        get { return mBattleControl; }
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

		GameObject goBattleRoot = VResources.Load<GameObject>(stBattleRootPath);
        if (goBattleRoot != null)
        {
            mBattleRoot = GameObject.Instantiate(goBattleRoot);
            if (mBattleRoot != null)
            {
                mBattleRoot.transform.name = "Battle_Root";
                mBattleRoot.transform.position = Vector3.zero;
                mBattleRoot.transform.rotation = Quaternion.identity;
                mBattleRoot.transform.localScale = Vector3.one;

                mBattleControl = mBattleRoot.GetComponent<Battle_Control>();
            }
        }

        bisLoading = false;
    }
}
