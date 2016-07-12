using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public	class ComDef
{
    public enum GameState
    { 
        GAMESTATE_TITLE = 0,
        GAMESTATE_LOBBY,
        GAMESTATE_BATTLE
    }
}

public class SaveData
{
    public int iHeroNo;
}
