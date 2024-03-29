﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Powerup : MonoBehaviour
{

    [SerializeField]
    float _speed;

    [SerializeField]
    PowerupType _powerupType;

    Transform _player;

    

    private void Start()
    {
        
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
        if(_player == null)
        {
            Debug.Log("No Player found");

        }


    }

    private void Update()
    {
        transform.Translate(Vector3.left * _speed * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Player")
        {
            AudioManager.Instance.PlayPowerUpSound();

            if (collision.GetComponent<Player>() != null)
            {
                collision.GetComponent<Player>().CollectPowerup(_powerupType);
            }

            
            Destroy(gameObject);
        }

       
    }
}

public enum PowerupType
{
    weaponPower,
    multiplier
    
}