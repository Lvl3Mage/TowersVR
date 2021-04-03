using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnMapSpawnPicker : MonoBehaviour
{
	[Header("Reference requirements")]
    [SerializeField] TeamsList TeamsList;
	[SerializeField] Transform[] SpawnPoints;
	[SerializeField] RectTransform Map;
	[SerializeField] Object ExamplePoint;
	[SerializeField] GameStarter GameStarter;
    [SerializeField] Button StartGameButton;
	[Header("Map rendering settings")]
	[SerializeField] Camera MapRendererCamera;
	[SerializeField] Material RenderMaterial_Preview;
	[SerializeField] RenderTexture RenderTexture_Preview;
	protected BaseTeam[] Teams;
	protected MapSpawnpoint[] UISpawnpoints;                         
    // Start is called before the first frame update
    void Start()
    {
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
    }
    BaseParticipant[] GenerateParticipants(){ // Can in theory generate the teams here as well
        List<BaseParticipant> CompiledParticipantsList = new List<BaseParticipant>();
        List<TeamCard> TeamCards = TeamsList.GetTeams();
        for (int i = 0; i < TeamCards.Count; i++){ // in this case i comes as the team id
            List<MapParticipant> ParticipantCards = TeamCards[i].GetParticipants();
            for (int j = 0; j < ParticipantCards.Count; j++){
                MapParticipant Participant = ParticipantCards[j];
                MapSpawnpoint MapIcon = Participant.GetConnectedSpawnPoint();
                if(MapIcon){//if the participant is connected to a spawnpoint
                    if(MapIcon.attachedParticipant == Participant){ // Check if the spawnpoint is attached to the participant (connection is complete and not still undergoing connection)

                        int SpawnpointIndex = FindSpawnpointIndex(MapIcon); // the index of the spawnpoint in all the related arrays
                        if(SpawnpointIndex == -1){
                            Debug.LogError("Given spawnpoint does not exist in this SpawnPicker");
                        }
                        Transform SpawnpointLocation = SpawnPoints[SpawnpointIndex];

                        ParticipantSettings.PlayerType playerType = Participant.GetPlayerType(); // Getting the exact participant type

                        // Creating the base participant object and adding it to the list
                        BaseParticipant CompiledParticipant = new BaseParticipant(SpawnpointLocation, playerType, i);
                        CompiledParticipantsList.Add(CompiledParticipant);
                    }
                }
                
            }
        }
        BaseParticipant[] CompiledParticipants = CompiledParticipantsList.ToArray();
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
    public void SpawnpointConnected(){

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
    public void StartGame(){
        if(CheckStartingConditions()){ // Check if the start conditions have been met
            
        }
    	
    }
    public void RevalidateStartConditions(){ // Recalculates in case the start conditions have been met
        StartGameButton.interactable = CheckStartingConditions();
    }
}
