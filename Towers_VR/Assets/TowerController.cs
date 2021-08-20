using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : DataContainer
{
	[SerializeField] TowerRelay TowerRelay;
	WeaponReloader CannonReloader;
	GameObject LastFiredProjectile;
	float CannonAngle;
	float HorizontalRotation;
	float ProjectileLinearVelocity;
	public override void SetValue<T>(DataType dataType, T value){
		 switch(dataType){
			case DataType.LastFiredProjectile:
				LastFiredProjectile = value as GameObject;
				break;
			case DataType.CannonAngle:
				CannonAngle = (float)(object)value;
				break;
			case DataType.HorizontalRotation:
				HorizontalRotation = (float)(object)value;
				break;
			case DataType.ProjectileLinearVelocity:
				ProjectileLinearVelocity = (float)(object)value;
				break;
			default:
				Debug.LogError("WTF is this => " + dataType + "?", gameObject);
				break;
		}
	}
	protected void SetYAngle(float angle){
		TowerRelay.SetValue(DataType.HorizontalRotation, angle);
	}
	protected void SetXAngle(float angle){
		TowerRelay.SetValue(DataType.CannonAngle, angle);
	}
	protected void LoadCannon(Ammunition ammo){
		if(CannonReloader == null){
			CannonReloader = TowerRelay.GetReloader();
		}
		if(!CannonReloader.LoadAmmo(ammo)){
			Debug.LogWarning("Unable to load cannon!", gameObject);
		}
	}
	protected void FireCannon(){
		TowerRelay.SetValue(DataType.CannonActivation, true);
	}
	protected GameObject GetLastProjectile(){
		return LastFiredProjectile;
	}
	protected float GetExitVelocity(){
		if(ProjectileLinearVelocity <= 0){
			Debug.LogError("The exit velocity is 0 or the cannon has not been loaded!", gameObject);
		}
		return ProjectileLinearVelocity;
	}
	protected Vector2 GetCurrentRotation(){
		return new Vector2(CannonAngle,HorizontalRotation);
	}
}
