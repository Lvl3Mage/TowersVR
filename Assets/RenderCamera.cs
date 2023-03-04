using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderCamera : FloatContainer
{

    
	[SerializeField] RenderTexture RenderTexturePreview;
	[SerializeField] Material MaterialPreview;
	[SerializeField] RadarRoom RadarRoom;
	protected MeshMaterial[] renderToObjects;
	// the render texture the camera should render to
	RenderTexture CameraRenderTexture;
	GlobalRenderCamera GlobalCam;
	float FOV;
    // Start is called before the first frame update
    void Start()
    {
    	
	}
	protected void Initialize(){
		if(CameraRenderTexture == null){ //if haven't initialized yet
			FOV = 60f;	
			CameraRenderTexture = Instantiate(RenderTexturePreview);
			Material clonedMat = Instantiate(MaterialPreview); 
			clonedMat.SetTexture("_UnlitColorMap", CameraRenderTexture);
			renderToObjects = RadarRoom.GetRenderObjects();
			foreach (MeshMaterial renderTo in renderToObjects) 
	    	{
	    		Material[] matArray = renderTo.MeshRenderer.materials;
	 			matArray[renderTo.id] = clonedMat;
	 			renderTo.MeshRenderer.materials = matArray;
	    	}
		}
		
		if(!GlobalCam){
			GlobalCam = GameObject.FindGameObjectWithTag("GlobalRenderCamera").GetComponent<GlobalRenderCamera>();
		}
	}
	protected void Render(){
		GlobalCam.RenderToTexture(transform, CameraRenderTexture, FOV);
	}
	protected override void SetFloat(DataType dataType, float value){
		FOV = value;
	}
}
[System.Serializable]
public class MeshMaterial{
	[Tooltip("The mesh renderer of the render-to object")]
	public MeshRenderer MeshRenderer;
	[Tooltip("The id of the desired material location in the mesh renderer materials array")]
	public int id;
}