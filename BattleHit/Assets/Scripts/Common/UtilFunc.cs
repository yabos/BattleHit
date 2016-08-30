using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UtilFunc 
{
    public static Hero_Control CreateHero( Transform tParant, int iHeroNo, int iLv, bool bMyTeam )
    {
        GameObject goHero = new GameObject();
        Hero_Control hero = goHero.AddComponent<Hero_Control>();
        if (hero == null) return null;

        Guid uid = Guid.NewGuid();
        goHero.transform.parent = tParant;
        goHero.transform.name = uid.ToString();
        
        TB_Hero tbHero;
        if (TBManager.Instance().cont_Hero.TryGetValue(iHeroNo, out tbHero))
        {
            hero.HeroUid = uid;
            hero.HeroNo = iHeroNo;
            hero.HP = tbHero.mHP + Mathf.CeilToInt(((float)(iLv - 1) * ((float)tbHero.mHP * 0.1f)));
            hero.MaxHP = hero.HP;
            hero.Atk = tbHero.mAtk + Mathf.CeilToInt(((float)(iLv - 1) * ((float)tbHero.mAtk * 0.1f)));
            hero.Def = tbHero.mDef + Mathf.CeilToInt(((float)(iLv - 1) * ((float)tbHero.mDef * 0.1f)));
            hero.BlowPower = tbHero.mBlowPower;
            hero.BlowTolerance = tbHero.mBlowTolerance;
            hero.Speed = tbHero.mSpeed;
            hero.StResPath = tbHero.stResPath;
            hero.MyTeam = bMyTeam;

			GameObject goRes = VResources.Load<GameObject> (hero.StResPath);
            if (goRes == null) return null;

            GameObject go = GameObject.Instantiate(goRes) as GameObject;
            if (go != null)
            {
                go.transform.parent = goHero.transform;
                go.transform.name = "Obj";

                go.transform.position = Vector3.zero;
                go.transform.rotation = Quaternion.identity; //Quaternion.Euler(new Vector3(0,90,0));
                go.transform.localScale = Vector3.one;
                go.transform.localScale *= tbHero.mScale;

                hero.HeroObj = go;
            }

			GameObject goAttackedRes = VResources.Load<GameObject> ("Heroes/Prefabs/AttakedPos");
            if (goAttackedRes == null) return null;

            GameObject goAttacked = GameObject.Instantiate(goAttackedRes) as GameObject;
            if (goAttacked != null)
            {
                goAttacked.transform.parent = goHero.transform;
                goAttacked.transform.name = "AttackedPos";

                goAttacked.transform.position = Vector3.zero;
                goAttacked.transform.rotation = Quaternion.identity;
                goAttacked.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }

            // create hero hp
			BattleUI_Control bcUI = UIManager.Instance().GetBattleUI() as BattleUI_Control;
            if (bcUI != null)
            {
                bcUI.CreateHeroHp(uid, bMyTeam);
            }
        }

        return hero;
    }

    public static void FadeIn()
    {
        GameObject goFade = GameObject.Find("FadeInOutManager");
        if (goFade != null)
        {
            FadeInOutManager fiom = goFade.GetComponent<FadeInOutManager>();
            if (fiom != null)
            {
                fiom.StartFadeIn();
            }
        }
    }
}
