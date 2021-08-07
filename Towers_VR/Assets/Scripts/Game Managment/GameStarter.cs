using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [SerializeField] Transform[] SpawnPoints;
    [SerializeField] Object Tower;
    [SerializeField] Object PCCharacter;
    [SerializeField] Object VRCharacter;
    [Header("TowerConfigs")]
    [SerializeField] Dictionary<CannonRoomTypes,Object> CannonRoomTypeObjects;
    [SerializeField] Dictionary<ControlRoomTypes,Object> ControlRoomTypeObjects;
    [SerializeField] Dictionary<AmmoRoomTypes,Object> AmmoRoomTypeObjects;
    [SerializeField] Dictionary<LoadingRoomTypes,Object> LoadingRoomTypeObjects;
    GameManager GameManager;
    void Start(){
        GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if(!GameManager){
            Debug.LogError("No GameManager found in scene");
        }
    }
    public Transform[] GetSpawnPoints(){
        return SpawnPoints;
    }
    void SpawnParticipants(BaseTeam[] Teams){
        TeamInstance[] instancedTeams = new TeamInstance[Teams.Length];
        List<PlayableTower> PlayableTowers = new List<PlayableTower>();
        List<TowerConfigs> TowerConfigs = new List<TowerConfigs>();
        List<List<Player>> GamePlayers = new List<List<Player>>(); // a 2D list of all players in all towers
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
                    TowerConfigs.Add(participant.Configs); // saving tower configs for later initialization
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
                    GamePlayers.Add(TowerPlayers); // adds the created players to the GamePlayers
                    PlayableTowers.Add(PlayableTower); // adds the playable tower to the towers array for later initialization
                }
            }
            instancedTeams[i] = new TeamInstance(instancedTowers, team);
    	}
        GameManager.StartManagment(instancedTeams);

        for(int i = 0; i < PlayableTowers.Count; i++){
            PlayableTower currentPlayableTower = PlayableTowers[i];
            currentPlayableTower.Players = GamePlayers[i]; // setting the players to the tower for initialization
            //getting the tower configs which were saved earlier
            TowerConfigs curTowerConfig = TowerConfigs[i];
            //using the data from the tower configs to find objects which are stored in dictionries
            Object CannonRoom = CannonRoomTypeObjects[curTowerConfig.CannonRoomType];
            Object ControlRoom = ControlRoomTypeObjects[curTowerConfig.ControlRoomType];
            Object AmmoRoom = AmmoRoomTypeObjects[curTowerConfig.AmmoRoomType];
            Object LoadingRoom = LoadingRoomTypeObjects[curTowerConfig.LoadingRoomType];
            currentPlayableTower.Initialize(new ConfiguredTower(CannonRoom,ControlRoom,AmmoRoom,LoadingRoom)); // initializing towers using those objects

        }
        
    }
    public void StartGame(BaseTeam[] Teams){
        SpawnParticipants(Teams);
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
