using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Enemy1Movement : MonoBehaviour
{
   public Transform Player;
   private Rigidbody2D rb;
   private Vector3 movement;

   [SerializeField] private float moveSpeed = 2f;
   [SerializeField] private float StopDistance = 5f;

   private void Start()
   {
       rb = this.GetComponent<Rigidbody2D>();
   }

   private void Update()
   {
       Vector3 direction = Player.position - transform.position;

       float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
       rb.rotation = angle;
       
       direction.Normalize();
       movement = direction;
   }

   private void FixedUpdate()
   {
       if ((Player.position - transform.position).magnitude <= StopDistance)
       {
           moveSpeed = 0;
       }
       else
       {
           moveSpeed = 1;
           moveEnemy(movement);
       }
   }

   private void moveEnemy(Vector3 direction)
   {
       rb.MovePosition(transform.position + (moveSpeed * Time.deltaTime * direction));
   }
}
