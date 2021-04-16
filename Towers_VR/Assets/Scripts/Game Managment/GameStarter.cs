using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [SerializeField] Transform[] SpawnPoints;
    [SerializeField] Object PlayerTower;
    [SerializeField] Object AITower;
    [SerializeField] Object PCCharacter;
    [SerializeField] Object VRCharacter;
    [SerializeField] GameManager GameManager;
    public Transform[] GetSpawnPoints(){
        return SpawnPoints;
    }
    void SpawnParticipants(BaseTeam[] Teams){
        TeamInstance[] instancedTeams = new TeamInstance[Teams.Length];

    	for(int i = 0; i < Teams.Length; i++)
    	{
            BaseTeam team = Teams[i];

            BaseParticipant[] participants = team.participants;
            Tower[] instancedTowers = new Tower[participants.Length];

            for(int j = 0; j < participants.Length; j++){
                BaseParticipant participant = participants[j];

                ParticipantSettings.PlayerType playerType = participant.playerType;
                Object Tower = null;
                Object Character = null;
                switch(playerType){ // this really makes it hard to implement new player types nut I really can't think of a way to simplify this without limiting the player types we can have
                    case ParticipantSettings.PlayerType.AI:
                    {
                        Tower = AITower;
                        break;
                    }
                    case ParticipantSettings.PlayerType.PC:
                    {
                        Tower = PlayerTower;
                        Character = PCCharacter;
                        break;
                    }
                    case ParticipantSettings.PlayerType.VR:
                    {
                        Tower = PlayerTower;
                        Character = VRCharacter;
                        break; 
                    }
                    default: break;
                }
                GameObject TowerObject = SpawnTower(participant.spawnPosition, participant.spawnRotation, Tower); // spanws the actual Tower

                // add the tower to the instanced list 
                Tower tower = TowerObject.GetComponent<Tower>();
                if(tower){
                    instancedTowers[j] = tower;
                }

                // spawns player character if needed
                if(Character){
                    PlayableTower TowerComponent = TowerObject.GetComponent<PlayableTower>();
                    Transform TowerSpawnpoint = TowerComponent.GetTowerSpawnPoint();
                    GameObject playerObject = Object.Instantiate(Character,TowerSpawnpoint.position, Quaternion.identity) as GameObject;
                    Player Player = playerObject.GetComponent<Player>();
                    TowerComponent.AssignPlayer(Player); //PLACEHOLDER this should really pass an array
                } 
                ConfigureTower(TowerObject);
            }

            instancedTeams[i] = new TeamInstance(instancedTowers, team);
    	}

        GameManager.StartManagment(instancedTeams);
    }
    public void StartGame(BaseTeam[] Teams){
        SpawnParticipants(Teams);
    }
    protected virtual void ConfigureTower(GameObject Tower){

    }
    GameObject SpawnTower(Vector3 spawnPosition, Quaternion spawnRotation, Object Tower){
    	GameObject SpawnedTower = Object.Instantiate(Tower,spawnPosition,spawnRotation) as GameObject;
    	// Configure the team

    	return SpawnedTower;
    }
}


public class TeamInstance
{
    public TeamInstance(Tower[] _towers, BaseTeam baseTeam){
        name = baseTeam.teamName;
        color = baseTeam.teamColor;
        towers = _towers;
        active = true;
    }
    public Tower[] towers;
    public string name;
    public Color color;
    public bool active;
}
