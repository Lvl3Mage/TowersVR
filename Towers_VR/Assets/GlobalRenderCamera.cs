using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalRenderCamera : MonoBehaviour
{
	[SerializeField] Camera Camera;
	void Awake(){
		Camera.enabled = false;
	}
    public void RenderToTexture(Transform cameraPoint, RenderTexture texture, float FOV){
    	transform.position = cameraPoint.position;
    	transform.rotation = cameraPoint.rotation;
    	Camera.targetTexture = texture;
    	Camera.fieldOfView = FOV;
    	Camera.Render();
    }
}
