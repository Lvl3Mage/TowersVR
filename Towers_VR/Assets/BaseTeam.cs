using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseTeam
{
	public string teamName;
	public Color teamColor;
	public BaseTeam(string _teamName, Color _teamColor){
		teamName = _teamName;
		teamColor = _teamColor;
	}
}
