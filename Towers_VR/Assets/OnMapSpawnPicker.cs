using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnMapSpawnPicker : MonoBehaviour
{
	[Header("Reference requirements")]
	[SerializeField] Transform[] SpawnPoints;
	[SerializeField] RectTransform Map;
	[SerializeField] Object ExamplePoint;
	[SerializeField] GameStarter GameStarter;
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
    }
    void RenderMapPreview(){ // Renders the map preview and then disables the camera to not impact performance
    	Material RenderMaterial = Object.Instantiate(RenderMaterial_Preview);
    	RenderTexture RenderTexture = Object.Instantiate(RenderTexture_Preview);
    	MapRendererCamera.targetTexture = RenderTexture;
    	Map.GetComponent<Image>().material = RenderMaterial;
    	RenderMaterial.SetTexture("_UnlitColorMap", RenderTexture);
    	//MapRendererCamera.Render();
    	//MapRendererCamera.enabled = false;
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
    /*void ConfigureButtonPress(MapSpawnpoint Button, int ButtonID){
    	Button.button.onClick.AddListener(
    		delegate{
    			SpawnPointButtonPressed(ButtonID);
    			}
    		);
    }*/
    void SpawnPointButtonPressed(int ButtonID){
    	IconPressed(ButtonID);
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

    	// Recalculates the color for the button using the teams array
    	Point.RecalculateColor(Teams);
        return Point;
    }
    void SendParameters(){
    	GameStarter.SetTeams(Teams);
    	BaseParticipant[] Participants = GenerateParticipants();
    	ModifyParticipants(Participants);
    	GameStarter.SetParticipants(Participants);
    }
    BaseParticipant[] GenerateParticipants(){
    	return null;
    }
    //Future Overridable methods
    protected virtual MapSpawnpoint PointGenerated(MapSpawnpoint generatedButton){
    	return generatedButton;
    }

    protected virtual void IconPressed(int ButtonID){

    }
    protected virtual BaseParticipant[] ModifyParticipants(BaseParticipant[] Participants){
    	return Participants;
    }

    //Public methods to be called from elsewhere
    public void StartGame(){
    	// Check if the start conditions have been met
    }
}
