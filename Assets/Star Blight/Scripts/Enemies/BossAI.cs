using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossAI : MonoBehaviour
{
    [SerializeField]
    float _speed;

    [SerializeField]
    int _scoreAmount;

    [SerializeField]
    GameObject[] _projectiles;

    [SerializeField]
    GameObject _explosionEffect;

    [SerializeField]
    Transform _topSideProjectileSpawnPos;
    [SerializeField]
    Transform _bottomSideProjectileSpawnPos;
    [SerializeField]
    Transform _frontProjectileSpawnPos;

    [SerializeField]
    Transform[] _effectPos;

    [SerializeField]
    GameObject _smokeEffect;

    bool _firstSmoke = false;
    bool _secondSmoke = false;

    float _timeToNextShot;
    float _nextBurstTime;
    float _timeToNextShotSide;

    [SerializeField]
    int _hits;

    int _upperHitsLimit;
    int _lowerHitsLimit;


    bool _isDead = false;
   
    Transform _projectileParent;
    Transform _playerTransform;

    PlayableDirector _playableDirector;
   
    [SerializeField]
    PlayableAsset[] _timeLines;

    Renderer _renderer;
    Color _defaultColor;

    [SerializeField]
    Transform _minionSpawnPos;

    [SerializeField]
    GameObject _minion;

    private void Start()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _defaultColor = _renderer.material.color;
        
        _projectileParent = GameObject.Find("ProjectilesParent").GetComponent<Transform>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        _playableDirector = GetComponent<PlayableDirector>();

        _upperHitsLimit = (int)(_hits * 0.6f);
        _lowerHitsLimit = (int)(_hits * 0.3f);

        DifficultyAdjustments();
     
    }

    void Update()
    {
                
        if (!_isDead)
        {
           
           BossMovement();
        }
    }



        void BossMovement()
        {
            if(transform.position.x > 14f)
                transform.Translate(Vector3.left * _speed * Time.deltaTime);

            if (transform.position.x <= 14f && _playableDirector.enabled == false)
            {
                _playableDirector.enabled = true;
                
            }
        }

    
    private void OnTriggerEnter(Collider other)
    {
            if (other.tag == "Player" && !_isDead)
            {

                if (other.GetComponent<Player>() != null)
                {
                    other.GetComponent<Player>().DamagePlayer();
                }
                

            }

            if (other.tag == "Projectile" && !_isDead)
            {
                Destroy(other.gameObject);
                _hits--;
                StartCoroutine(Hurt());
            
            }

            if(_hits <= 0 && !_isDead)
            {
                _isDead = true;
                _playableDirector.enabled = false;
                StartCoroutine(DyingRoutine());
            }
                
    }

    IEnumerator Hurt()
    {
        if (_hits > _lowerHitsLimit && _hits <= _upperHitsLimit && _firstSmoke == false)
        {
            GameObject _effectA = Instantiate(_smokeEffect, _effectPos[0].position, Quaternion.identity);
            _effectA.transform.parent = _effectPos[0];

            GameObject _effectB = Instantiate(_smokeEffect, _effectPos[2].position, Quaternion.identity);
            _effectB.transform.parent = _effectPos[2];
            _firstSmoke = true;
        }
        else if (_hits <= _lowerHitsLimit && _secondSmoke == false)
        {
            GameObject _effectC = Instantiate(_smokeEffect, _effectPos[4].position, Quaternion.identity);
            _effectC.transform.parent = _effectPos[4];

            GameObject _effectD = Instantiate(_smokeEffect, _effectPos[6].position, Quaternion.identity);
            _effectD.transform.parent = _effectPos[6];

            _secondSmoke = true;
        }

        _renderer.material.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        _renderer.material.color = _defaultColor;
        

    }

    IEnumerator DyingRoutine()
    {
        GameManager.Instance.IncreaseScore(_scoreAmount);

        foreach (var collider in GetComponents<BoxCollider>())
        {
            collider.enabled = false;
        }

        foreach (var effectPos in _effectPos)
        {
            AudioManager.Instance.PlayExplosionSound();
            Instantiate(_explosionEffect, effectPos.position, Quaternion.identity);
            yield return new WaitForSeconds(0.25f);
        }

        foreach (var effectPos in _effectPos)
        {
            AudioManager.Instance.PlayExplosionSound();
            Instantiate(_explosionEffect, effectPos.position, Quaternion.identity);
            yield return new WaitForSeconds(0.25f);
        }

        Destroy(gameObject);
        GameManager.Instance.LevelFinished();
        Destroy(gameObject);

    }

    public void ChangeRoutine()
    {
        if(_hits > _upperHitsLimit)
        {
            return;
        }
        else if (_hits > _lowerHitsLimit && _hits <= _upperHitsLimit)
        {
            _playableDirector.time = 0;
            _playableDirector.playableAsset = _timeLines[1];
            _playableDirector.Play();
        }
        else if (_hits <= _lowerHitsLimit )
        {
            _playableDirector.time = 0;
            _playableDirector.playableAsset = _timeLines[2];
            _playableDirector.Play();
        }
                     
    }

    

    public void SpawnMinion()
    {

        Instantiate(_minion, _minionSpawnPos.position, Quaternion.identity);

    }

    public void ShootFront()
    {
        GameObject projectile = Instantiate(_projectiles[0]);
        projectile.transform.parent = _projectileParent;
        projectile.transform.position = _frontProjectileSpawnPos.position;

        AudioManager.Instance.PlayShootSound();
    }

    public void ShootBottom()
    {
        GameObject projectile = Instantiate(_projectiles[1]);
        projectile.transform.parent = _projectileParent;
        projectile.transform.position = _bottomSideProjectileSpawnPos.position;

        AudioManager.Instance.PlayShootSound();
    }

    public void ShootTop()
    {
        GameObject projectile = Instantiate(_projectiles[2]);
        projectile.transform.parent = _projectileParent;
        projectile.transform.position = _topSideProjectileSpawnPos.position;

        AudioManager.Instance.PlayShootSound();
    }

    void DifficultyAdjustments()
    {
        switch (SettingsManager.Instance.GetDifficulty())
        {
            case 0:
                _scoreAmount /= 2;
                _hits /= 2;
                _lowerHitsLimit /= 2;
                _upperHitsLimit /= 2;
                break;

            case 1:
                break;

            case 2:
                _scoreAmount *= 2;
                _hits *= 2;
                _lowerHitsLimit *= 2;
                _upperHitsLimit *= 2;
                break;


        }
    }

}
