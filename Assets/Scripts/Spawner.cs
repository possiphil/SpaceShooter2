using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform playerTransform;
    public float tutorialspawnDelay;
    public float gameSpawnDelay;
    private float minSpawnDistance;
    private float maxSpawnDisatnce;
    private float difficultyIncreaseInterval;

    private bool isTutorialComplete = false;
    private int currentLevel = 1;

    private void Start()
    {
        GetReferences();
        StartTutorial();
    }

    private void GetReferences()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
    }

    private void StartTutorial()
    {
        StartCoroutine(SpawnTutorialEnemies());
    }

    void StartGame()
    {
        StartCoroutine(SpawnGameEnemies());
        StartCoroutine(IncreaseDifficulty());
    }

    IEnumerator SpawnTutorialEnemies()
    {
            int numEnemies = currentLevel;
            for (int i = 0; i < enemyPrefabs.Length; i++)
            {
                GameObject newEnemy = SpawnEnemy(enemyPrefabs[i]);
                
                yield return new WaitUntil(() => newEnemy == null || !newEnemy.activeSelf);
            }

            isTutorialComplete = true;
            StartGame();
    }

    private GameObject SpawnEnemy(GameObject enemyPrefab)
    {
        throw new NotImplementedException();
    }

    private GameObject SpawnEnemy(GameObject enemyPrefab, float spawnDelay)
    {
        float yoffset = 4f;
        Vector3 spawnPosition = new Vector3(playerTransform.position.x, playerTransform.position.y + yoffset, 0f);
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        return enemy;
    }

    private IEnumerator IncreaseDifficulty()
    {
        while (true)
        {
            yield return new WaitForSeconds(difficultyIncreaseInterval);

            currentLevel++;
        }
    }

    IEnumerator SpawnGameEnemies()
    {
        int numEnemies = currentLevel;

        for (int i = 0; i < numEnemies; i++)
        {
            GameObject newEnemy = SpawnEnemy(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], gameSpawnDelay);
            yield return new WaitUntil(() => newEnemy == null || !newEnemy.activeSelf);

            yield return new WaitForSeconds(gameSpawnDelay);
        }

        yield return null; 
    }
    
}
