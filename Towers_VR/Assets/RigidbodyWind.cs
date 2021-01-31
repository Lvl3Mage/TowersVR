using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyWind : PerlinGeneratedWind
{
	[SerializeField] bool MassAffected;
	protected List<Rigidbody> AffectedObjectsRigidbody = new List<Rigidbody>();
	void OnTriggerEnter(Collider other){
		if(other.attachedRigidbody){
			bool HasCollided = false; // Checks if the object has colided with this collider previously
	        for (int i = 0; i < AffectedObjectsRigidbody.Count && !HasCollided; i++) 
	        {
	            if(AffectedObjectsRigidbody[i] == other.attachedRigidbody){
	                HasCollided = true;
	            }
	        }
	        if(!HasCollided){
	            AffectedObjectsRigidbody.Add(other.attachedRigidbody);
	        }			
		}

	}
	void OnTriggerExit(Collider other){
    	for (int i = 0; i < AffectedObjectsRigidbody.Count; i++) 
	    {
            if(AffectedObjectsRigidbody[i] == other.attachedRigidbody){
                AffectedObjectsRigidbody.RemoveAt(i);
            }
	    } 
	}
	protected override void ApplyWind(Vector3 Wind){
		for (int i = 0; i < AffectedObjectsRigidbody.Count; i++) 
        {
        	float MassFactor = 1;
        	if(MassAffected){
        		MassFactor = AffectedObjectsRigidbody[i].mass;
        	}
			AffectedObjectsRigidbody[i].velocity += new Vector3(Wind.x,0,Wind.y)*Time.deltaTime*40*MassFactor;
        }
	}
}
