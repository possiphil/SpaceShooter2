using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability
{
    public string name;
    public float cooldown;
    public float duration;
    
    public virtual void Activate(GameObject parent, PlayerControls controls) {}
}
