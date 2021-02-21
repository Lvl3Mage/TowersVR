using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseParticipant
{
	public Transform spawnPoint; // the position where to spawn the participant
	public ParticipantSettings.PlayerType playerType; // The type of the player
	public Object participant; // the object pf the participant
	public int teamID; // team id of the participant
	public BaseParticipant(){

	}
}
