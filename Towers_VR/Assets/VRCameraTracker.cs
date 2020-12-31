using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRCameraTracker : MonoBehaviour
{
	[SerializeField] Transform Camera;
	public enum Tracking{ Vertical, Horizontal}
	[SerializeField] Tracking TrackingType;
    void Update()
    {
    	if(TrackingType == Tracking.Vertical){
    		transform.localPosition = new Vector3(0,Camera.localPosition.y,0);
    	}
    	else{
	        transform.localPosition = new Vector3(Camera.localPosition.x,0,Camera.localPosition.z);
	        transform.localEulerAngles = new Vector3(0,Camera.localEulerAngles.y,0);	
    	}
    }
}
