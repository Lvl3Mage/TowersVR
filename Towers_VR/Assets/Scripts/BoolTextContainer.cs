using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoolTextContainer : DataContainer
{
	[SerializeField] string OnTrue, OnFalse;
	[SerializeField] Color OnColor, OffColor;
	[SerializeField] TextMeshProUGUI TextField;
	[SerializeField] bool defaultValue;
	bool BoolValue;
	void Awake(){
		BoolValue = defaultValue;
	}
	protected override void ChangeValue(string varName, float value){
		BoolValue = value != 0;
		string SetValue;
		Color SetColor;
		if(BoolValue){
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
