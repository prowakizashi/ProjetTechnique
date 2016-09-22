using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BallThrower : MonoBehaviour
{
    [SerializeField]
    private GameObject canonListObject;
    [SerializeField]
    private GameObject ballPool; // TO CHANGE

    private List<Transform> canonList = new List<Transform>();
    private int currentIndex = 0;

    public delegate bool BoolDelegate();
    private BoolDelegate isRunning = null;
    private Action onHit = null;

    APunchSequence sequence = null;

    void Awake()
    {
        for (int i = 0; i < canonListObject.transform.childCount; ++i)
        {
            canonList.Add(canonListObject.transform.GetChild(i));
        }
    }

    public void Init(BoolDelegate isRunningAction, Action onHitAction)
    {
        isRunning = isRunningAction;
        onHit = onHitAction;
    }

    public void StartSequence(APunchSequence seq)
    {
        sequence = seq;
        currentIndex = 0;
        sequence.InitSequence(canonList.Count);
        StartCoroutine(PlaySequence(2.0f));
    }
    
    private IEnumerator PlaySequence(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        while (isRunning())
        {
            Vector2 throwInfo = sequence.Get(currentIndex++);
            Fire((int)throwInfo.x);
            yield return new WaitForSeconds(throwInfo.y);
        }
    }

    private void Fire(int n)
    {
        //TODO
    }
}
