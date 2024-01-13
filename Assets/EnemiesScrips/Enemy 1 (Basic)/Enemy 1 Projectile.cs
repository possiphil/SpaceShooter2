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
  [SerializeField] private GameObject projectile;
  [SerializeField] private float interval = 5f;
  [SerializeField] private float timer = 2f;
  [SerializeField] private float DistanceToShoot = 10f;

  [SerializeField] private GameObject player;
  [SerializeField] private GameObject enemy;

  private Transform playerTransform;

  private void Start()
  {
      playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
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
      Transform playerTransform = this.playerTransform.transform;
      Transform enemyTransform = enemy.transform;

      float distance = Vector3.Distance(playerTransform.position, enemyTransform.position);

      if (distance <= DistanceToShoot)
      {
          Instantiate(projectile, transform.position, Quaternion.identity);
      }

  }
}
