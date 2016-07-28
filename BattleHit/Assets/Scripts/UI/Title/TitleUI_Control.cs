using UnityEngine;
using System.Collections;

public class TitleUI_Control : BaseUI
{
	// Use this for initialization
	void Start ()
    {
	
	}

    void GoLobby(GameObject go)
    {
        //StartCoroutine(GameMain.Instance().LoadBattleRoot());
        GameMain gm = GameMain.Instance();
        if (gm == null) return;

        gm.GoLobby();
    }
}
