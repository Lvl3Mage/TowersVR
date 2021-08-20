﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlRoom : Room
{
    [SerializeField] RadarScreen Radar;
    [SerializeField] MeshMaterial[] RenderObjects;
    public MeshMaterial[] GetRenderObjects(){
        return RenderObjects;
    }
    public RadarScreen GetRadar(){
        return Radar;
    }
    public Transform GetRotationIndicator(){
        return towerRelay.GetRotationIndicator();
    }
}
