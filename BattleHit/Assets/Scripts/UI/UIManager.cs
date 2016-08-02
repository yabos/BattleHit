using UnityEngine;
using System.Collections;
using System;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    private static GameObject container;
    public static UIManager Instance()
    {
        if (!instance)
        {
            container = new GameObject();
            container.name = "UIManager";
            instance = container.AddComponent(typeof(UIManager)) as UIManager;
        }
        return instance;
    }

    public enum eUIState
    {
        UIState_None = 0,
        UIState_Title,
        UIState_Lobby,
        UIState_Battle,
        UIState_Max,
    }

    string[] UIPath = new string[]
    { 
        "",
        "UI/Title/TitleUI",
        "UI/Lobby/LobbyUI",
        "UI/Battle/BattleUI"
    };

    Transform mUICameraRoot = null;
    Camera mUICamera = null;
    eUIState mUISate = eUIState.UIState_None;
    GameObject mCurrUI = null;

     
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Duplicate UIManager");
        }
    }

    // Use this for initialization
    public void TitleUILoad () 
    {
        mUICameraRoot = GameObject.Find("UIRoot/Camera").transform;
        if (mUICameraRoot == null)
        {
            Debug.LogError("Not Find UICameraRoot!");
            return;
        }

        mUICamera = mUICameraRoot.GetComponent<Camera>();
        if (mUICamera == null)
        {
            Debug.LogError("Not Find UICamera!");
            return;
        }

        LoadUI(eUIState.UIState_Title);
    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void LoadUI(eUIState state)
    {
        StartCoroutine(LoadUICoroutine(state));
    }

    IEnumerator LoadUICoroutine(eUIState state)
    {
        DestroyUI();
        yield return new WaitForEndOfFrame();

        string stPath = UIPath[(int)state];
		GameObject goUI = VResources.Load<GameObject>(stPath);
        if (goUI != null)
        {
            GameObject uiRoot = GameObject.Instantiate(goUI);
            if (uiRoot != null)
            {
                uiRoot.transform.name = state.ToString();
                uiRoot.transform.parent = mUICameraRoot;

                uiRoot.transform.position = Vector3.zero;
                uiRoot.transform.rotation = Quaternion.identity;
                uiRoot.transform.localScale = Vector3.one;
            }

            switch ((int)state)
            {
                case (int)eUIState.UIState_Title:
                    uiRoot.AddComponent<TitleUI_Control>();
                    break;

                case (int)eUIState.UIState_Lobby:
                    uiRoot.AddComponent<LobbyUI_Control>();
                    break;

                case (int)eUIState.UIState_Battle:
                    uiRoot.AddComponent<BattleUI_Control>();
                    break;
            }

            mCurrUI = uiRoot;
            mUISate = state;
        }        
    }

    void DestroyUI()
    {
        for (int i = mUICameraRoot.childCount - 1; i >= 0; --i)
        {
            Transform tChild = mUICameraRoot.GetChild(i);
            if (tChild == null) continue;

            NGUITools.Destroy(tChild.gameObject);
        }
    }   

//    public T GetUI<T>()
//    {
//        if (mCurrUI == null)
//        {
//            return default(T);
//        }
//
//        return mCurrUI.GetComponent<T>(); ;
//    }    

	public BaseUI GetUI()
	{
		if (mCurrUI == null)
        {
			return null;
        }

		return mCurrUI.GetComponent<BaseUI>();
	}
}
