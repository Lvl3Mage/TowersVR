using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAngleCalculator : ReferenceContainer
{
	Dictionary<string,float> NamedData = new Dictionary<string,float>(){
		{"AngleCalcOutputType",0},
		{"CannonAlpha",0},
		{"TargetYPosition",0},
		{"ProjectileLinearVelocity",0}
	};
	protected bool OutputDistanceType = false;

	protected override void ChangeValue(string varName, float value){
        NamedData[varName] = value;
    }
	protected override void OnValueChange(string varName){// Calculating the x using the parabola formula constructed with the velocity vector -> y = t^2 * g * 1/2 + t * V.y

		//Getting all the needed values for the calculations
		float alpha = NamedData["CannonAlpha"];
		float y = NamedData["TargetYPosition"];
		float LinearV = NamedData["ProjectileLinearVelocity"];

		// Setting the value
		Vector2 Dist = ProjectileMotion.ResolveParabolaForX(alpha,LinearV,y); // calculates the two distances (x is the far away and y is the close one)

		if(NamedData["AngleCalcOutputType"] != 0){ // in case the switch is pulled then we need to return the close distance
			InvokeAllReferences(Dist.y);
		}
		else{ // otherwise return the long one
			InvokeAllReferences(Dist.x);
		}
	}
}
