using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloaderRoom : Room
{
    [SerializeField] WeaponReloader Reloader;
    void Awake(){
        Reloader.SetWeapon(towerRelay.GetCannon());
    }
    public WeaponReloader GetReloader(){
        return Reloader;
    }
}
