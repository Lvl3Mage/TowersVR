using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptimizedRenderCamera : RenderCamera
{
	[SerializeField] int Framerate;
	[SerializeField] Camera[] RenderForCameras;
    bool Rendering = false;
    Coroutine RenderCycle = null;
    // Start is called before the first frame update
    void Start()
    {
    }
    IEnumerator RenderLoop(){
    	while(Rendering){
            if(CheckRenderReqirement()){
                Render();
            }
    		
    		yield return new WaitForSeconds(1f/Framerate);
    	}
    }
    bool CheckRenderReqirement(){
    	bool NeedRender = false;
    	foreach (Camera Cam in RenderForCameras) 
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
        RenderForCameras = Cameras;
    }
    public void ToggleRendering(bool value){
        Rendering = value;
        if(value){ // if the rendering has started
            Debug.Log("Render true");
            if(RenderCycle == null){ // if the render loop has not been started
                Initialize();
                Debug.Log("Start cycle");
                RenderCycle = StartCoroutine(RenderLoop()); // start a render loop
            }
        }
    }
}
