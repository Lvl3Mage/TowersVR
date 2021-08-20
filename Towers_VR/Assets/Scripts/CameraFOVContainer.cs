using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFOVContainer : NumberContainer
{
	[SerializeField] float DefaultValue;
	[SerializeField] RenderCamera Cam;
	void Awake(){
		_floatValue = DefaultValue;
	}
	protected override void ValueChanged(){
		Cam.SetFOV(_floatValue);
	}
}
