using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleFireWeapon : Weapon
{
	protected override void FireWeapon(){
		Fire();
	}
	[ContextMenu("Fire")]
	void Fire(){
		Clip.ammoCount -= 1; 
		ShootProjectile();
		WeaponFired();
	}
}
