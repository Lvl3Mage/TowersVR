using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public class InstantiatedTower
    {
        public PlayableTower Tower;
        public List<Player> Players;
        public ConfiguredTower ConfiguredTower;
        //also tower configs
        public InstantiatedTower(PlayableTower tower, List<Player> players, ConfiguredTower configuredTower)
        {
            Tower = tower;
            Players = players;
            ConfiguredTower = configuredTower;
        }
    }
    [SerializeField] Object TowerObject;
    [SerializeField] Object PCCharacter;
    [SerializeField] Object VRCharacter;
    [SerializeField] TowerConfigurationSettings TowerConfigSettings;
    GameManager GameManager;
    void Start(){
        GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if(!GameManager){
            Debug.LogError("No GameManager found in scene");
        }
    }
    void SpawnParticipants(BaseTeam[] Teams){
        TeamInstance[] instancedTeams = new TeamInstance[Teams.Length]; // the teams array that will serve as the backbone for the GameManager
        List<InstantiatedTower> InstantiatedTowers = new List<InstantiatedTower>(); // the array of towers that contains all the necessary info for their initialization

        for(int i = 0; i < Teams.Length; i++){
            BaseTeam currentTeam = Teams[i];
            BaseParticipant[] currentParticipants = currentTeam.participants;

            Tower[] spawnedTowers = new Tower[currentParticipants.Length]; // an array of Towers that will be spawned
            for(int j = 0; j < currentParticipants.Length; j++){
                BaseParticipant currentParticipant = currentParticipants[j];

                PlayableTower spawnedTower = SpawnParticipant(currentParticipant, TowerObject);

                List<Player> players = SpawnPlayers(currentParticipant, spawnedTower); // spawnning the players for the tower


                spawnedTowers[j] = spawnedTower; // adding the spawned tower to the array for later use
                InstantiatedTowers.Add(new InstantiatedTower(spawnedTower, players, TowerConfigSettings.GetConfiguredTower(currentParticipant.participantConfiguration))); // the tower is now fully spawned so we save the needed data for later initialization
            }

            instancedTeams[i] = new TeamInstance(spawnedTowers, currentTeam);
        }

        GameManager.StartManagment(instancedTeams);
        foreach(InstantiatedTower InstantiatedTower in InstantiatedTowers){
            PlayableTower curTower = InstantiatedTower.Tower;
            curTower.Initialize(InstantiatedTower.ConfiguredTower);
            curTower.Players = InstantiatedTower.Players;
        }
        
        
    }
    public void StartGame(BaseTeam[] Teams){
        SpawnParticipants(Teams);
    }
    List<Player> SpawnPlayers(BaseParticipant participant, PlayableTower tower){
        List<Player> Players = new List<Player>(); // the list that will contain all the tower's players
        switch(participant.playerType){ // this really makes it hard to implement new player types but I really can't think of a way to simplify this without limiting the player types we can have
            case ParticipantSettings.PlayerType.AI:
            {
                break;
            }
            case ParticipantSettings.PlayerType.PC:
            {
                Player SpawnedPlayer = SpawnPlayer(tower.GetTowerSpawnPoint(), PCCharacter);
                Players.Add(SpawnedPlayer);
                break;
            }
            case ParticipantSettings.PlayerType.VR:
            {
                Player SpawnedPlayer = SpawnPlayer(tower.GetTowerSpawnPoint(), VRCharacter);
                Players.Add(SpawnedPlayer);
                break; 
            }
            default: break;
        }
        return Players;
    }
    Player SpawnPlayer(Transform Spawnpoint, Object Player){
        GameObject playerObject = Object.Instantiate(Player, Spawnpoint.position, Spawnpoint.rotation) as GameObject;
        return playerObject.GetComponent<Player>();
    }
    PlayableTower SpawnParticipant(BaseParticipant participant, Object tower){
    	GameObject SpawnedTower = Object.Instantiate(tower, participant.spawnPosition, participant.spawnRotation) as GameObject;
    	return SpawnedTower.GetComponent<PlayableTower>();
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