using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamsList : MonoBehaviour
{
	[SerializeField] Object ExampleTeamCard, ExamplePlayersWindow;
	[SerializeField] Transform CardParent, PlayerWindowsAddPoint, AddTeamButton;
	[SerializeField] RectTransform ScrollingRect;
	List<TeamCard> Teams = new List<TeamCard>();
	List<ParticipantsList> TeamRelatedLists = new List<ParticipantsList>();
	List<GameObject> TeamCards = new List<GameObject>();
	List<GameObject> PlayersWindows = new List<GameObject>();
	public void AddTeam(){
		//INstantiating the team related players window
		GameObject NewPlayersWindow = Object.Instantiate(ExamplePlayersWindow, PlayerWindowsAddPoint) as GameObject;

		//Setting up the players window
		RectTransform PlayersWindowTransform = NewPlayersWindow.GetComponent<RectTransform>();
		PlayersWindowTransform.anchorMin = new Vector2(1,0);
		PlayersWindowTransform.anchorMax = new Vector2(2,1);
		PlayersWindowTransform.anchoredPosition = Vector3.zero;

		//Getting the ParticipantsList component from the players window and adding it to the list
    	ParticipantsList newPlayersList = NewPlayersWindow.GetComponent<ParticipantsList>();
    	TeamRelatedLists.Add(newPlayersList);

    	//Instantiating team card and getting the teamcard component
		GameObject NewTeamCard = Object.Instantiate(ExampleTeamCard, CardParent) as GameObject;
    	TeamCard newTeam = NewTeamCard.GetComponent<TeamCard>();
    	Teams.Add(newTeam);

    	//Putting the add button at the end of the list
    	AddTeamButton.SetAsLastSibling();

    	//Modifying the scrollrect height by the added card height
    	float CardHeight = NewTeamCard.GetComponent<RectTransform>().sizeDelta.y;
    	AddRectHeight(ScrollingRect, CardHeight);

		//Adding a teamcard reference to the player window for it to report when a new player is added 
    	newPlayersList.relatedTeamCard = newTeam; 
    	newTeam.relatedPlayersWindow = newPlayersList;
    	newTeam.ChangeTeamColor(new Color(Random.Range(0f, 1f),Random.Range(0f, 1f),Random.Range(0f, 1f),1));
	}
	void AddRectHeight(RectTransform BaseRect, float height){
		Vector2 ScrollSize = BaseRect.offsetMin;
    	ScrollSize.y -= height;
    	BaseRect.offsetMin = ScrollSize;
	}
}
