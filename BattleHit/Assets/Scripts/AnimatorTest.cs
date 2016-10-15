using UnityEngine;
using System.Collections;

public class AnimatorTest : MonoBehaviour {

    Animation animator = null;
	// Use this for initialization
	void Start ()
    {
        animator = gameObject.GetComponent<Animation>();
        Attack();
    }

    void Update()
    {
        Attack();
    }

    void Idle()
    {
        //animator.SetTrigger("idle");
    }

    void Attack()
    {
        animator.Play("att");
    }

    void Hit()
    {
        //animator.SetTrigger("hit");
    }

    //public bool IsPlaying(string stName)
    //{
    //    return animator.GetCurrentAnimatorStateInfo(0).IsName(stName);            
    //}

    //public bool PlayAnimation(string stName, bool bLoop = false)
    //{
    //    animator.SetTrigger(stName);
    //    return true;
    //}
}
