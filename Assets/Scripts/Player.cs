using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum State
{
    Playing,
    Explosion,
    Invincible
}

public class Player : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private TMPro.TextMeshProUGUI scoreUI;
    [SerializeField] private TMPro.TextMeshProUGUI livesUI;
    [SerializeField] private float playerSpeed = 5.0f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject asteroid;
    [SerializeField] private float offset = 1f;
    [SerializeField] private GameObject explosionPrefab;

    public static int score;
    public static int lives = 3;

    private State playerState = State.Playing;
    private static readonly Vector3 START_POSITION = new(0f, -2.5f, 0f);

    private const float edge = 10f;
    private const float RESPAWN_TIME = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        SetLivesAndScore();
        SpawnAsteroids();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerState != State.Explosion)
        {
            Move3D();
            Shoot();
        }
        scoreUI.text = "Score: " + score;
        livesUI.text = "Lives: " + lives;

        // Todo: Change speed of asteroids
        // Todo: Rotation when moving
        // Todo: Show player on other side before leaving on initial side
        // Todo: Upgrade to new input system
    }

    private void Move()
    {
        float amtToMove = Input.GetAxis("Horizontal") * playerSpeed * Time.deltaTime;
        transform.Translate(Vector3.right * amtToMove);

         keepInBounds();
    }

    private void Move3D()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        
        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, 0f);
        
        var cameraTwoThirds = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.66f, 0f));

        if (transform.position.y < -3.5 && moveDir.y < 0)
        {
            moveDir = new Vector3(moveDir.x, 0f, 0f).normalized;
        }
        else if (transform.position.y > cameraTwoThirds.y && moveDir.y > 0)
        {
            moveDir = new Vector3(moveDir.x, 0f, 0f).normalized;
        }

        transform.position += moveDir * (playerSpeed * Time.deltaTime);
        
        keepInBounds();
    }

    private void keepInBounds()
    {
        if (transform.position.x < -edge)
        {
            transform.position = new Vector3(edge, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > edge)
        {
            transform.position = new Vector3(-edge, transform.position.y, transform.position.z);
        } 
    }

    private void Rotate()
    {
        if (Input.GetAxis("Horizontal") >= 1)
        {
            
        }
        else if (Input.GetAxis("Horizontal") <= -1)
        {
            
        }
    }

    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var position = transform.position;
            position.y += offset;
            Instantiate(projectilePrefab, position, Quaternion.identity);
        }
    }

    private void SpawnAsteroids()
    {
        for (int i = 0; i < 5; i++)
        {
            var position = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), 1, 0));
            Instantiate(asteroid, position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy collideWith = other.GetComponent<Enemy>();
        if (collideWith != null && playerState == State.Playing)
        {
            other.GetComponent<Enemy>().SetSpeedAndPosition();
            lives--;

            if (lives <= 0)
            {
                SceneManager.LoadScene(3);
            }
            else
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                StartCoroutine(Respawn());
            }
        }
    }

    private void SetLivesAndScore()
    {
        lives = 3;
        score = 0;
    }

    private IEnumerator Respawn()
    {
        playerState = State.Explosion;
        GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(RESPAWN_TIME);
        transform.position = START_POSITION;
        GetComponent<Renderer>().enabled = true;
        playerState = State.Invincible;
        yield return new WaitForSeconds(RESPAWN_TIME);
        playerState = State.Playing;
    }
}
