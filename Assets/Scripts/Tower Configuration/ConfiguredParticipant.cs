using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ConfiguredParticipant
{
    public ConfiguredParticipant(WeaponConfigType weaponConfigs, ControlRoomType controlRoomConfigs, LoadingRoomType loadingRoomConfigs, RadarRoomType radarRoomConfigs){
        WeaponConfiguration = weaponConfigs;
        ControlRoomConfiguration = controlRoomConfigs;
        LoadingRoomConfiguration = loadingRoomConfigs;
        RadarRoomConfiguration = radarRoomConfigs;
    }
    public WeaponConfigType WeaponConfiguration;
    public ControlRoomType ControlRoomConfiguration;
    public LoadingRoomType LoadingRoomConfiguration;
    public RadarRoomType RadarRoomConfiguration;

}
