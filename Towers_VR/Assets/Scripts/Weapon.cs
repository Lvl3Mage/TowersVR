using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : BoolContainer
{
    [SerializeField] Transform gunPoint;
	[SerializeField] Object MuzzleFlash; // the muzzleflash of the weapon's
	[SerializeField] Rigidbody Barrel; // a reference to the barrel of the weapon's
    [SerializeField] AmmoCaliber WeaponCaliber;
 	[SerializeField] float Recoil, ReloadDelay;
    [Tooltip("A callback array which identifies the objects that receive the weapon's loading state")]
	[SerializeField] DataContainer[] CallBackLoaded;
    [Tooltip("A callback array which identifies the objects that receive the weapon's projectile speed")]
	[SerializeField] DataContainer[] CallBackProjectileSpeed;
    [Tooltip("A callback array which identifies the objects that receive the weapon's last fired projectile")]
    [SerializeField] DataContainer[] CallBackLastProjectile;
    protected Ammunition Clip; // The ammo clip loaded into the weapon
	protected bool State = false;
    public void Reload(Ammunition Ammo){ // Call this to reload 
    	StartCoroutine(Delay(Ammo));
    }
    IEnumerator Delay(Ammunition Ammo){
    	yield return new WaitForSeconds(ReloadDelay);
    	Clip = Ammo;
    	State = true;
        CallBackLoadedState(0); // Loaded
        CallBackProjVel();
    }
    public bool Loaded(){
    	return State;
    }
    public bool Loadable(AmmoCaliber AmmoCaliber){
        return AmmoCaliber == WeaponCaliber;
    }
    protected override void SetBool(DataType dataType, bool toggleValue){ // weapon activation
    	TriggerPressed(toggleValue);
    	if(State){
    		if(toggleValue){ // checks if the trigger is pulled
				FireWeapon();
    		}
    	}
    }
    protected virtual void FireWeapon(){ // gets called if the trigger is pressed and the gun is supposed to start firing

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
    protected void WeaponFired(){ // call this afeter the weapon has been fired
        if(Clip.ammoCount <= 0){
            CallBackLoadedState(2); // unloaded
            State = false; // sets the state to false so the weapon can be reloaded (this will be reworked in the future cause you should obviously be able to reload your weapon without emptying it)
        }
    }
    //call this method to fire the gun
    protected virtual void ShootProjectile(){ // Override this method if you want to create a weapon that will shoot the projectile in a different way
        GameObject LastFiredProjectile = SpawnBullet(Clip.bullet, Clip.velocity, Clip.spread, gunPoint);
        SetFiredProjectile(LastFiredProjectile);
        SpawnEffect(gunPoint);
        PushBack(Recoil, gunPoint);
    }
    GameObject SpawnBullet(Object bullet, float velocity, float spread, Transform gunPoint){ // the function for shooting out the projectile
        GameObject Projectile = Object.Instantiate(bullet, gunPoint.position, gunPoint.rotation) as GameObject;
        Vector2 RandomSpreadPoint = Random.insideUnitCircle * spread * velocity; // calculating the randomized spread point with the spread as the precentage from the velocity
        Projectile.GetComponent<Rigidbody>().velocity = Projectile.transform.TransformDirection(Vector3.forward * velocity + new Vector3(RandomSpreadPoint.x,RandomSpreadPoint.y,0)); // adding the velocity and the spread point velocity
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
