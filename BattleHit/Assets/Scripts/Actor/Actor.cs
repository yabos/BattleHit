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

    //public Animation anim = null;
    public Animator anim = null;
    AnimationActor mAniState = AnimationActor.ANI_IDLE;

    Hero_Control mHero = null;

    public AnimationActor AniState
    {
        set { mAniState = value; }
        get { return mAniState; }
    }

    // Use this for initialization
    void Start () 
    {
        //anim = transform.GetComponent<Animation>();
        anim = transform.GetComponent<Animator>();

        mHero = transform.parent.GetComponent<Hero_Control>();
        if (mHero == null)
        {
            Debug.LogError("Class : HeroActionEvent => mHero is null");
        }
    }

    public bool IsPlaying(AnimationActor eActiveAni)
    {
        //return anim.IsPlaying(ClipName[(int)eActiveAni]);
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(ClipName[(int)eActiveAni]))
        {
            return AnimatorIsPlaying();
        }

        return false;
    }

    bool AnimatorIsPlaying()
    {
        return anim.GetCurrentAnimatorStateInfo(0).length >=
               anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    public bool PlayAnimation(AnimationActor eActiveAni, bool bLoop = false)
    {
        bool bResult = false;
        if (AnimatorIsPlaying() == false)
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
                    anim.Play(ClipName[(int)AnimationActor.ANI_IDLE]);
                }

                bResult = false;
            }
        }
        else
        {
            anim.Play(ClipName[(int)eActiveAni]);
            bResult = true;
        }

        return bResult;
    }

    public void SetAnimationSpeed(float fSeepd = 1.0f)
    {
        anim.speed = fSeepd;
    }
}
