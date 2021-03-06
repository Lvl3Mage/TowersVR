﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayDistanceCalculator : NumberContainer
{
	[SerializeField] NumberContainer[] Targets;
	[SerializeField] float MaxDistance;
	[SerializeField] LayerMask Layers;
	/*Vector3 LastPos;
	Quaternion LastRot;
	void Update(){
		if(LastPos != transform.position || LastRot != transform.rotation){
			CalculateDistance();
			LastPos = transform.position;
			LastRot = transform.rotation;
		}
	}*/
	protected override void ValueChanged(){ //Will recalculate the distance on the NumberContainer value change. Add this NumberContainer to any inputs for them to activate distance recalculation
		CalculateDistance();
	}
	public void RecalculateDistance(){ // Call this to update the distance
		CalculateDistance();
	}
	void CalculateDistance(){
		float Distance = RaycastDistance();
		foreach (NumberContainer Target in Targets) 
		{
			Target.floatValue = Distance;
		}
	}
    float RaycastDistance(){
    	RaycastHit hit;
    	if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, MaxDistance, Layers)){
    		return (hit.point - transform.position).magnitude;
    	}
    	else{
    		return Mathf.Infinity;
    	}
    }
}
