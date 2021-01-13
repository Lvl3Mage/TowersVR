using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatWriter_IntSwitch : AnimatedIntSwitch
{
	[SerializeField] float[] NumberValues;
	[SerializeField] NumberContainer[] Targets;
	protected override void Animated(){
		foreach (NumberContainer Target in Targets) 
		{
			Target.floatValue = NumberValues[State];
		}
    }
}
