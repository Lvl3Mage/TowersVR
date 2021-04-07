using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntToTextContainer : IntContainer
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
	void Awake(){
		intValue = defaultValue;
	}
	protected override void ValueChanged(){
		IntToTextField SetValue = Values[intValue];

		TextField.color = SetValue.ColorValue;
		TextField.text = SetValue.TextValue;
	}
}
