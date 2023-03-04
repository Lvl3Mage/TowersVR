using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPhysicsSpiningJoint : FloatContainer
{
	[SerializeField] Transform ConnectedBody;
	[SerializeField] bool Invert;
	[SerializeField] float Offset;
	protected override void SetFloat(DataType type, float value){
		int inv;
		if(Invert){
			inv = -1;
		}
		else {
			inv = 1;
		}
		ConnectedBody.localEulerAngles = new Vector3(ConnectedBody.localEulerAngles.x, (value + Offset)*inv, ConnectedBody.localEulerAngles.z);
	}
}
