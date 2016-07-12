using UnityEngine;
using System.Collections;

public class DestroyObject : MonoBehaviour
{
    public float mDestroyTime = 0;
	// Use this for initialization
	void Awake ()
    {
        Destroy(gameObject, mDestroyTime);
	}
}
