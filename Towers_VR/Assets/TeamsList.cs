using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamsList : MonoBehaviour
{
	[SerializeField] Object ExampleTeamCard, ExamplePlayersWindow;
	[SerializeField] Transform CardParent, PlayerWindowsAddPoint;
	List<TeamCard> Teams = new List<TeamCard>();
	List<ParticipantsList> TeamRelatedLists = new List<ParticipantsList>();
	List<GameObject> TeamCards = new List<GameObject>();
	List<GameObject> PlayersWindows = new List<GameObject>();
	public void AddTeam(){
		GameObject NewPlayersWindow = Object.Instantiate(ExamplePlayersWindow, PlayerWindowsAddPoint) as GameObject;

		//Setting up the players windo
		RectTransform PlayersWindowTransform = NewPlayersWindow.GetComponent<RectTransform>();
		PlayersWindowTransform.anchorMin = new Vector2(1,0);
		PlayersWindowTransform.anchorMax = new Vector2(2,1);
		PlayersWindowTransform.anchoredPosition = Vector3.zero;

    	ParticipantsList newPlayersList = NewPlayersWindow.GetComponent<ParticipantsList>();
    	TeamRelatedLists.Add(newPlayersList);

		GameObject NewTeamCard = Object.Instantiate(ExampleTeamCard, CardParent) as GameObject;
    	TeamCard newTeam = NewTeamCard.GetComponent<TeamCard>();
    	Teams.Add(newTeam);

    	newPlayersList.relatedTeamCard = newTeam; //adding a teamcard reference to the player window for it to report when a new player is added 
    	newTeam.relatedPlayersWindow = newPlayersList;
    	newTeam.ChangeTeamColor(new Color(Random.Range(0f, 1f),Random.Range(0f, 1f),Random.Range(0f, 1f),1));
	}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
