using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RowScoreGUI : MonoBehaviour
{
    [SerializeField]
    private Text Count;
    [SerializeField]
    private Text Score;

    public void SetScore(int pos, float score)
    {
        Count.text = "" + pos;

        int min = (int)(score / 60);
        score -= min * 60000;
        int sec = (int)score;
        score -= sec;
        int cent = (int)(score * 100);
        Score.text = min + "'" + sec + "''" + cent;
    }
}
