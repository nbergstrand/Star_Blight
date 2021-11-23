using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingsManager : MonoBehaviour
{

    #region Singleton
    private static SettingsManager _instance;
    public static SettingsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Failed to load UIManager");
            }

            return _instance;
        }
    }
    #endregion

    [SerializeField]
    Image _screenBrightness;

    [SerializeField]
    int _difficulty = 1;

    Animator _animator;

    private void Awake()
    {
        if (_instance)
            Destroy(gameObject);
        else
            _instance = this;

        DontDestroyOnLoad(this);

    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        _animator = GetComponent<Animator>();
        FadeIn();
    }

    public void AdjustScreenBrightness(float brightness)
    {
        
        _screenBrightness.color = new Color(0f, 0f, 0f, brightness);
    }

    public void SetDifficulty(int difficulty)
    {
        _difficulty = difficulty;
    }

    public int GetDifficulty()
    {
        return _difficulty;
    }
        

    public void FadeIn()
    {
        _animator.SetTrigger("FadeIn");

    }

   

}


