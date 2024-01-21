using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assault : Class
{
    public Assault()
    {
        name = "Assault";
        hp = 3;
        movementSpeed = 5f;
        shootingCooldown = 0.3f;
        ability = new Dash();
    }
}
