using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderCamera : DataContainer
{

    [System.Serializable]
	public class MeshMaterial{
		[Tooltip("The mesh renderer of the render-to object")]
		public MeshRenderer MeshRenderer;
		[Tooltip("The id of the desired material location in the mesh renderer materials array")]
		public int id;
	}
	protected override void ChangeValue(string varName, float value){
		FOV = value;
	}
	[SerializeField] RenderTexture RenderTexturePreview;
	[SerializeField] Material MaterialPreview;
	[SerializeField] protected MeshMaterial[] renderToObjects;
	// the render texture the camera should render to
	RenderTexture CameraRenderTexture;
	GlobalRenderCamera GlobalCam;
	float FOV;
    // Start is called before the first frame update
    void Awake()
    {
    	FOV = 60f;
    	CameraRenderTexture = Instantiate(RenderTexturePreview);
    	Material clonedMat = Instantiate(MaterialPreview); 

    	clonedMat.SetTexture("_UnlitColorMap", CameraRenderTexture);

    	foreach (MeshMaterial renderTo in renderToObjects) 
    	{
    		Material[] matArray = renderTo.MeshRenderer.materials;
 			matArray[renderTo.id] = clonedMat;
 			renderTo.MeshRenderer.materials = matArray;
    	}

    	GlobalCam = GameObject.FindGameObjectWithTag("GlobalRenderCamera").GetComponent<GlobalRenderCamera>();
	}
	protected void Render(){
		GlobalCam.RenderToTexture(transform, CameraRenderTexture, FOV);
	}
}
