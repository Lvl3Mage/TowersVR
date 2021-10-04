using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRelay : DataContainer
{
    [SerializeField] PlayableTower SelfTower;
    RadarRoom RadarRoom;
    ControlRoom ControlRoom;
    CannonRoom CannonRoom;
    ReloaderRoom ReloaderRoom;
    AmmoRoom AmmoRoom;
    Dictionary<DataType,DataContainer[]> RelayData;
    public void Initialize(RadarRoom radarRoom, ControlRoom controlRoom, CannonRoom cannonRoom, ReloaderRoom reloaderRoom, AmmoRoom ammoRoom){
        Room[] Rooms = new Room[] {radarRoom, controlRoom, cannonRoom, reloaderRoom, ammoRoom};

        Dictionary<DataType,List<DataContainer>> ModifiableRelayData = new Dictionary<DataType,List<DataContainer>>();

        foreach(Room room in Rooms){
            room.SetTowerRelay(this);
            HashSet<DataType> roomRelayInputs = room.GetRelayInputs(); // gets all the inputs for this room

            foreach(DataType input in roomRelayInputs) // for every input in this room
            {
                if(ModifiableRelayData.ContainsKey(input)){ // if the entry for this input type already exists
                    ModifiableRelayData[input].Add(room); // add the room
                }
                else{
                    ModifiableRelayData.Add(input, new List<DataContainer>(){room}); // create a new entry with the room in it
                }
            }
        }
        RelayData = new Dictionary<DataType,DataContainer[]>(); //initializes the relay data dict
        foreach(KeyValuePair<DataType,List<DataContainer>> entry in ModifiableRelayData){ // passes all the modifiable data to it
            RelayData.Add(entry.Key,entry.Value.ToArray());
        }
        RadarRoom = radarRoom;
        ControlRoom = controlRoom;
        CannonRoom = cannonRoom;
        ReloaderRoom = reloaderRoom;
        AmmoRoom = ammoRoom;

    }
    public override void SetValue<T>(DataType dataType, T value){
        //Debug.Log("Input " + dataType + ", value " + value);
        if(RelayData.ContainsKey(dataType)){
            DataContainer[] DestContiners = RelayData[dataType];
            foreach(DataContainer container in DestContiners){
                container.SetValue(dataType, value);
            }
            
        }
        else{
            Debug.LogError("No type found for input " + dataType);
        }
    }
    public WeaponReloader GetReloader(){
        return ReloaderRoom.GetReloader();
    }
    public Weapon GetCannon(){
        return CannonRoom.GetCannon();
    }
    public MeshMaterial[] GetRenderObjects(){
        return ControlRoom.GetRenderObjects();
    }
    public OptimizedRenderCamera GetRenderCamera(){
        return RadarRoom.GetRenderCamera();
    }
    public RadarScreen GetRadar(){
        return ControlRoom.GetRadar();
    }
    public Transform GetRotationIndicator(){
        return RadarRoom.GetRotationIndicator();
    }
    public PlayableTower GetSelfTower(){
        return SelfTower;
    }
    public Transform GetGunpoint(){
        return CannonRoom.GetGunpoint();
    }
    public AmmoRoom.FiringGroup GetAmmo(){
        return AmmoRoom.GetAmmo();
    }
}
