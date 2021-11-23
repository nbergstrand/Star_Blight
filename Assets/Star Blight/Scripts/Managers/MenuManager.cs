using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject _pauseMenu;

    private void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex != 0 && Input.GetKeyDown(KeyCode.Escape))
        {
            ShowPauseMenu();
        }
    }

    private void ShowPauseMenu()
    {
        if(!_pauseMenu.activeSelf)
            _pauseMenu.SetActive(true);
        else
            _pauseMenu.SetActive(false);


    }

    public void ExitToMainMenu()
    {
        AudioManager.Instance.PlayMenuClickSound();
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        SettingsManager.Instance.FadeIn();
    }

    public void StartGame()
    {
       
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        
        Time.timeScale = 1f;
        AudioManager.Instance.PlayMenuClickSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetDifficulty(int difficulty)
    {
        SettingsManager.Instance.SetDifficulty(difficulty);
    }

    public void AdjustScreenBrightness(float brightness)
    {
        SettingsManager.Instance.AdjustScreenBrightness(brightness);
    }

    public void SetVolume(float newValue)
    {
        AudioManager.Instance.SetVolume(newValue);

    }

    public void PlayMenuClickSound()
    {
        AudioManager.Instance.PlayMenuClickSound();
    }

    public void PlayMenuSliderSound()
    {
        AudioManager.Instance.PlayMenuSliderSound();
    }

}
