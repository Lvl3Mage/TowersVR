using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenRenderer : MonoBehaviour
{
	[System.Serializable]
	public class MaterialMeshIndex{
		public MeshRenderer MR;
		public int id;
	}
	[SerializeField] RenderTexture rt_preview;
	[SerializeField] Material m_preview;
	[SerializeField] MaterialMeshIndex[] Screens;
    // Start is called before the first frame update
    void Start()
    {
    	RenderTexture rt = Instantiate(rt_preview);
    	GetComponent<Camera>().targetTexture = rt;
    	Material m = Instantiate(m_preview);
    	m.SetTexture("_UnlitColorMap", rt);
    	foreach (MaterialMeshIndex Obj in Screens) 
    	{
    		Material[] matArray = Obj.MR.materials;
 			matArray[Obj.id] = m;
 			Obj.MR.materials = matArray;
    	}
	}

}
