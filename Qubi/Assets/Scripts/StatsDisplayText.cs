using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This behaviour is used to display stats form the game on a Text component
public class StatsDisplayText : MonoBehaviour
{
    private Text textDisplay;
    string stringToDisplay;

    public enum GameStats
    {
        CoinCountCurrentRound,
        CoinCountTotal,
        CoinCountHighScore,
        LevelTime,
        LevelTimeTotal,
        LevelCurrent,
        LevelCount,
        SessionBreathCount,
        SessionSetCount
    }

    public GameStats StatsToDisplay = (GameStats)0;

    private void Start()
    {
        textDisplay = this.GetComponent<Text>();
        stringToDisplay = " ";
    }

    // Update is called once per frame
    void Update()
    {
        switch (StatsToDisplay)
        {
            case GameStats.CoinCountCurrentRound:
                stringToDisplay = ScoreManager.Instance.CurrentLevel.CoinCount.ToString();
                break;

            case GameStats.CoinCountTotal:
                stringToDisplay = ScoreManager.Instance.TotalCoins().ToString();
                break;

            case GameStats.CoinCountHighScore:
                stringToDisplay = ScoreManager.Instance.CoinHighScore.ToString();
                break;

            case GameStats.LevelTime:
                stringToDisplay = ScoreManager.Instance.CurrentLevel.LevelTime.ToString();
                break;

            case GameStats.LevelTimeTotal:
                stringToDisplay = ScoreManager.Instance.LevelTimeTotal().ToString();
                break;

            case GameStats.LevelCurrent:
                stringToDisplay = (ScoreManager.Instance.CurrentLevelIndex + 1).ToString();
                break;

            case GameStats.LevelCount:
                stringToDisplay = ScoreManager.Instance.Levels.Count.ToString();
                break;

            case GameStats.SessionBreathCount:
                stringToDisplay = ScoreManager.Instance.SessionBreathCount.ToString();
                break;

            case GameStats.SessionSetCount:
                stringToDisplay = ScoreManager.Instance.SessionSetCount.ToString();
                break;
        }

        if (stringToDisplay != null)
            textDisplay.text = stringToDisplay;
    }
}
