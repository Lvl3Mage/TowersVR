using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWind : PerlinGeneratedWind
{
	protected List<Rigidbody> AffectedObjectsRigidbody = new List<Rigidbody>();
	protected List<Projectile> AffectedObjectsProjectile = new List<Projectile>();

	void OnTriggerEnter(Collider other){
		if(other.attachedRigidbody){
			Projectile Projectile = other.attachedRigidbody.gameObject.GetComponent<Projectile>();
			if(Projectile){
				bool HasCollided = false; // Checks if the object has colided with this collider previously
		        for (int i = 0; i < AffectedObjectsRigidbody.Count && !HasCollided; i++) 
		        {
		            if(AffectedObjectsRigidbody[i] == other.attachedRigidbody){
		                HasCollided = true;
		            }
		        }
		        if(!HasCollided){
		            AffectedObjectsRigidbody.Add(other.attachedRigidbody);
		            AffectedObjectsProjectile.Add(Projectile);
		        }			
			}
			
		}

	}
	void OnTriggerExit(Collider other){
    	for (int i = 0; i < AffectedObjectsRigidbody.Count; i++) 
	    {
            if(AffectedObjectsRigidbody[i] == other.attachedRigidbody){
                AffectedObjectsRigidbody.RemoveAt(i);
                AffectedObjectsProjectile.RemoveAt(i);
            }
	    } 
	}
	protected override void ApplyWind(Vector3 Wind){
		for (int i = 0; i < AffectedObjectsRigidbody.Count; i++) 
        {
        	float ProjectileWindFactor = AffectedObjectsProjectile[i].WindFactor;
			AffectedObjectsRigidbody[i].velocity += new Vector3(Wind.x,0,Wind.y)*Time.deltaTime*40*ProjectileWindFactor;
        }
	}
}
