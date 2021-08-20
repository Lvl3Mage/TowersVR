using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolSwitch_BoolValueWriter : BoolSwitch //should be renamed to output type writer
{
	[SerializeField] CannonAngleCalculator outputTypeTarget; // so far this serves only to change the output type
	protected override void ValueChanged(){
		outputTypeTarget.WriteOutputType(boolState); 
	}
}
