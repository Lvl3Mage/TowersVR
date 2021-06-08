using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableTower : Tower
{
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
}
