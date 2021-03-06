﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TeamCard : MonoBehaviour
{
	public ParticipantsList relatedPlayersWindow;
	List<MapParticipant> TeamParticipants = new List<MapParticipant>();
	[SerializeField] Graphic ColorIndicator;
	Color TeamColor;
    string teamName;
    // Start is called before the first frame update
    public void ChangeName(string newName){
        teamName = newName;
    }
    public void ChangeTeamColor(Color newColor){
		TeamColor = newColor;
		ColorIndicator.color = TeamColor;
		for (int i = 0; i < TeamParticipants.Count; i++) 
		{
			TeamParticipants[i].SetColor(TeamColor);
		}
    }
    public void AddPlayerToList(MapParticipant Player){
    	TeamParticipants.Add(Player);
    	Player.SetColor(TeamColor);
    }
    public void MovePlayerWindowIn(){
    	relatedPlayersWindow.MoveIn();
    }
    public void RandomizeColor(){
    	ChangeTeamColor(new Color(Random.Range(0f, 1f),Random.Range(0f, 1f),Random.Range(0f, 1f)));

    }
    public Color GetTeamColor(){
        return TeamColor;
    }
    public List<MapParticipant> GetParticipants(){
        return TeamParticipants;
    }
    public string GetTeamName(){
        return teamName;
    }
}
