using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolSwitch_BoolValueWriter : BoolSwitch
{
	protected override void ValueChanged(){
		InvokeAllReferences(boolState ? 1 : 0);
	}
}
