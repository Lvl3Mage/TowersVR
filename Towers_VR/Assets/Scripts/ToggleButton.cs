using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : Interactable
{

    void Awake() // Collision Interaction is handled in Base Class
    {
    	Material ButtonMat; 
		MeshRenderer MR = GetComponent<MeshRenderer>();
    	ButtonMat = MR.materials[0];
    	MR = GetComponent<MeshRenderer>();
    	MR.material = Instantiate(ButtonMat);
		//OnInteraction.AddListener(Activate);
    }
    protected override void Activate(bool ToggleValue){ // Toggle value is true only when the button is pressed
		if(ToggleValue){ 
			ToggleOn();
		}
		else{
			ToggleOff();
		}
    }
    protected virtual void ToggleOn() {}
    protected virtual void ToggleOff() {}
}
