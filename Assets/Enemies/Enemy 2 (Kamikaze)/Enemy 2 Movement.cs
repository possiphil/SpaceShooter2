using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Enemy2Movement : MonoBehaviour
{
    [SerializeField] private float enemy2speed = 5f;
    [SerializeField] public float distanceToExplode = 1f;

    [SerializeField] public GameObject explosionEnemy2;

    private Rigidbody2D rb;
    public Transform player;
    private Vector2 movement;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void Update()
    {
        Vector3 direction = player.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
        
        direction.Normalize();
        movement = direction;
    }

    private void FixedUpdate()
    {
        if ((player.position - transform.position).magnitude <= distanceToExplode)
        {
            var transform1 = transform;
            Instantiate(explosionEnemy2, transform1.position, transform1.rotation);
            Destroy(gameObject);
        }
        else
        {
            moveEnemy(movement);
        }
    }

    private void moveEnemy(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (enemy2speed * Time.deltaTime * direction));
    }
}

