using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	enum WinConditions
	{
		LastAlive,
	    MostKills__Not_Implemented__
	}
	[Tooltip("Defines how many seconds will pass before all the towers are revised in terms of being intact")]
	[SerializeField] [Range(0.2f, 5f)] float IntactCheckRate;
	[SerializeField] WinConditions WinCondition;
	TeamInstance[] teams;
    public void StartManagment(TeamInstance[] instancedTeams){
    	teams = instancedTeams;
    	StartCoroutine(TeamIntactChecker());
    }
    public TeamInstance[] RequestEnemyTeams(Tower requesterTower){
    	List<TeamInstance> enemyTeams = new List<TeamInstance>();
    	TeamInstance requesterTeam = FindTowersTeam(requesterTower);
    	foreach(TeamInstance team in teams){
    		if(team != requesterTeam){
				enemyTeams.Add(team);
    		}
    	}
    	return enemyTeams.ToArray();
    }
    TeamInstance FindTowersTeam(Tower searchTower){
    	TeamInstance towersTeam = null;
    	foreach(TeamInstance team in teams){
    		foreach(Tower tower in team.towers){
    			if(tower == searchTower){
    				towersTeam = team;
    				break;
    			}
    		}
    		if(towersTeam != null){ // if the towersTeam has been found break
    			break;
    		}
    	}
    	if(towersTeam == null){
    		Debug.LogError("requested tower does not belongto any team");
    		return null;
    	}
    	else{
    		return towersTeam;
    	}
    }
    void WinConditionReached(){
    	List<TeamInstance> aliveTeams = new List<TeamInstance>();
    	for(int i = 0; i < teams.Length; i++){
    		if(teams[i].active){
    			aliveTeams.Add(teams[i]);
    		}
    	}
    	if(aliveTeams.Count == 0){
    		Debug.LogError("EVERY TEAM IS dEd");
    	}
    	else{
    		for (int i = 0; i < aliveTeams.Count; i++){
    			Debug.Log("Team " + aliveTeams[i].name + " won!");
    		}
    	}
    }
    IEnumerator TeamIntactChecker(){
    	bool WinReached = false;
    	while(!WinReached){
    		yield return new WaitForSeconds(IntactCheckRate); // lets all towers initialize correctly

    		foreach(TeamInstance team in teams){
    			RecalculateTeam(team);

    		}
    		WinReached = CheckWinConditions(teams);
    	}
    	WinConditionReached();
    }
    void RecalculateTeam(TeamInstance team){
		Tower[] towers = team.towers;

		bool TeamActive = false; // determines whether the team is active

		foreach(Tower tower in towers){
			tower.RecalculateIntegrity();

			if(tower.TowerIntact()){ // if even a single player is intact then the team is active
				TeamActive = true;
			}
		}
		team.active = TeamActive;
    }
    bool CheckWinConditions(TeamInstance[] CheckTeams){
    	switch(WinCondition)
    	{
    		case WinConditions.LastAlive:

    			int activeteams = 0; // number of active teams
    			foreach(TeamInstance team in CheckTeams){
    				if(team.active){
    					
    					activeteams ++;
    					if(activeteams > 1){ // if there is more than one active team then there is no need to check
    						break;
    					}
    				}
    			}
    			return activeteams < 2; // returns true if there is one or less active teams

    		default:
    			return false;
    			break;
    	}
    }
}