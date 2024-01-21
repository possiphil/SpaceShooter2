using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameLogic : MonoBehaviour
{
    
    [SerializeField] private TMPro.TextMeshProUGUI scoreUI;
    [SerializeField] private TMPro.TextMeshProUGUI livesUI;
    
    [SerializeField] private GameObject asteroid;
    
    public static int score;
    public static int lives;
    [SerializeField] private GameObject losingCanvas;

    private void Start()
    {
        SpawnAsteroids();
    }

    private void Update()
    {
        scoreUI.text = "Score: " + score;
        livesUI.text = "Lives: " + lives;
    }

    public static void SetLivesAndScore(float hp)
    {
        lives = (int) hp;
        score = 0;
    }
    
    private void SpawnAsteroids()
    {
        for (int i = 0; i < 5; i++)
        {
            var position = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), 1, 0));
            Instantiate(asteroid, position, Quaternion.identity);
        }
    }


    public static bool HandleLiveDecrease()
    {
        lives--;

        if (lives <= 0)
        {
            //SceneManager.LoadScene(3);
            return false;
        }

        return true;
    }
    
    public static bool HandleScoreIncrease()
    {
        score += 100;
        
        if (score == 5000)
        {
            SceneManager.LoadScene(2);
            return false;
        }
        
        return true;
    }
}
