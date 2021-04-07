using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolSwitch : Switch
{
	[SerializeField] Animator Animator;
	[SerializeField] string AnimatorBoolName;
	[SerializeField] bool InitialState;
	protected bool boolState;

	void Awake(){
		boolState = InitialState;
	}
	protected override void Switched(){
		boolState = !boolState;
		Animator.SetBool(AnimatorBoolName,boolState);
		ValueChanged();
	}
	protected virtual void ValueChanged(){

	}
}
