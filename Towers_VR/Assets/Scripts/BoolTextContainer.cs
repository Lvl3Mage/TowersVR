using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoolTextContainer : BoolContainer
{
	[SerializeField] string OnTrue, OnFalse;
	[SerializeField] Color OnColor, OffColor;
	[SerializeField] TextMeshProUGUI TextField;
	[SerializeField] bool defaultValue;
	void Awake(){
		boolValue = defaultValue;
	}
	protected override void ValueChanged(){
		string SetValue;
		Color SetColor;
		if(_boolValue){
			SetColor = OnColor;
			SetValue = OnTrue;
		}
		else{
			SetColor = OffColor;
			SetValue = OnFalse;
		}
		TextField.color = SetColor;
		TextField.text = SetValue;
	}
}
