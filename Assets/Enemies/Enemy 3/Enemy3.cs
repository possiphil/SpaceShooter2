using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy3 : MonoBehaviour
{
   //Phasing
   [SerializeField] private Transform[] phasePoints;
   [SerializeField] private float TimeBetweenPhasing;
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
   private bool isTeleporting = false;
   //Movement
   private Vector3 movement;

 
  

   private void Start()
   {
      GetNeededComponents();
      StartCoroutine(PhaseLoop());
    
   }

   private void Update()
   {
      LookToPlayer();
    
      timer += Time.deltaTime;
      if (!isBlinking && timer >= interval)
      {
         Shoot();
         timer = 0;
      }

      if (!isTeleporting)
      {
         StartCoroutine(TeleportInSight());
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
   
   private IEnumerator Teleport()
   { 
   // Debug.Log("Teleport");
   Vector3 randomPosition = GetRandomPointInCameraView();
   transform.position = randomPosition;

   yield return new WaitForSeconds(1f);
   }

   private IEnumerator PhaseLoop()
   {
      while (true)
      {
         yield return StartCoroutine(Blink());
         yield return StartCoroutine(Teleport());
      
         
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

   private IEnumerator TeleportInSight()
   {
      isTeleporting = true;
      if (!IsInCamerView())
      {
         Vector3 randomPosition = GetRandomPointInCameraView();
         transform.position = randomPosition;
         yield return new WaitForSeconds(5f);
      }
      isTeleporting = false;
   }

   private bool IsInCamerView()
   {
      Vector3 viewPointposition = Camera.main.WorldToViewportPoint(transform.position);
      return viewPointposition.x >= 0 && viewPointposition.x <= 1 && viewPointposition.y >= 0 && viewPointposition.y <= 1;
   }

   private Vector3 GetRandomPointInCameraView()
   {
      float screenHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
      float screenHalfHeight = Camera.main.orthographicSize;

      float randomX = Random.Range(-screenHalfWidth, screenHalfWidth);
      float randomY = Random.Range(-screenHalfHeight, screenHalfHeight);

      return new Vector3(randomX, randomY, 0f);
   }
}
