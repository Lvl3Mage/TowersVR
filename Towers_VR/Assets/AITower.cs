using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITower : Tower
{
    public TeamInstance[] GetEnemyTeams(){
		return GameManager.RequestEnemyTeams(this);
	}
	public bool GameRunning(){
		return GameManager.GameRunning();
	}
}
