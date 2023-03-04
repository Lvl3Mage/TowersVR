using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstFireWeapon : Weapon
{
    [SerializeField] float burstDelay = 0.1f;
    [SerializeField] int burstLength = 3;
    bool BurstFiring = false;
    protected override void FireWeapon(){
        if(!BurstFiring){
            StartCoroutine(BurstFire(burstLength, burstDelay));
        }
    }
    IEnumerator BurstFire(int burstAmount, float delay){
        BurstFiring = true;
        while(State && burstAmount > 0){ // while we have ammo and the burst hasn't ended yet
            
            burstAmount--;
            Fire();
            yield return new WaitForSeconds(delay);
        }
        BurstFiring = false;
        
    }
    void Fire(){
        Clip.ammoCount--;
        ShootProjectile();
        WeaponFired();
    }
}
