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
		SetBool(DataType.NULL, defaultValue);
	}
	protected override void SetBool(DataType dataType, bool value){
		string SetValue;
		Color SetColor;
		if(value){
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
