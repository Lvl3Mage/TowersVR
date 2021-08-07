using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarRoom : Room
{
    [SerializeField] OptimizedRenderCamera Camera;
    public OptimizedRenderCamera GetCamera(){
        return Camera;
    }
    public MeshMaterial[] GetCamRenderObjects(){
        return ParentTower.GetCamRenderObjects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
