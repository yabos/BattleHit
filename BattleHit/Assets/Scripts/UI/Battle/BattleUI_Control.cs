using UnityEngine;
using System.Collections;

public class BattleUI_Control : BaseUI
{
	Transform mBattleLoading = null;

    Transform mHeroHp = null;

	// Use this for initialization
	void Start ()
    {
        mHeroHp = transform.FindChild("Anchor/HeroHP");
        if (mHeroHp == null) return;

		mBattleLoading = transform.FindChild ("Anchor/Loading");
		if (mBattleLoading == null)return;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

	public void ActiveLoadingIMG(bool bActive)
	{
		mBattleLoading.gameObject.SetActive (bActive);
	}

    public void CreateHeroHp(System.Guid uid, bool bMyTeam)
    {
		GameObject goHPRes = VResources.Load<GameObject>("UI/Common/Prefabs/HPGauge");
        if (goHPRes == null) return;

        GameObject goHP = GameObject.Instantiate(goHPRes) as GameObject;
        if (goHP != null)
        {
            goHP.transform.parent = mHeroHp.transform;
            goHP.transform.name = uid.ToString();

            goHP.transform.position = Vector3.zero;
            goHP.transform.rotation = Quaternion.identity;
            goHP.transform.localScale = Vector3.one;

            Transform tSlider = goHP.transform.FindChild("SpriteSlider");
            if (tSlider != null)
            {
                UISprite sprite = tSlider.GetComponent<UISprite>();
                if (sprite != null)
                {
                    if (bMyTeam)
                    {
                        sprite.spriteName = "gauge_green";
                    }
                    else
                    {
                        sprite.spriteName = "gauge_red";
                    }
                }
            }

            goHP.SetActive(true);
        }
    }

    public void UpdateHPGauge(System.Guid uid, float fFillAmountHp)
    {
        if (mHeroHp == null) return;

        for (int i = 0; i < mHeroHp.childCount; ++i)
        {
            Transform tChild = mHeroHp.GetChild(i);
            if (tChild == null) continue;

            if (tChild.name.Equals(uid.ToString()))
            {
                Transform tSlider = tChild.FindChild("SpriteSlider");
                if (tSlider == null) continue;
                UISprite sprite = tSlider.GetComponent<UISprite>();
                if (sprite == null) continue;
                sprite.fillAmount = fFillAmountHp;
            }
        }
    }

    public void UpdatePosHPGauge(System.Guid uid, Transform tEf_HP)
    {
        if (mHeroHp == null) return;

        for (int i = 0; i < mHeroHp.childCount; ++i)
        {
            Transform tChild = mHeroHp.GetChild(i);
            if (tChild == null) continue;

            if (tChild.name.Equals(uid.ToString()))
            {
                tChild.position = tEf_HP.position;
            }
        }
    }

    public void DestroyHPGauge(System.Guid uid)
    {
        if (mHeroHp == null) return;

        for (int i = 0; i < mHeroHp.childCount; ++i)
        {
            Transform tChild = mHeroHp.GetChild(i);
            if (tChild == null) continue;

            if (tChild.name.Equals(uid.ToString()))
            {
                NGUITools.Destroy(tChild.gameObject);
            }
        }
    }
}
