using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactActivatedProjectile : Projectile
{
	void OnCollisionEnter(Collision collisionInfo){
		Activate(collisionInfo);
	}
	protected virtual void Activate(Collision collisionInfo){

	}
}
