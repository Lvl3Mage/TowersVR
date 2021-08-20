using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsJoint : NumberContainer
{
	[SerializeField] HingeJoint HJ;
	[SerializeField] float baseOffset, angleOffset, slowdownAngle, maxVelocity, value;
	[SerializeField] Gradient DebugSlowdown;
	[SerializeField] bool Invert, DebugRotation;
	Rigidbody connectedRB, baseRB;
	
	void Awake(){
		connectedRB = HJ.connectedBody;
		baseRB = HJ.gameObject.GetComponent<Rigidbody>();

	}
	void Update(){
		float value = _floatValue+angleOffset;
		if(Invert){
			value *= -1;
		}
		float angleDifference = CalcAngleDifference(connectedRB.gameObject.transform.localEulerAngles.y+baseOffset, value);

		ApplyPhysicsRotation(angleDifference);
	}
	protected virtual void ValueChanged(){

		
		/*Vector3 targetAngVel = baseRB.angularVelocity + connectedRB.gameObject.transform.TransformDirection(0, angleDifference*Mathf.Deg2Rad, 0);
		connectedRB.angularVelocity = targetAngVel;*/
		

	}
	void ApplyPhysicsRotation(float DifAngle){
		float TargetVel = maxVelocity * DifAngle/Mathf.Abs(DifAngle);
		if(Mathf.Abs(DifAngle)<slowdownAngle){
			if(Mathf.Abs(DifAngle)<= 0.001f){//precision
				TargetVel = 0;
			}
			else{
				TargetVel *= (Mathf.Abs(DifAngle)/slowdownAngle); // a lerp between 0 and 1 which starts at DifAngle = slowdownAngle
			}
		}
		Debug.Log(TargetVel);
		Vector3 targetAngVel = baseRB.angularVelocity + connectedRB.gameObject.transform.TransformDirection(0, TargetVel, 0);
		float Delta = TargetVel - connectedRB.angularVelocity.y;
		connectedRB.angularVelocity = targetAngVel;
		baseRB.angularVelocity += baseRB.gameObject.transform.TransformDirection(0, -Delta, 0);
	}

	float ClampRotation(float rot){
		float rotClamped = rot % 360; // clamping the value to 0-360
		if(rot<0){
			rotClamped += 360; // check for positive to negative spin
		}
		return rotClamped;
	}
	float CalcAngleDifference(float current,float target){ // calculates the angle between 2 angles
		float currentClamped = ClampRotation(current);
		float targetClamped = ClampRotation(target);

		float angDif = targetClamped - currentClamped; // calculating the difference
		if(Mathf.Abs(angDif)>180){
			angDif =  (Mathf.Abs(angDif) - 360)*(angDif/Mathf.Abs(angDif)); // check for angle bigger than 180 and then invert the angle
			// the explanation for this is kinda long but in short its (360 - absolute angle) but then negative of that cause obviously it goes in the opposite direction
			// we then multiply this by the sign of the angle so that it works both for the positive transition and for the negative
		}
		if(DebugRotation){
			DebugRay(currentClamped,angDif/slowdownAngle);
			DebugRay(targetClamped,1);
		}
		return angDif;
	}
	void DebugRay(float rotation,float color){
		rotation += 90;
		float x = -Mathf.Cos(rotation*Mathf.Deg2Rad);
		float y = Mathf.Sin(rotation*Mathf.Deg2Rad);
		Vector3 TargetPoint = baseRB.gameObject.transform.TransformDirection(x*6, 0, y*6);
		Debug.DrawRay(connectedRB.gameObject.transform.position, TargetPoint, DebugSlowdown.Evaluate(color));
	}
}
