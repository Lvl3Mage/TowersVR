using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAngleCalculator : MultipleNumberContainer
{
	[SerializeField] NumberContainer OutputDistance;

	protected bool OutputDistanceType = false;
	public void WriteOutputType(bool val){
		OutputDistanceType = val;
		OnListChange(0);
	}
	protected override void OnListChange(int id){// Calculating the x using the parabola formula constructed with the velocity vector -> y = t^2 * g * 1/2 + t * V.y

		//Getting all the needed values for the calculations
		float alpha = NumberList[0].value;
		float y = NumberList[1].value;
		float LinearV = NumberList[2].value;

		// Setting the value
		Vector2 Dist = ProjectileMotion.ResolveParabolaForX(alpha,LinearV,y); // calculates the two distances (x is the far away and y is the close one)
		Debug.Log(Dist);

		if(OutputDistanceType){ // in case the switch is pulled then we need to return the close distance
			OutputDistance.floatValue = Dist.y;
		}
		else{ // otherwise return the long one
			OutputDistance.floatValue = Dist.x;
		}
	}
}
