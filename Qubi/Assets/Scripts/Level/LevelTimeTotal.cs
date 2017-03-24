using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimeTotal : MonoBehaviour
{
    private Text uiText;

    // Use this for initialization
    void Start()
    {
        uiText = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        float newTimeTotal = 0f;

        foreach (PlatformLevel level in ScoreManager.Instance.Levels)
        {
            newTimeTotal += level.LevelTime;
        }

        uiText.text = newTimeTotal.ToString("0.0") + " secs";
    }
}
