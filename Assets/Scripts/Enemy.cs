using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2.0f;
    public int miniEnemySpawnCount;
    public bool isBoss = false;
    public float spawnInterval;

    private float nextSpawn;
    private Rigidbody enemyRb;
    private GameObject player;    
    private SpawnManager spawnManager;


    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");

        if (isBoss)
        {
            spawnManager = FindObjectOfType<SpawnManager>();
        }
    }

    void Update()
    {
        TargetTracking();
        BossSpawn();
        DestObje();
    }


    // Hedef Takip
    public void TargetTracking()
    {
        // Enemy objemiz takip etmesi için gameObjemizin konumundan
        // kendi konumunu çýkarýyor ve sürekli olarak bizi takip ediyor.
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed);
    }

    // Aþaðý düþenleri yok eder.
    public void DestObje()
    {
        // Aþaðý düþenleri yok eder.
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    // Boss Spawn
    public void BossSpawn()
    {
        if (isBoss)
        {
            if (Time.time > nextSpawn)
            {
                nextSpawn = Time.time + spawnInterval;
                spawnManager.SpawnMiniEnemy(miniEnemySpawnCount);
            }
        }
    }
}
