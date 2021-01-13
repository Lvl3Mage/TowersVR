using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAngleCalculator : MultipleNumberContainer
{
	[SerializeField] NumberContainer OutputDistance;

	protected bool OutputDistanceType;
	public void WriteOutputType(bool val){
		OutputDistanceType = val;
		OnListChange(0);
	}
	protected override void OnListChange(int id){// Calculating the x using the parabola formula constructed with the velocity vector -> y = t^2 * g * 1/2 + t * V.y

		//Getting all the needed values for the calculations
		float alfa = NumberList[0].value*Mathf.Deg2Rad;
		float y = NumberList[1].value;
		float LinearV = NumberList[2].value;

		//Calculating the velocity Vector
		Vector2 V = new Vector2(LinearV*Mathf.Cos(alfa), LinearV*Mathf.Sin(alfa));

		// Calculating the a b and c parameters for the equation of t
		float a = -9.81f/2;
		float b = V.y;
		float c = -y;


		// Calculating both of the t values for given a, b and c values
		Vector2 t = ResolveQuadEcuation(a,b,c);
		t *= V.x; // multiplying the t values by the speed to calculate the distance

		// Setting the value
		if(OutputDistanceType){
			OutputDistance.floatValue = t.y;
		}
		else{
			OutputDistance.floatValue = t.x;
		}
		
	}
	Vector2 ResolveQuadEcuation(float a, float b, float c){ // solves the quadratic equation with given values
		float Sqrt = Mathf.Sqrt(b*b-4*a*c);
		Vector2 Values = new Vector2((-b+Sqrt)/(2*a),(-b-Sqrt)/(2*a));
		return Values;
	}
}
