using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableTower : Tower
{
    [SerializeField] Transform Spawnpoint;
    [SerializeField] OptimizedRenderCamera RadarCamera;
    Player Player;
    public void AssignPlayer(Player player){ // PLACEHOLDER this should reall recieve an array of players
    	AssignRenderForCamera(player);
    	player.SetSpawnpoint(Spawnpoint);
    	Player = player;
    }
    void AssignRenderForCamera(Player player){
    	Camera[] PlayerCameras = new Camera[1]; 
    	PlayerCameras[0] = player.GetPlayerCamera();// PLACEHOLDER this should really recieve an array of players and iterate through them
    	RadarCamera.AssignRenderForCamera(PlayerCameras);
    }
    public Transform GetTowerSpawnPoint(){
    	return Spawnpoint;
    }
}
