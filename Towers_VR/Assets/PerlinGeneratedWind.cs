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
	[Tooltip("Defines the number of steps in the pregenerated 256 unit perlin range (the more there are the more perlin values will be generated within the same range)")]
	[SerializeField] int ForcePerlinSteps;
	[SerializeField] Vector2 WindangleRange;
	[Tooltip("Defines the number of steps in the pregenerated 256 unit perlin range (the more there are the more perlin values will be generated within the same range)")]
	[SerializeField] int AnglePerlinSteps;
	Vector2 BaseWind;
	float[] AngleGeneratedPerlin;
	float[] ForceGeneratedPerlin;
	void Awake(){
		Vector2 Direction = GenerateBaseWindDirection();
		float force = GenerateBaseWindForce();
		BaseWind = Direction*force;
		float ForceSeed = Random.Range(ForceSeedRange.x, ForceSeedRange.y);
		ForceGeneratedPerlin = GeneratePerlin(ForceSeed, ForcePerlinSteps);

		float AngleSeed = Random.Range(RotationSeedRange.x, RotationSeedRange.y);
		AngleGeneratedPerlin = GeneratePerlin(AngleSeed, AnglePerlinSteps);
	}
	float[] GeneratePerlin(float seed, int PerlinSteps){ // generates an array of perlin values in a range of 0-256 with a defined length
		float[] GeneratedPerlin = new float[PerlinSteps];
		float step = 256f/((float)(PerlinSteps)); // calculating the step size defined as the fraction of 256 devided into the steps defined by the user
		for(int i = 0; i<PerlinSteps; i++){ // iterating to for the array
			float sampleCoord = i*step + seed; // calculating the coordinates of this step and adding the seed to it
			float sampleVal = Mathf.PerlinNoise(sampleCoord, sampleCoord); // sampling perlin
			GeneratedPerlin[i] = sampleVal;
		}
		return GeneratedPerlin;
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
		float RangedValue = ChangeRange(SampleAnglePerlin(), new Vector2(0f,1f), WindangleRange);// converting a 0-1 range to the new windangle range
		return RangedValue;
	}
	float GenerateForceVariation(){
		float RangedValue = ChangeRange(SampleForcePerlin(), new Vector2(0f,1f), WindforceRange);// converting a 0-1 range to the new windforce range
		return RangedValue;
	}
	float ChangeRange(float value, Vector2 OldRange, Vector2 NewRange){
		float NewValue = (((value - OldRange.x) * (NewRange.y - NewRange.x)) / (OldRange.y - OldRange.x)) + NewRange.x;
		return NewValue;
	}
	float SampleAnglePerlin(){
		float unroundedID = (Time.time*timeScale)%AngleGeneratedPerlin.Length; // Calculates the coordinates using multiplication of current time and the timescale clamped in between the array length
		int prevID = Mathf.FloorToInt(unroundedID); 
		int nextID = Mathf.CeilToInt(unroundedID);
		float idPercentage = unroundedID-prevID; // defines where the unrounded id was on the prevID-nextID spectrum 
		float prevVal = AngleGeneratedPerlin[prevID];
		float nextVal = AngleGeneratedPerlin[nextID];
		float perlinVal = Mathf.Lerp(prevVal, nextVal, idPercentage); //lerps between the previous value and the next value based on how close we were to the next/ previous value
		return perlinVal;
	}
	float SampleForcePerlin(){
		float unroundedID = (Time.time*timeScale)%ForceGeneratedPerlin.Length; // Calculates the coordinates using multiplication of current time and the timescale clamped in between the array length
		int prevID = Mathf.FloorToInt(unroundedID); 
		int nextID = Mathf.CeilToInt(unroundedID);

		float idPercentage = unroundedID-prevID; // defines where the unrounded id was on the prevID-nextID spectrum 
		float prevVal = ForceGeneratedPerlin[prevID];
		float nextVal = ForceGeneratedPerlin[nextID];
		float perlinVal = Mathf.Lerp(prevVal, nextVal, idPercentage); //lerps between the previous value and the next value based on how close we were to the next/ previous value
		return perlinVal;
	}
}
