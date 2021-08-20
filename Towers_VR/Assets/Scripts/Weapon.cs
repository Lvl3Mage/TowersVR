using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : BoolContainer
{
	[SerializeField] protected Object MuzzleFlash; // the muzzleflash of the weapon's
	[SerializeField] protected Rigidbody Barrel; // a reference to the barrel of the weapon's
    [SerializeField] AmmoCaliber WeaponCaliber;
 	[SerializeField] protected float Recoil, ReloadDelay;
    [Tooltip("A callback array which identifies the objects that receive the weapon's loading state")]
	[SerializeField] protected DataContainer[] CallBackLoaded;
    [Tooltip("A callback array which identifies the objects that receive the weapon's projectile speed")]
	[SerializeField] protected DataContainer[] CallBackProjectileSpeed;
    [Tooltip("A callback array which identifies the objects that receive the weapon's last fired projectile")]
    [SerializeField] protected DataContainer[] CallBackLastProjectile;
    protected Ammunition Clip; // The ammo clip loaded into the weapon
	protected bool State = false;
    public void Reload(Ammunition Ammo){ // Call this to reload 
    	StartCoroutine(Delay(Ammo));
    }
    IEnumerator Delay(Ammunition Ammo){
    	yield return new WaitForSeconds(ReloadDelay);
    	Clip = Ammo;
    	State = true;
        CallBackLoadedState(0);
        CallBackProjVel();
    }
    public bool Loaded(){
    	return State;
    }
    public bool Loadable(AmmoCaliber AmmoCaliber){
        return AmmoCaliber == WeaponCaliber;
    }
    protected override void SetBool(DataType dataType, bool toggleValue){
    	TriggerPressed(toggleValue);
    	if(State){
    		if(toggleValue){ // checks if the trigger is pulled
				FireWeapon();
    		}
    	}
    }
    protected virtual void FireWeapon(){

    }
    protected virtual void TriggerPressed(bool toggleValue){

    }
    protected void CallBackLoadedState(int Val){ // reports weapon's loaded state to all
        foreach (DataContainer Cont in CallBackLoaded) 
        {
            Cont.SetValue(DataType.CannonLoadState, Val);
        }
    }
    void CallBackProjVel(){ // reports weapon's projectile velocity to all
        foreach (DataContainer Cont in CallBackProjectileSpeed) 
        {
            Cont.SetValue(DataType.ProjectileLinearVelocity, Clip.velocity);
        }
        
    }
    protected virtual void ShootProjectile(Object bullet,float velocity,float Force, Transform gunPoint){ // Override this method if you want to create a weapon that will shoot the projectile in a different way
        GameObject LastFiredProjectile = SpawnBullet(bullet, velocity, gunPoint);
        SetFiredProjectile(LastFiredProjectile);
        SpawnEffect(gunPoint);
        PushBack(Force, gunPoint);
    }
    GameObject SpawnBullet(Object bullet, float velocity, Transform gunPoint){ // the function for shooting out the projectile
        GameObject Projectile = Object.Instantiate(bullet, gunPoint.position, gunPoint.rotation) as GameObject;
        Projectile.GetComponent<Rigidbody>().velocity = Projectile.transform.TransformDirection(Vector3.forward * velocity /*RandomInaccuracy * velocity*/);
        return Projectile;
    }
    void SpawnEffect(Transform gunPoint){
        Object.Instantiate(MuzzleFlash, gunPoint.position, gunPoint.rotation); // spawns muzzleflash
    }
    void PushBack(float Force, Transform gunPoint){
        //Barrel.AddForceAtPosition(gunPoint.TransformDirection(Vector3.back) * Force, gunPoint.position); // adds recoil
        Barrel.velocity -= gunPoint.TransformDirection(Vector3.forward * Force);
    }

    public void SetFiredProjectile(GameObject Projectile){
        foreach (DataContainer Cont in CallBackLastProjectile) 
        {
            Cont.SetValue(DataType.LastFiredProjectile, Projectile);
        }

    }
}
