using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    private enum PlayerState
    {
        Vulnerable,
        Invincible
    }

    [SerializeField] private GameObject playerModel;
    
    [SerializeField] private GameObject explosionPrefab;
    
    [SerializeField] private bool isInvincible;
    
    private PlayerState playerState = PlayerState.Vulnerable;
    
    private Renderer modelRenderer;
    
    private const float INVINCIBILITY_DURATION = 3f;
    private const float BLINKING_DURATION = 0.2f;

    private static readonly WaitForSeconds waitForInvinibilityTime = new WaitForSeconds(INVINCIBILITY_DURATION);
    private static readonly WaitForSeconds waitForBlinkingTime = new WaitForSeconds(BLINKING_DURATION);

    private void Awake()
    {
        modelRenderer = playerModel.GetComponent<Renderer>();
    }
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
            modelRenderer.enabled = b;
            b =!b;
            yield return waitForBlinkingTime;
        }
        modelRenderer.enabled = true;
        playerState = PlayerState.Vulnerable;
    }
}
