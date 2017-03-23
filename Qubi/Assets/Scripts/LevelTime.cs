﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTime : MonoBehaviour
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
        uiText.text = ScoreManager.Instance.CurrentLevel.LevelTime.ToString("0.0") + " secs";
    }
}
