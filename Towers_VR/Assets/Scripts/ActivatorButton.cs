using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivatorButton : SingleActivatedButton
{
	[SerializeField] OutputDataContainer[] Outputs;
	protected override void SingleActivation(){
		foreach(OutputDataContainer Output in Outputs){
			Output.InvokeOutput(true);
		}
	}
}
