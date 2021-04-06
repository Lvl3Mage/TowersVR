using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableTower : Tower
{
    [SerializeField] Transform Spawnpoint;
    [SerializeField] ScreenCameraOptimizer RadarCamera;
    GameObject Player;
    public void AssignPlayer(GameObject player){ // PLACEHOLDER this should really be passed an array of player base classes
    	AssignRenderForCamera(player);
    }
    void AssignRenderForCamera(GameObject player){
    	Camera[] PlayerCameras = new Camera[1]; // PLACEHOLDER this should be rewritten in a way that both players would derrive from the same class and you could simply ask it to give you the player's camera
    	PlayerCameras[0] = player.GetComponentInChildren<Camera>();
    	RadarCamera.AssignRenderForCamera(PlayerCameras);
    }
    public Transform GetTowerSpawnPoint(){
    	return Spawnpoint;
    }
}
