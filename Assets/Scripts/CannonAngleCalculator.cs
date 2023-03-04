using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAngleCalculator : FloatContainer
{
	[SerializeField] DataContainer OutputDistance;
	float alpha;
	float y;
	float LinearVelocity;
	bool OutputDistanceType = false;
	public void WriteOutputType(bool val){
		OutputDistanceType = val;
		CalculateDistance();
	}
	protected override void SetFloat(DataType dataType, float value){// Calculating the x using the parabola formula constructed with the velocity vector -> y = t^2 * g * 1/2 + t * V.y

		switch(dataType){
			case DataType.CannonAngle:
				alpha = value;
				break;
			case DataType.Height:
				y = value;
				break;
			case DataType.ProjectileLinearVelocity:
				LinearVelocity = value;
				break;
			default:
				Debug.LogError("WTF is this => " + dataType + "?", gameObject);
				break;
		}
		CalculateDistance();
	}
	void CalculateDistance(){
		// Setting the value
		Vector2 Dist = ProjectileMotion.ResolveParabolaForX(alpha,LinearVelocity,y); // calculates the two distances (x is the far away and y is the close one)

		if(OutputDistanceType){ // in case the switch is pulled then we need to return the close distance
			OutputDistance.SetValue(DataType.EstimatedProjectileTravelDistance, Dist.y);
		}
		else{ // otherwise return the long one
			OutputDistance.SetValue(DataType.EstimatedProjectileTravelDistance, Dist.x);
		}
	}
}
