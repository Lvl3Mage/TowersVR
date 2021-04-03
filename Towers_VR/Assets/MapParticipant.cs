using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapParticipant : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	
	//[Header("Color Settings")]

	OnMapSpawnPicker MapGenerator;
	Color TeamColor, BaseSecColor;
	[Header("Transformation settings")]
	[SerializeField] float SnappingDistance;
	[SerializeField] float ColorLerpSpeed;
	[SerializeField] float SizeLerpSpeed;
	[SerializeField] float MovementSpeed;
	[SerializeField] float RotationSize;
	[SerializeField] Vector2 HoveringScaledDownSize;
	[SerializeField] Vector2 ScaledDownSize;
	Vector2 BaseScale;
	[Header("Self references")]
	[SerializeField] Graphic ColorIndicator;
	[SerializeField] Graphic SecondaryColorIndicator;
	[SerializeField] Animator Animator;

	RectTransform rectTransform;
	Coroutine ColorLerp, ResizeLerp;
	Canvas attachedCanvas;
	RectTransform CanvasRect;
	bool Drag;
	Vector2 LerpPos;
	Transform OrigParent;
	bool Shortened = false;

	MapSpawnpoint[] SpawnPoints;
	MapSpawnpoint ConnectedSpawnpoint;

	ParticipantSettings.PlayerType playerType = ParticipantSettings.PlayerType.AI; // The type of the player

	void Start(){

		OrigParent = transform.parent;
		MapGenerator = GameObject.FindGameObjectWithTag("BaseMapGenerator").GetComponent<OnMapSpawnPicker>();
		rectTransform = GetComponent<RectTransform>();
		BaseScale = transform.localScale;
		attachedCanvas = GetComponentInParent<Canvas>();
		CanvasRect = attachedCanvas.GetComponent<RectTransform>();
		BaseSecColor = SecondaryColorIndicator.color;
	}
	//Interaction methods
	public void SetColor(Color newColor){
		TeamColor = newColor;
		ColorIndicator.color = newColor;
		if(Shortened){
			LerpRecolor(newColor);
		}
		
	}
	public void PlayerTypeDropdownChange(Dropdown change){
		playerType = (ParticipantSettings.PlayerType)change.value;
	}
	//Drag Handling
	public void OnBeginDrag(PointerEventData eventData){
		if(ConnectedSpawnpoint){
			DisconnectFromSpawnpoint(); //disconnects the spawnpoint from self
			LerpResize(HoveringScaledDownSize); // Lerps the card to hovering size
		}
		UpdateSpawnpoints();
		LerpPos = CalculateCursorPos(eventData); // updates lerp to cursor position
		Drag = true;
		transform.parent = attachedCanvas.gameObject.transform;
		transform.SetAsLastSibling(); // making the object get drawn on top of everything else when dragged
		Shorten();
	}
	public void OnDrag(PointerEventData eventData){
		Vector2 CursorPos = CalculateCursorPos(eventData);
		MapSpawnpoint ClosestPoint = CalculateClosestSpawnpoint(eventData);
		if(ClosestPoint){
			Vector2 PointPosition = CalculateObjCanvasRelativePos(ClosestPoint.gameObject, eventData.pressEventCamera);

			LerpPos = PointPosition; // updates lerp to the spawnpoint position
			if(!ConnectedSpawnpoint){ // in case the spawnpoint is beign newly set (from null)
				LerpResize(HoveringScaledDownSize); // starts lerping the size to small (but still hovering)
			}
			if(!ConnectedSpawnpoint || ClosestPoint != ConnectedSpawnpoint){ // in case the spawnpoint is beign set or reset
				ConnectedSpawnpoint = ClosestPoint; // connects self to spawnpoint
			}
			
		}
		else{
			if(ConnectedSpawnpoint){
				LerpResize(BaseScale); // Starts lerping size to big again
				ConnectedSpawnpoint = null;
			}
			LerpPos = CursorPos; // updates lerp to the cursor position
		}
		
	}
	public void OnEndDrag(PointerEventData eventData){

		if(ConnectedSpawnpoint){
			ConnectToSpawnpoint(ConnectedSpawnpoint,eventData.pressEventCamera);
			 // Connects the spawnpoint to self
		}
		else{ // returns back to the list 
			ReturnToOrigin();
		}
		rectTransform.rotation =  Quaternion.EulerAngles(0f, 0f, 0f);
		Drag = false;
	}
	MapSpawnpoint CalculateClosestSpawnpoint(PointerEventData eventData){ // returns the Closest spawnpoint. in case there isn't any close enough returns null
		float SmallestDistance = SnappingDistance;
		MapSpawnpoint ClosestPoint = null;
		Camera Camera = eventData.pressEventCamera;
		Vector2 screenPointerPosition = eventData.position;
		foreach(MapSpawnpoint spawnpoint in SpawnPoints){
			if(spawnpoint.attachedParticipant == null){ // checks the availabuility of the spawnpoint
				Vector2 spawnpointPos = RectTransformUtility.WorldToScreenPoint(Camera, spawnpoint.gameObject.transform.position);
				//Debug.Log(spawnpointPos + " and " + screenPointerPosition);
				float delta = (spawnpointPos - screenPointerPosition).magnitude;

				if(SmallestDistance > delta){ // if delta is smaller than the smallest distance to spawnpoint this value is equal to the maximum snappingDistance by default
					SmallestDistance = delta;
					ClosestPoint = spawnpoint;
				}
			}
			
		}
		return ClosestPoint;
	}
	void ReturnToOrigin(){
		transform.parent = OrigParent;
		transform.SetAsFirstSibling();
		Grow();
	}
	//Animation
	void Shorten(){
		Shortened = true;
		LerpRecolor(TeamColor);
		Animator.SetBool("Shortened", true);
	}
	void Grow(){
		Shortened = false;
		LerpRecolor(BaseSecColor);
		Animator.SetBool("Shortened", false);
	}

	//Connection Handling
	void ConnectToSpawnpoint(MapSpawnpoint spawnpoint, Camera Camera){
		spawnpoint.attachedParticipant = this;

		LerpResize(ScaledDownSize);//Lerps to the scaled down size (when the card is static over a point)
		rectTransform.anchoredPosition = CalculateObjCanvasRelativePos(spawnpoint.gameObject, Camera); // sets the position to the point position

	}
	void DisconnectFromSpawnpoint(){
		if(ConnectedSpawnpoint){
			ConnectedSpawnpoint.attachedParticipant = null;
		}
		
	}
	
	void UpdateSpawnpoints(){
		SpawnPoints = MapGenerator.RequestSpawnpoints();
	}
	

	//Math Calculations
	Vector2 CalculateCursorPos(PointerEventData eventData){
		Vector2 CursorPos;
		Vector2 CursorScreenPos = eventData.position;
		Camera Camera = eventData.pressEventCamera;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRect, CursorScreenPos, Camera, out CursorPos);
		CursorPos = CompensateCornerAnchor(CursorPos);
		return CursorPos;
	}
	Vector2 CalculateObjCanvasRelativePos(GameObject obj, Camera Camera){
		Vector2 ScreenPoint = RectTransformUtility.WorldToScreenPoint(Camera, obj.transform.position);
		Vector2 CanvasRelativePos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRect, ScreenPoint, Camera, out CanvasRelativePos);
		CanvasRelativePos = CompensateCornerAnchor(CanvasRelativePos);
		return CanvasRelativePos;
	}
	Vector2 CompensateCornerAnchor(Vector2 Input){ // compensates the card's anchors pointing to the left top corner
		Input.x += CanvasRect.sizeDelta.x/2;
		Input.y -= CanvasRect.sizeDelta.y/2;
		return Input;
	}

	//Lerps
	void LerpToPos(Vector2 pos){
		float posMag = pos.magnitude;
		float selfPosMag = rectTransform.anchoredPosition.magnitude;
		float delta = selfPosMag - posMag;
		rectTransform.rotation =  Quaternion.EulerAngles(0f, 0f, delta*RotationSize);
		rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, pos, Time.deltaTime*MovementSpeed);
	}
	void LerpResize(Vector2 newScale){ // Call this instead of the enumerator
		if(ResizeLerp != null){
			StopCoroutine(ResizeLerp);
		}
		ResizeLerp = StartCoroutine(LerpSize(newScale));
	}
	IEnumerator LerpSize(Vector2 scale){
		Vector3 newScale = new Vector3(scale.x, scale.y, 1);
		while (transform.localScale != newScale) 
		{
			transform.localScale = Vector2.Lerp(transform.localScale,newScale,SizeLerpSpeed*Time.deltaTime);
			yield return null;
		}
	}

	void LerpRecolor(Color newColor){ // Call this instead of the enumerator
		if(ColorLerp != null){
			StopCoroutine(ColorLerp);
		}
		ColorLerp = StartCoroutine(LerpGraphicColor(newColor));
	}
	IEnumerator LerpGraphicColor(Color LerpColor){
		while (SecondaryColorIndicator.color != LerpColor) 
		{
			SecondaryColorIndicator.color = Color.Lerp(SecondaryColorIndicator.color, LerpColor, Time.deltaTime*ColorLerpSpeed);
			yield return null;
		}
	}
	void Update(){
		if(Drag){ // lerps the card if dragging
			if(LerpPos != null){
				LerpToPos(LerpPos);
			}
		}
	}
	// Get methods
	public ParticipantSettings.PlayerType GetPlayerType(){
		return playerType;
	}
	public MapSpawnpoint GetConnectedSpawnPoint(){
		return ConnectedSpawnpoint;
	}
	public bool ShortenedState(){
		return Shortened;
	}
	public Color GetTeamColor(){ // returns color the secondary should switch to. If it doesn't then returns null
		return TeamColor;
	}
}
