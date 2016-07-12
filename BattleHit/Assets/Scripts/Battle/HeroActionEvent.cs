using UnityEngine;
using System.Collections;

public class HeroActionEvent : MonoBehaviour
{
    Hero_Control mHero = null;

    void Start()
    {
        mHero = transform.parent.GetComponent<Hero_Control>();
        if (mHero == null)
        {
            Debug.LogError("Class : HeroActionEvent => mHero is null");
        }
    }

    void OnAttack()
    {
        if (mHero.Target.IsDie)
        {
            mHero.Target = null;
            mHero.HeroState = Hero_Control.eHeroState.HEROSTATE_IDLE;
        }
        else
        {
            mHero.Target.OnBeHit(mHero, 0);
        }
    }

    void OnSound()
    { 
    
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if ( col.transform.name != "Obj")
            return;

        // mHero가 col에게 맞음.
        // mHero.OnHit();
    }
}