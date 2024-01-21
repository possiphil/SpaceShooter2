using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Ability
{
    private CharacterController controller;
    
    private float dashSpeed;
    public Dash()
    {
        name = "Dash";
        cooldown = 2f;
        duration = 0.25f;
        dashSpeed = 20f;
    }

    public override void Activate(GameObject parent, PlayerControls controls)
    {
        controller = parent.GetComponent<CharacterController>();
        
        //StartCoroutine(DashRoutine(controls));
    }

    private IEnumerator DashRoutine(PlayerControls controls)
    {
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            controller.Move(controls.Controls.Movement.ReadValue<Vector2>() * (dashSpeed * Time.deltaTime));
            yield return null;
        }
    }
}
