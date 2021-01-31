using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
	[Header("Wind application settings")]
	[SerializeField] float WindForce = 1;
	Vector2 _WindVector;
	public Vector2 WindVector{
		get{
			return _WindVector;
		}
	}
	protected virtual Vector2 CalculateWind(){
		return Vector2.zero;
	}
	void Update(){
		// Calculations are neccesary every frame because of the compass
		_WindVector = CalculateWind();
		ApplyWind(_WindVector*WindForce);
	}
	protected virtual void ApplyWind(Vector3 Wind){

	}
}
