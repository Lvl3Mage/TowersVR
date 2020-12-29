using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTogglerButton : SingleActivatedButton
{
	[SerializeField] string BoolName = "State";
	[SerializeField] Animator TargetAnimator;
    // Start is called before the first frame update
	protected override void SingleActivation(){
		TargetAnimator.SetBool(BoolName, !TargetAnimator.GetBool(BoolName)); // toggles the animator bool
		
	}
}
