using UnityEngine;
using System.Collections;

public class BattleUI_Control : BaseUI
{
    Transform mHeroHp = null;
    Transform mDamage = null;

    // Use this for initialization
    void Start ()
    {
        mHeroHp = transform.FindChild("Anchor_T/HeroHP");
        if (mHeroHp == null) return;

        mDamage = transform.FindChild("Anchor/Damage");
        if (mDamage == null) return;
    }
	
    public void CreateHeroHp(System.Guid uid, bool bMyTeam)
    {
        StartCoroutine(CreateHp(uid, bMyTeam));
    }

    IEnumerator CreateHp(System.Guid uid, bool bMyTeam)
    {
        yield return new WaitForEndOfFrame();

		GameObject goHPRes = VResources.Load<GameObject>("UI/Common/Prefabs/HPGauge");
        if (goHPRes != null)
        {
            GameObject goHP = GameObject.Instantiate(goHPRes) as GameObject;
            if (goHP != null)
            {
                goHP.transform.parent = mHeroHp;
                goHP.transform.name = uid.ToString();

                goHP.transform.localPosition = Vector3.zero;
                goHP.transform.localRotation = Quaternion.identity;
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

	public void DestroyAllHPGauge()
	{
		if (mHeroHp == null)
			return;

		for (int i = mHeroHp.childCount - 1; i >= 0; --i) 
		{
			Transform tChild = mHeroHp.GetChild(i);
			if (tChild == null)
				continue;

			NGUITools.Destroy (tChild.gameObject);
		}
	}

    public void CreateDamage(int iDamage, Vector3 vPos, bool bMyTeam)
    {
        GameObject goDamageRes = null;
        if (bMyTeam)
        {
            goDamageRes = VResources.Load<GameObject>("UI/Common/Prefabs/HeroDamage1");
        }
        else
        {
            goDamageRes = VResources.Load<GameObject>("UI/Common/Prefabs/HeroDamage2");
        }

        if (goDamageRes != null)
        {
            GameObject goDamage = GameObject.Instantiate(goDamageRes) as GameObject;
            if (goDamage != null)
            {
                goDamage.transform.parent = mDamage;

                goDamage.transform.position = new Vector3( vPos.x, vPos.y, 0);
                goDamage.transform.localRotation = Quaternion.identity;
                goDamage.transform.localScale = Vector3.one;
                HeroDamage hd = goDamage.GetComponent<HeroDamage>();
                if (hd != null)
                {
                    hd.m_LabelDamage.text = iDamage.ToString();
                    if (bMyTeam)
                    {
                        hd.m_LabelDamage.color = Color.white;
                    }
                }
            }
        }
    }
}
