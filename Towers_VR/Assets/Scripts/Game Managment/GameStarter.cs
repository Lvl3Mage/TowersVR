using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [SerializeField] Transform[] SpawnPoints;
    [SerializeField] Object Tower;
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

                GameObject TowerObject = SpawnTower(participant.spawnPosition, participant.spawnRotation, Tower); // spawns the actual Tower
                PlayableTower PlayableTower = TowerObject.GetComponent<PlayableTower>();
                if(PlayableTower){
                    instancedTowers[j] = PlayableTower;// add the PlayableTower to the instanced list

                    ParticipantSettings.PlayerType playerType = participant.playerType;
                    List<Player> TowerPlayers = new List<Player>(); // the list that will contain all the tower's players
                    switch(playerType){ // this really makes it hard to implement new player types nut I really can't think of a way to simplify this without limiting the player types we can have
                        case ParticipantSettings.PlayerType.AI:
                        {
                            break;
                        }
                        case ParticipantSettings.PlayerType.PC:
                        {
                            Player SpawnedPlayer = SpawnPlayer(PlayableTower.GetTowerSpawnPoint().position, PCCharacter);
                            TowerPlayers.Add(SpawnedPlayer);
                            break;
                        }
                        case ParticipantSettings.PlayerType.VR:
                        {
                            Player SpawnedPlayer = SpawnPlayer(PlayableTower.GetTowerSpawnPoint().position, VRCharacter);
                            TowerPlayers.Add(SpawnedPlayer);
                            break; 
                        }
                        default: break;
                    }
                    PlayableTower.Players = TowerPlayers; // if it's a playable tower we will set the players
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
    Player SpawnPlayer(Vector3 Position, Object Player){
        GameObject playerObject = Object.Instantiate(Player, Position, Quaternion.identity) as GameObject;
        return playerObject.GetComponent<Player>();
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
