using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    [SerializeField] private Transform[] phasePoints;
   [SerializeField] private int numberOfPhasePoints = 5;
   
   [SerializeField] private float TimeBetweenPhasing;

   [SerializeField] private int numberOfBlinks = 3;
   [SerializeField] private float timeBetweenBlinks = 2f;

   private Transform player;
   private Rigidbody rb;

   [SerializeField] private GameObject projectile;
   [SerializeField] private float interval;
   [SerializeField] private float timer;
   [SerializeField] private GameObject enemy;

   private bool isBlinking = false;

   private Vector3 movement;

   private Renderer EnemyRenderer;
  

   private void Start()
   {
      GetNeededComponents();
      GenerateRandomPhasePoints();
      StartCoroutine(PhaseLoop());
    
   }

   private void Update()
   {
      timer += Time.deltaTime;
      if (!isBlinking && timer >= interval)
      {
         Shoot();
         timer = 0;
      }

      LookToPlayer();
   }


   private void GenerateRandomPhasePoints()
   {
      phasePoints = new Transform[numberOfPhasePoints];

      float screenHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
      float screenHalfHeight = Camera.main.orthographicSize;

      for (int i = 0; i < numberOfPhasePoints; i++)
      {
         float randomX = Random.Range(-screenHalfWidth, screenHalfWidth);
         float randomY = Random.Range(-screenHalfHeight, screenHalfHeight);

         GameObject point = new GameObject("PhasePoint_" + i);
         point.transform.position = new Vector3(randomX, randomY, 0f);

         phasePoints[i] = point.transform;
      }
   }
   
   private IEnumerator Blink()
   {

      isBlinking = true;
         for (int blinkCount = 0; blinkCount < numberOfBlinks; blinkCount++)
         {
            if (EnemyRenderer != null)
            {
               GetComponent<Renderer>().enabled = false;
               EnemyRenderer.enabled = false;
               yield return new WaitForSeconds(timeBetweenBlinks);

               GetComponent<Renderer>().enabled = true;
               EnemyRenderer.enabled = true;
               yield return new WaitForSeconds(timeBetweenBlinks);
            }
         }

         isBlinking = false;
   }

   private IEnumerator Teleport()
   { 
   // Debug.Log("Teleport");
         int nextPosition = Random.Range(0, phasePoints.Length);

         transform.position = phasePoints[nextPosition].position;

         yield return new WaitForSeconds(TimeBetweenPhasing);
         
      
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
      
         EnemyRenderer = childRenderers[0];
   }

   

   private void Shoot()
   {
      //Debug.Log("Shooting");
      
      Transform playerTransform = player.transform;
      Transform enemyTransform = enemy.transform;

      float distance = Vector3.Distance(playerTransform.position, enemyTransform.position);

      if (distance <= 10f)
      {
         Instantiate(projectile, transform.position, Quaternion.identity);
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
}
