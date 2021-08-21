using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{
	[SerializeField] protected float _Health = 0;
	[SerializeField] bool OutsideDamage = true;
	[SerializeField] float OutsideDamageMultiplier = 1;
	[SerializeField] bool PhysicsDamage = true;
	[SerializeField] float PhysicsDamageMultiplier = 1, PhysicsDamageThreshold = 0; /*VelocityDamageMultiplier = 1, MaximumMassDamageMultiplier = 1, MassDamageDecay = 90,  ;*/
	public float Health
	{
		get{
			return _Health;
		}
	}
    [ContextMenu("Damage Object")]
    public void DamageObject(float Damage){
    	if(OutsideDamage){
 			ApplyDamage(Damage*OutsideDamageMultiplier);
    	}
    }
    protected virtual void OnCollisionEnter(Collision collisionInfo){ // change this to use applied force for damage calculation
    	if(PhysicsDamage){
    		//Rigidbody ColidedRigidbody = collisionInfo.rigidbody; // refers to the rigidbody we've collided with
    		//float VelocityDamage = collisionInfo.relativeVelocity.magnitude* VelocityDamageMultiplier;
    		//float MassDamage;

    		//rewrite with impulse
    		/*if(ColidedRigidbody){ // if colided with a rigidbody then it calculates the mass damage, but if the collision occured with a static object the mass damage is set to 1 (it doesn't depend on mass in this case)
    			MassDamage = ColidedRigidbody.mass;
    			MassDamage = (MassDamage/(MassDamageDecay+MassDamage)) * MaximumMassDamageMultiplier; // recalculating using the hyperbolic formula
    		}
    		else{
    			MassDamage = 1;
    		}*/
    		//float Damage = VelocityDamage * MassDamage; // This can be done in three multiplication but I am keeping the multipliers separate because it makes more sence this way
    		float Damage = collisionInfo.impulse.magnitude * PhysicsDamageMultiplier;
    		if(Damage > PhysicsDamageThreshold){
    			ApplyDamage(Damage);
    			//Debug.Log("Damaged by " + Damage, gameObject);
    		}
    	}
    }
    protected virtual void ApplyDamage(float Damage){
    	if(_Health > 0){
    		_Health -= Damage;
    		OnDamage();
	    	if(_Health <= 0){
	    		Destroy();
	    	}
    	}  
    }
    protected virtual void OnDamage(){

    }
    protected virtual void Destroy(){
    	
    }
}
