using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PresetTeamsOnMapPicker : OnMapSpawnPicker
{
	[Header("Default team settings")]
	[SerializeField] BaseTeam[] DefaultTeams;
	[SerializeField] int DefaultTeam;
    // Start is called before the first frame update
    void Awake(){
    	Teams = DefaultTeams;
    }
	protected override MapSpawnpoint PointGenerated(MapSpawnpoint Button){
		return Button;
	}
}
