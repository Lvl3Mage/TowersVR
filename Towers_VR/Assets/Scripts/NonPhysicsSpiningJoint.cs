using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPhysicsSpiningJoint : NumberContainer
{
	[SerializeField] Transform ConnectedBody;
	[SerializeField] bool Invert;
	[SerializeField] float Offset;
	protected override void ValueChanged(){
		int inv;
		if(Invert){
			inv = -1;
		}
		else {
			inv = 1;
		}
		ConnectedBody.localEulerAngles = new Vector3(ConnectedBody.localEulerAngles.x, (_floatValue + Offset)*inv, ConnectedBody.localEulerAngles.z);
	}
}
