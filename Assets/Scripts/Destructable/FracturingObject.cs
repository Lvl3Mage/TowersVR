using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FracturingObject : DestructableObject
{
	[SerializeField] Object FracturedObject;
	[Tooltip("If set to true the fractured object will have the same velocity as the original one")]
	[SerializeField] bool KeepVelocity = true;
	
	Rigidbody ownRigidbody;
	void Start(){
		ownRigidbody = GetComponent<Rigidbody>();
	}
	protected override void Destroy(){
		SpawnFracturedObject();
		DisableObject();
	}
	void DisableObject(){
		gameObject.SetActive(false);
	}
	void SpawnFracturedObject(){
		GameObject FracturedInstance = Object.Instantiate(FracturedObject, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent) as GameObject;
		if(ownRigidbody && KeepVelocity){
			ApplyVelocity(FracturedInstance,ownRigidbody.velocity,ownRigidbody.angularVelocity);
		}
	}
	void ApplyVelocity(GameObject fracturedParent, Vector3 Velocity, Vector3 AngularVelocity){
		Rigidbody[] FracturedBodies = fracturedParent.GetComponentsInChildren<Rigidbody>(); // returns all rigidbody components in children and in object
		foreach (Rigidbody Body in FracturedBodies) 
		{
			Body.velocity = Velocity;
			Body.angularVelocity = AngularVelocity;
		}
	}
}
