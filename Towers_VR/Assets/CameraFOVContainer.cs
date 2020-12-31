using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFOVContainer : NumberContainer
{
	[SerializeField] float DefaultValue;
	[SerializeField] Camera Cam;
	void Awake(){
		_floatValue = DefaultValue;
		//Cam = GetComponent<Camera>();
	}
	protected override void ValueChanged(){
		Cam.fieldOfView = _floatValue;
	}
}
