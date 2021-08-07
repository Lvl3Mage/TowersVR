using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableTower : Tower
{   

    //Rooms
    CannonRoom CannonRoom;
    Transform CannonRoomPoint;
    ControlRoom ControlRoom;
    Transform ControlRoomPoint;
    LoadingRoom LoadingRoom;
    Transform LoadingRoomPoint;
    //AmmoRoom AmmoRoom;
    Transform AmmoRoomPoint;
    //
    [SerializeField] Transform Spawnpoint;
    [SerializeField] OptimizedRenderCamera RadarCamera;
    [SerializeField] RadarScreen RadarScreen;
    [SerializeField] AITowerController AI;
    private List<Player> _Players;
    public List<Player> Players{
        get { 
            return _Players; 
        }
        set { 
            _Players = value;
            PlayersChanged();

        }
    }
    protected override void OnDestroy(){
        ToggleRendering(false);
    }
    public void Initialize(ConfiguredTower ConfiguredTower){ // initializing all the references to the rooms
        CannonRoom = (Instantiate(ConfiguredTower.CannonRoom, CannonRoomPoint.position, CannonRoomPoint.rotation, CannonRoomPoint) as GameObject).GetComponent<CannonRoom>();
        ControlRoom = (Instantiate(ConfiguredTower.ControlRoom, ControlRoomPoint.position, ControlRoomPoint.rotation, ControlRoomPoint) as GameObject).GetComponent<ControlRoom>();
        LoadingRoom = (Instantiate(ConfiguredTower.LoadingRoom, LoadingRoomPoint.position, LoadingRoomPoint.rotation, LoadingRoomPoint) as GameObject).GetComponent<LoadingRoom>();
        Instantiate(ConfiguredTower.AmmoRoom, AmmoRoomPoint.position, AmmoRoomPoint.rotation, AmmoRoomPoint);
        // create rooms
        Room[] Rooms = new Room[] {CannonRoom, ControlRoom, LoadingRoom};
        foreach(Room room in Rooms){
            room.SetTower(this);
        }
        foreach(Room room in Rooms){
            InitializeRelayForRoom(room, Rooms);
        }
    }
    void InitializeRelayForRoom(Room OrigRoom, Room[] Rooms){
        Dictionary<string, DataContainer[]> RoomRefData = OrigRoom.GetReferenceData();
        foreach(KeyValuePair<string, DataContainer[]> entry in RoomRefData){ // for each reference in the room
            if(System.Array.IndexOf(entry.Value, this) != -1){ // if the room  contains reference to tower
                if(!ReferenceData.ContainsKey(entry.Key)){ // if the reference doesn't exist yet
                    ReferenceData.Add(entry.Key,FindDestRoomsFor(entry.Key, Rooms));
                }
            }
        }
    }
    DataContainer[] FindDestRoomsFor(string varName, Room[] Rooms){
        List<DataContainer> DestRooms = new List<DataContainer>();
        foreach(Room room in Rooms){
            if(!room.ContainsReferenceTo(varName,this)){
                DestRooms.Add(room);
            }
        }
        return DestRooms.ToArray();
    }
    public void PlayersChanged(){
        if(_Players.Count > 0){
            AI.active = false; // disabling the AI
            List<Camera> PlayerCameras = new List<Camera>();
            for(int i = 0; i < _Players.Count; i++){

                PlayerCameras.Add(_Players[i].GetPlayerCamera()); // getting the player camera and adding it to the list
                _Players[i].SetSpawnpoint(Spawnpoint);
            }
            RadarCamera.AssignRenderForCamera(PlayerCameras.ToArray()); // assigning the new cameras
            ToggleRendering(true); // enabling the render camera
        }
        else{
            ToggleRendering(false); // disabling the render camera
            AI.active = true; // enabling the AI
        }
        
    }
    void ToggleRendering(bool toggle){
        RadarScreen.ToggleRendering(toggle);
        RadarCamera.ToggleRendering(toggle);
    }
    public Transform GetTowerSpawnPoint(){
    	return Spawnpoint;
    }

    public TeamInstance[] GetEnemyTeams(){
        return GameManager.RequestEnemyTeams(this);
    }
    public bool GameRunning(){
        return GameManager.GameRunning();
    }
    // Room Coms
    public MeshMaterial[] GetCamRenderObjects(){
        return ControlRoom.GetCamRenderObjects();
    }
    public Weapon GetCannon(){
        return CannonRoom.GetCannon();
    }
}
