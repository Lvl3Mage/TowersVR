using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonRoom : Room
{
    [SerializeField] Weapon Cannon;
    public Weapon GetCannon(){
        return Cannon;
    }
}
