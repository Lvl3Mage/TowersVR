using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerColorIndicator : MonoBehaviour
{
	[SerializeField] Graphic ColorIndicator;
	[SerializeField] MapParticipant PlayerCard;
    // Start is called before the first frame update
    void Start()
    {
    	if(PlayerCard.ShortenedState()){
	    	ColorIndicator.color = PlayerCard.GetTeamColor();
    	}
    	
    }

}
