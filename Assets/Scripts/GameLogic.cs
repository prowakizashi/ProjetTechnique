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
    public Action OnGameStart;
    public Action OnGameStop;
    public Action OnExperienceStart;
    public Action OnExperienceStop;
    public Action<int, float> OnSequenceStop;

    public void StartGame()
    {
        if (OnGameStart != null)
            OnGameStart();
        gunner.SetupSequence(GameProperties.GetSequenceCtor(gunner.CannonNb), GameProperties.GetSequenceGrower());
        gunner.OnEndOfSequence += OnSequenceEnd;
        gameStarted = true;
        StartExperience();
    }

    public void StopGame()
    {
        if (OnGameStop != null)
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
        if (OnExperienceStart != null)
            OnExperienceStart();

        MakeANewGeneration();
        gunner.MakeSequenceGrow();
        StartSequence();
    }

    private void MakeANewGeneration()
    {
        evolver = new GeneticEvolver(DNAScores);
        Debug.Log("New generation made giving " + evolver.newDNAs.Count + " species.");
        currentIndex = 0;
        DNAScores.Clear();
    }

    private void OnExperienceEnd()
    {
        if (OnExperienceStart != null)
            OnExperienceStart();

        StartExperience();
    }

    private void StartSequence()
    {
        if (currentCharacter != null)
            Destroy(currentCharacter);
        currentCharacter = Instantiate(characterPrefab);
        Debug.Log("Current test: " + currentIndex + " / " + evolver.newDNAs.Count);
        currentCharacter.GetComponent<GeneticAI>().InitWithDNA(evolver.newDNAs[currentIndex]);
        StartCoroutine("waitForStanceThenStart");
    }

    private IEnumerator waitForStanceThenStart()
    {
        yield return new WaitForSeconds(3); // wait for stance to stabilize before shooting
        SequenceStartTime = Time.time;
        print(SequenceStartTime);
        gunner.StartFireSequence();
    }

    private void OnSequenceEnd()
    {
        float score = Time.time - SequenceStartTime;
        if (OnSequenceStop != null)
            OnSequenceStop(currentIndex, score);
        DNAScores.Add(evolver.newDNAs[currentIndex++], score);
        if (currentIndex < GameProperties.POPULATION_SIZE)
            StartSequence();
        else
            OnExperienceEnd();
    }
}
