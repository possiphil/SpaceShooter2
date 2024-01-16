using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] private GameObject losingCanvas;
    
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
            if (!hasLivesLeft)
            {
                losingCanvas.SetActive(true);
                StartCoroutine(GradualTimeScaleChange(0.1f, 1f, 1f));
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                SoundManager.soundManager.PlayExplosionSound();
                playerModel.SetActive(false);
            }

            if (hasLivesLeft)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                SoundManager.soundManager.PlayExplosionSound();
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
    private IEnumerator GradualTimeScaleChange(float targetTimeScale, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);

        float currentTime = 0f;
        float initialTimeScale = Time.timeScale;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            Time.timeScale = Mathf.Lerp(initialTimeScale, targetTimeScale, currentTime / duration);
            yield return null;
        }

        Time.timeScale = targetTimeScale; // Ensure that the target timescale is set precisely
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }
}
