using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameLogic : MonoBehaviour {

    [SerializeField]
    private GameObject characterPrefab;
    [SerializeField]
    private Gunner gunner;

    private GameObject currentCharacter = null;
    private GeneticEvolver evolver = null;
    private Dictionary<RagdollDNA, float> DNAScores = new Dictionary<RagdollDNA, float>();
    private int currentIndex = 0;

    private bool gameStarted = false;
    private float SequenceStartTime = 0;

    //Actions
    public Action OnExperienceStart;
    public Action OnExperienceStop;
    public Action<int, float> OnSequenceStop;
    public Action OnGameStart;
    public Action OnGameStop;

    public void StartGame()
    {
        OnGameStart();
        gunner.SetupSequence(GameProperties.GetSequenceCtor(gunner.CannonNb), GameProperties.GetSequenceGrower());
        gunner.OnEndOfSequence += OnSequenceEnd;
        gameStarted = true;
        StartExperience();
    }

    public void StopGame()
    {
        OnGameStop();

        gameStarted = false;
        gunner.OnEndOfSequence -= OnSequenceEnd;
        gunner.EndSequence();
        DNAScores.Clear();
        Destroy(currentCharacter);
    }

    public bool IsGameRunning()
    {
        return gameStarted;
    }

    private void StartExperience()
    {
        OnExperienceStart();

        MakeANewGeneration();
        gunner.MakeSequenceGrow();
        StartSequence();
    }

    private void MakeANewGeneration()
    {
        evolver = new GeneticEvolver(DNAScores);
        currentIndex = 0;
        DNAScores.Clear();
    }

    private void OnExperienceEnd()
    {
        OnExperienceStart();

        StartExperience();
    }

    private void StartSequence()
    {
        if (currentCharacter != null)
            Destroy(currentCharacter);
        currentCharacter = Instantiate(characterPrefab);
        currentCharacter.GetComponent<GeneticAI>().InitWithDNA(evolver.newDNAs[currentIndex]);
        StartCoroutine("waitForStanceThenStart");
    }

    private IEnumerator waitForStanceThenStart()
    {
        yield return new WaitForSeconds(3); // wait for stance to stabilize before shooting
        SequenceStartTime = Time.realtimeSinceStartup;
        print(SequenceStartTime);
        gunner.StartFireSequence();
    }

    private void OnSequenceEnd()
    {
        float score = Time.realtimeSinceStartup - SequenceStartTime;
        OnSequenceStop(currentIndex, score);
        DNAScores.Add(evolver.newDNAs[currentIndex++], score);
        if (currentIndex < GameProperties.POPULATION_SIZE)
            StartSequence();
        else
            OnExperienceEnd();
    }
}
