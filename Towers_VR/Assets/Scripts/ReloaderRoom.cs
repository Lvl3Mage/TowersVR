using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloaderRoom : Room
{
    [SerializeField] WeaponReloader Reloader;
    public WeaponReloader GetReloader(){
        return Reloader;
    }
    public Weapon GetCannon(){
        return towerRelay.GetCannon();
    }
}
