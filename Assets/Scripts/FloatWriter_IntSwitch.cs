using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatWriter_IntSwitch : AnimatedIntSwitch
{
	[SerializeField] float[] NumberValues;
	[SerializeField] OutputDataContainer[] Outputs;
	protected override void Animated(){
		foreach (OutputDataContainer Output in Outputs) 
		{
			Output.InvokeOutput(NumberValues[State]);
		}
    }
}
