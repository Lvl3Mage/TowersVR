using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
	[SerializeField] NumberContainer CannonRotation;
	[SerializeField] NumberContainer TowerRotation;
	[SerializeField] Weapon Cannon;
	[SerializeField] WeaponReloader CannonReloader;

	protected void SetYAngle(float angle){
		TowerRotation.floatValue = angle;
	}
	protected void SetXAngle(float angle){
		CannonRotation.floatValue = angle;
	}
	protected void LoadCannon(Ammunition ammo){
		if(!CannonReloader.LoadAmmo(ammo)){
			Debug.LogWarning("Unable to load cannon!", gameObject);
		}
	}
	protected void FireCannon(){
		Cannon.Activate(true);
	}
	protected GameObject GetLastProjectile(){
		return Cannon.GetLastProjectile();
	}
	protected float GetExitVelocity(){
		float velocity = Cannon.GetProjectileSpeed();
		if(velocity <= 0){
			Debug.LogError("The exit velocity is 0 or the cannon has not been loaded!", gameObject);
		}
		return velocity;
	}
	protected Vector2 GetCurrentRotation(){
		return new Vector2(CannonRotation.floatValue,TowerRotation.floatValue);
	}
}
