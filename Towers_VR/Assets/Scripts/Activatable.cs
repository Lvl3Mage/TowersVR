using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activatable : MonoBehaviour
{
	public void Activate(bool toggleValue){ // gets the toggle value that determines whether the button is being pressed or unpressed (in case it is a toggle)
		OnActivate(toggleValue);
	}
	protected virtual void OnActivate(bool toggleValue){

	}
}
