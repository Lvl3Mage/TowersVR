using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FracturingObject : DestructableObject
{
	[SerializeField] Object FracturedObject;
	protected override void Destroy(){
		DisableObject();
		SpawnFracturedObject();
	}
	protected virtual void DisableObject(){
		gameObject.SetActive(false);
	}
	protected virtual void SpawnFracturedObject(){
		Object.Instantiate(FracturedObject, gameObject.transform.position, gameObject.transform.rotation, gameObject.transform.parent);
	}
}
