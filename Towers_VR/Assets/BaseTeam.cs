using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseTeam
{
	[SerializeField] string TeamName;
	public Color teamColor;
	public BaseTeam(Color _teamColor){
		teamColor = _teamColor;
	}
}
