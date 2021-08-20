using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleContainer : NumberContainer
{
	[SerializeField] NumberContainer[] AngleTarget;
	[SerializeField] bool Clamp;
	[SerializeField] Vector2 ClampNum;
	[SerializeField] float DefaultValue;
	void Awake(){
		floatValue = DefaultValue;
	}
	protected override void ValueChanged(){
		if(Clamp){
			_floatValue = Mathf.Clamp(_floatValue, ClampNum.x, ClampNum.y); 			
		}
		foreach (NumberContainer Container in AngleTarget) 
		{
			Container.floatValue = _floatValue;
		}
	}
}
