using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.Search;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Enemy1Projectile : MonoBehaviour
{
 
  [SerializeField] private float interval = 5f;
  [SerializeField] private float timer = 2f;
  [SerializeField] private float DistanceToShoot = 100f;

  [SerializeField] private string playerTag = "Player";
  [SerializeField] private string enemyTag = "Enemy";
  [SerializeField] private GameObject projectilePrefab;//projectileTag = "Projectile";


  private Transform playerTransform;
  private Transform enemyTransform;
  //private GameObject projectilePrefab;

  private void Start()
  {
     GameObject playerObject = GameObject.FindWithTag(playerTag);
     GameObject enemyObject = GameObject.FindGameObjectWithTag(enemyTag);
    // projectilePrefab = GameObject.FindGameObjectWithTag(projectileTag);

     playerTransform = playerObject.transform;
     enemyTransform = enemyObject.transform;
     //Debug.Log("DistanceToShoot" + DistanceToShoot);
  }

  private void Update()
  {
      timer += Time.deltaTime;

      if (timer >= interval)
      {
          Shoot();
          timer = 0;
      }
  }

  private void Shoot()
  {
      if (playerTransform != null && enemyTransform != null)
      {
          float distance = Vector3.Distance(playerTransform.position, enemyTransform.position);
         // Debug.Log("Distance to player: " + distance);
          
          if (distance < DistanceToShoot)
          {
            Debug.Log("Shooting!");
              Instantiate(projectilePrefab, transform.position, Quaternion.identity);
          }
      }
      
  }
}
