using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinCountTotal : MonoBehaviour
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
        int newCount = 0;

        foreach (PlatformLevel level in ScoreManager.Instance.Levels)
        {
            newCount += level.CoinCount;
        }

        uiText.text = newCount.ToString();
    }
}
