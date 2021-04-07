using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseParticipant
{
	public Transform spawnPoint; // the position where to spawn the participant
	public ParticipantSettings.PlayerType playerType; // The type of the player
	public string name; // name of the participant
	public BaseParticipant(Transform _spawnPoint, ParticipantSettings.PlayerType _playerType, string _name){
		spawnPoint = _spawnPoint;
		playerType = _playerType;
		name = _name;
	}
}
