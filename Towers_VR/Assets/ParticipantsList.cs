using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticipantsList : MonoBehaviour
{
	[HideInInspector] public TeamCard relatedTeamCard;
	[SerializeField] Animator Animator;
	[SerializeField] Transform CardParent, AddPlayerButton;
	[SerializeField] RectTransform ScrollingRect;
	[SerializeField] Object PlayerCardExample;
	OnMapSpawnPicker MapGenerator;
	void Start(){
		MapGenerator = GameObject.FindGameObjectWithTag("BaseMapGenerator").GetComponent<OnMapSpawnPicker>();
	}
	public void AddPlayer(){
		GameObject NewPlayerCard = Object.Instantiate(PlayerCardExample, CardParent) as GameObject;
    	MapParticipant participant = NewPlayerCard.GetComponent<MapParticipant>();

    	AddPlayerButton.SetAsLastSibling();

    	float CardHeight = NewPlayerCard.GetComponent<RectTransform>().sizeDelta.y;
    	AddRectHeight(ScrollingRect, CardHeight);
    	//Adds the player to the list of team's players and changes his color
    	relatedTeamCard.AddPlayerToList(participant);
    	ReportPlayerAddition(participant);
	}
	void ReportPlayerAddition(MapParticipant Participant){ // reports that a certain type of player has been added
		MapGenerator.TypeAdded(Participant.GetPlayerType());
	}
	public void MoveIn(){
		Animator.SetBool("PlayerWindow",true);
	}
    void AddRectHeight(RectTransform BaseRect, float height){
		Vector2 ScrollSize = BaseRect.offsetMin;
    	ScrollSize.y -= height;
    	BaseRect.offsetMin = ScrollSize;
	}
}
