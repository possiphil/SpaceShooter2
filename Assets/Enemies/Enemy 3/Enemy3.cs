using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy3 : MonoBehaviour
{
   //Teleport
   [SerializeField] private float teleportDistance = 2000f;
   [SerializeField] private float teleportRadius = 10f;
   [SerializeField] private float timeBetweenTeleports = 15f;
   [SerializeField] private float timeAtFarAwayPosition = 5f;

   private bool isFarAway = false;
   //Blinking
   [SerializeField] private int numberOfBlinks = 3;
   [SerializeField] private float timeBetweenBlinks = 2f;
   private bool isBlinking = false;
   //Components
   private Transform player;
   private Rigidbody rb;
   //Shooring
   [SerializeField] private GameObject bigProjectilePrefab;
   [SerializeField] private GameObject smallProjectilePrefab;
   [SerializeField] private float interval;
   [SerializeField] private float timer;
   [SerializeField] private GameObject enemy;
   [SerializeField] private Transform[] cannons;
   //TeleportInSight
  // private bool isTeleporting = false;
  
   //Movement
   private Vector3 movement;
   //Health
   private int Health = 5;
   private int currentHealth;

 
  

   private void Start()
   {
      currentHealth = Health;
      GetNeededComponents();
      StartCoroutine(TeleportLoop());
    
   }

   private void Update()
   {
      LookToPlayer();
    
      timer += Time.deltaTime;
      if (!isBlinking && timer >= interval && !isFarAway)
      {
        // Debug.Log("Shooting");
         Shoot();
         timer = 0;
      }
      else
      {
        // Debug.Log("Not");
      }
   }
   
   private IEnumerator Blink()
   {

      isBlinking = true;

      Renderer[] childRenderes = GetComponentsInChildren<Renderer>(true);
      
         for (int blinkCount = 0; blinkCount < numberOfBlinks; blinkCount++)
         {
            foreach (Renderer childRenderer in childRenderes)
            {
               if (childRenderer != null)
               {
                //  Debug.Log("Renderer Enabled: " + childRenderer.enabled);
                  childRenderer.enabled = false;
               }
            }

            yield return new WaitForSeconds(timeBetweenBlinks);

            foreach (Renderer childRenderer in childRenderes)
            {
               if (childRenderer != null)
               {
                 // Debug.Log("Renderer Enabled: " + childRenderer.enabled);
                  childRenderer.enabled = true;
               }
            }

            yield return new WaitForSeconds(timeBetweenBlinks);
         }
            
         
         isBlinking = false;
   }
   
   private IEnumerator TeleportLoop()
   {
      while (true)
      {
         yield return StartCoroutine(TeleportAway());
         yield return StartCoroutine(TeleportNearPlayer());
      }
   }

   private void GetNeededComponents()
   {
      rb = GetComponent<Rigidbody>();
      GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
      player = playerObject.transform;
      
      Renderer[] childRenderers = GetComponentsInChildren<Renderer>(true);
      if (childRenderers.Length > 0)
      {
         //EnemyRenderer = childRenderers[0];
      }
      else
      {
         Debug.Log("No renderer found on child object");
      }

      cannons = new Transform[4];
      cannons[0] = transform.Find("Cannon1");
      cannons[1] = transform.Find("Cannon2");
      cannons[2] = transform.Find("Cannon3");
      cannons[3] = transform.Find("Cannon4");
      
   }
   

   private void Shoot()
   {
      for (int i = 0; i < cannons.Length; i++)
      {
        

         if (i == 0 || i == 3)
         {
            Instantiate(bigProjectilePrefab, cannons[i].position, cannons[i].rotation);
         }
         else if (i == 1 || i == 2)
         {
            Instantiate(smallProjectilePrefab, cannons[i].position, cannons[i].rotation);
         
         }
        
      }
      
   }

   private void LookToPlayer()
   {
      Vector3 direction = player.position - transform.position;

      direction.Normalize();
      movement = direction;

      float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
      Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);


      rb.MoveRotation(rotation);
   }

   private IEnumerator TeleportAway()
   {
      yield return StartCoroutine(Blink());
      
      Vector3 randomDirection = Random.onUnitSphere;
      randomDirection.y = 0;

      Vector3 teleportPosition = player.position + randomDirection * teleportDistance;
      transform.position = teleportPosition;
      isFarAway = true;

      yield return new WaitForSeconds(timeAtFarAwayPosition);
   }

   private IEnumerator TeleportNearPlayer()
   {
      Vector3 randomOffset = Random.insideUnitCircle * teleportRadius;
      Vector3 teleportPosition = player.position + new Vector3(randomOffset.x, randomOffset.y, 0f);
      transform.position = teleportPosition;
      isFarAway = false;

      yield return new WaitForSeconds(timeBetweenTeleports);
   }
   
   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("PlayerProjectile"))
      {
         TakeDamage(1);
      }
   }
   private void TakeDamage(int damage)
   {
      currentHealth -= damage;
      if (currentHealth <= 0)
      {
         DestroyEnemy();
      }
   }

   private void DestroyEnemy()
   {
      AudioSource audioSource = GetComponent<AudioSource>();
      if (audioSource != null && audioSource.clip != null)
      {
         audioSource.Play();
      }
      Destroy(gameObject);
   }

}
