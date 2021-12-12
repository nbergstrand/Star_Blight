using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class SpawnManager : MonoBehaviour
{

    #region Singleton
    private static SpawnManager _instance;
    public static SpawnManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Failed to load SpawnManager");
            }

            return _instance;
        }
    }
    #endregion

    private bool _isPlayerAlive = true;

    [SerializeField]
    private GameObject[] _enemies;

    [SerializeField]
    private Transform[] _spawnPoints;

    [SerializeField]
    private int _checkPointTime;

    private PlayableDirector _playableDirector;
    private SignalReceiver _signalReceiver;

    private int _checkpointScore;
    private bool _checkpointReached = false;
    
    private void Awake()
    {
        _instance = this;
        _playableDirector = GetComponent<PlayableDirector>();
        _signalReceiver = GetComponent<SignalReceiver>();
    }
         
    public void PauseTimeline()
    {
        _playableDirector.enabled = false;
    }

    public void ResumeTimeline()
    {
        _playableDirector.enabled = true;
        _playableDirector.Play();
    }

    public IEnumerator CheckpointTimeline()
    {
        if(_checkpointReached)
        {
            _signalReceiver.enabled = false;
            _playableDirector.time = _checkPointTime;
            _playableDirector.enabled = true;
             yield return new WaitForSeconds(0.5f);
            _signalReceiver.enabled = true;
            GameManager.Instance.SetScore(_checkpointScore);
            _playableDirector.Play();
        }
        else
        {
            _signalReceiver.enabled = false;
            _playableDirector.time = 0;
            _playableDirector.enabled = true;
            yield return new WaitForSeconds(0.5f);
            _signalReceiver.enabled = true;
            GameManager.Instance.SetScore(0);
            _playableDirector.Play();
        }
       


    }

    public void OnPlayerDeath()
    {
        _isPlayerAlive = false;
        _playableDirector.enabled = false;
    }

    public void ShowWarning()
    {
        UIManager.Instance.ShowWarningText();

    }
    

    public void Type_A_Pattern_1()
    {
        if(_isPlayerAlive)
        {
            Instantiate(_enemies[0], _spawnPoints[0].position, Quaternion.identity);

        }
    }

    public void Type_A_Pattern_2()
    {
        if (_isPlayerAlive)
        {
            Instantiate(_enemies[1], _spawnPoints[2].position, Quaternion.identity);

        }
    }

    public void Wave_Group()
    {
        if (_isPlayerAlive)
        {
            for(int i = 0; i < 5; i++)
            {
                Instantiate(_enemies[2], _spawnPoints[1].position + new Vector3(i * 7, 0, 0), Quaternion.identity);
            }
            

        }
    }


    public void Mid_Level_Boss_First()
    {
        if (_isPlayerAlive)
        {
            Instantiate(_enemies[3], _spawnPoints[1].position, Quaternion.identity);

        }
    }

    public void Mid_Level_Boss_Second()
    {
        if (_isPlayerAlive)
        {
            Instantiate(_enemies[11], _spawnPoints[0].position, Quaternion.Euler(new Vector3(90f, 180f, 0f)));
            Instantiate(_enemies[12], _spawnPoints[2].position, Quaternion.Euler(new Vector3(90f, 180f, 0f)));


        }
    }

    public void Boss_Level_1()
    {
        if (_isPlayerAlive)
        {
            Instantiate(_enemies[4], _spawnPoints[1].position, Quaternion.identity);

        }
    }


    public void PowerUp_Upgrade()
    {
        if (_isPlayerAlive)
        {
            Instantiate(_enemies[5], _spawnPoints[1].position, Quaternion.identity);

        }
    }

    public void PowerUp_Multiplier()
    {
        if (_isPlayerAlive)
        {
            Instantiate(_enemies[6], _spawnPoints[1].position, Quaternion.identity);

        }
    }

    public void Type_C_Pattern_1()
    {
        if (_isPlayerAlive)
        {
            Instantiate(_enemies[7], _spawnPoints[4].position, Quaternion.identity);

        }
    }

    public void Type_C_Pattern_2()
    {
        if (_isPlayerAlive)
        {
            Instantiate(_enemies[8], _spawnPoints[3].position, Quaternion.identity);

        }
    }

    public void Type_D_Pattern_1()
    {
        if (_isPlayerAlive)
        {
            Instantiate(_enemies[9], _spawnPoints[3].position, Quaternion.identity);

        }
    }

    public void Type_D_Pattern_2()
    {
        if (_isPlayerAlive)
        {
            Instantiate(_enemies[10], _spawnPoints[4].position, Quaternion.identity);

        }
    }

    public void Checkpoint()
    {
        _checkpointReached = true;
        _checkpointScore = GameManager.Instance.GetScore();
    }

}



