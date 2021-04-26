using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarScreen : MonoBehaviour
{
	[SerializeField] [Range(0,2)] float updatePeriod;
	[SerializeField] Object TowerMarker;
	[SerializeField] Object ViewConeObject;
	[SerializeField] Color DestroyedColor;
	[SerializeField] Transform TowerRotationIndicator;

	[Header("Marker position settings")]
	[SerializeField] Vector2 GlobalRangeX;
	[SerializeField] Vector2 GlobalRangeY;
	[SerializeField] Vector2 LocalRangeX;
	[SerializeField] Vector2 LocalRangeY;
	[SerializeField] Vector2 LocalRadarScale;
	[SerializeField] float MarkerDepthOffset;
	[SerializeField] float ViewConeDepthOffset;

	bool Rendering = false;
	Coroutine RadarCycle = null;
	List<SpriteRenderer> Markers = new List<SpriteRenderer>();
	Transform ViewCone;
	GameManager GameManager;
	void Start(){
		GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}
	public void ToggleRendering(bool value){
		Rendering = value;
		if(value){ // if the rendering has started
			if(RadarCycle == null){ // if the radar cycle has not been started
				RadarCycle = StartCoroutine(RadarUpdateCycle()); // start a radar cycle
			}
		}
	}
	IEnumerator RadarUpdateCycle(){
		SetUpViewCone();
		while (Rendering){ // will exit when rendering stops
			yield return new WaitForSeconds(updatePeriod);
			SetUpMarkers();
			RecalculateViewCone();
		}
	}
	void SetUpMarkers(){
		TeamInstance[] Teams = GameManager.RequestTeams();
		int TowerID = 0; // the index that will serve for marker identification (it is incremented for each tower)
		for (int i = 0; i < Teams.Length; i++){
			Tower[] Towers = Teams[i].towers;
			for (int j = 0; j < Towers.Length; j++){
				Tower Tower = Towers[j];
				Vector2 HorizMarkerPosition = CalculateIndicatorPosition(Tower.gameObject.transform);
				Vector3 MarkerPosition = new Vector3(HorizMarkerPosition.x, HorizMarkerPosition.y, MarkerDepthOffset);
				SpriteRenderer Marker;
				if(Markers.Count<=TowerID){
					Marker = CreateMarker(MarkerPosition);
					Markers.Add(Marker);
				}
				else{
					Marker = Markers[TowerID];
					Marker.gameObject.transform.localPosition = MarkerPosition; //Vector3.Scale(MarkerPosition,Marker.gameObject.transform.localScale); // sets the marker position with sacle in mind
				}
				if(Tower.TowerIntact()){ // if tower intact sets the color to team color
					Marker.color = Teams[i].color;
				}
				else{ // else sets to destroyed color
					Marker.color = DestroyedColor;
				}
				TowerID++; // incrementing the TowerID for each tower in each team
			}
		}
	}
	void SetUpViewCone(){
		if(!ViewCone){
			ViewCone = (Instantiate(ViewConeObject,transform) as GameObject).transform;
		}
		
		ViewCone.localRotation = Quaternion.identity;
	}
	void RecalculateViewCone(){
		Vector2 HorizViewConePosition = CalculateIndicatorPosition(TowerRotationIndicator); // calculates the on screen position using the TowerRotationIndicator
		Vector3 ViewConePosition = new Vector3(HorizViewConePosition.x, HorizViewConePosition.y, ViewConeDepthOffset);
		ViewCone.localPosition = ViewConePosition;
		
	}
	Vector2 CalculateIndicatorPosition(Transform GlobalObject){
		Vector3 GlobalPosition = GlobalObject.position;
		Vector3 TransformedGlobalPosition = TowerRotationIndicator.InverseTransformPoint(GlobalPosition); // transforming the global position to local coordinates for this tower
		TransformedGlobalPosition = Vector3.Scale(TransformedGlobalPosition,new Vector3(LocalRadarScale.x,1,LocalRadarScale.y));//scaling the final position

		Vector2 IndicatorPos;

		//Transforms the x and y range of the object global position to the radar local position
		IndicatorPos.x = ChangeRange(TransformedGlobalPosition.x, GlobalRangeX, LocalRangeX);
		IndicatorPos.y = ChangeRange(TransformedGlobalPosition.z, GlobalRangeY, LocalRangeY);


		return IndicatorPos;

	}
	float ChangeRange(float value, Vector2 OldRange, Vector2 NewRange){
		float NewValue = (((value - OldRange.x) * (NewRange.y - NewRange.x)) / (OldRange.y - OldRange.x)) + NewRange.x;
		NewValue = Mathf.Clamp(NewValue,NewRange.x,NewRange.y); // adding clamping
		return NewValue;
	}
	SpriteRenderer CreateMarker(Vector3 position){
		GameObject MarkerObject = Instantiate(TowerMarker, transform) as GameObject;
		MarkerObject.transform.localPosition = position;
		MarkerObject.transform.localRotation = Quaternion.identity;
		SpriteRenderer Marker = MarkerObject.GetComponent<SpriteRenderer>();
		if(!Marker){
			Debug.LogError("The specified marker object does not contain a sprite renderer", gameObject);
		}
		return Marker;
	}
}
