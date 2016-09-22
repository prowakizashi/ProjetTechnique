using UnityEngine;
using System.Collections;
using System;

public class RagdollCharacter : MonoBehaviour {

    [SerializeField]
    private Rigidbody head;
    [SerializeField]
    private Rigidbody torso;
    [SerializeField]
    private Rigidbody leftHand;
    [SerializeField]
    private Rigidbody rightHand;

    private Rigidbody[] muscles = new Rigidbody[4];
    
    void Awake ()
    {
        muscles[0] = head;
        muscles[1] = torso;
        muscles[2] = leftHand;
        muscles[3] = rightHand;
    }

    public void moveMuscle(int index, Vector3 dir, int power)
    {
        muscles[index].AddForce(dir * (index == 0 ? power / 10 : power), ForceMode.Impulse);
    }
}
