using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Configuration Settings", menuName = "TowerConfiguration/ConfigurationSettings")]
public class TowerConfigurationSettings : ScriptableObject
{
    [System.Serializable]
    public class WeaponConfiguration
    {
        public WeaponConfigType Type; 
        public Object CannonRoom;
        public Object AmmoRoom;
    }
    [System.Serializable]
    public class ControlRoomConfiguration
    {
        public ControlRoomType Type;
        public Object ControlRoom;
    }
    [System.Serializable]
    public class LoadingRoomConfiguration
    {
        public LoadingRoomType Type;
        public Object LoadingRoom;
    }
    [System.Serializable]
    public class RadarRoomConfiguration
    {
        public RadarRoomType Type;
        public Object RadarRoom;
    }

    [SerializeField] WeaponConfiguration[] WeaponConfigs;
    [SerializeField] ControlRoomConfiguration[] ControlRoomConfigs;
    [SerializeField] LoadingRoomConfiguration[] LoadingRoomConfigs;
    [SerializeField] RadarRoomConfiguration[] RadarRoomConfigs;


    public ConfiguredTower GetConfiguredTower(ConfiguredParticipant configuredParticipant){
        WeaponConfiguration weaponConfig = FindWeaponConfiguration(configuredParticipant.WeaponConfiguration);
        ControlRoomConfiguration controlRoomConfig = FindControlRoomConfiguration(configuredParticipant.ControlRoomConfiguration);
        LoadingRoomConfiguration loadingRoomConfig = FindLoadingRoomConfiguration(configuredParticipant.LoadingRoomConfiguration);
        RadarRoomConfiguration radarRoomConfig = FindRadarRoomConfiguration(configuredParticipant.RadarRoomConfiguration);
        return new ConfiguredTower(weaponConfig.CannonRoom, weaponConfig.AmmoRoom, controlRoomConfig.ControlRoom, loadingRoomConfig.LoadingRoom, radarRoomConfig.RadarRoom);
    }


    WeaponConfiguration FindWeaponConfiguration(WeaponConfigType type){
        foreach(WeaponConfiguration weaponConfig in WeaponConfigs){
            if(weaponConfig.Type == type){
                return weaponConfig;
            }
        }
        // if nothing found
        Debug.LogError("No configuration found for weapon of type " + type);
        return null;
    }
    ControlRoomConfiguration FindControlRoomConfiguration(ControlRoomType type){
        foreach(ControlRoomConfiguration controlRoomConfig in ControlRoomConfigs){
            if(controlRoomConfig.Type == type){
                return controlRoomConfig;
            }
        }
        // if nothing found
        Debug.LogError("No configuration found for control room of type " + type);
        return null;
    }
    LoadingRoomConfiguration FindLoadingRoomConfiguration(LoadingRoomType type){
        foreach(LoadingRoomConfiguration loadingRoomConfig in LoadingRoomConfigs){
            if(loadingRoomConfig.Type == type){
                return loadingRoomConfig;
            }
        }
        // if nothing found
        Debug.LogError("No configuration found for loading room of type " + type);
        return null;
    }
    RadarRoomConfiguration FindRadarRoomConfiguration(RadarRoomType type){
        foreach(RadarRoomConfiguration radarRoomConfig in RadarRoomConfigs){
            if(radarRoomConfig.Type == type){
                return radarRoomConfig;
            }
        }
        // if nothing found
        Debug.LogError("No configuration found for radar room of type " + type);
        return null;
    }
}

public enum WeaponConfigType
{
    flak,
    medium,
    heavy,
    extreme,
}
public enum ControlRoomType
{
    defaultRoom
}
public enum LoadingRoomType
{
    defaultRoom
}
public enum RadarRoomType
{
    defaultRoom
}
