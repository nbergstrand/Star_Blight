using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class Enemy : MonoBehaviour
{
    [SerializeField]
    float _speed;

    [SerializeField]
    int _hits = 1;

    [SerializeField]
    int _scoreAmount;

    [SerializeField]
    GameObject _projectile;

    [SerializeField]
    Transform _projectileSpawnPos;

    float _timeToNextShot;

    bool _isDead = false;
    public bool IsDead
    {
        get { return _isDead; }
    }

    PlayableDirector _playable;

    Renderer _mainRenderer;
    Renderer[] _allRenderers;

    Transform _projectileParent;

    [SerializeField]
    EnemyType _enemyType;

    float _startPosY;

    [SerializeField]
    float _waveMovementSpeed;
    [SerializeField]
    float _waveMovementAmount;

    [SerializeField]
    GameObject _explosion;

    [SerializeField]
    GameObject _powerUp;

    [SerializeField]
    Transform _powerUpPos;

    bool _gotTarget = false;
    
    [SerializeField]
    Transform _target;

    Player _player;

    Color defaultColor = new Color();

    private void Start()
    {
         
        _player = GameObject.FindObjectOfType<Player>();

        _startPosY = transform.position.y;
              
        if (_enemyType == EnemyType.Kamikaze)
            InvokeRepeating("LookForTarget", 0f, 0.25f);

        _playable = GetComponent<PlayableDirector>();
        
        _timeToNextShot = Time.time + Random.Range(4f, 8f);
       
        _projectileParent = GameObject.Find("ProjectilesParent").GetComponent<Transform>();

        _mainRenderer = GetComponentInChildren<Renderer>();
        _allRenderers = GetComponentsInChildren<Renderer>();
        defaultColor = _mainRenderer.material.color;

        DifficultyAdjustments();

    }

    void Update()
    {
        if (!_isDead)
        {
            if (_enemyType == EnemyType.Shooter || _enemyType == EnemyType.MidBoss)
                Shoot();

           

        }

        EnemyMovement();
    }

    void EnemyMovement()
    {

        switch (_enemyType)
        {
            case EnemyType.Passive:
                transform.Translate(Vector3.left * _speed * Time.deltaTime);
                 break;

            case EnemyType.Shooter:
                transform.Translate(Vector3.left * _speed * Time.deltaTime);
                break;
                            

            case EnemyType.Kamikaze:
                if (_target == null)
                {
                    transform.Translate(Vector3.left * _speed * Time.deltaTime);
                    break;
                }
                else
                {
                    float rotationSpeed = 100;

                    Quaternion rotatation = Quaternion.FromToRotation(Vector3.left, _target.position - transform.position);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotatation, rotationSpeed * Time.deltaTime);
                    
                    transform.Translate(Vector3.left * _speed * Time.deltaTime, Space.Self);
                }
                break;

            case EnemyType.Waver:
                transform.Translate(Vector3.left * _speed * Time.deltaTime);
                transform.localPosition = new Vector3(transform.position.x, _startPosY + WaveMovement(_waveMovementSpeed, _waveMovementAmount), transform.localPosition.z);

                break;

            case EnemyType.MidBoss:
                transform.Translate(Vector3.left * _speed * Time.deltaTime);
                if (transform.position.x <= 16 && _playable.enabled == false)
                {
                    _playable.enabled = true;
                    transform.position = new Vector3(16f, 0f, 0f);
;                    _speed = 0;
                }
                break;

        }



        if (transform.position.x < -36f && !_isDead)
            Destroy(gameObject);
    }

    
    private void Shoot()
    {


        if (Time.time > _timeToNextShot && GameManager.Instance.IsPlayerAlive)
        {

            _timeToNextShot = Time.time + Random.Range(4f, 8f);

            GameObject projectile = Instantiate(_projectile, _projectileSpawnPos.position, Quaternion.FromToRotation(Vector3.right, _player.transform.position -_projectileSpawnPos.position));
            projectile.transform.parent = _projectileParent;
                        
            AudioManager.Instance.PlayShootSound();
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(TakeDamage());

            if (other.GetComponent<Player>() != null)
            {
                other.GetComponent<Player>().DamagePlayer();
            }
                 

        }

        if (other.tag == "Projectile")
        {
            Destroy(other.gameObject);

            
            StartCoroutine(TakeDamage());

          

        }
    }


    float WaveMovement(float speed, float amount)
    {
        return (((Mathf.Cos((Time.time * speed) + 1) / 2) * amount));
    }

       

    private void LookForTarget()
    {
       
        if (_gotTarget)
            return;
                
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        float chaseDistance = 100;
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer < chaseDistance)
        {
            _target = player.transform;
            _gotTarget = true;

        }

    }

       
    

    IEnumerator TakeDamage()
    {
        
        _mainRenderer.material.color = Color.red;
        _hits--;

        if (_hits <= 0)
            Die();
        AudioManager.Instance.PlayHitSound();
        yield return new WaitForSeconds(.25f);

        _mainRenderer.material.color = defaultColor;
    }

    void Die()
    {
        GameManager.Instance.IncreaseScore(_scoreAmount);

        foreach(var renderer in _allRenderers)
        {
            renderer.enabled = false;
        }
               
        if(_powerUp != null)
        {
            Instantiate(_powerUp, _powerUpPos.position, Quaternion.identity);
        }

        GameObject explosionGO = Instantiate(_explosion, transform.position, Quaternion.identity);
        Destroy(explosionGO, 2f);

        AudioManager.Instance.PlayExplosionSound();
        _isDead = true;
        _playable.enabled = false;
        _speed = 0;

        foreach (var collider in GetComponents<BoxCollider>())
        {
            collider.enabled = false;
        }

        if(_enemyType == EnemyType.MidBoss)
        {
            SpawnManager.Instance.ResumeTimeline();
        }

        Destroy(gameObject, 3f);
    }

    void DifficultyAdjustments()
    {
        switch(SettingsManager.Instance.GetDifficulty())
        {
            case 0:
               _scoreAmount /= 2;
                _hits /= 2;
                if (_hits <= 0)
                    _hits = 1;
                break;

            case 1:
                break;

            case 2:
                _scoreAmount *= 2;
                _hits *= 2;
                break;


        }
    }
    
}


public enum EnemyType
{
    Passive,
    Shooter,
    Kamikaze,
    Waver,
    MidBoss

    
}