using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleContainer : ReferenceContainer
{
	[SerializeField] bool Clamp;
	[SerializeField] Vector2 ClampNum;
	[SerializeField] float angle;
 
	public void ChangeAngle(float change){
		angle += change;
		Mathf.Clamp(angle,ClampNum.x,ClampNum.y);
		InvokeAllReferences(angle);
	}
	protected override void ChangeValue(string varName, float value){
		angle = value;
        InvokeAllReferences(angle);
    }

}
