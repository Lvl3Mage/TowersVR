using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : DataContainer
{
	[SerializeField] protected ControlRoom ControlRoom;
	WeaponReloader CannonReloader;
	GameObject LastFiredProjectile;
	float CannonAngle;
	float HorizontalRotation;
	float ProjectileLinearVelocity;
	Vector2 LastAim = Vector2.zero;
	public override void SetValue<T>(DataType dataType, T value){
		 switch(dataType){
			case DataType.LastFiredProjectile:
				LastFiredProjectile = value as GameObject;
				break;
			case DataType.ProjectileLinearVelocity:
				ProjectileLinearVelocity = (float)(object)value;
				break;
			case DataType.ToggleAI:
				ToggleAI((bool)(object)value);
				break;
			default:
				Debug.LogError("WTF is this => " + dataType + "?", gameObject);
				break;
		}
	}
	protected virtual void ToggleAI(bool value){}
	protected void SetYAngle(float angle){
		LastAim.y = angle;
		ControlRoom.SetValue(DataType.HorizontalRotation, angle);
	}
	protected void SetXAngle(float angle){
		LastAim.x = angle;
		ControlRoom.SetValue(DataType.CannonAngle, angle);
	}
	protected void LoadCannon(Ammunition ammo){
		if(CannonReloader == null){
			CannonReloader = ControlRoom.GetReloader();
		}
		if(!CannonReloader.LoadAmmo(ammo)){
			Debug.LogWarning("Unable to load cannon!", gameObject);
		}
	}
	protected void FireCannon(){
		ControlRoom.SetValue(DataType.CannonActivation, true);
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
		return LastAim;
	}
}
