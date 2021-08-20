using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderCamera : MonoBehaviour
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
    void Awake()
    {
    	renderToObjects = RadarRoom.GetRenderObjects();
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
	public void SetFOV(float fov){
		FOV = fov; 
	}
}
[System.Serializable]
public class MeshMaterial{
	[Tooltip("The mesh renderer of the render-to object")]
	public MeshRenderer MeshRenderer;
	[Tooltip("The id of the desired material location in the mesh renderer materials array")]
	public int id;
}
