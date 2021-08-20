using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleActivatorButton : ToggleButton
{
	[SerializeField] Activatable[] Targets;
    // Start is called before the first frame update
    protected override void ToggleOn() {
    	foreach (Activatable Target in Targets) 
    	{
    		Target.Activate(true);
    	}
    }
    protected override void ToggleOff() {
    	foreach (Activatable Target in Targets) 
    	{
    		Target.Activate(false);
    	}
    }
}
