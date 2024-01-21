using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Class
{
    public Tank()
    {
        name = "Tank";
        hp = 4;
        movementSpeed = 4;
        shootingCooldown = 0.4f;
        ability = new Shield();
    }
}
