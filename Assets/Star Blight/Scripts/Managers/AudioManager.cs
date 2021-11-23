using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Failed to load AudioManager");
            }

            return _instance;
        }
    }
    #endregion

    //Sound effects
    [SerializeField]
    AudioClip shoot, powerUp, explosion, menuClick, menuSlider, hit;
    
    [SerializeField]
    float sfxVolume;
       
    [SerializeField]
    AudioSource backgroundMusicSource;

    [SerializeField]
    AudioClip[] backgroundMusicClips;


    AudioSource audioSource;

    private void Awake()
    {
        if (_instance)
            Destroy(gameObject);
        else
            _instance = this;

        DontDestroyOnLoad(this);

    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceeneLoaded;
        audioSource = GetComponent<AudioSource>();
        SetBackgroundMusic(); 
    }

    void OnSceeneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetBackgroundMusic();
    }


    public void SetBackgroundMusic()
    {
        switch(SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                backgroundMusicSource.clip = backgroundMusicClips[SceneManager.GetActiveScene().buildIndex];
                backgroundMusicSource.Play();
                break;

            case 1:
                backgroundMusicSource.clip = backgroundMusicClips[SceneManager.GetActiveScene().buildIndex];
                backgroundMusicSource.Play();
                break;

            default:
                backgroundMusicSource.clip = backgroundMusicClips[1];
                backgroundMusicSource.Play();
                break;

        }
        

    }

    public void EnableWinMusic()
    {
        backgroundMusicSource.clip = backgroundMusicClips[2];
        backgroundMusicSource.Play();
    }

    public void EnableGameOverMusic()
    {
        backgroundMusicSource.clip = backgroundMusicClips[3];
        backgroundMusicSource.Play();
    }


    public void SetVolume(float newValue)
    {
        sfxVolume = newValue;
        backgroundMusicSource.volume = newValue;

    }

    public void PlayShootSound()
    {
       audioSource.PlayOneShot(shoot, sfxVolume);
    }

    public void PlayPowerUpSound()
    {
       audioSource.PlayOneShot(powerUp, sfxVolume);
    }

    public void PlayExplosionSound()
    {
        audioSource.PlayOneShot(explosion, sfxVolume);
    }

    public void PlayMenuClickSound()
    {
        audioSource.PlayOneShot(menuClick, sfxVolume);
    }

    public void PlayMenuSliderSound()
    {
        audioSource.PlayOneShot(menuSlider, sfxVolume);
    }

    public void PlayHitSound()
    {
        audioSource.PlayOneShot(hit, sfxVolume);
    }

}
