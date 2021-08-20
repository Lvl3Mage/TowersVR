using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberChangerButton : ToggleButton
{
	[SerializeField] ChangableValueContainer NumberTarget;
	[SerializeField] float Change, Acceleration;
	float CurChange;
	bool Toggled = false;
	Animator Animator;
	void Start(){
		Animator = GetComponent<Animator>();
	}
	void FixedUpdate(){
		if(Toggled){
			NumberTarget.ChangeValueBy(Time.fixedDeltaTime*CurChange);
			CurChange += Acceleration*Time.fixedDeltaTime;

		}

	}
    protected override void ToggleOn(){
    	CurChange = Change;
    	Debug.Log("Toggling on");
    	Toggled = true;
    	Animator.Play("ToggleOn");
    }
    protected override void ToggleOff(){
    	Debug.Log("Toggling off");
    	Toggled = false;
    	Animator.Play("ToggleOff");	
    }
}
