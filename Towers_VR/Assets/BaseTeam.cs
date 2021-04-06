using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseTeam
{
	public string teamName;
	public Color teamColor;
	private BaseParticipant[] _participants;
	public BaseParticipant[] participants{
		get { 
			return _participants;
		}
		private set { 
			_participants = value;
		}
	}
	public BaseTeam(string _teamName, Color _teamColor){
		teamName = _teamName;
		teamColor = _teamColor;
	}
	public void SetParticipants(BaseParticipant[] newParticipants){
		participants = newParticipants;
	}
}
