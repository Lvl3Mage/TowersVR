using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerConfigs
{
    public CannonRoomTypes CannonRoomType;
    public ControlRoomTypes ControlRoomType;
    public AmmoRoomTypes AmmoRoomType;
    public LoadingRoomTypes LoadingRoomType;
    public TowerConfigs(CannonRoomTypes _CannonRoomType, ControlRoomTypes _ControlRoomType, AmmoRoomTypes _AmmoRoomType, LoadingRoomTypes _LoadingRoomType){
        CannonRoomType = _CannonRoomType;
        ControlRoomType = _ControlRoomType;
        AmmoRoomType = _AmmoRoomType;
        LoadingRoomType = _LoadingRoomType;
    }
    public TowerConfigs(){

    }
}
public class ConfiguredTower
{
    public Object CannonRoom, ControlRoom, AmmoRoom, LoadingRoom;
    public ConfiguredTower(Object _CannonRoom, Object _ControlRoom, Object _AmmoRoom, Object _LoadingRoom){
        CannonRoom = _CannonRoom;
        CannonRoom = _ControlRoom;
        AmmoRoom = _AmmoRoom;
        LoadingRoom = _LoadingRoom;
    }
}
public enum CannonRoomTypes {LowCaliber, MediumCaliber, HighCaliber}; // specifies the type of the cannon room
public enum ControlRoomTypes {Basic}; // specifies the type of the control room
public enum AmmoRoomTypes {LowCaliber, MediumCaliber, HighCaliber}; // specifies the type of the ammo room
public enum LoadingRoomTypes {Basic}; // specifies the type of the ammo loading room

