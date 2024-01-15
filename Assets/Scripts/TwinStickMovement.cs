using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class TwinStickMovement : MonoBehaviour
{
    
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float controllerDeadzone = 0.1f;
    [SerializeField] private float gamepadRotationSmoothing = 1000f;
    
    [SerializeField] private bool isGamepad;
    
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletModel;

    private float cooldown;
    
    private const float SHOOTING_COOLDOWN = 0.3f;
    
    private CharacterController controller;
    
    private Vector2 movement;
    private Vector2 aim;
    
    private bool isShooting;
    
    private PlayerControls playerControls;
    private PlayerInput playerInput;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerControls = new PlayerControls();
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        HandleInput();
        HandleMovement();
        HandleRotation();
        HandleShooting();
    }

    private void HandleInput()
    {
        movement = playerControls.Controls.Movement.ReadValue<Vector2>();
        aim = playerControls.Controls.Aim.ReadValue<Vector2>();
        isShooting = playerControls.Controls.Shooting.ReadValue<float>() > 0.1f;
    }

    private void HandleMovement()
    {
        Vector2 move = new Vector2(movement.x, movement.y).normalized;
        controller.Move(move * (Time.deltaTime * playerSpeed));
    }

    private void HandleRotation()
    {
        if (isGamepad)
        {
            // Gamepad Rotation
            
            if (Mathf.Abs(aim.x) > controllerDeadzone || Mathf.Abs(aim.y) > controllerDeadzone)
            {
                Vector3 playerDirection = Vector3.right * aim.x + Vector3.up * aim.y;
                
                if (playerDirection.sqrMagnitude > 0.0f)
                {
                    float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg - 90f;
                    Quaternion newRotation = Quaternion.Euler(0, 0, angle);
                    
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, gamepadRotationSmoothing * Time.deltaTime);
                }
            }
        }
        else
        {
            // Mouse Rotation
            
            Vector3 mousePos = cam.ScreenToWorldPoint(aim);
            Vector3 lookDir = mousePos - transform.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void HandleShooting()
    {
        cooldown -= Time.deltaTime;

        if (isShooting && cooldown <= 0)
        {
            Instantiate(bulletModel, firePoint.position, firePoint.rotation);
            cooldown = SHOOTING_COOLDOWN;
            SoundManager.soundManager.PlayFiringSound();
        }
    }


    public void OnDeviceChange(PlayerInput input)
    {
        isGamepad = input.currentControlScheme.Equals("Gamepad");
    }
}
