using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    [SerializeField]
    private float _speed;
    
    [SerializeField]
    Transform target;
    

    [SerializeField]
    bool _homing;

    bool _gotTarget;
    
    void Start()
    {
        if(_homing == true)
            InvokeRepeating("UpdateTarget", 0f, 0.25f);
    }
    
    void Update()
    {
        MoveProjectile();
        DestroyProjectile();
    }

    void DestroyProjectile()
    {
        if (transform.position.x > 35f)
        {
            if(transform.parent == null || transform.parent.name == "ProjectilesParent")
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(transform.parent.gameObject);
            }
        }
        else if (transform.position.x < -35f)
        {
            if (transform.parent == null || transform.parent.name == "ProjectilesParent")
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(transform.parent.gameObject);
            }
        }
        else if (transform.position.y > 35f)
        {
            if (transform.parent == null || transform.parent.name == "ProjectilesParent")
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(transform.parent.gameObject);
            }
        }
        else if (transform.position.y < -35f)
        {
            if (transform.parent == null || transform.parent.name == "ProjectilesParent")
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }

    private void MoveProjectile()
    {
                 

        if(target == null)
        {

            transform.Translate(Vector3.right * _speed * Time.deltaTime, Space.Self);

        }
        else
        {
            
            transform.right = target.position - transform.position;

            if (target.GetComponent<Enemy>().IsDead)
                target = null;

            transform.Translate(Vector3.right * _speed * Time.deltaTime, Space.Self);
            

            
        }

    }

    private void UpdateTarget()
    {
        if (_gotTarget)
            return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float shortestDistance = Mathf.Infinity;

        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<Enemy>() == null || enemy.GetComponent<Enemy>().IsDead)
                continue;

            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;

                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
            _gotTarget = true;
        }
        else
        {
            target = null;
        }

        
    }

   

}
