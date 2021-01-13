using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolSwitch_BoolValueWriter : BoolSwitch
{
	[SerializeField] BoolContainer BoolContainer;
	protected override void ValueChanged(){
		BoolContainer.boolValue = boolState; 
	}
}
