using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    
    [SerializeField] private float projectileSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float amtToMove = projectileSpeed * Time.deltaTime;
        transform.Translate(Vector3.up * amtToMove);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Enemy>().SetSpeedAndPosition();
        Player.score += 100;
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        if (Player.score == 5000)
        {
            SceneManager.LoadScene(2);
        }
    }
}
