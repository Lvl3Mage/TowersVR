using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCameraOptimizer : MonoBehaviour
{
	[SerializeField] Camera LimitedCamera;
	[SerializeField] Camera[] RenderForCamera;
	[SerializeField] Renderer[] Screens;
	[Tooltip("The amount of frames that this will be skipped for this camera before rendering again")]
	[SerializeField] [Range(1,60)] int FrameSkip;
    // Start is called before the first frame update
    int SkippedFrames = 0;
    void Start()
    {
        LimitedCamera.enabled = false;
        //StartCoroutine(FPSWait());
    }
    void FixedUpdate(){
    	if(SkippedFrames<FrameSkip){
    		SkippedFrames++;
    	}
    	else{
    		SkippedFrames = 0;
    		Render();
    	}
    }
    // This shit eats up more performance then the camera itself
    /*IEnumerator FPSWait(){
    	yield return new WaitForSeconds(1/FrameSkip);
    	Render();
    	StartCoroutine(FPSWait());
    }*/
    void Render(){
    	bool NeedRender = false;
    	foreach (Camera Cam in RenderForCamera) 
    	{
	    	foreach (Renderer rend in Screens) 
	    	{
	    		//For each camera and render screen checks if it is visible. if it is visible even once it will set the need render to true and break all the loops
	    		if(!NeedRender){
		    		NeedRender = VisibleFromCamera(rend,Cam);
	    		}
	    		else{
	    			break;
	    		}
	    	}
	    	if(NeedRender){
	    		break;
	    	}
    	}
    	if(NeedRender){
	    	LimitedCamera.Render();	
    	}
    }
    //Check out sebastian lague's portals video for this cool solution 
    bool VisibleFromCamera(Renderer Renderer, Camera BoundCamera){
    	Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(BoundCamera);
    	return GeometryUtility.TestPlanesAABB(frustumPlanes, Renderer.bounds);
    }
}
