using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SingleActivatedButton : Interactable
{
    protected Animator Animator;
    [SerializeField] string ActivationClipName = "SingleButtonActivation";
    void Awake() // Collision Interaction is handled in Base Class
    {
    	Material ButtonMat; 
		MeshRenderer MR = GetComponent<MeshRenderer>();
    	ButtonMat = MR.materials[0];
    	MR = GetComponent<MeshRenderer>();
    	MR.material = Instantiate(ButtonMat);
        Animator = GetComponent<Animator>();
    }
    /*void Activate(bool ToggleValue){
		if(ToggleValue){ // Toggle value is true only when the button is pressed
		Debug.Log("Heey");
			//OnActivate.Invoke();
		}
    }*/
    protected override void Activate(bool ToggleVal){
        if(ToggleVal){
            SingleActivation();
        }
        Debug.Log(Animator.gameObject.name);
        Animator.Play(ActivationClipName);
    }
    protected virtual void SingleActivation(){

    }
}
