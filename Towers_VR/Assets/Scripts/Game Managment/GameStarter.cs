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
    public Transform[] GetSpawnPoints(){
        return SpawnPoints;
    }
    void SpawnParticipants(BaseTeam[] Teams){
    	foreach(BaseTeam team in Teams)
    	{
            BaseParticipant[] Participants = team.participants;
            foreach(BaseParticipant participant in Participants){
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
                GameObject TowerObject = SpawnTower(participant.spawnPoint, Tower);
                if(Character){
                    PlayableTower TowerComponent = TowerObject.GetComponent<PlayableTower>();
                    Transform TowerSpawnpoint = TowerComponent.GetTowerSpawnPoint();
                    GameObject playerObject = Object.Instantiate(Character,TowerSpawnpoint.position, Quaternion.identity) as GameObject;
                    Player Player = playerObject.GetComponent<Player>();
                    TowerComponent.AssignPlayer(Player); //PLACEHOLDER this should really pass an array
                } 
                ConfigureTower(TowerObject);
            }
    	}
    }
    public void StartGame(BaseTeam[] Teams){
        SpawnParticipants(Teams);
    }
    protected virtual void ConfigureTower(GameObject Tower){

    }
    GameObject SpawnTower(Transform spawnPoint, Object Tower){
    	GameObject SpawnedTower = Object.Instantiate(Tower,spawnPoint.position,spawnPoint.rotation) as GameObject;
    	// Configure the team

    	return SpawnedTower;
    }
}
