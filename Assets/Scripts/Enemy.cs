using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float minSpeed = 3f;
    [SerializeField] private float maxSpeed = 6f;
    [SerializeField] private float maxRotationSpeed = 30f;
    [SerializeField] private Vector2 minMaxScale = new Vector2(1000f, 2500f);
    private float speed;
    private Vector3 rotationSpeed;
    private float scale;

    public void SetSpeedAndPosition()
    {
        // Speed
        speed = Random.Range(minSpeed, maxSpeed);
        
        // Position
        var position = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), 1, 0));
        position.y += 2;
        position.z = 0;
        transform.position = position;
        
        // Rotation
        rotationSpeed.x = Random.Range(-maxRotationSpeed, maxRotationSpeed);
        rotationSpeed.y = Random.Range(-maxRotationSpeed, maxRotationSpeed);
        rotationSpeed.z = Random.Range(-maxRotationSpeed, maxRotationSpeed);
        
        // Scale
        // TODO: Find good values for min and max scale
        scale = Random.Range(minMaxScale.x, minMaxScale.y);
    }
    void Start()
    {
        SetSpeedAndPosition();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float amtToMove = speed * Time.deltaTime;
        transform.Translate(Vector3.down * amtToMove, Space.World);
        
        Vector3 amtToRotate = new Vector3();
        amtToRotate.x = rotationSpeed.x * Time.deltaTime;
        amtToRotate.y = rotationSpeed.y * Time.deltaTime;
        amtToRotate.z = rotationSpeed.z * Time.deltaTime;
        transform.Rotate(amtToRotate);

        transform.localScale = Vector3.one * scale;

        if (transform.position.y <= -5f)
        {
            SetSpeedAndPosition();
        }
    }
    
    
}
