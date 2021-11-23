using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{

    #region Singleton
    private static UIManager _instance;
    public static UIManager Instance
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
    TMP_Text _scoreText;

    [SerializeField]
    TMP_Text _livesText;

    [SerializeField]
    TMP_Text _powerLevel;

    [SerializeField]
    TMP_Text _multiplier;

    [SerializeField]
    GameObject _gameOverMenu;
               
    [SerializeField]
    TMP_Text _warningText;

    [SerializeField]
    GameObject _winMenu;
    
    [SerializeField]
    TMP_Text _winScoreText;

    private void Awake()
    {
        _instance = this;
    }


    public void UpdateScoreUI(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateLivesUI(int lives)
    {
        _livesText.text = "Lives: " + lives;
    }

    public void UpdatePowerUI(int power)
    {
        switch(power)
        {
            case 1:
                _powerLevel.text = "Power: Lvl 1";
                break;

            case 2:
                _powerLevel.text = "Power: Lvl 2";
                break;

            case 3:
                _powerLevel.text = "Power: Lvl 3";
                break;

            case 4:
                _powerLevel.text = "Power: Lvl 4";
                break;
        }
    }

    public void UpdateMultiplierUI(int multiplier)
    {
        _multiplier.text = "Multiplier: " + multiplier + "x";
    }

    public void ShowGameOver()
    {
       
        _gameOverMenu.SetActive(true);
        
    }

    public void ShowWinScreen(int score)
    {
        _winMenu.SetActive(true);
        _winScoreText.text = score.ToString();
    }
             
    
    public void ShowWarningText()
    {
           
        StartCoroutine("TextFlicker");
    }

    public void HideText()
    {
        _scoreText.enabled = false;
        _livesText.enabled = false;
        _powerLevel.enabled = false;
        _multiplier.enabled = false;
}

    IEnumerator TextFlicker()
    {
        for(int i = 0; i < 3; i++)
        {
            _warningText.enabled = true;
            yield return new WaitForSeconds(1f);
            _warningText.enabled = false;
            yield return new WaitForSeconds(1f);

        }


    }
}
