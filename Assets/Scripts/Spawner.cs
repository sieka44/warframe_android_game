using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject bombPrefab;
    public GameObject lootLockerPrefab;

    private Vector3 spawnPosition;
    private Vector2 spawnVelocity;

    IEnumerator spawnEnemiesProcess;

    int enemyLevel = 1;

    // Start is called before the first frame update

    Vector3 generateBombSpawnPosition()
    {
        float x = Random.Range(-7f, 7f);
        if (x < 0) x = -7f;
        else x = 7f;
        Vector3 spawnPosition = new Vector3(x, Random.Range(-3, 0), 0f);
        return spawnPosition;
    }

    void createNewBomb()
    {
        var newBomb = Instantiate(bombPrefab);
        Vector3 spawnPosition = generateBombSpawnPosition();
        spawnVelocity.Set(Random.Range(5, 7), Random.Range(7, 10));
        if (spawnPosition.x < 0)
        {
            spawnVelocity.x = Random.Range(4, 6);
        }
        else
        {
            spawnVelocity.x = Random.Range(-6, -4);
        }

        newBomb.gameObject.GetComponent<CorpusBombScript>().spawn(spawnPosition, spawnVelocity, enemyLevel);
    }

    void createNewLocker()
    {
        var newLocker = Instantiate(lootLockerPrefab);
        Vector3 spawnPosition = generateBombSpawnPosition();
        spawnVelocity.Set(Random.Range(5, 7), Random.Range(7, 10));
        if (spawnPosition.x < 0)
        {
            spawnVelocity.x = Random.Range(4, 6);
        }
        else
        {
            spawnVelocity.x = Random.Range(-6, -4);
        }

        newLocker.gameObject.GetComponent<CorpusLootLocker>().spawn(spawnPosition, spawnVelocity);
    }

    void Start()
    {
        spawnEnemiesProcess = SpawnEnemies();
        StartCoroutine(spawnEnemiesProcess);
    }

    // Update is called once per frame
    void Update()
    {
        enemyLevel = 1 + (int)(Time.timeSinceLevelLoad / 10);
    }

    void spawnAllFromBottom(int numberOfSpawnedEnemies, bool willSpawnWithBomb, bool willSpawnWithLootLocker)
    {
        for (int i = 0; i < numberOfSpawnedEnemies; i++)
        {
            var newEnemy = Instantiate(enemyPrefab);

            spawnPosition.Set(Random.Range(-3, 3), -5.5f, 0f);
            spawnVelocity.Set(Random.Range(-1, 1), Random.Range(11, 14));
            if (spawnVelocity.x < 0)
            {
                float power = (-5 - spawnPosition.x) / 3;
                spawnVelocity.x = Random.Range(power, 0);
            }
            else
            {
                float power = (5 - spawnPosition.x) / 3;
                spawnVelocity.x = Random.Range(0, power);
            }

            newEnemy.gameObject.GetComponent<CorpusCrewmanEnemyScript>().spawn(enemyLevel, spawnPosition, spawnVelocity);
        }
        if (willSpawnWithBomb) createNewBomb();
        if (willSpawnWithLootLocker) createNewLocker();
    }

    private IEnumerator spawnOneByOneFromBotton(int numberOfSpawnedEnemies, bool willSpawnWithBomb, bool willSpawnWithLootLocker)
    {
        int bombSpawnOrder =Random.Range(1, numberOfSpawnedEnemies);
        int lockerSpawnOrder = Random.Range(1, numberOfSpawnedEnemies);
        for (int i = 0; i < numberOfSpawnedEnemies; i++)
        {
            var newEnemy = Instantiate(enemyPrefab);

            spawnPosition.Set(Random.Range(-3, 3), -5.5f, 0f);
            spawnVelocity.Set(Random.Range(-1, 1), Random.Range(11, 14));
            if (spawnVelocity.x < 0)
            {
                float power = (-5 - spawnPosition.x) / 3;
                spawnVelocity.x = Random.Range(power, 0);
            }
            else
            {
                float power = (5 - spawnPosition.x) / 3;
                spawnVelocity.x = Random.Range(0, power);
            }

            newEnemy.gameObject.GetComponent<CorpusCrewmanEnemyScript>().spawn(enemyLevel, spawnPosition, spawnVelocity);

            if ((willSpawnWithBomb) && (bombSpawnOrder == i)) createNewBomb();
            if ((willSpawnWithLootLocker) && (lockerSpawnOrder == i)) createNewLocker();
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            if (enemies.Length == 0)
            {
                int numberOfSpawnedEnemies = (int)Random.Range(1, 6);
                int spawnScriptNumber = (int)Random.Range(1, 3);

                float bombSpawnPorbability = Random.Range(0f, 100f);
                bool willSpawnWithBomb = false;
                if (bombSpawnPorbability < 15f) willSpawnWithBomb = true;

                float lootLockerSpawnProbability = Random.Range(0f, 100f);
                bool willSpawnWithLootLocker = false;
                if (lootLockerSpawnProbability < 15f) willSpawnWithLootLocker = true;

                switch (spawnScriptNumber)
                {
                    case 1:
                        {
                            spawnAllFromBottom(numberOfSpawnedEnemies, willSpawnWithBomb, willSpawnWithLootLocker);
                            break;
                        }
                    case 2:
                        {
                            StartCoroutine(spawnOneByOneFromBotton(numberOfSpawnedEnemies, willSpawnWithBomb, willSpawnWithLootLocker));
                            
                            break;
                        }
                }          
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void stopTheGame()
    {
        StopCoroutine(spawnEnemiesProcess);
    }
}
