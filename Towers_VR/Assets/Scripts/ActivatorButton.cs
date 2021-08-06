using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivatorButton : SingleActivatedButton
{
	protected override void SingleActivation(){
		InvokeAllReferences(1);
	}
}
