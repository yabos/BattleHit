﻿using UnityEngine;
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
        UIState_Field,
        UIState_Battle,
        UIState_Max,
    }

    string[] UIPath = new string[]
    { 
        "",
        "UI/Lobby/LobbyUI",
        "UI/Battle/BattleUI"
    };

    Transform mUICameraRoot = null;
    Camera mUICamera = null;
    GameObject mFieldUI = null;
    GameObject mBattleUI = null;

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

    void Start()
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

        LoadUI();
    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void LoadUI()
    {
        StartCoroutine(LoadUICoroutine());
    }

    IEnumerator LoadUICoroutine()
    {
        string stPath = "UI/Field/FieldUI";
        GameObject goUI = VResources.Load<GameObject>(stPath);
        if (goUI != null)
        {
            GameObject uiRoot = GameObject.Instantiate(goUI);
            if (uiRoot != null)
            {
                uiRoot.transform.name = "FieldUI";
                uiRoot.transform.parent = mUICameraRoot;

                uiRoot.transform.position = Vector3.zero;
                uiRoot.transform.rotation = Quaternion.identity;
                uiRoot.transform.localScale = Vector3.one;
                uiRoot.AddComponent<FieldUI_Control>();

                mFieldUI = uiRoot;
            }
        }

        stPath = "UI/Battle/BattleUI";
        goUI = VResources.Load<GameObject>(stPath);
        if (goUI != null)
        {
            GameObject uiRoot = GameObject.Instantiate(goUI);
            if (uiRoot != null)
            {
                uiRoot.transform.name = "BattleUI";
                uiRoot.transform.parent = mUICameraRoot;

                uiRoot.transform.position = Vector3.zero;
                uiRoot.transform.rotation = Quaternion.identity;
                uiRoot.transform.localScale = Vector3.one;
                uiRoot.AddComponent<BattleUI_Control>();
                uiRoot.SetActive(false);

                mBattleUI = uiRoot;
            }
        }

        yield return new WaitForEndOfFrame();
    }

    public BaseUI GetFieldUI()
    {
        if (mFieldUI == null)
        {
            return null;
        }

        return mFieldUI.GetComponent<BaseUI>();
    }

    public BaseUI GetBattleUI()
	{
		if (mBattleUI == null)
        {
			return null;
        }

		return mBattleUI.GetComponent<BaseUI>();
	}

    public void ActiveUI(eUIState state)
    {
        mFieldUI.SetActive(state == eUIState.UIState_Field);
        mBattleUI.SetActive(state == eUIState.UIState_Battle);
    }
}
