using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
	[Header("Wind application settings")]
	[SerializeField] float WindForce = 1;
	List<Rigidbody> AffectedObjects = new List<Rigidbody>();
	protected Vector2 _WindVector;
	public Vector2 WindVector{
		get{
			return _WindVector;
		}
	}
	protected virtual Vector2 CalculateWind(){
		return Vector2.zero;
	}
	void Update(){
		//if(AffectedObjects.Count>0){
		_WindVector = CalculateWind();
		for (int i = 0; i < AffectedObjects.Count; i++) 
        {
			AffectedObjects[i].velocity += new Vector3(_WindVector.x,0,_WindVector.y)*WindForce*Time.deltaTime*40;
        }		
		//}

	}
	void OnTriggerEnter(Collider other){
		if(other.attachedRigidbody){
			bool HasCollided = false; // Checks if the object has colided with this collider previously
	        for (int i = 0; i < AffectedObjects.Count && !HasCollided; i++) 
	        {
	            if(AffectedObjects[i] == other.attachedRigidbody){
	                HasCollided = true;
	            }
	        }
	        if(!HasCollided){
	            AffectedObjects.Add(other.attachedRigidbody);
	        }			
		}

	}
	void OnTriggerExit(Collider other){
    	for (int i = 0; i < AffectedObjects.Count; i++) 
	    {
            if(AffectedObjects[i] == other.attachedRigidbody){
                AffectedObjects.RemoveAt(i);
            }
	    } 
	}
}
