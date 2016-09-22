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

    void Start()
    {
    }

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
        Destroy(currentCharacter);
    }

    public bool IsGameRunning()
    {
        return gameStarted;
    }

    private void StartExperience()
    {
        //TODO : notify gui

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
        //TODO : notify gui

        StartExperience();
    }

    private void StartSequence()
    {
        //TODO : notify gui

        if (currentCharacter != null)
            Destroy(currentCharacter);
        currentCharacter = Instantiate(characterPrefab);
        currentCharacter.GetComponent<GeneticAI>().InitWithDNA(evolver.newDNAs[currentIndex]);
        SequenceStartTime = Time.realtimeSinceStartup;
        gunner.StartFireSequence();
    }

    private void OnSequenceEnd()
    {
        //TODO : notify gui

        float score = Time.realtimeSinceStartup - SequenceStartTime;
        DNAScores.Add(evolver.newDNAs[currentIndex++], score);
        if (currentIndex < GameProperties.POPULATION_SIZE)
            StartSequence();
        else
            OnExperienceEnd();
    }
}
