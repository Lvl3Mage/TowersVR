using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Interactable
{
    protected override void Activate(bool ToggleVal) {
    	if(ToggleVal){ // change only if pressed down
    		Switched();
    	}
    }
    protected virtual void Switched(){

    }
}
