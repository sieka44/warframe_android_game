using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject bombPrefab;
    private Vector3 spawnPosition;
    private Vector2 spawnVelocity;

    List<Material> bodyMaterials = new List<Material>();
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
        newBomb.transform.position = generateBombSpawnPosition();
        spawnVelocity.Set(Random.Range(5, 7), Random.Range(7, 10));

        if (newBomb.transform.position.x < 0)
        {
            spawnVelocity.x = Random.Range(4, 6);
        }
        else
        {
            spawnVelocity.x = Random.Range(-6, -4);
        }
        newBomb.gameObject.GetComponent<CorpusBombScript>().setStartVelocity(spawnVelocity);
    }

    void Start()
    {
        //enemyPrefab = Resources.Load("CorpusCrewmanEnemy", typeof(GameObject)) as GameObject;
        for (int i = 1; i < 11; i++)
        {
            bodyMaterials.Add(Resources.Load<Material>("Materials/CorpusCrewman/body" + i));
        }
        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnAllFromBottom(int numberOfSpawnedEnemies, bool willSpawnWithBomb)
    {
        for (int i = 0; i < numberOfSpawnedEnemies; i++)
        {
            var newEnemy = Instantiate(enemyPrefab);
            spawnPosition.Set(Random.Range(-3, 3), -5.5f, 0f);
            newEnemy.transform.position = spawnPosition;
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
            newEnemy.transform.Find("corpusCrewmanContainer").Find("corpusCrewman").Find("crewman_body").GetComponent<Renderer>().material = bodyMaterials[(int)Random.Range(0, bodyMaterials.Count - 1)];
            newEnemy.gameObject.GetComponent<CorpusCrewmanEnemyScript>().setVelocity(spawnVelocity);
        }
        if (willSpawnWithBomb) createNewBomb();
    }

    private IEnumerator spawnOneByOneFromBotton(int numberOfSpawnedEnemies, bool willSpawnWithBomb)
    {
        int bombSpawnOrder =Random.Range(1, numberOfSpawnedEnemies);
        for (int i = 0; i < numberOfSpawnedEnemies; i++)
        {
            var newEnemy = Instantiate(enemyPrefab);
            spawnPosition.Set(Random.Range(-3, 3), -5.5f, 0f);
            newEnemy.transform.position = spawnPosition;
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
            newEnemy.transform.Find("corpusCrewmanContainer").Find("corpusCrewman").Find("crewman_body").GetComponent<Renderer>().material = bodyMaterials[(int)Random.Range(0, bodyMaterials.Count - 1)];
            newEnemy.gameObject.GetComponent<CorpusCrewmanEnemyScript>().setVelocity(spawnVelocity);

            if ((willSpawnWithBomb) && (bombSpawnOrder == i)) createNewBomb();
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while(true)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            if (enemies.Length == 0)
            {
                int numberOfSpawnedEnemies = (int)Random.Range(1, 6);
                int spawnScriptNumber = (int)Random.Range(1, 3);
                Debug.Log(spawnScriptNumber);

                float bombSpawnPorbability = Random.Range(0f, 100f);
                bool willSpawnWithBomb = false;
                if (bombSpawnPorbability < 10f) willSpawnWithBomb = true;

                switch (spawnScriptNumber)
                {
                    case 1:
                        {
                            spawnAllFromBottom(numberOfSpawnedEnemies, willSpawnWithBomb);
                            break;
                        }
                    case 2:
                        {
                            StartCoroutine(spawnOneByOneFromBotton(numberOfSpawnedEnemies, willSpawnWithBomb));
                            
                            break;
                        }
                }          
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
