using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerTypeLimit
{
	[SerializeField] string Type = "Write the corresponding type here to not get confused (by index)";
	public int MaxPlayersOfType;
	public bool ApplyLimit;

}
public class OnMapSpawnPicker : MonoBehaviour
{
	[Header("Reference requirements")]
    [SerializeField] TeamsList TeamsList;
	Transform[] SpawnPoints;
	[SerializeField] RectTransform Map;
	[SerializeField] Object ExamplePoint;
	[SerializeField] GameStarter GameStarter;
    [SerializeField] Button StartGameButton;
	[Header("Map rendering settings")]
	[SerializeField] Camera MapRendererCamera;
	[SerializeField] Material RenderMaterial_Preview;
	[SerializeField] RenderTexture RenderTexture_Preview;

	[SerializeField] PlayerTypeLimit[] PlayerTypeLimits; //Specifies how many players of type are possible
	protected BaseTeam[] Teams;
	protected MapSpawnpoint[] UISpawnpoints;
	int[] PlayerTypeQuantities;                     
    // Start is called before the first frame update
    void Start()
    {
    	SpawnPoints = GameStarter.GetSpawnPoints();
    	PlayerTypeQuantities = new int[ParticipantSettings.PlayerType.GetNames(typeof(ParticipantSettings.PlayerType)).Length];
    	RenderMapPreview();
    	PlacePoints();
        RevalidateStartConditions();
    }
    void RenderMapPreview(){ // Renders the map preview and then disables the camera to not impact performance
    	Material RenderMaterial = Object.Instantiate(RenderMaterial_Preview);
    	RenderTexture RenderTexture = Object.Instantiate(RenderTexture_Preview);
    	MapRendererCamera.targetTexture = RenderTexture;
    	Map.GetComponent<Image>().material = RenderMaterial;
    	RenderMaterial.SetTexture("_UnlitColorMap", RenderTexture);
    	MapRendererCamera.Render();
    	MapRendererCamera.enabled = false;
    }
    void PlacePoints(){ // Places all the spawnpoints in UI
    	UISpawnpoints = new MapSpawnpoint[SpawnPoints.Length];
    	for (int i = 0; i < SpawnPoints.Length; i++) 
		{
			Vector3 RelativePos = MapRendererCamera.WorldToViewportPoint(SpawnPoints[i].position);
			UISpawnpoints[i] = CreateSpawnpoint(ExamplePoint,new Vector2(RelativePos.x,RelativePos.y));
			//ConfigureButtonPress(UISpawnpoints[i],i);
		}
    }
    MapSpawnpoint CreateSpawnpoint(Object Example, Vector2 Position){
    	GameObject PointInstance = Object.Instantiate(Example, Map) as GameObject;
    	RectTransform PointTrns = PointInstance.GetComponent<RectTransform>();

    	//Setting the position in using the persentage and the map size
        PointTrns.anchoredPosition = Position*Map.rect.width;

        //Scaling the anchor to match the button (this also scales the button but we adjust for it later)
        Vector2 offset = new Vector2((PointTrns.rect.width/Map.rect.width),(PointTrns.rect.height/Map.rect.height))/2;
        PointTrns.anchorMax = new Vector2(Position.x+offset.x,Position.y+offset.y);
        PointTrns.anchorMin = new Vector2(Position.x-offset.x,Position.y-offset.y);

        // Normalizing button size after the anchor scale (it changes the actual size aswell so we have to adjust for that after the scale happens)
        PointTrns.offsetMax = Vector2.zero; 
        PointTrns.offsetMin = Vector2.zero;

        //Puts the button in front of everything else  
    	Vector3 DimPos = PointTrns.anchoredPosition3D;
    	DimPos.z = -10;
    	PointTrns.anchoredPosition3D = DimPos;
    	//Creates the spawnpoint object to return later on
    	MapSpawnpoint Point = PointInstance.GetComponent<MapSpawnpoint>();
    	Point = PointGenerated(Point); // an overridable function that can be used to modify the button before it is returned

        return Point;
    }
    void SendParameters(){
    	GameStarter.SetTeams(Teams);
    	BaseParticipant[] Participants = GenerateParticipants();
    	ModifyParticipants(Participants);
    	GameStarter.SetParticipants(Participants);
    	GameStarter.SetTeams(GenerateTeams());
    }
    List<MapParticipant> GetAllParticipants(){
    	List<MapParticipant> Participants = new List<MapParticipant>();
    	List<TeamCard> TeamCards = TeamsList.GetTeams();
        for (int i = 0; i < TeamCards.Count; i++){ // in this case i comes as the team id
            List<MapParticipant> TeamParticipants = TeamCards[i].GetParticipants();
            for (int j = 0; j < TeamParticipants.Count; j++){
                Participants.Add(TeamParticipants[j]);
            }
        }
        return Participants;
    }
    BaseTeam[] GenerateTeams(){ // Right now all the teams just get dumped into the generation because all the players are generated with total team index but this should be changed in the future
    	List<TeamCard> TeamCards = TeamsList.GetTeams();
    	BaseTeam[] CompiledTeams = new BaseTeam[TeamCards.Count];
    	for(int i = 0; i< TeamCards.Count; i++){
    		CompiledTeams[i] = new BaseTeam(TeamCards[i].GetTeamName(), TeamCards[i].GetTeamColor());
    	}
    	return CompiledTeams;
    }
    BaseParticipant[] GenerateParticipants(){ // Can in theory generate the teams here as well
        List<MapParticipant> Participants = GetAllParticipants();
        List<BaseParticipant> CompiledParticipantList = new List<BaseParticipant>();
        for (int i = 0; i <Participants.Count; i++){
			MapParticipant Participant = Participants[i];
            MapSpawnpoint MapIcon = Participant.GetConnectedSpawnPoint();
            if(MapIcon){//if the participant is connected to a spawnpoint
                if(MapIcon.attachedParticipant == Participant){ // Check if the spawnpoint is attached to the participant (connection is complete and not still undergoing connection)

                    int SpawnpointIndex = FindSpawnpointIndex(MapIcon); // the index of the spawnpoint in all the related arrays
                    if(SpawnpointIndex == -1){
                        Debug.LogError("Given spawnpoint does not exist in this SpawnPicker");
                    }
                    Transform SpawnpointLocation = SpawnPoints[SpawnpointIndex];

                    ParticipantSettings.PlayerType playerType = Participant.GetPlayerType(); // Getting the exact participant type
                    string playerName = Participant.GetPlayerName();
                    // Creating the base participant object and adding it to the list
                    BaseParticipant CompiledParticipant = new BaseParticipant(SpawnpointLocation, playerType, i, playerName);
                    CompiledParticipantList.Add(CompiledParticipant);
                }
            }
        }
        BaseParticipant[] CompiledParticipants = CompiledParticipantList.ToArray();
        return CompiledParticipants;
    }
    int FindSpawnpointIndex(MapSpawnpoint spawnpoint){
        for(int i = 0; i < UISpawnpoints.Length; i++){
            if(UISpawnpoints[i] == spawnpoint){
                return i;
            }
        }
        return -1;
    }
    //Future Overridable methods
    protected virtual MapSpawnpoint PointGenerated(MapSpawnpoint generatedButton){
    	return generatedButton;
    }
    protected virtual BaseParticipant[] ModifyParticipants(BaseParticipant[] Participants){
    	return Participants;
    }

