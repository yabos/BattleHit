using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectManager : MonoBehaviour
{
    public enum eEffectType
    {
        EFFECT_BATTLE_HIT
    }

    private static EffectManager instance;
    private static GameObject container;
    public static EffectManager Instance()
    {
        if (!instance)
        {
            container = new GameObject();
            container.name = "EffectManager";
            instance = container.AddComponent(typeof(EffectManager)) as EffectManager;
        }
        return instance;
    }
    
    private Dictionary<eEffectType, GameObject> mDicEffectPool = new Dictionary<eEffectType, GameObject>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Duplicate EffectManager");
        }
    }

    public void EffectLoad()
    {
		GameObject efc = VResources.Load<GameObject> ("Effect/Hero/Hit");
        if (efc == null)
        {
            Debug.LogError("Not Find Effect/Hero/Hit!");
        }

        mDicEffectPool.Add(eEffectType.EFFECT_BATTLE_HIT, efc);
    }

    public GameObject GetEffect(eEffectType eType)
    {
        if (mDicEffectPool.ContainsKey(eType))
        {
            return GameObject.Instantiate(mDicEffectPool[eType]);
        }

        return null;
    }
}
