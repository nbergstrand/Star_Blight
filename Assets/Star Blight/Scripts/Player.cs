using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField]
    float _playerSpeed = 3.5f;

    [SerializeField]
    Transform _projectileSpawnPos;

    [SerializeField]
    Transform _projectilesParent;
       
    [SerializeField]
    float _fireRate;
    float _timeToNextShot;

    [SerializeField]
    int _power = 1;

    [SerializeField]
    int _lives = 1;

    [SerializeField]
    GameObject[] _projectiles;

    [SerializeField]
    FireMode _fireMode;

    [SerializeField]
    GameObject _explosion;

    [SerializeField]
    GameObject _thrusters;

    Animator _animator;
    Renderer _renderer;
    Color _defaultColor;

    BoxCollider _boxCollider;


    void Start()
    {
        transform.position = new Vector3(-25f, 0f, 0f);
        _fireMode = FireMode.standard;
        _animator = GetComponentInChildren<Animator>();
        _renderer = GetComponentInChildren<Renderer>();
        _defaultColor = _renderer.material.color;
        _boxCollider = GetComponentInChildren<BoxCollider>();


    }

    void Update()
    {
              
        PlayerMovement();
        Shoot();
        PlayerAnimation();
        Thrusters();
    }

   

    private void Shoot()
    {
        
        if (Input.GetKey(KeyCode.Space) && Time.time > _timeToNextShot)
        {
            _timeToNextShot = Time.time + _fireRate;

            var projectile = Instantiate(_projectiles[(int)_fireMode]);
            projectile.transform.parent = _projectilesParent;
            projectile.transform.position = _projectileSpawnPos.position;

            AudioManager.Instance.PlayShootSound();

        }
    }

    void PlayerAnimation()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            _animator.SetBool("up", true);
            _animator.SetBool("down", false);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _animator.SetBool("up", false);
            _animator.SetBool("down", true);
        }

       if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            _animator.SetBool("up", false);
            _animator.SetBool("down", false);
        }

    }

    void Thrusters()
    {
        bool keyReleased = true;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            _thrusters.transform.localScale = Vector3.Lerp(_thrusters.transform.localScale, new Vector3(0.2f, 1f, 0.5f), 0.25f);
            _thrusters.transform.localPosition = Vector3.Lerp(_thrusters.transform.localPosition, new Vector3(-3.5f, 0f, 0f), 0.25f);

            keyReleased = false;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _thrusters.transform.localScale = Vector3.Lerp(_thrusters.transform.localScale, new Vector3(0.2f, 0.2f, 0.5f), 0.25f);
            _thrusters.transform.localPosition = Vector3.Lerp(_thrusters.transform.localPosition, new Vector3(-2.5f, 0f, 0f), 0.25f);
            keyReleased = false;
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            keyReleased = true;
        }

        if(keyReleased)
        {
            _thrusters.transform.localScale = Vector3.Lerp(_thrusters.transform.localScale, new Vector3(0.2f, .5f, 0.5f), 0.25f);
            _thrusters.transform.localPosition = Vector3.Lerp(_thrusters.transform.localPosition, new Vector3(-3f, 0, 0f), 0.25f);
        }
    }


    private void PlayerMovement()
    {
        Vector3 newPosition;
        newPosition = transform.position + new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f) * _playerSpeed * Time.deltaTime;
                
        newPosition.y = Mathf.Clamp(newPosition.y, -16f, 16f);

        newPosition.x = Mathf.Clamp(newPosition.x, -30f, 30f);
                
        transform.position = newPosition;
    }
          

    public void DamagePlayer()
    {        
        _power--;
        GameManager.Instance.ResetMultiplier();
        SetWeaponPower(_power);
        StartCoroutine(CameraShake());
        StartCoroutine(PowerDownRoutine());
       
        if(_power <= 0)
        {
            Die();
        }

        
    }


    public void CollectPowerup(PowerupType powerup)
    {
        switch(powerup)
        {
            case PowerupType.weaponPower:
                _power++;
                if(_power > 4)
                {
                    _power = 4;
                    GameManager.Instance.IncreaseScore(1000);
                }
                SetWeaponPower(_power);
                StartCoroutine(PowerUpRoutine());
                break;

            case PowerupType.multiplier:
                GameManager.Instance.Multiplier++;
                UIManager.Instance.UpdateMultiplierUI(GameManager.Instance.Multiplier);
                break;

        }
    }
   

    void Die()
    {
       
        

        

        if(_lives >= 1)
        {
            GameObject explosion = Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(explosion, 1);
            AudioManager.Instance.PlayExplosionSound();
            SetWeaponPower(1);
            _power = 1;
            GameManager.Instance.RestartFromCheckpoint();
            _lives--;
            UIManager.Instance.UpdateLivesUI(_lives);
            transform.position = new Vector3(-25f, 0f, 0f);

        }

        else
        {
            GameObject explosion = Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(explosion, 2);
            AudioManager.Instance.PlayExplosionSound();

            GameManager.Instance.GameOver();
            UIManager.Instance.ShowGameOver();
            SpawnManager.Instance.PauseTimeline();
            Destroy(gameObject);

        }
       
    }

    void SetWeaponPower(int power)
    {
        switch(power)
        {
            case 0:
                break;

            case 1:
                _fireMode = FireMode.standard;
                UIManager.Instance.UpdatePowerUI(power);
                break;

            case 2:
                _fireMode = FireMode.doubleShot;
                UIManager.Instance.UpdatePowerUI(power);
                break;

            case 3:
                _fireMode = FireMode.doubleShotAndHoming;
                UIManager.Instance.UpdatePowerUI(power);
                break;

            case 4:
                _fireMode = FireMode.fireball;
                UIManager.Instance.UpdatePowerUI(power);
                break;

            
        }
    }
   

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "EnemyProjectile")
        {
            DamagePlayer();

            Destroy(other.gameObject);
        }

        
    }


    IEnumerator CameraShake()
    {
        Vector3 camCurrentPos = Camera.main.transform.position;

        for (int i = 0; i < 30; i++)
        {
            float randomX = Random.Range(-.5f, .5f);
            float randomY = Random.Range(-.5f, .5f);

            Camera.main.transform.position = new Vector3(randomX, randomY, camCurrentPos.z);
            yield return null;

        }

        Camera.main.transform.position = camCurrentPos;
    }

    IEnumerator PowerUpRoutine()
    {
        for(int i = 0; i < 5; i++)
        {
            _renderer.material.color = Color.green;
            yield return new WaitForSeconds(0.25f);
            _renderer.material.color = _defaultColor;
            yield return new WaitForSeconds(0.25f);
        }
               
    }

    IEnumerator PowerDownRoutine()
    {
        for (int i = 0; i < 5; i++)
        {
            _renderer.material.color = Color.red;
            _boxCollider.enabled = false;
             yield return new WaitForSeconds(0.25f);
            _renderer.material.color = _defaultColor;
            yield return new WaitForSeconds(0.25f);
            _boxCollider.enabled = true;

        }
    }

    

}

public enum FireMode
{
    standard,
    doubleShot,
    doubleShotAndHoming,
    fireball

}
       
