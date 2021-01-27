using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinGeneratedWind : Wind
{
	[Header("Base generation settings")]
	[SerializeField] Vector2 BaseWindforceRange;
	[SerializeField] Vector2 ForceSeedRange;
	[SerializeField] Vector2 RotationSeedRange;
	[Header("Wind variation settings")]
	[SerializeField] float timeScale;
	[SerializeField] Vector2 WindforceRange;
	[SerializeField] Vector2 WindangleRange;
	Vector2 BaseWind;
	float ForceSeed,AngleSeed;
	void Awake(){
		Vector2 Direction = GenerateBaseWindDirection();
		float force = GenerateBaseWindForce();
		BaseWind = Direction*force;
		ForceSeed = Random.Range(ForceSeedRange.x, ForceSeedRange.y);
		AngleSeed = Random.Range(RotationSeedRange.x, RotationSeedRange.y);
	}
	Vector2 GenerateBaseWindDirection(){
		Vector2 Dir = Random.insideUnitCircle;
		return Dir.normalized;
	}
	float GenerateBaseWindForce(){
		float force = Random.Range(BaseWindforceRange.x,BaseWindforceRange.y);
		return force;
	}
	protected override Vector2 CalculateWind(){
		Vector2 VariatedWind;
		float angle = GenerateAngleVariation();
		VariatedWind = RotateVector(BaseWind,angle)*GenerateForceVariation();

		return VariatedWind;
	}
	Vector2 RotateVector(Vector2 Input, float angle){
		angle *= Mathf.Deg2Rad;
		Vector2 Output;
		float sine = Mathf.Sin(angle);
		float cosine = Mathf.Cos(angle);
		Output.x = Input.x*cosine-Input.y*sine;
		Output.y = Input.x*sine+Input.y*cosine;
		return Output;
	}
	float GenerateAngleVariation(){
		float RangedValue = ChangeRange(GetPerlin(AngleSeed), new Vector2(0f,1f), WindangleRange);// converting a 0-1 range to the new windangle range
		return RangedValue;
	}
	float GenerateForceVariation(){
		float RangedValue = ChangeRange(GetPerlin(ForceSeed), new Vector2(0f,1f), WindforceRange);// converting a 0-1 range to the new windforce range
		return RangedValue;
	}
	float ChangeRange(float value, Vector2 OldRange, Vector2 NewRange){
		float NewValue = (((value - OldRange.x) * (NewRange.y - NewRange.x)) / (OldRange.y - OldRange.x)) + NewRange.x;
		return NewValue;
	}
	float GetPerlin(float Seed){
		return Mathf.PerlinNoise((Time.time%256)*timeScale+Seed, (Time.time%256)*timeScale+Seed);
	}
}
