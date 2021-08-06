using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatWriter_IntSwitch : AnimatedIntSwitch
{
	[SerializeField] float[] NumberValues;
	protected override void Animated(){
		InvokeAllReferences(NumberValues[State]);
    }
}
