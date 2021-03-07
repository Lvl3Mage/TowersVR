using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticipantsList : MonoBehaviour
{
	[HideInInspector] public TeamCard relatedTeamCard;
	[SerializeField] Animator Animator;
	[SerializeField] Transform CardParent;
	[SerializeField] Object PlayerCardExample;
	public void AddPlayer(){
		GameObject NewPlayerCard = Object.Instantiate(PlayerCardExample, CardParent) as GameObject;
    	MapParticipant participant = NewPlayerCard.GetComponent<MapParticipant>();

    	//Adds the player to the list of team's players and changes his color
    	relatedTeamCard.AddPlayerToList(participant);
	}
	public void MoveIn(){
		Animator.SetBool("PlayerWindow",true);
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
