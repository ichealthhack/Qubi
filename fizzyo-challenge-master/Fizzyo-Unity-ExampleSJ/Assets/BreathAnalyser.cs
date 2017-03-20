﻿
using System;

public class ExhalationCompleteEventArgs : EventArgs
{
    private float breathLength = 0;
    private int breathCount = 0;
    private float exhaledVolume = 0;
    private bool breathGood = false;

    public ExhalationCompleteEventArgs(float breathLength, int breathCount, float exhaledVolume, bool breathGood)
    {
        this.breathLength = breathLength;
        this.breathCount = breathCount;
        this.exhaledVolume = exhaledVolume;
        this.breathGood = breathGood;
    }

    /// The length of the exhaled breath in seconds
    public float Breathlength
    {
        get
        {
            return breathLength;
        }
    }

    /// The total number of exhaled breaths this session
    public int BreathCount
    {
        get
        {
            return breathCount;
        }
    }

    /// The total exhaled volume of this breath
    public float ExhaledVolume
    {
        get
        {
            return exhaledVolume;
        }
    }

    /// Returns true if the breath was within the toterance of a 'good breath'
    public bool BreathGood
    {
        get
        {
            return breathGood;
        }
    }
}

public delegate void ExhalationCompleteEventHandler(object sender, ExhalationCompleteEventArgs e);

public class BreathAnalyser
{
    private float breathLength = 0;
    private int breathCount = 0;
    private float exhaledVolume = 0;
    private bool isExhaling = false;
    private float maxPressure = 0;
    private float maxBreathLength = 0;
    private const float kTollerance = 0.80f;
    private float minBreathThreshold = .05f;

    public event ExhalationCompleteEventHandler ExhalationComplete;

    /// The length of the current exhaled breath in seconds
    public float Breathlength
    {
        get
        {
            return this.breathLength;
        }
    }

    /// The total number of exhaled breaths this session
    public int BreathCount
    {
        get
        {
            return this.breathCount;
        }
    }

    /// The total exhaled volume for this breath
    public float ExhaledVolume
    {
        get
        {
            return this.exhaledVolume;
        }
    }

    /// True if the user is exhaling
    public bool IsExhaling
    {
        get
        {
            return this.isExhaling;
        }
    }

    /// The maximum pressure recorded during calibration
    public float MaxPressure
    {
        get
        {
            return this.maxPressure;
        }
        set
        {
            this.maxPressure = value;
        }
    }


    /// The maximum breath length recorded during calibration
    public float MaxBreathLength
    {
        get
        {
            return this.maxBreathLength;
        }
        set
        {
            this.maxBreathLength = value;
        }
    }

    /// Adds a sample to the BreathAnalyser
    public void AddSample(float dt, float value)
    {
        if (this.isExhaling && value < this.minBreathThreshold)
        {
            // Notify the delegate that the exhaled breath is complete
            bool isBreathGood = this.IsBreathGood(this.breathLength, this.maxBreathLength, this.exhaledVolume, this.maxPressure);
            ExhalationCompleteEventArgs eventArgs = new ExhalationCompleteEventArgs(
                this.breathLength,
                this.breathCount,
                this.exhaledVolume,
                isBreathGood);
            this.OnExhalationComplete(this, eventArgs);

            // Reset the state
            this.breathLength = 0;
            this.exhaledVolume = 0;
            this.isExhaling = false;
            this.breathCount++;
        }
        else if (value >= this.minBreathThreshold)
        {
            this.isExhaling = true;
            this.exhaledVolume += dt * value;
            this.breathLength += dt;
        }
    }

    /// Returns true if the breath was within the toterance of a 'good breath'
    public bool IsBreathGood(float breathLength, float maxBreathLength, float exhaledVolume, float maxPressure)
    {
        bool breathGood = false;

        // Is the breath the right within 80% of the correct length
        breathGood =  breathLength > BreathAnalyser.kTollerance * maxBreathLength;

        // Is the average pressure within 80% of the max pressure
        if (this.breathLength > 0)
        {
            breathGood = breathGood && ((exhaledVolume / breathLength) > BreathAnalyser.kTollerance * maxPressure);
        }

        return breathGood;
    }

    /// Resest the BreathAnalyser
    public void ResetSession()
    {
        this.breathLength = 0;
        this.breathCount = 0;
        this.exhaledVolume = 0;
        this.isExhaling = false;
    }

    /// Invoke the event - called whenever exhalation finishes
    protected virtual void OnExhalationComplete(object sender, ExhalationCompleteEventArgs e)
    {
        if (ExhalationComplete != null)
        {
            ExhalationComplete(this, e);
        }
    }
}
