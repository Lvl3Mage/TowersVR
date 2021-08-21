using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonRoom : Room
{
    [SerializeField] Weapon Cannon;
    [SerializeField] Transform gunPoint;
    public Weapon GetCannon(){
        return Cannon;
    }
    public Transform GetGunpoint(){
        return gunPoint;
    }
}
