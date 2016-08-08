using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour 
{
    public enum AnimationActor
    { 
        ANI_IDLE,
        ANI_WALK,
        ANI_HIT,
        ANI_ATT1,
        ANI_CAST1,
        ANI_DIE1,
        ANI_DIE2,
        ANI_MAX,
    }

    public string[] ClipName = new string[]
    {
        "idle",
        "walk",
        "hit",
        "attack1",
        "cast1",
        "death1",
        "death2"
    };

    public Animation anim = null;
    AnimationActor mAniState = AnimationActor.ANI_IDLE;
    float[] mAniTime = null;

    Hero_Control mHero = null;

    public AnimationActor AniState
    {
        set { mAniState = value; }
        get { return mAniState; }
    }

    // Use this for initialization
    void Start () 
    {
        anim = transform.GetComponent<Animation>();

        mAniTime = new float[(int)AnimationActor.ANI_MAX];
        for (int i = 0; i < (int)AnimationActor.ANI_MAX; ++i)
        {
            if( i == (int)AnimationActor.ANI_HIT )
                mAniTime[i] = anim[ClipName[i]].length * 2f;
            else
                mAniTime[i] = anim[ClipName[i]].length;
        }

        mHero = transform.parent.GetComponent<Hero_Control>();
        if (mHero == null)
        {
            Debug.LogError("Class : HeroActionEvent => mHero is null");
        }
    }

    public bool IsPlaying(AnimationActor eActiveAni)
    {
        return anim.IsPlaying(ClipName[(int)eActiveAni]);
    }

    public bool PlayAnimation(AnimationActor eActiveAni, bool bLoop = false)
    {
        bool bResult = false;    
        if (anim.isPlaying == false)
        {
            if (eActiveAni == AnimationActor.ANI_DIE1 ||
                eActiveAni == AnimationActor.ANI_DIE2)
            {
                bResult = false;
            }
            else
            {
                //if (bLoop)
                //{
                //    anim.CrossFade(ClipName[(int)eActiveAni]);
                //}
                //else
                {
                    anim.CrossFade(ClipName[(int)AnimationActor.ANI_IDLE], 0.1f);
                }

                bResult = false;
            }
        }
        else
        {
            anim.CrossFade(ClipName[(int)eActiveAni], 0.1f);
            bResult = true;
        }

        return bResult;
    }

    public void SetAnimationSpeed(AnimationActor eActiveAni, float fSeepd = 1.0f)
    {
        anim[ClipName[(int)eActiveAni]].speed = fSeepd;
    }
}
