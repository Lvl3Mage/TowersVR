using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptimizedRenderCamera : RenderCamera
{
	[SerializeField] int Framerate;
	[SerializeField] Camera[] RenderForCamera;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RenderLoop());
    }
    IEnumerator RenderLoop(){
    	while(true){
    		if(CheckRenderReqirement()){
    			Render();
    		}
    		yield return new WaitForSeconds(1f/Framerate);
    	}
    }
    bool CheckRenderReqirement(){
    	bool NeedRender = false;
    	foreach (Camera Cam in RenderForCamera) 
    	{
	    	foreach (MeshMaterial renderTo in renderToObjects) 
	    	{
	    		//For each camera and render screen checks if it is visible. if it is visible even once it will set the need render to true and break all the loops
	    		if(!NeedRender){
	    			Renderer Renderer = renderTo.MeshRenderer;
		    		NeedRender = VisibleFromCamera(Renderer,Cam);
	    		}
	    		else{
	    			break;
	    		}
	    	}
	    	if(NeedRender){
	    		break;
	    	}
    	}
    	return NeedRender;
    }
    bool VisibleFromCamera(Renderer Renderer, Camera BoundCamera){
    	Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(BoundCamera);
    	return GeometryUtility.TestPlanesAABB(frustumPlanes, Renderer.bounds);
    }
    public void AssignRenderForCamera(Camera[] Cameras){
        RenderForCamera = Cameras;
    }
}
