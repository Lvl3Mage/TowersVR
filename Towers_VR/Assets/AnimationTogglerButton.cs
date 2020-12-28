using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTogglerButton : SingleActivatedButton
{
	[SerializeField] string BoolName = "State";
	[SerializeField] Animator TargetAnimator;
	[SerializeField] bool InitialState = false;
	bool State;
	void Start(){
		State = InitialState;
	}
    // Start is called before the first frame update
	protected override void SingleActivation(){
		State = !State;
		TargetAnimator.SetBool(BoolName, State);
		
	}
}
