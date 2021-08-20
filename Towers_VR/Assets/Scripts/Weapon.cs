using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Activatable
{
    [SerializeField] protected Object MuzzleFlash; // the muzzleflash of the weapon's
	[SerializeField] protected Rigidbody Barrel; // a reference to the barrel of the weapon's
	[SerializeField] protected float Recoil, ReloadDelay;
    [Tooltip("A callback array which identifies the objects that receive the weapon's loading state")]
	[SerializeField] protected DataContainer[] CallBackLoaded;
    [SerializeField] string CallbackLoadedVarName;
    [Tooltip("A callback array which identifies the objects that receive the weapon's projectile speed")]
	[SerializeField] protected DataContainer[] CallBackProjectileSpeed;
    [SerializeField] string CallBackProjectileSpeedVarName;
    protected Ammunition Clip; // The ammo clip loaded into the weapon
	protected bool State = false;
    GameObject LastFiredProjectile; // contains the last fired projectile

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
    protected override void OnActivate(bool toggleValue){
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
            Cont.SetValue(CallbackLoadedVarName, Val);
        }
    }
    void CallBackProjVel(){ // reports weapon's projectile velocity to all
        foreach (DataContainer Cont in CallBackProjectileSpeed) 
        {
            Cont.SetValue(CallBackProjectileSpeedVarName, Clip.velocity);
        }
        
    }
    protected virtual void ShootProjectile(Object bullet,float velocity,float Force, Transform gunPoint){ // Override this method if you want to create a weapon that will shoot the projectile in a different way
        LastFiredProjectile = SpawnBullet(bullet, velocity, gunPoint);
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

    public GameObject GetLastProjectile(){
        return LastFiredProjectile;
    }
    public float GetProjectileSpeed(){
        if(Clip != null){
            return Clip.velocity;
        }
        else{
            return 0;
        }
    }
}
