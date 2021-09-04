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
	List<AmmoRoom.AmmoGroup> AmmoGroups;

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
	protected bool LoadCannon(){
		if(AmmoGroups == null){
			AmmoGroups = ControlRoom.GetAmmo();
		}
		if(CannonReloader == null){
			CannonReloader = ControlRoom.GetReloader();
		}

		AmmoObjectIdentifier ammunition = null;
		for(int i = AmmoGroups.Count-1; i >= 0; i--){
			List<AmmoObjectIdentifier> ammo = AmmoGroups[i].ammo;
			for(int j = ammo.Count-1; j >= 0; j--){
				//saving the ammo
				ammunition = ammo[j];

				//removing the selected ammo after remembering it (it has to be removed either way)
				AmmoGroups[i].ammo.RemoveAt(j);


				if(ammunition != null){ // if the ammo is valid
					break; // we have found the needed ammo;
				}
			}

			if(ammo.Count == 0){
				AmmoGroups.RemoveAt(i);
			}
			if(ammunition != null){ // if we have found the needed ammo 
				break;
			}
		}
		if(ammunition != null){ // if the ammo was found
			if(CannonReloader.LoadAmmo(new Ammunition(ammunition))){ // we try to load it
				return true; // success
			}
		}
		
		// if we weren't able to load the ammo
		return false;
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
