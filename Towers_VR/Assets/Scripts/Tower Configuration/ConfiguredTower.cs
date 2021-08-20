using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfiguredTower
{
    public ConfiguredTower(Object cannonRoom, Object ammoRoom, Object controlRoom, Object loadingRoom, Object radarRoom){
        CannonRoom = cannonRoom;
        AmmoRoom = ammoRoom;
        ControlRoom = controlRoom;
        LoadingRoom = loadingRoom;
    }
    public Object RadarRoom;
    public Object CannonRoom;
    public Object AmmoRoom;
    public Object ControlRoom;
    public Object LoadingRoom;
}
