using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConfigGUI : MonoBehaviour {

    [SerializeField]
    private InputField CrossField;
    [SerializeField]
    private InputField MutationField;
    [SerializeField]
    private InputField PopulationField;

    [SerializeField]
    private Button StartStopButton;

    private bool gameIsRunning = false;
    private GameLogic gameLogic;

    void Start ()
    {
        gameLogic = GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GameLogic>();
        gameLogic.OnGameStart += OnGameStart;
        gameLogic.OnGameStop += OnGameStop;
        StartStopButton.onClick.AddListener(ToggleGame);
    }

    private void ToggleGame()
    {
        if (!gameIsRunning)
        {
            gameLogic.StartGame();
            GameProperties.CROSS_COUNT = int.Parse(CrossField.text);
            GameProperties.MUTATION_RATE = int.Parse(MutationField.text);
            GameProperties.POPULATION_SIZE = int.Parse(PopulationField.text);
        }
        else
            gameLogic.StopGame();
    }

    private void OnGameStart()
    {
        gameIsRunning = true;
        CrossField.interactable = false;
        MutationField.interactable = false;
        PopulationField.interactable = false;
        StartStopButton.GetComponentInChildren<Text>().text = "Stop Game";
    }

    private void OnGameStop()
    {
        gameIsRunning = false;
        CrossField.interactable = true;
        MutationField.interactable = true;
        PopulationField.interactable = true;
        StartStopButton.GetComponentInChildren<Text>().text = "Start Game";
    }
}
