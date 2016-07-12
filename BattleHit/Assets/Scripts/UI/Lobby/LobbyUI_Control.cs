using UnityEngine;
using System.Collections;

public class LobbyUI_Control : MonoBehaviour
{
	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void GoBattle(GameObject go)
    {
        GameMain gm = GameMain.Instance();
        if (gm == null) return;

        
        gm.GoBattle();
    }
}
