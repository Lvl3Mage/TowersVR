using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleFireWeapon : Weapon
{
	[SerializeField] Transform gunPoint;
	protected override void FireWeapon(){
		Fire();
	}
	[ContextMenu("Fire")]
	void Fire(){ // the function for shooting out the projectile
		if(Clip.ammoCount>0){
			Clip.ammoCount -= 1; 
			if(Clip.ammoCount == 0){
		    	CallBackLoadedState(2);
				State = false; // sets the state to false so the weapon can be reloaded (this will be reworked in the future cause you should obviously be able to reload your weapon without emptying it)
			}
			ShootProjectile(Clip.bullet, Clip.velocity, Recoil,gunPoint);
		}

	}
}