    //Public methods to be called from elsewhere
    public MapSpawnpoint[] RequestSpawnpoints(){
        return UISpawnpoints;
    }
    protected virtual bool CheckStartingConditions(){ // override this for new starting conditions
        int Teamtypes = 0;
        List<TeamCard> Teams = TeamsList.GetTeams();
        for (int i = 0; i <Teams.Count; i++){
            TeamCard TeamCard = Teams[i];
            List<MapParticipant> Participants = TeamCard.GetParticipants();
            for (int j = 0; j < Participants.Count; j++){
                MapParticipant Participant = Participants[j];
                MapSpawnpoint MapIcon = Participant.GetConnectedSpawnPoint();
                if(MapIcon){//if the participant is connected to a spawnpoint
                    if(MapIcon.attachedParticipant == Participant){ // Check if the spawnpoint is attached to the participant (connection is complete and not still undergoing connection)
                        Teamtypes ++;
                        break; // breaks out of the participant loop
                    }
                }
            }
            if(Teamtypes >= 2){
                break; // breaks out of the main loop
            }
        }
        return Teamtypes >= 2;
    }
    public bool IsTypeAvailable(ParticipantSettings.PlayerType Type){

    	int TypeIndex = (int)Type;

    	if(PlayerTypeLimits[TypeIndex].ApplyLimit){

	    	int currentPlayersOfType = PlayerTypeQuantities[TypeIndex];

			if(currentPlayersOfType>= PlayerTypeLimits[TypeIndex].MaxPlayersOfType){ 
				return false;// if the limit has been saturated then return false
			}
			else{
				return true; // if more types can be added
			}
    	}
    	else{
    		return true; // in case the limit was marked as not to apply simply return true
    	}
    	
    }
    public void StartGame(){
        if(CheckStartingConditions()){ // Check if the start conditions have been met
            
        }
    }
    //Inform methods
    public void RevalidateStartConditions(){ // Recalculates in case the start conditions have been met
        StartGameButton.interactable = CheckStartingConditions();
    }
    public void TypeAdded(ParticipantSettings.PlayerType PlayerType){
    	PlayerTypeQuantities[(int)PlayerType]++;
    }
    public void TypeRemoved(ParticipantSettings.PlayerType PlayerType){
		PlayerTypeQuantities[(int)PlayerType]--;
    }
}
