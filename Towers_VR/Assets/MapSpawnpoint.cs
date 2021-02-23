using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MapSpawnpoint : MonoBehaviour
{
	[SerializeField] Color defaultColor;
	public Button button;
	public RectTransform transform;
	[HideInInspector] public BaseParticipant attachedParticipant;
	public void RecalculateColor(BaseTeam[] teamArray){
		Graphic IconGraphic = button.targetGraphic;
		if(attachedParticipant != null){
			IconGraphic.color = teamArray[attachedParticipant.teamID].teamColor;
		}
		else{
			IconGraphic.color = defaultColor;
		}
	}
}
