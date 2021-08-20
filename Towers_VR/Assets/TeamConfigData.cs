using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TeamConfigData // Savedata
{
	public TeamConfig[] Teams;

	public TeamConfigData(BaseTeam[] baseTeams){ // this will trigger a cascade of constructors creating the entire team structure
		Teams = new TeamConfig[baseTeams.Length];
		for(int i = 0; i < baseTeams.Length; i++){
			Teams[i] = new TeamConfig(baseTeams[i]);
		}
	}
	public BaseTeam[] ExtractBaseTeams(){
		BaseTeam[] baseTeams = new BaseTeam[Teams.Length];
		for(int i = 0; i < Teams.Length; i++){
			//Setting up the team
			TeamConfig configTeam = Teams[i];

			float[] configTeamColor = configTeam.teamColor;
			Color teamColor = new Color(configTeamColor[0],configTeamColor[1],configTeamColor[2],configTeamColor[3]);

			BaseTeam baseTeam = new BaseTeam(configTeam.teamName, teamColor);

			//Setting up the players
			PlayerConfig[] configPlayers = configTeam.players;
			BaseParticipant[] baseParticipants = new BaseParticipant[configPlayers.Length];

			for(int j = 0; j < configPlayers.Length; j++){
				PlayerConfig configPlayer = configPlayers[j];

				ParticipantSettings.PlayerType playerType = (ParticipantSettings.PlayerType)configPlayer.playerType; // casting the saved int to the player type enum
				
				// Creating a vector3 for the player
				float[] configPlayerPosition = configPlayer.PlayerPosition;
				Vector3 playerPosition = new Vector3(configPlayerPosition[0],configPlayerPosition[1],configPlayerPosition[2]);

				//Creating a quaternion for the player
				float[] configPlayerRotation = configPlayer.PlayerRotation;
				Quaternion playerRotation= new Quaternion(configPlayerRotation[0],configPlayerRotation[1],configPlayerRotation[2],configPlayerRotation[3]);

				string playerName = configPlayer.playerName;


				//creates the actual base participant
				BaseParticipant baseParticipant = new BaseParticipant(playerPosition, playerRotation, playerType, playerName);

				//assigns the created base participant to the base participant array
				baseParticipants[j] = baseParticipant;

			}
			//assigns the created base participants to the team
			baseTeam.SetParticipants(baseParticipants);

			//assings the finished team to the array of teams
			baseTeams[i] = baseTeam;
		}
		return baseTeams;
	}
}
[System.Serializable]
public class TeamConfig
{
	public float[] teamColor;
	public string teamName;
	public PlayerConfig[] players;

	public TeamConfig(BaseTeam baseTeam){
		//Setting up the base team
		Color baseTeamColor = baseTeam.teamColor;

		teamColor = new float[4]{
			baseTeamColor.r,
			baseTeamColor.g,
			baseTeamColor.b,
			baseTeamColor.a
		};

		teamName = baseTeam.teamName;
 		

 		//Setting up the participants
		BaseParticipant[] baseParticipants = baseTeam.participants;
		players = new PlayerConfig[baseParticipants.Length];
		for(int i = 0; i < baseParticipants.Length; i++){
			players[i] = new PlayerConfig(baseParticipants[i]);
		}
	}
}
[System.Serializable]
public class PlayerConfig
{
	public float[] PlayerPosition;
	public float[] PlayerRotation;
	public int playerType;
	public string playerName;

	public PlayerConfig(BaseParticipant baseParticipant){
		Vector3 baseParticipantPosition = baseParticipant.spawnPosition;
		PlayerPosition = new float[3]{
			baseParticipantPosition.x,
			baseParticipantPosition.y,
			baseParticipantPosition.z
		};

		Quaternion baseParticipantRotation = baseParticipant.spawnRotation;
		PlayerRotation = new float[4]{
			baseParticipantRotation.x,
			baseParticipantRotation.y,
			baseParticipantRotation.z,
			baseParticipantRotation.w
		};
		playerType = (int)baseParticipant.playerType;

		playerName = baseParticipant.name;
	}
}
