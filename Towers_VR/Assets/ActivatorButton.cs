using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivatorButton : SingleActivatedButton
{
	[SerializeField] Activatable Target;
	protected override void SingleActivation(){
		Target.Activate(true);
	}
}
