using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{
	[SerializeField] protected float _Health = 0;
	[SerializeField] bool OutsideDamage = true;
	[SerializeField] float OutsideDamageMultiplier = 1;
	[SerializeField] bool PhysicsDamage = true;
	[SerializeField] float VelocityDamageMultiplier = 1, OtherMassDamageMultiplier = 1, PhysicsDamageThreshold = 0;
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
    protected virtual void OnCollisionEnter(Collision collisionInfo){
    	if(PhysicsDamage){
    		Rigidbody ColidedRigidbody = collisionInfo.rigidbody; // refers to the rigidbody we've collided with
    		float VelocityDamage = collisionInfo.relativeVelocity.magnitude* VelocityDamageMultiplier;
    		float MassDamage;
    		if(ColidedRigidbody){ // if colided with a rigidbody then it calculates the mass damage, but if the cillision occured with a static object the mass famage is set to 1 (it doesn't depend on mass in this case)
    			MassDamage = ColidedRigidbody.mass * OtherMassDamageMultiplier;
    		}
    		else{
    			MassDamage = 1;
    		}

    		float Damage = VelocityDamage * MassDamage; // This can be done in three multiplication but I am keeping the multipliers separate because it makes more sence this way
    		if(Damage > PhysicsDamageThreshold){
    			ApplyDamage(Damage);
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
