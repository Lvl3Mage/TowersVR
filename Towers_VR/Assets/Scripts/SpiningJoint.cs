using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiningJoint : DataContainer
{
    [SerializeField] HingeJoint HJ;
    [SerializeField] bool InvertAngle;	
    [SerializeField] float maxVelocity;
    [SerializeField] float slowdownAngle, baseOffset, angleOffset;
    [SerializeField] Gradient DebugSlowdown;
    [SerializeField] bool DebugRotation;
    float targetRotation = 0;
    // Start is called before the first frame update


    protected override void ChangeValue(string varName, float value){ // when the data container is invoked it sets the desired rotation to passed value
        targetRotation = value;
    }
    // Update is called once per frame
    void Update()
    {
        float ownRotation = HJ.connectedBody.gameObject.transform.localEulerAngles.y + baseOffset;
        if(InvertAngle){
            targetRotation *= -1;
        }
        targetRotation += angleOffset;
        float AngleDifference = CalcAngleDifference(ownRotation,targetRotation);
        ApplyPhysicsRotation(AngleDifference);
    }
    void ApplyPhysicsRotation(float DifAngle){
        float TargetVel = -maxVelocity * DifAngle/Mathf.Abs(DifAngle);
        if(Mathf.Abs(DifAngle)<slowdownAngle){
            if(Mathf.Abs(DifAngle)<= 0.001f){//precision
                TargetVel = 0;
            }
            else{
                TargetVel *= (Mathf.Abs(DifAngle)/slowdownAngle); // a lerp between 0 and 1 which starts at DifAngle = slowdownAngle
            }
        }
        JointMotor mot = HJ.motor;
        mot.targetVelocity = TargetVel;
        HJ.motor = mot;
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
        Vector3 TargetPoint = HJ.gameObject.transform.TransformDirection(x*6, 0, y*6);
        Debug.DrawRay(HJ.connectedBody.gameObject.transform.position, TargetPoint, DebugSlowdown.Evaluate(color));
    }
}
