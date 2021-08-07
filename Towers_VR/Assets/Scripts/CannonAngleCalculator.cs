using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAngleCalculator : DataContainer
{
	Dictionary<string,float> NamedData = new Dictionary<string,float>(){
		{"AngleCalcOutputType",0},
		{"CannonAngle",0},
		{"Height",0},
		{"ProjectileVelocity",0}
	};
    [Tooltip("A callback array which identifies the objects that receive the projectile travel distance")]
    [SerializeField] protected DataContainer[] CallBackDist;
    [SerializeField] string DistVarName; 
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

		float distance;
		if(NamedData["AngleCalcOutputType"] != 0){ // in case the switch is pulled then we need to return the close distance
			distance = Dist.y;
		}
		else{ // otherwise return the long one
			distance = Dist.x;
		}
		foreach(DataContainer cont in CallBackDist){
            cont.SetValue(DistVarName,distance);
        }
	}
}
