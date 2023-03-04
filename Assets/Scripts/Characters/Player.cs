using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] Movement PlayerMovement;
	[SerializeField] Camera PlayerCamera;
	public void SetSpawnpoint(Transform Spawnpoint){
		PlayerMovement.SetSpawnpoint(Spawnpoint);
	}
	public Camera GetPlayerCamera(){
		return PlayerCamera;
	}
}
