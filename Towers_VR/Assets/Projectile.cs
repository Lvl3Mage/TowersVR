﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	Rigidbody RB;
	[SerializeField] bool RotateToVelocity;
    // Start is called before the first frame update
    void Start()
    {
    	RB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    	if(RotateToVelocity){
    		transform.rotation = Quaternion.LookRotation(RB.velocity, Vector3.up);
    	}   
    }
}