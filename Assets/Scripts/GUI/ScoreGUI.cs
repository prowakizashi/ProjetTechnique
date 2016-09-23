using UnityEngine;
using System.Collections;

public class ScoreGUI : MonoBehaviour
{

    [SerializeField]
    private GameObject child;
    [SerializeField]
    private GameObject scoreList;
    [SerializeField]
    private GameObject rowPrefab;

    private GameLogic gameLogic;
    
	void Start ()
    {
        OpenWindow(false);
        gameLogic = GameObject.FindGameObjectWithTag("GameLogic").GetComponent<GameLogic>();
        gameLogic.OnGameStart = () => { OpenWindow(true); };
        gameLogic.OnGameStop = () => { OpenWindow(false); };
        gameLogic.OnExperienceStart = () => { ClearScores(); };
        gameLogic.OnSequenceStop = AddScore;
    }

    private void OpenWindow(bool open)
    {
        child.SetActive(open);
    }

    private void ClearScores()
    {
        for (int i = 0; i < scoreList.transform.childCount; ++i)
        {
            Destroy(scoreList.transform.GetChild(i).gameObject);
        }
    }

    private void AddScore(int pos, float score)
    {
        var obj = Instantiate(rowPrefab);
        obj.transform.SetParent(scoreList.transform);
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<RowScoreGUI>().SetScore(pos, score);
    }
}
