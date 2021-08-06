using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberTextContainer : DataContainer
{
	[SerializeField] int Digits;
	[SerializeField] bool IgnoreNegative;
	[SerializeField] TextMeshProUGUI TextField;
	protected override void ChangeValue(string varName, float value){
		if(value != value){
			TextField.text = "--";
		}
		else if (IgnoreNegative && value<0) {
			TextField.text = "--";
		}
		else{
			float val = Mathf.Round(value * Mathf.Pow(10, Digits)) * (1/Mathf.Pow(10, Digits));
			TextField.text = val.ToString();			
		}

	}
}
