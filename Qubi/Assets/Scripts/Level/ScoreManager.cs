using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Level
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance;

        public enum GameStage { SessionSetup, LevelPlaying, LevelEnd, GameEnd }
        public GameStage currentStage = GameStage.SessionSetup;

        // player prefs to load
        public int SessionBreathCount = 8;
        public int SessionSetCount = 3;

        // Levels
        public int CurrentLevelIndex;
        public PlatformLevel CurrentLevel;
        public List<PlatformLevel> Levels;

        // UI
        public GameObject HUD;
        public GameObject LevelSetupUI;
        public GameObject LevelEndUI;
        public GameObject GameEndUI;

        // In-Game objects
        public GameObject Player;
        public GameObject LevelEndPrefab;
        private GameObject levelEnd;

        // Particle Effects
        public GameObject GoodBreathParticles;
        public GameObject BadBreathParticles;

        // Audio
        public AudioSource GoodBreathSound;
        public AudioSource BadBreathSound;
        public AudioSource LevelEndSound;
        public AudioSource GameEndSound;
        private AudioSource CoinEffect;

        // Save keys
        private string sessionBreathCountKey = "BreathsCount";
        private string sessionSetCountKey = "SetsCount";

        // Events
        public delegate void LevelResetEventHandler();
        public event LevelResetEventHandler LevelStartEvent;
        public event LevelResetEventHandler LevelEndEvent;
        public event LevelResetEventHandler GameEndEvent;

        // First thing to be called
        private void Awake()
        {
            Instance = this;
        }

        // Called at the start
        private void Start()
        {
            LoadPlayerPrefs();

            CoinEffect = this.GetComponent<AudioSource>();
        }

        #region SaveLoad
        // Loads the player prefs
        public void LoadPlayerPrefs()
        {
            if (PlayerPrefs.HasKey(sessionBreathCountKey))
                SessionBreathCount = PlayerPrefs.GetInt(sessionBreathCountKey);

            if (PlayerPrefs.HasKey(sessionSetCountKey))
                SessionSetCount = PlayerPrefs.GetInt(sessionSetCountKey);
        }

        // Saves the player prefs
        public void SavePlayerPrefs()
        {
            PlayerPrefs.SetInt(sessionBreathCountKey, SessionBreathCount);
            PlayerPrefs.SetInt(sessionSetCountKey, SessionSetCount);
            PlayerPrefs.Save();
        }
        #endregion

        // Called once per frame
        private void Update()
        {
            if (currentStage == GameStage.LevelPlaying)
            {
                CurrentLevel.LevelTime += Time.deltaTime;

                if (CurrentLevel.ExhalationCount >= CurrentLevel.ExhalationMax && levelEnd == null)
                {
                    CreateLevelEnd();
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ButtonPressed();
            }
        }

        #region Breaths

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

            CurrentLevel.BadBreathCount++;

            GameObject particles = Instantiate(BadBreathParticles);
            particles.transform.position = Player.transform.GetChild(0).position;
            particles.transform.parent = Player.transform;
            Destroy(particles, 2f);
        }

        #endregion

        // handle button presses depending on current game stage
        public void ButtonPressed()
        {
            switch (currentStage)
            {
                case GameStage.SessionSetup:
                    SavePlayerPrefs();
                    CreateLevels();
                    StartNewLevel();
                    break;

                case GameStage.LevelPlaying:

                    break;

                case GameStage.LevelEnd:
                    IncrementLevel();
                    StartNewLevel();
                    break;

                case GameStage.GameEnd:
                    NewSession();
                    break;
            }
        }

        // Called when a coin is touched
        public void GetCoin()
        {
            CurrentLevel.CoinCount++;
            CoinEffect.Stop();
            CoinEffect.Play();
        }

        // shows the new session ui
        public void NewSession()
        {
            currentStage = GameStage.SessionSetup;

            LevelSetupUI.SetActive(true);
            HUD.SetActive(false);
            LevelEndUI.SetActive(false);
            GameEndUI.SetActive(false);
        }

        // Creates the levels at the start of the game, after setting up number of breaths and sets/levels
        public void CreateLevels()
        {
            Levels = new List<PlatformLevel>();
            Levels.Clear();

            for (int i = 0; i < SessionSetCount; i++)
            {
                PlatformLevel newLevel = new PlatformLevel();
                newLevel.ExhalationMax = SessionBreathCount;
                Levels.Add(newLevel);
            }

            CurrentLevelIndex = 0;
            CurrentLevel = Levels[CurrentLevelIndex];
        }

        // Increments the level
        public void IncrementLevel()
        {
            CurrentLevelIndex++;
            CurrentLevel = Levels[CurrentLevelIndex];
        }

        // Begin playing the current Level
        public void StartNewLevel()
        {
            currentStage = GameStage.LevelPlaying;

            if (LevelStartEvent != null)
                LevelStartEvent();

            LevelSetupUI.SetActive(false);
            HUD.SetActive(true);
            LevelEndUI.SetActive(false);

            if (levelEnd != null)
            {
                Destroy(levelEnd);
            }

            Player.transform.position = Vector3.zero;
        }

        // Shows the end of level scores and stops the game
        public void EndLevel()
        {
            if (CurrentLevelIndex == Levels.Count - 1)
            {
                EndGame();
            }
            else
            {
                currentStage = GameStage.LevelEnd;

                if (LevelEndEvent != null)
                    LevelEndEvent();

                LevelSetupUI.SetActive(false);
                HUD.SetActive(false);
                LevelEndUI.SetActive(true);
                GameEndUI.SetActive(false);

                LevelEndSound.Stop();
                LevelEndSound.Play();
            }
        }

        public void EndGame()
        {
            currentStage = GameStage.GameEnd;

            if (GameEndEvent != null)
                GameEndEvent();

            LevelSetupUI.SetActive(false);
            HUD.SetActive(false);
            LevelEndUI.SetActive(false);
            GameEndUI.SetActive(true);

            GameEndSound.Stop();
            GameEndSound.Play();
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
        public int BadBreathCount = 0;

        public int CoinCount = 0;
        public float LevelTime = 0f;

        public float difficulty = 1f;

        public float MinPlayerSpeed = 8f;
        public float MaxPlayerSpeed = 16f;

        public PlatformLevel()
        {

        }
    }
}