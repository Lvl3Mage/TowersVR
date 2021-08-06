using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPhysicsSpiningJoint : DataContainer
{
	[SerializeField] Transform ConnectedBody;
	[SerializeField] bool Invert;
	[SerializeField] float Offset;
	float rotation;
	protected override void ChangeValue(string varName, float value){
		rotation = value;
		int inv;
		if(Invert){
			inv = -1;
		}
		else {
			inv = 1;
		}
		ConnectedBody.localEulerAngles = new Vector3(ConnectedBody.localEulerAngles.x, (rotation + Offset)*inv, ConnectedBody.localEulerAngles.z);
	}
}
