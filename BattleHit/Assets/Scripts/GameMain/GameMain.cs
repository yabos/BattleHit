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

    public float mGameSpeed = 1f;

    GameObject mUIRoot = null;

    GameObject mBattleRoot = null;
    Battle_Control mBattleControl = null;

	GameObject mFieldPlayer = null;
	GameObject mFieldMainCamera = null;
	CreativeSpore.RpgMapEditor.Camera2DController mCamera2DControl = null;

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

	public GameObject FieldPlayer
	{
		set { mFieldPlayer = value;}
		get { return mFieldPlayer;}
	}

	public GameObject FieldMainCamera
	{
		set { mFieldMainCamera = value;}
		get { return mFieldMainCamera;}
	}

	public CreativeSpore.RpgMapEditor.Camera2DController Camera2DControl
	{
		set { mCamera2DControl = value;}
		get { return mCamera2DControl;}
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

		DontDestroyOnLoad(this);
    }
	
	// Update is called once per frame
	void Update () 
    {
        Time.timeScale = mGameSpeed;
    }

    void Init()
    {
        TBManager.Instance().LoadTableAll();
    }

	void OnLevelWasLoaded()
	{
		mFieldPlayer = GameObject.Find ("Player");
		if (mFieldPlayer == null) 
		{
			Debug.LogError ("Not find field Player");
		}

		mFieldMainCamera = GameObject.Find("PlayerCamera");
		if (mFieldMainCamera == null) 
		{
			Debug.LogError ("Not find field PlayerCamera");
		}

		mCamera2DControl = mFieldMainCamera.GetComponent<CreativeSpore.RpgMapEditor.Camera2DController> ();
	}

    void ResourcesLoad()
    {
        // 3d model load

        // effect load
        EffectManager.Instance().EffectLoad();
    }

    //void TitleUILoad()
    //{
    //    UIManager.Instance().TitleUILoad();
    //}

    public void GoLobby()
    {
        //UIManager.Instance().LoadUI(UIManager.eUIState.UIState_Lobby);
    }

    public void LoadBattle()
    {
        UIManager.Instance().ActiveUI(UIManager.eUIState.UIState_Battle);
        StartCoroutine(LoadBattleRoot());
    }

    IEnumerator LoadBattleRoot()
    {
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
				mBattleRoot.SetActive (true);
            }
        }
    }

	public void BattleStart()
	{
		SetCameraFloowObjBehaviour (0);
		LoadBattle ();
    }

	public void SetCameraPixelToUnit(float fValue)
	{
		if (mCamera2DControl != null) 
		{
			mCamera2DControl.PixelToUnits = fValue;
		}
	}

	public void SetCameraFloowObjBehaviour(float fValue)
	{
		if (mFieldMainCamera != null)
		{
			CreativeSpore.RpgMapEditor.FollowObjectBehaviour fob = mFieldMainCamera.GetComponent<CreativeSpore.RpgMapEditor.FollowObjectBehaviour>();
			if (fob != null)
			{
				fob.DampTime = fValue;
			}
		}
	}
}
