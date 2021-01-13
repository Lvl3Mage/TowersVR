using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedIntSwitch : IntSwitch
{
	[SerializeField] Animator Animator;
	[SerializeField] string AnimatorIntName;
    // Start is called before the first frame update
    protected override void StateChanged(){
    	Animator.SetInteger(AnimatorIntName, State);
    	Animated();
    }
    protected virtual void Animated(){

    }
}
