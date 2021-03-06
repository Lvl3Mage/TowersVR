﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntSwitch : Switch
{
	[SerializeField] int InitialState;
	[SerializeField] int MaxState;
	[SerializeField] IntContainer TextDisplay;
	protected int State = 0;
	void Awake(){
		State = InitialState;
	}
    protected override void Switched(){
    	State ++;
    	if(State>MaxState){
    		State = 0;
    	}
    	TextDisplay.intValue = State;
    	StateChanged();
    }
    protected virtual void StateChanged(){

    }
}
