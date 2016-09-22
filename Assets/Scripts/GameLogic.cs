using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameLogic : MonoBehaviour {

    [SerializeField]
    private GameObject characterPrefab;
    [SerializeField]
    private BallThrower ballThrower;

    private GameObject currentCaracter = null;
    private GeneticEvolver evolver = null;
    private Dictionary<RagdollDNA, float> DNAScores = new Dictionary<RagdollDNA, float>();
    private int currentIndex = 0;

    private bool gameStarted = false;
    private float SequenceStartTime = 0;
    private APunchSequence punchSequence = null;

    //Actions
    public Action OnGameStart;
    public Action OnGameStop;

    void Start()
    {
        ballThrower.Init(IsGameRunning, OnSequenceEnd);
    }

    public void StartGame()
    {
        OnGameStart();

        gameStarted = true;
        punchSequence = new RandomPunchSequence();
        StartExperience();
    }

    public void StopGame()
    {
        OnGameStop();

        gameStarted = false;
    }

    public bool IsGameRunning()
    {
        return gameStarted;
    }

    private void StartExperience()
    {
        //TODO : notify gui

        MakeANewGeneration();
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

        if (currentCaracter != null)
            DestroyImmediate(currentCaracter);
        currentCaracter = Instantiate(characterPrefab);
        currentCaracter.GetComponent<RagdollCharacter>().OnHitFace = OnSequenceEnd;
        currentCaracter.GetComponent<GeneticAI>().InitWithDNA(evolver.newDNAs[currentIndex]);
        SequenceStartTime = Time.realtimeSinceStartup;
        ballThrower.StartSequence(punchSequence);
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
