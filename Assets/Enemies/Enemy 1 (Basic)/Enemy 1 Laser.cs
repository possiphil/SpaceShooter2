using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private PlayerLogic target;

    [SerializeField] private float destroyAfterSeconds;

    private bool isDestroyed = false;


    private void Start()
    {
        ShootLaser();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
    private void ShootLaser()
    {


        rb = GetComponent<Rigidbody>();
        target = FindObjectOfType<PlayerLogic>();

        if (target != null)
        {
            Vector3 moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
            rb.velocity = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z);
        }
        else
        {
            Debug.LogWarning("No Player");
        }

        DestroyAfterDelay();
    }
    
    private void DestroyAfterDelay()
    {
        Invoke("DestroyBullet", destroyAfterSeconds);
    }

    private void DestroyBullet()
    {
        if (!isDestroyed)
        {
            isDestroyed = true;
            
            Destroy(gameObject);
        }
      
    }
    
}
