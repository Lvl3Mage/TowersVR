using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFOVContainer : FloatContainer
{
	[SerializeField] float DefaultValue;
	[SerializeField] RenderCamera Cam;
	void Awake(){
		Cam.SetFOV(DefaultValue);
	}
	protected override void SetFloat(DataType type, float value){
		Cam.SetFOV(value);
	}
}
