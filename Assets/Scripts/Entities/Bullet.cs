using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public delegate void HeadHitHandler();
    public event HeadHitHandler OnHeadHit;
    private bool hasHit = false;

    public void Reset()
    {
        hasHit = false;
    }

	void OnCollisionEnter(Collision c)
    {
        if (!hasHit && c.gameObject.tag == "Head")
        {
            hasHit = true;
            if (OnHeadHit != null)
                OnHeadHit();
        }
	}
}
