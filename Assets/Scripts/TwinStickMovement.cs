using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class TwinStickMovement : MonoBehaviour
{
    private enum PlayerState
    {
        Vulnerable,
        Invincible
    }
    
    public Class playerClass;

    [SerializeField] private GameObject assaultModel;
    [SerializeField] private GameObject tankModel;
    [SerializeField] private GameObject wraithModel;
    [SerializeField] private GameObject shieldModel;
    [SerializeField] private GameObject explosionPrefab;
    
    [SerializeField] private AbilityBar abilityBar;
    
    [SerializeField] private float playerSpeed;
    [SerializeField] private float controllerDeadzone = 0.1f;
    [SerializeField] private float gamepadRotationSmoothing = 1000f;
    
    [SerializeField] private bool isGamepad;
    
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletModel;
<<<<<<< Updated upstream
    [SerializeField]private float CurrentPlayerSpeed;
    [SerializeField]private float pitchChangeSpeed = 15.0f;
    private float cooldown;
=======
>>>>>>> Stashed changes
    
    private GameObject playerModel;
    private Renderer modelRenderer;
    private Renderer shieldRenderer;
    
    private PlayerState playerState = PlayerState.Vulnerable;
    
    private float shootingCooldown;
    private float abilityCooldown;
    
    private float dashSpeed = 20f;
    private float dashTime = 0.25f;

    private float shieldTime = 5f;

    private float teleportDistance = 10f;

    private float ABILITY_COOLDOWN;
    private float SHOOTING_COOLDOWN;
    
    private CharacterController controller;
    
    private Vector2 movement;
    private Vector2 aim;
    private Vector2 move;
    
    private bool isShooting;
    private bool isUsingAbility;
    private bool doesAbility;
    private bool isInvincible;
    
    private PlayerControls playerControls;
    private PlayerInput playerInput;

    private Camera cam;
    
<<<<<<< Updated upstream

    private void Start()
    {
        cam = Camera.main;
    }
=======
    private const float INVINCIBILITY_DURATION = 3f;
    private const float BLINKING_DURATION = 0.2f;
    
    private static readonly WaitForSeconds waitForInvinibilityTime = new WaitForSeconds(INVINCIBILITY_DURATION);
    private static readonly WaitForSeconds waitForBlinkingTime = new WaitForSeconds(BLINKING_DURATION);
>>>>>>> Stashed changes

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerControls = new PlayerControls();
        playerInput = GetComponent<PlayerInput>();

        cam = Camera.main;

        playerClass = new Wraith();
        playerSpeed = playerClass.movementSpeed;
        SHOOTING_COOLDOWN = playerClass.shootingCooldown;
        ABILITY_COOLDOWN = playerClass.ability.cooldown;
        
        SetModel();
        GameLogic.SetLivesAndScore(playerClass.hp);
        abilityBar.SetMaxProgress(ABILITY_COOLDOWN);
    }

    private void SetModel()
    {
        if (playerClass.GetType() == typeof(Assault))
        {
            playerModel = assaultModel;
            assaultModel.SetActive(true);
            tankModel.SetActive(false);
            wraithModel.SetActive(false);
        }
        else if (playerClass.GetType() == typeof(Tank))
        {
            playerModel = tankModel;
            assaultModel.SetActive(false);
            tankModel.SetActive(true);
            wraithModel.SetActive(false);
        }
        else if (playerClass.GetType() == typeof(Wraith))
        {
            playerModel = wraithModel;
            assaultModel.SetActive(false);
            tankModel.SetActive(false);
            wraithModel.SetActive(true);
        }
        
        modelRenderer = playerModel.GetComponent<Renderer>();
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
        if (doesAbility) return;
        HandleInput();
        HandleMovement();
        HandleRotation();
        HandleShooting();
<<<<<<< Updated upstream
        GetPlayerSpeed();

        // Adjust audio pitch based on player speed
        AdjustAudioPitch(CurrentPlayerSpeed);
=======
        HandleAbility();
    }

    private void FixedUpdate()
    {
        if (doesAbility) return;
>>>>>>> Stashed changes
    }

    private void HandleInput()
    {
        movement = playerControls.Controls.Movement.ReadValue<Vector2>();
        aim = playerControls.Controls.Aim.ReadValue<Vector2>();
        isShooting = playerControls.Controls.Shooting.ReadValue<float>() > 0.1f;
        isUsingAbility = playerControls.Controls.Ability.ReadValue<float>() > 0.1f;
    }

    private void HandleMovement()
    {
        move = new Vector2(movement.x, movement.y).normalized;
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
        shootingCooldown -= Time.deltaTime;

        if (isShooting && shootingCooldown <= 0)
        {
            Instantiate(bulletModel, firePoint.position, firePoint.rotation);
<<<<<<< Updated upstream
            cooldown = SHOOTING_COOLDOWN;
            SoundManager.soundManager.PlayFiringSound();
=======
            shootingCooldown = SHOOTING_COOLDOWN;
>>>>>>> Stashed changes
        }
    }

    private void HandleAbility()
    {
        abilityCooldown -= Time.deltaTime;
        abilityBar.SetProgress(ABILITY_COOLDOWN - abilityCooldown);
        
        if (isUsingAbility && abilityCooldown <= 0) // move != Vector2.zero
        {
            if (playerClass.GetType() == typeof(Assault))
            {
                StartCoroutine(Dash());
            } else if (playerClass.GetType() == typeof(Tank))
            {
                StartCoroutine(Shield());
            }
            else
            {
                StartCoroutine(Teleport());
            }
            abilityCooldown = ABILITY_COOLDOWN;
        }
        /*
        abilityCooldown -= Time.deltaTime;
        
        if (isUsingAbility && abilityCooldown <= 0)
        {
            playerClass.ability.Activate(playerModel, playerControls);
            abilityCooldown = playerClass.ability.cooldown;
        }
        */
    }

    public void OnDeviceChange(PlayerInput input)
    {
        isGamepad = input.currentControlScheme.Equals("Gamepad");
    }
