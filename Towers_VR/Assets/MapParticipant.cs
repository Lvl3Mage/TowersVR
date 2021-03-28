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
	[SerializeField] float MovementSpeed;
	[SerializeField] float RotationSize;
	[Header("Self references")]
	[SerializeField] Graphic ColorIndicator;
	[SerializeField] Graphic SecondaryColorIndicator;
	[SerializeField] Animator Animator;

	RectTransform rectTransform;
	Coroutine ColorLerp;
	Canvas attachedCanvas;
	RectTransform CanvasRect;
	bool Drag;
	Vector2 LerpPos;
	Transform OrigParent;

	MapSpawnpoint[] SpawnPoints;
	MapSpawnpoint ConnectedSpawnpoint;
	void Start(){
		OrigParent = transform.parent;
		MapGenerator = GameObject.FindGameObjectWithTag("BaseMapGenerator").GetComponent<OnMapSpawnPicker>();
		rectTransform = GetComponent<RectTransform>();
		attachedCanvas = GetComponentInParent<Canvas>();
		CanvasRect = attachedCanvas.GetComponent<RectTransform>();
		BaseSecColor = SecondaryColorIndicator.color;
	}
	public void SetColor(Color newColor){
		TeamColor = newColor;
		ColorIndicator.color = newColor;
	}
	void Shorten(){
		if(ColorLerp != null){
			StopCoroutine(ColorLerp);
		}
		ColorLerp = StartCoroutine(LerpGraphicColor(TeamColor));
		Animator.SetBool("Shortened", true);
	}
	void Grow(){
		if(ColorLerp != null){
			StopCoroutine(ColorLerp);
		}
		ColorLerp = StartCoroutine(LerpGraphicColor(BaseSecColor));
		Animator.SetBool("Shortened", false);
	}
	IEnumerator LerpGraphicColor(Color LerpColor){
		while (SecondaryColorIndicator.color != LerpColor) 
		{
			Debug.Log("Lerping color to ");
			SecondaryColorIndicator.color = Color.Lerp(SecondaryColorIndicator.color, LerpColor, Time.deltaTime*ColorLerpSpeed);
			yield return null;
		}
	}
	public void OnBeginDrag(PointerEventData eventData){
		if(ConnectedSpawnpoint){
			ConnectedSpawnpoint.attachedParticipant = null; //disconnects the spawnpoint from self
		}
		UpdateSpawnpoints();
		LerpPos = CalculateCursorPos(eventData); // updates lerp to cursor position
		Drag = true;
		transform.parent = attachedCanvas.gameObject.transform;
		Shorten();
	}
	public void OnDrag(PointerEventData eventData){
		Vector2 CursorPos = CalculateCursorPos(eventData);
		MapSpawnpoint ClosestPoint = CalculateClosestSpawnpoint(eventData);
		if(ClosestPoint){
			Vector2 PointPosition = CalculateObjCanvasRelativePos(ClosestPoint.gameObject, eventData.pressEventCamera);

			LerpPos = PointPosition; // updates lerp to the spawnpoint position
			ConnectedSpawnpoint = ClosestPoint; // sets the lerping to spawnpoint to the closest one
		}
		else{
			if(ConnectedSpawnpoint){
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
	void ConnectToSpawnpoint(MapSpawnpoint spawnpoint, Camera Camera){
		spawnpoint.attachedParticipant = this;

		rectTransform.anchoredPosition = CalculateObjCanvasRelativePos(spawnpoint.gameObject, Camera);

	}
	void ReturnToOrigin(){
		transform.parent = OrigParent;
		transform.SetAsFirstSibling();
		Grow();
	}
	void UpdateSpawnpoints(){
		SpawnPoints = MapGenerator.RequestSpawnpoints();
	}
	void LerpToPos(Vector2 pos){
		float posMag = pos.magnitude;
		float selfPosMag = rectTransform.anchoredPosition.magnitude;
		float delta = selfPosMag - posMag;
		rectTransform.rotation =  Quaternion.EulerAngles(0f, 0f, delta*RotationSize);
		rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, pos, Time.deltaTime*MovementSpeed);
	}
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
	void Update(){
		if(Drag){ // lerps the card if dragging
			if(LerpPos != null){
				LerpToPos(LerpPos);
			}
		}
	}
}
