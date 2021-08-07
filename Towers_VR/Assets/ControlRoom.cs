using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlRoom : Room
{
    [SerializeField] MeshMaterial[] RenderScreens;
    public MeshMaterial[] GetCamRenderObjects(){
        return RenderScreens;
    }
}
