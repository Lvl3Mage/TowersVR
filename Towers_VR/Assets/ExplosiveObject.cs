﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveObject : DestructableObject
{
	[SerializeField] Object ExplosionEffect, Explosion;
    protected override void OnDamage(){
    	// Play a sound effect
    }
    protected override void Destroy(){
    	Object.Instantiate(ExplosionEffect, transform.position, transform.rotation); // Spawn this in garbage later 
    	Object.Instantiate(Explosion, transform.position, transform.rotation); // Spawn this in garbage later 
    }
}