using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public bool LevelRunning = false;
    private AudioSource CoinEffect;

    public int CurrentLevelIndex;
    public PlatformLevel CurrentLevel;

    public GameObject HUD;
    public GameObject LevelEndUI;
    public GameObject GameEndUI;

    public GameObject Player;
    public GameObject LevelEndPrefab;
    private GameObject levelEnd;

    public GameObject GoodBreathParticles;
    public GameObject BadBreathParticles;

    public List<PlatformLevel> Levels;

    public AudioSource GoodBreathSound;
    public AudioSource BadBreathSound;
    public AudioSource LevelEndSound;
    public AudioSource GameEndSound;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CoinEffect = this.GetComponent<AudioSource>();
        CurrentLevel = Levels[CurrentLevelIndex];
        LevelRunning = true;
    }

    private void Update()
    {
        if (LevelRunning)
        {
            CurrentLevel.LevelTime += Time.deltaTime;

            if (CurrentLevel.ExhalationCount >= CurrentLevel.ExhalationMax && levelEnd == null)
            {
                CreateLevelEnd();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartNewLevel();
            }
        }
    }


    public void GoodBreathAnimation()
    {
        GoodBreathSound.Stop();
        GoodBreathSound.Play();

        CurrentLevel.ExhalationCount++;

        GameObject particles = Instantiate(GoodBreathParticles);
        particles.transform.position = Player.transform.GetChild(0).position;
        particles.transform.parent = Player.transform;
        Destroy(particles, 2f);
    }

    public void BadBreathAnimation()
    {
        BadBreathSound.Stop();
        BadBreathSound.Play();

        GameObject particles = Instantiate(BadBreathParticles);
        particles.transform.position = Player.transform.GetChild(0).position;
        particles.transform.parent = Player.transform;
        Destroy(particles, 2f);
    }

    // Update is called once per frame
    public void GetCoin()
    {
        CurrentLevel.CoinCount++;
        CoinEffect.Stop();
        CoinEffect.Play();
    }

    public void EndLevel()
    {
        if (CurrentLevelIndex == Levels.Count - 1)
        {
            EndGame();
        }
        else
        {
            LevelRunning = false;

            HUD.SetActive(false);
            LevelEndUI.SetActive(true);

            LevelEndSound.Stop();
            LevelEndSound.Play();
        }
    }

    public void EndGame()
    {
        LevelRunning = false;

        HUD.SetActive(false);
        LevelEndUI.SetActive(false);
        GameEndUI.SetActive(true);

        GameEndSound.Stop();
        GameEndSound.Play();
    }

    public void StartNewLevel()
    {
        LevelRunning = true;
        CurrentLevelIndex++;
        CurrentLevel = Levels[CurrentLevelIndex];

        HUD.SetActive(true);
        LevelEndUI.SetActive(false);

        if (levelEnd != null)
        {
            Destroy(levelEnd);
        }

        Player.transform.position = Vector3.zero;
    }

    public void CreateLevelEnd()
    {
        levelEnd = Instantiate(LevelEndPrefab);
        levelEnd.transform.position = Player.transform.position + Vector3.right * 30f;
    }
}

[System.Serializable]
public class PlatformLevel
{
    public int ExhalationMax = 8;
    public int ExhalationCount = 0;

    public int CoinCount = 0;
    public float LevelTime = 0f;

    public float difficulty = 1f;

    public float MinPlayerSpeed = 4f;
    public float MaxPlayerSpeed = 10f;

    public PlatformLevel()
    {

    }
}
