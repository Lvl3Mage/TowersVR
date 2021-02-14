using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactDestroyedProjectile : ImpactActivatedProjectile
{
	[SerializeField] Object OnContactEffect;
	protected override void Activate(Collision collisionInfo){
		if(OnContactEffect){
			Object.Instantiate(OnContactEffect, transform.position, transform.rotation);			
		}

		gameObject.SetActive(false);
	}
}
