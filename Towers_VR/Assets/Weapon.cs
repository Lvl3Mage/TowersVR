using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Activatable
{
	[SerializeField] protected Object MuzzleFlash;
	[SerializeField] protected Rigidbody Barrel;
	[SerializeField] protected float Recoil, ReloadDelay;
    [Tooltip("Horizontal and vertical inaccuracy multiplied by the shell velocity")]
    [SerializeField] protected Vector2 Inaccuracy;
	[SerializeField] protected IntContainer[] CallBackLoaded; // a callback array which identifies the object that are in need of informing about the cannon loaded state
	[SerializeField] protected NumberContainer[] CallBackProjectileSpeed;
    protected AmmoIdentifier Clip; // The ammo clip loaded into the weapon
	protected bool State = false;
    public void Reload(AmmoIdentifier Object){ // Call this to reload 
    	StartCoroutine(Delay(Object));
    }
    IEnumerator Delay(AmmoIdentifier Object){
    	yield return new WaitForSeconds(ReloadDelay);
    	Clip = Object;
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
    protected void CallBackLoadedState(int Val){
        foreach (IntContainer Cont in CallBackLoaded) 
        {
            Cont.intValue = Val;
        }
    }
    void CallBackProjVel(){
        foreach (NumberContainer Cont in CallBackProjectileSpeed) 
        {
            Cont.floatValue = Clip.velocity;
        }
        
    }
    protected virtual void ShootProjectile(Object bullet,float velocity,float Force, Transform gunPoint){ // Override this method if you want to create a weapon that will shoot the projectile in a different way
        SpawnBullet(bullet, velocity, gunPoint);
        SpawnEffect(gunPoint);
        PushBack(Force);
    }
    GameObject SpawnBullet(Object bullet, float velocity, Transform gunPoint){ // the function for shooting out the projectile
        GameObject Projectile = Object.Instantiate(bullet, gunPoint.position, gunPoint.rotation) as GameObject;
        Vector3 RandomInaccuracy = new Vector3(Random.Range(-Inaccuracy.x, Inaccuracy.x),Random.Range(-Inaccuracy.y, Inaccuracy.y),0);
        Projectile.GetComponent<Rigidbody>().velocity = Projectile.transform.TransformDirection(Vector3.forward * velocity /*RandomInaccuracy * velocity*/);
        return Projectile;
    }
    void SpawnEffect(Transform gunPoint){
        Object.Instantiate(MuzzleFlash, gunPoint.position, gunPoint.rotation); // spawns muzzleflash
    }
    void PushBack(float Force){
        Barrel.velocity -= Barrel.gameObject.transform.TransformDirection(Vector3.forward * Force); // adds recoil
    }
}
