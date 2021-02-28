using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorBoolController : MonoBehaviour
{
	[SerializeField] Animator Animator;
	[SerializeField] string boolName;
	public void SetBool(bool value){
		Animator.SetBool(boolName, value);
	}
}
