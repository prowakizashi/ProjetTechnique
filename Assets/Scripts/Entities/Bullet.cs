using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public delegate void HeadHitHandler();
    public event HeadHitHandler OnHeadHit;

	void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Head")
            if (OnHeadHit != null)
                OnHeadHit();
	}
}
