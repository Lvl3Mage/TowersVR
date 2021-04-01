using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class MapSpawnpoint : MonoBehaviour 
{
	OnMapSpawnPicker MapGenerator;
	void Start(){
		MapGenerator = GameObject.FindGameObjectWithTag("BaseMapGenerator").GetComponent<OnMapSpawnPicker>();
	}
	private MapParticipant _attachedParticipant; // this could in theory just be a bool but this approach allows some extension (we could for example communicate with the other participant attached top the team for it to move)
	[HideInInspector] public MapParticipant attachedParticipant{
		get { 
			return _attachedParticipant; 
		} 
		set {
			_attachedParticipant = value;
			connectionStateChangedCallback();
		}
	}
	void connectionStateChangedCallback(){
		MapGenerator.RevalidateStartConditions();
	}
}
