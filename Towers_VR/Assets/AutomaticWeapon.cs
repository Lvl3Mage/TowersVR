using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticWeapon : Weapon
{
	[SerializeField] Transform gunPoint;
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
		if(Clip.count>0){
			Clip.count -= 1; 
			if(Clip.count == 0){
				CallBackLoadedState(2);
				State = false; // sets the state to false so the weapon can be reloaded (this will be reworked in the future cause you should obviously be able to reload your weapon without emptying it)
			}
			ShootProjectile(Clip.bullet, Clip.velocity, Recoil,gunPoint);
			StartCoroutine(StartDelay());
		}
	}
	IEnumerator StartDelay(){ // starts the delay for the next shot
		weaponResetting = true;
		yield return new WaitForSeconds(shootingDelay);
		weaponResetting = false; // sets the weapon state to ready for firing
		if(triggerActive){
			Fire();// fires the weapon if the trigger is pressed
		}
	}
}
