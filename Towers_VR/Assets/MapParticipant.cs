using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapParticipant : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	[HideInInspector] public int teamID;
	MapSpawnpoint attachedPoint;
	Canvas Canvas;
	Transform CardParent;
	RectTransform rectTransform;
	[SerializeField] CanvasGroup canvasGroup;
	void Start(){
		CardParent = transform.parent;
		Canvas = GetComponentInParent<Canvas>();
		rectTransform = GetComponent<RectTransform>();
	}
	public void OnBeginDrag(PointerEventData eventData){
		canvasGroup.blocksRaycasts = false;
		transform.parent = Canvas.gameObject.transform;
		rectTransform.SetAsLastSibling();
		
	}
	public void OnDrag(PointerEventData eventData){
		rectTransform.anchoredPosition += eventData.delta;
	}
	public void OnEndDrag(PointerEventData eventData){
		canvasGroup.blocksRaycasts = true;
		MapSpawnpoint point = eventData.pointerCurrentRaycast.gameObject.GetComponent<MapSpawnpoint>();
		if(!point){
			Return(); // Even without the return the positions is set to 0 in the map spawnpoint for some reason
		} // Make the whole thing less finicky and figure out how to make it more intuitive
	}
	public void Return(){
		transform.parent = CardParent;
		rectTransform.anchoredPosition = Vector2.zero;	
	}
	public void AttachTo(MapSpawnpoint point){
		attachedPoint = point;
	}
}
