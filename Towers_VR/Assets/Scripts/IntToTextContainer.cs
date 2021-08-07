using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntToTextContainer : DataContainer
{
	[System.Serializable]
	public class IntToTextField
	{
		[SerializeField] string _TextValue;
		[HideInInspector] public string TextValue{

			get {
				return _TextValue;
			}
		}
		[SerializeField] Color _ColorValue;
		[HideInInspector] public Color ColorValue{

			get {
				return _ColorValue;
			}
		}
	}
	[SerializeField] IntToTextField[] Values;
	[SerializeField] TextMeshProUGUI TextField;
	[SerializeField] int defaultValue;
	protected int IntValue;
	void Awake(){
		IntValue = defaultValue;
	}
	protected override void ChangeValue(string varName, float value){
		IntValue = (int)value;
		IntToTextField SetValue = Values[IntValue];

		TextField.color = SetValue.ColorValue;
		TextField.text = SetValue.TextValue;
	}
}
