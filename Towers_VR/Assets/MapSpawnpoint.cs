using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class MapSpawnpoint : MonoBehaviour//, IDropHandler
{
	[SerializeField] Color defaultColor;
	[SerializeField] Graphic IconGraphic;
	[HideInInspector] public MapParticipant attachedParticipant;
	public void RecalculateColor(BaseTeam[] teamArray){
		/*if(attachedParticipant != null){
			IconGraphic.color = teamArray[attachedParticipant.teamID].teamColor;
		}
		else{
			IconGraphic.color = defaultColor;
		}*/
	}
	public void OnDrop(PointerEventData eventData){
		/*attachedParticipant = eventData.pointerDrag.GetComponent<MapParticipant>();
		if(attachedParticipant){
			attachedParticipant.AttachTo(this);
		}
		eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;*/
	}
}
