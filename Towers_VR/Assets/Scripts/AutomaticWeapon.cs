using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticWeapon : Weapon
{
	[SerializeField] float shootingDelay;
	bool triggerActive, weaponResetting;
	protected override void FireWeapon(){
		if(!weaponResetting){ // checks if the weapon is ready to fire(this prevents the player from mashing the fire button and getting high firerates) and if it's actually toggled on 
			Fire();
		}
		
	}
	protected override void TriggerPressed(bool toggleValue){ // this exists cause the trigger can be pressed even if the weapon cannot fire
		triggerActive = toggleValue; // sets the trigger state
	}
	void Fire(){
		Clip.ammoCount -= 1;
		ShootProjectile();
		WeaponFired();
		StartCoroutine(StartDelay());
	}
	IEnumerator StartDelay(){ // starts the delay for the next shot
		weaponResetting = true;
		yield return new WaitForSeconds(shootingDelay);
		weaponResetting = false; // sets the weapon state to ready for firing
		if(triggerActive && State){
			Fire();// fires the weapon if the trigger is pressed
		}
	}
}
