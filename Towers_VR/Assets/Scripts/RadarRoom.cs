using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarRoom : Room
{
    [SerializeField] OptimizedRenderCamera Camera;
    [SerializeField] Transform RotationIndicator;
    public OptimizedRenderCamera GetRenderCamera(){
        return Camera;
    }
    public MeshMaterial[] GetRenderObjects(){
        return towerRelay.GetRenderObjects();
    }
    public Transform GetRotationIndicator(){
        return RotationIndicator;
    }
}
