using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseParticipant
{
	//public Transform spawnPoint; // the position where to spawn the participant
	public Vector3 spawnPosition;
	public Quaternion spawnRotation;
	public ParticipantSettings.PlayerType playerType; // The type of the player
	public string name; // name of the participant
	public ConfiguredParticipant participantConfiguration;
	public BaseParticipant(Vector3 _spawnPosition, Quaternion _spawnRotation, ParticipantSettings.PlayerType _playerType, string _name){
		spawnPosition = _spawnPosition;
		spawnRotation = _spawnRotation;
		playerType = _playerType;
		name = _name;
	}
	public BaseParticipant(Vector3 _spawnPosition, Quaternion _spawnRotation, ParticipantSettings.PlayerType _playerType, string _name, ConfiguredParticipant newParticipantConfiguration){
		spawnPosition = _spawnPosition;
		spawnRotation = _spawnRotation;
		playerType = _playerType;
		name = _name;
		participantConfiguration = newParticipantConfiguration;
	}
}
