using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableTower : Tower
{
    [SerializeField] Transform ControlRoomSpawnpoint;
    [SerializeField] Transform AmmoRoomSpawnpoint;
    [SerializeField] Transform CannonRoomSpawnpoint;
    [SerializeField] Transform RadarRoomSpawnpoint;
    [SerializeField] Transform LoadingRoomSpawnpoint;
    [SerializeField] Transform RoomParent;

    [SerializeField] Transform Spawnpoint;
    OptimizedRenderCamera RadarCamera;
    RadarScreen RadarScreen;
    [SerializeField] AITowerController AI;
    [SerializeField] TowerRelay TowerRelay;
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
    public void Initialize(List<Player> players, ConfiguredTower configuredTower){
        //Spawning all the rooms
        RadarRoom RadarRoom = (Instantiate(configuredTower.RadarRoom, RadarRoomSpawnpoint.position, RadarRoomSpawnpoint.rotation, RoomParent) as GameObject).GetComponent<RadarRoom>();
        CannonRoom CannonRoom = (Instantiate(configuredTower.CannonRoom, CannonRoomSpawnpoint.position, CannonRoomSpawnpoint.rotation, RoomParent) as GameObject).GetComponent<CannonRoom>();
        ControlRoom ControlRoom = (Instantiate(configuredTower.ControlRoom, ControlRoomSpawnpoint.position, ControlRoomSpawnpoint.rotation, RoomParent) as GameObject).GetComponent<ControlRoom>();
        ReloaderRoom LoadingRoom = (Instantiate(configuredTower.RadarRoom, LoadingRoomSpawnpoint.position, LoadingRoomSpawnpoint.rotation, RoomParent) as GameObject).GetComponent<ReloaderRoom>();
        Instantiate(configuredTower.AmmoRoom, AmmoRoomSpawnpoint.position, AmmoRoomSpawnpoint.rotation, RoomParent);

        TowerRelay.Initialize(RadarRoom, ControlRoom, CannonRoom, LoadingRoom);
        InitializeKeypoints();
        //perform configs first
        Players = players;
    }
    void PlayersChanged(){
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
        if(RadarScreen == null){
            RadarScreen = TowerRelay.GetRadar();
        }
        RadarScreen.ToggleRendering(toggle);
        if(RadarCamera == null){
            RadarCamera = TowerRelay.GetRenderCamera();
        }
        RadarCamera.ToggleRendering(toggle);
    }
    public Transform GetTowerSpawnPoint(){
    	return Spawnpoint;
    }

    public TeamInstance[] GetEnemyTeams(){ // gets all the enemy teams of this tower
        return GameManager.RequestEnemyTeams(this);
    }
    public bool GameRunning(){
        return GameManager.GameRunning();
    }
}
