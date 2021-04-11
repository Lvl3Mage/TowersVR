using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PresetTeam
{
	public PresetParticipant[] Participants;
	public Color TeamColor;
	public string TeamName;
}
[System.Serializable]
public class PresetParticipant
{
	public ParticipantSettings.PlayerType PlayerType;
	public Transform Spawnpoint;
	public string name;
}

public class PresetMatchConfigurator : MonoBehaviour
{
	[SerializeField] GameStarter GameStarter;
	[SerializeField] PresetTeam[] PresetTeams;
    void Start(){
    	BaseTeam[] Teams = new BaseTeam[PresetTeams.Length];
    	int i = 0;
    	foreach(PresetTeam PresetTeam in PresetTeams){
    		int j = 0;
    		PresetParticipant[] TeamParticipants = PresetTeam.Participants;
    		BaseParticipant[] Participants = new BaseParticipant[TeamParticipants.Length];
    		foreach(PresetParticipant TeamParticipant in TeamParticipants){
    			Participants[j] = new BaseParticipant(TeamParticipant.Spawnpoint,TeamParticipant.PlayerType,TeamParticipant.name);
    			j++;
    		}
    		Teams[i] = new BaseTeam(PresetTeam.TeamName,PresetTeam.TeamColor);
    		Teams[i].SetParticipants(Participants);
    		i++;

    	}
    	GameStarter.StartGame(Teams);
    	//StartCoroutine(Wait());
    }
    /*IEnumerator Wait(){
    	//yield return new WaitForSeconds(0.1f);
    }*/
}
