using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionExplosive : MonoBehaviour // rewrite this to be inherited
{
	[SerializeField] Object ExplosionEffect, Explosion;
	bool Exploded = false;
	void OnCollisionEnter(Collision collisionInfo){
		if(!Exploded){
			Object.Instantiate(ExplosionEffect, transform.position, transform.rotation);
			Object.Instantiate(Explosion, transform.position, transform.rotation);
			gameObject.SetActive(false); 			
		}

	}
}
