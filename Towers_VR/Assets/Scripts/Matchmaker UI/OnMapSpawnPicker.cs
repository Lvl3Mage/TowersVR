using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField] Button[] ValidatedButtons;
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
    void InitiateGameStarter(){
    	BaseTeam[] Teams = CompileTeams();
    	Teams = ModifyResults(Teams);
    	GameStarter.StartGame(Teams);
        HidePicker();
    }
    void HidePicker(){
        gameObject.SetActive(false);
    }
    BaseTeam[] CompileTeams(){ // Compiles all the necessary teams and the participants in them
        List<TeamCard> TeamCards = TeamsList.GetTeams();
        List<BaseTeam> Teams = new List<BaseTeam>();
        for (int i = 0; i < TeamCards.Count; i++){ // will run for each team created
            TeamCard teamCard = TeamCards[i];
            BaseTeam Team = new BaseTeam(teamCard.GetTeamName(),teamCard.GetTeamColor());
            List<MapParticipant> Participants = teamCard.GetParticipants();
            List<BaseParticipant> relevantParticipants =  new List<BaseParticipant>();
            for (int j = 0; j < Participants.Count; j++){ // will run for each participant in every team
                MapParticipant curParticipant = Participants[j];
                MapSpawnpoint MapIcon = curParticipant.GetConnectedSpawnPoint();
                if(MapIcon){ // checks if the participant has been attached to a spawnpoint
                    if(MapIcon.attachedParticipant == curParticipant){ // double checking that the spawnpoint is attached to the participant
                        int SpawnpointIndex = FindSpawnpointIndex(MapIcon); // the index of the spawnpoint in all the related arrays
                        if(SpawnpointIndex == -1){
                            Debug.LogError("Given spawnpoint does not exist in this SpawnPicker");
                        }
                        Transform SpawnpointLocation = SpawnPoints[SpawnpointIndex];

                        ParticipantSettings.PlayerType playerType = curParticipant.GetPlayerType(); // Getting the exact participant type
                        string playerName = curParticipant.GetPlayerName();
                        // Creating the base participant object and adding it to the list
                        BaseParticipant CompiledParticipant = new BaseParticipant(SpawnpointLocation.position, SpawnpointLocation.rotation, playerType, playerName);
                        relevantParticipants.Add(CompiledParticipant); // adds the participant to the relevant participants list
                    }
                }
            }
            if(relevantParticipants.Count > 0){ // in case any of the team participants were attached to spawnpoints it adds the team to the teams list
                Team.SetParticipants(relevantParticipants.ToArray());
                Teams.Add(Team);
            }
        }
        return Teams.ToArray();

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
    protected virtual BaseTeam[] ModifyResults(BaseTeam[] Teams){
    	return Teams;
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
            InitiateGameStarter();
        }
    }
    public void SaveConfiguration(){
        if(CheckStartingConditions()){ // Check if the start conditions have been met
            BaseTeam[] CompiledTeams = CompileTeams();
            string savefileName = GetSceneName();
            SaveSystem.SaveTeamConfigs(CompiledTeams, savefileName);
        }
    }
    public void LoadConfiguration(){
        string savefileName = GetSceneName();
        BaseTeam[] LoadedTeams = SaveSystem.LoadTeamConfigs(savefileName);
        if(LoadedTeams != null){
            GameStarter.StartGame(LoadedTeams);
        }
    }
    string GetSceneName(){ // used for save file naming
        return SceneManager.GetActiveScene().name;
    }
    //Inform methods
    public void RevalidateStartConditions(){ // Recalculates in case the start conditions have been met
        bool Validated = CheckStartingConditions();
        foreach(Button ValidatedButton in ValidatedButtons){
            ValidatedButton.interactable = Validated;
        }
    }
    public void TypeAdded(ParticipantSettings.PlayerType PlayerType){
    	PlayerTypeQuantities[(int)PlayerType]++;
    }
    public void TypeRemoved(ParticipantSettings.PlayerType PlayerType){
		PlayerTypeQuantities[(int)PlayerType]--;
    }

}
