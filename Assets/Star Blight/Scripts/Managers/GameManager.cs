using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameManager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        
    }
    #endregion

    private int _score;
    public bool IsPlayerAlive { get;  set; }
    public int Multiplier { get; set; }


            
    private void Start()
    {
        IsPlayerAlive = true;

        Multiplier = 1;

        SettingsManager.Instance.FadeIn();
       
    }



    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && !IsPlayerAlive)
        {
            SceneManager.LoadScene(1);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            AudioManager.Instance.EnableWinMusic();
        }

    }

    public void GameOver()
    {
        AudioManager.Instance.EnableGameOverMusic();
        IsPlayerAlive = false;
    }

    public void IncreaseScore(int score)
    {
        _score += score * Multiplier;

        UIManager.Instance.UpdateScoreUI(_score);
    }

    public void ResetMultiplier()
    {
        Multiplier = 1;
        UIManager.Instance.UpdateMultiplierUI(Multiplier);
    }

    public void TogglePause()
    {
        AudioManager.Instance.PlayMenuClickSound();

        if (Time.timeScale == 1)
        {
            
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
            
        }
         
                   
    }

    public int GetScore()
    {
        return _score;
    }

    public void SetScore(int score)
    {
        _score = score;
        UIManager.Instance.UpdateScoreUI(score);

    }
        

    public void RestartFromCheckpoint()
    {
        GameObject[] _allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(var enemy in _allEnemies)
        {
            Destroy(enemy);
        }
        SettingsManager.Instance.FadeIn();
        StartCoroutine(SpawnManager.Instance.CheckpointTimeline());
    }

    public void LevelFinished()
    {
        AudioManager.Instance.EnableWinMusic();
        UIManager.Instance.ShowWinScreen(_score);
    }

    public void QuitGame()
    {
        Application.Quit();        
    }

    
}