<<<<<<< Updated upstream

    public void GetPlayerSpeed()
    {
    Vector2 move = new Vector2(movement.x, movement.y);
    CurrentPlayerSpeed = (move.magnitude / Time.deltaTime) / 100; // Calculate speed by dividing magnitude by time
    //return CurrentPlayerSpeed;
    }

    private void AdjustAudioPitch(float targetSpeed)
{
    AudioSource audioSource = GetComponent<AudioSource>();

    // Calculate the target pitch based on the target speed
    float targetPitch = Mathf.Clamp(targetSpeed, 1f, 1.2f);

    // Smoothly interpolate towards the target pitch
    float smoothedPitch = Mathf.Lerp(audioSource.pitch, targetPitch, Time.deltaTime * pitchChangeSpeed);

    audioSource.pitch = smoothedPitch;
}
    
=======
    
    private void OnTriggerEnter(Collider other)
    {
        Enemy collideWith = other.GetComponent<Enemy>();
        if (collideWith != null && playerState == PlayerState.Vulnerable)
        {
            other.GetComponent<Enemy>().SetSpeedAndPosition();

            bool hasLivesLeft = GameLogic.HandleLiveDecrease();

            if (hasLivesLeft)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                StartCoroutine(HandleHit());
            }
        }
    }
    
    private IEnumerator HandleHit()
    {
        playerState = PlayerState.Invincible;
        float endTime = Time.time + INVINCIBILITY_DURATION;
        bool b = false;
        while (Time.time <= endTime)
        {
            playerModel.SetActive(b);
            b = !b;
            yield return waitForBlinkingTime;
        }
        playerModel.SetActive(true);
        playerState = PlayerState.Vulnerable;
    }

    private IEnumerator Shield()
    {
        playerState = PlayerState.Invincible;
        shieldModel.SetActive(true);

        yield return new WaitForSeconds(shieldTime);

        shieldModel.SetActive(false);
        playerState = PlayerState.Vulnerable;
    }

    private IEnumerator Dash()
    {
        float startTime = Time.time;
        doesAbility = true;

        while (Time.time <= startTime + dashTime)
        {
            if (move == Vector2.zero) move = Vector2.up;
            controller.Move(move * (dashSpeed * Time.deltaTime));
            yield return null;
        }

        doesAbility = false;
    }

    private IEnumerator Teleport()
    {
        if (move == Vector2.zero) move = Vector2.up;
        Vector3 position = transform.position;
        position = new Vector3(position.x + move.x * teleportDistance, position.y + move.y * teleportDistance,
            position.z);
        transform.position = position;
        Physics.SyncTransforms();

        yield return null;
    }
>>>>>>> Stashed changes
}
