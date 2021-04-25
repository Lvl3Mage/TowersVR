using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableTower : Tower
{
    [SerializeField] Transform Spawnpoint;
    [SerializeField] OptimizedRenderCamera RadarCamera;
    [SerializeField] AITowerController AI;
    [Header("Color Marker Settings")]
    [SerializeField] Renderer[] ColorMarkers;
    [SerializeField] Color DisabledColor;
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
    protected override void OnTowerDestroyed(){
        foreach(Renderer Marker in ColorMarkers){
            Marker.material.SetColor("_UnlitColor",DisabledColor);
        }
    }
    public void PlayersChanged(){
        if(_Players.Count > 0){
            RadarCamera.SetRendering(true); // enabling the render camera
            AI.active = false; // disabling the AI
            List<Camera> PlayerCameras = new List<Camera>();
            for(int i = 0; i < _Players.Count; i++){

                PlayerCameras.Add(_Players[i].GetPlayerCamera()); // getting the player camera and adding it to the list
                _Players[i].SetSpawnpoint(Spawnpoint);
            }
            RadarCamera.AssignRenderForCamera(PlayerCameras.ToArray()); // assigning the new cameras
        }
        else{
            RadarCamera.SetRendering(false); // disabling the render camera
            AI.active = true; // enabling the AI
        }
        
    }
    public void SetColor(Color color){
        if(intact){
            foreach(Renderer Marker in ColorMarkers){
                Marker.material.SetColor("_UnlitColor",color);
            }
        }
        
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
