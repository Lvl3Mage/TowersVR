using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberTextContainer : NumberContainer
{
	[SerializeField] int Digits;
	[SerializeField] TextMeshProUGUI TextField;
	protected override void ValueChanged(){
		if(_floatValue != _floatValue){
			TextField.text = "--";
		}
		else{
			float val = Mathf.Round(_floatValue * Mathf.Pow(10, Digits)) * (1/Mathf.Pow(10, Digits));
			TextField.text = val.ToString();			
		}

	}
}
