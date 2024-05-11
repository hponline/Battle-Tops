using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject[] powerupPrefabs;
    public GameObject bossPrefabs;
    public GameObject[] miniEnemyPrefabs;

    public int bossRound;
    public int enemyCount;
    public int waveNumber = 1;

    private float spawnRange = 9.0f;

    public static int roundSayac = 1;

    public float delayTime = 3.0f;

    public bool isStart = false;

    void Start()
    {
        StartCoroutine(Delay());
        

    }


    void Update()
    {
        if (isStart == true)
        {
            // Düþman sayacý
            enemyCount = FindObjectsOfType<Enemy>().Length;
            if (enemyCount == 0)
            {

                waveNumber++;

                if (waveNumber % bossRound == 0)
                {
                    SpawnBossWave(waveNumber);
                }
                else
                {
                    SpawnEnemyWave(waveNumber);
                }

                int randomPowerup = Random.Range(0, powerupPrefabs.Length);
                Instantiate(powerupPrefabs[randomPowerup], GenerateSpawnPosition(), powerupPrefabs[randomPowerup].transform.rotation);

                roundSayac += 1;
            }

        }

    }

    // Delay
    public IEnumerator Delay()
    {
        yield return new WaitForSeconds(delayTime);
        // Düþman spawn
        SpawnEnemyWave(waveNumber);
        int randomPowerup = Random.Range(0, powerupPrefabs.Length);
        Instantiate(powerupPrefabs[randomPowerup], GenerateSpawnPosition(), powerupPrefabs[randomPowerup].transform.rotation);

        isStart = true;
    }

    // Düþman spawn manager
    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int randomEnemy = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[randomEnemy], GenerateSpawnPosition(), enemyPrefabs[randomEnemy].transform.rotation);
        }
    }

    // Random spawn fonksiyonu
    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);

        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);

        return randomPos;
    }

    // Boss spawn
    void SpawnBossWave(int currentRound)
    {
        int miniEnemysToSpawn;
        if (bossRound != 0)
        {
            miniEnemysToSpawn = currentRound / bossRound;
        }
        else
        {
            miniEnemysToSpawn = 1;
        }

        var boss = Instantiate(bossPrefabs, GenerateSpawnPosition(), bossPrefabs.transform.rotation);
        boss.GetComponent<Enemy>().miniEnemySpawnCount = miniEnemysToSpawn;
    }

    // Mini Düþman Spawn eder
    public void SpawnMiniEnemy(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int randomMini = Random.Range(0, miniEnemyPrefabs.Length);
            Instantiate(miniEnemyPrefabs[randomMini], GenerateSpawnPosition(), miniEnemyPrefabs[randomMini].transform.rotation);
        }
    }
}
