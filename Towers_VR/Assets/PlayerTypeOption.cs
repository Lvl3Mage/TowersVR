using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerTypeOption : MonoBehaviour
{
	[SerializeField] Graphic ColorIndicator;
	[SerializeField] MapParticipant PlayerCard;
	[SerializeField] Dropdown Dropdown;
	[SerializeField] Toggle Toggle;
    // Start is called before the first frame update
    void Start()
    {
    	if(PlayerCard.ShortenedState()){
	    	ColorIndicator.color = PlayerCard.GetTeamColor();
    	}
    	int sibIndex = transform.GetSiblingIndex();
		// Gets the text value of the dropdown option with the same index as the card sibling index (gets option related to card) and gets the enum type based on that name
    	ParticipantSettings.PlayerType Type = (ParticipantSettings.PlayerType)System.Enum.Parse(typeof(ParticipantSettings.PlayerType), Dropdown.options[sibIndex-1].text); 
    	
    	if(PlayerCard.IsTypeAvailable(Type)){
    		Toggle.interactable = true;
    	}
    	else{
    		Toggle.interactable = false;
    	}
    }

}
