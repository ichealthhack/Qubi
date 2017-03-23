using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FizzyoBreath : MonoBehaviour
{
    BreathAnalyser breathAnalyser = new BreathAnalyser();

    public float FizzyoPressure;
    public float breathVolume;

    public Image OuterBar;
    public float OuterBarFill = 0f;
    public Image InnerBar;
    public float InnerBarFill = 0f;

    // Use this for initialization
    void Start()
    {
        breathAnalyser.MaxPressure = 0.4f;
        breathAnalyser.MaxBreathLength = 3f;

        breathAnalyser.ExhalationComplete += BreathAnalyser_ExhalationComplete;
    }

    private void BreathAnalyser_ExhalationComplete(object sender, ExhalationCompleteEventArgs e)
    {
        if (e.IsBreathGood)
        {
            ScoreManager.Instance.GoodBreathAnimation();
        }
        else
        {
            ScoreManager.Instance.BadBreathAnimation();
        }
    }


    // Update is called once per frame
    void Update()
    {
        breathAnalyser.AddSample(Time.deltaTime, FizzyoDevice.Instance().Pressure());

        FizzyoPressure = FizzyoDevice.Instance().Pressure();

        breathVolume = breathAnalyser.ExhaledVolume;

        // Set Visuals
        OuterBarFill = breathAnalyser.Breathlength / breathAnalyser.MaxBreathLength;
        OuterBar.fillAmount = OuterBarFill;

        InnerBarFill = (float)ScoreManager.Instance.CurrentLevel.ExhalationCount / (float)ScoreManager.Instance.CurrentLevel.ExhalationMax;
        InnerBar.fillAmount = InnerBarFill;
    }
}
