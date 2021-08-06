using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleActivatorButton : ToggleButton
{
    // Start is called before the first frame update
    protected override void ToggleOn() {
    	InvokeAllReferences(1);
    }
    protected override void ToggleOff() {
    	InvokeAllReferences(0);
    }
}
