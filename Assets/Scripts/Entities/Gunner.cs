using UnityEngine;
using System.Collections;


public class Gunner : MonoBehaviour
{
    public delegate void EndOfSequenceHandler();
    public event EndOfSequenceHandler OnEndOfSequence;

    [SerializeField]
    private Rigidbody bulletPrefab;
    [SerializeField]
    private int poolSize = 5;
    [SerializeField]
    private Vector3 impulseVector = new Vector3(0, 0, -50);
    private AFloatPairSequence seq;
    private System.Action<AFloatPairSequence> sequenceGrower;

    private Transform[] cannons;
    private Rigidbody[] bulletPool;
    private uint poolIt;
    private Coroutine fireCouroutine;

    public bool IsSequenceRunning { get; private set; }
    public uint CannonNb
    {
        get
        {
            return (uint)cannons.Length;
        }
    }

	// Use this for initialization
	private void Awake ()
    {
        cannons = GetComponentsInChildren<Transform>();

        if (!bulletPrefab.GetComponent<Bullet>())
            bulletPrefab.gameObject.AddComponent<Bullet>();

        bulletPool = new Rigidbody[poolSize];
        for (poolIt = 0; poolIt < poolSize; ++poolIt)
        {
            bulletPool[poolIt] = Instantiate<Rigidbody>(bulletPrefab);
            bulletPool[poolIt].GetComponent<Bullet>().OnHeadHit += onMannequinHeadHit;
            bulletPool[poolIt].gameObject.SetActive(false);
        }
        poolIt = 0;

        Intervalle cannonLimits = new Intervalle();
        cannonLimits.Max = (float)cannons.Length;
        cannonLimits.Min = 0f;
        Intervalle timerLimits = new Intervalle();
        timerLimits.Max = 2f;
        timerLimits.Min = 0.2f;
	}

    private void onMannequinHeadHit()
    {
        EndSequence();
    }

    public void SetupSequence(System.Func<AFloatPairSequence> ctor, System.Action<AFloatPairSequence> grower)
    {
        seq = ctor();
        sequenceGrower = grower;
        //MakeSequenceGrow();
    }

    public void MakeSequenceGrow()
    {
        sequenceGrower(seq);
    }

    private void Update()
    {
        if (!IsSequenceRunning && Input.GetKeyDown(KeyCode.F))
            StartFireSequence();
    }

    public void StartFireSequence()
    {
        if (IsSequenceRunning)
            throw new System.Exception("Fire sequence already running");

        IsSequenceRunning = true;
        fireCouroutine = StartCoroutine(fireNext());
    }
	
    private IEnumerator fireNext()
    {
        if (seq.MoveNext())
        {
            Vector2 curr = seq.CurrentValue;
            yield return new WaitForSeconds(curr.y);
            fire(cannons[(uint)curr.x]);
            fireCouroutine = StartCoroutine(fireNext());
        }
        else
        {
            yield return new WaitForSeconds(GameProperties.MAX_TIME_BETWEEN_SHOTS);
            EndSequence();
        }
	}

    public void EndSequence()
    {
        StopCoroutine(fireCouroutine);
        seq.ResetIterator();
        foreach (var b in bulletPool)
        {
            b.velocity = Vector2.zero;
            b.gameObject.SetActive(false);
        }

        IsSequenceRunning = false;
        if (OnEndOfSequence != null)
            OnEndOfSequence();
    }

    private void fire(Transform cannon)
    {
        Rigidbody bullet = bulletPool[poolIt++];
        poolIt = poolIt % (uint)bulletPool.Length;

        bullet.transform.position = cannon.position;
        bullet.gameObject.SetActive(true);
        bullet.velocity = Vector2.zero;
        bullet.AddForce(impulseVector, ForceMode.Impulse);
    }
}
