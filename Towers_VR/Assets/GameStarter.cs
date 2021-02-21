using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
	[SerializeField] BaseTeam[] Teams;
	BaseParticipant[] Participants;

    public void SetParticipants(BaseParticipant[] _Participants){
    	Participants = _Participants;
    }
    public void SpawnParticipants(){
    	for (int i = 0; i<Participants.Length; i++) 
    	{
    		BaseParticipant Participant = Participants[i];
    		GameObject Tower = SpawnTower(Participant.spawnPoint, Participant.playerType, Participant.participant, Participant.teamID);
    		ConfigureTower(Tower,i);
    	}
    }
    protected virtual void ConfigureTower(GameObject Tower, int ParticipantID){

    }
    GameObject SpawnTower(Transform spawnPoint, ParticipantSettings.PlayerType playerType, Object Tower, int TeamID){
    	GameObject SpawnedTower = Object.Instantiate(Tower,spawnPoint.position,spawnPoint.rotation) as GameObject;

    	// Configure the team

    	return SpawnedTower;
    }
}
