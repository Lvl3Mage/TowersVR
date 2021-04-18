using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : Projectile
{
	[SerializeField] Object ExplosionEffect, Explosion, Decal;
	bool Exploded = false;
	protected override void Activate(Collision collisionInfo){
		if(!Exploded){
			CreateExplosion();
			CreateDecall(collisionInfo);
			gameObject.SetActive(false); 			
		}
	}
	void CreateExplosion(){
		Object.Instantiate(ExplosionEffect, transform.position, transform.rotation);
		Object.Instantiate(Explosion, transform.position, transform.rotation);
	}
	void CreateDecall(Collision Collision){
		Transform colidedobj = Collision.collider.gameObject.transform;
		Vector3 direction = Collision.collider.ClosestPoint(transform.position)-transform.position;
		Object.Instantiate(Decal, transform.position, Quaternion.LookRotation(direction, Vector3.up), colidedobj);
	}
}
