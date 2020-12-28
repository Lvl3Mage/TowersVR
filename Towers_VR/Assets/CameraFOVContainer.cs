using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFOVContainer : NumberContainer
{

	Camera Cam;
	void Awake(){
		Cam = GetComponent<Camera>();
	}
	protected override void ValueChanged(){
		Cam.fieldOfView = _floatValue;
	}
}
