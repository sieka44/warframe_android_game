using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    private Vector3 spawnPosition;
    private Vector2 spawnVelocity;

    List<Material> bodyMaterials = new List<Material>();
    // Start is called before the first frame update
    void Start()
    {
        //enemyPrefab = Resources.Load("CorpusCrewmanEnemy", typeof(GameObject)) as GameObject;
        for (int i = 1; i < 11; i++)
        {
            bodyMaterials.Add(Resources.Load<Material>("Materials/CorpusCrewman/body" + i));
        }
        InvokeRepeating("SpawnEnemies", 0f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnEnemies()
    {
        int numberOfSpawnedEnemies = (int)Random.Range(1, 5);
        for(int i = 0; i < numberOfSpawnedEnemies; i++)
        {
            var newEnemy = Instantiate(enemyPrefab);
            spawnPosition.Set(Random.Range(-3, 3), -5.5f, 0f);
            newEnemy.transform.position = spawnPosition;
            spawnVelocity.Set(Random.Range(-1, 1), Random.Range(11, 14));
            if(spawnVelocity.x < 0)
            {
                float power = (-5 - spawnPosition.x)/3;
                spawnVelocity.x = Random.Range(power, 0);
            }
            else
            {
                float power = (5 - spawnPosition.x)/3;
                spawnVelocity.x = Random.Range(0, power);
            }
            newEnemy.transform.Find("corpusCrewman").Find("crewman_body").GetComponent<Renderer>().material = bodyMaterials[(int)Random.Range(0, bodyMaterials.Count -1)];
            newEnemy.gameObject.GetComponent<CorpusCrewmanEnemyScript>().setStartVelocity(spawnVelocity);
        }

        Debug.Log(numberOfSpawnedEnemies);
    }
}
