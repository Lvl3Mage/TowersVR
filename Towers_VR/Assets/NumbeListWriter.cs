using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberListWriter : NumberContainer
{
	[System.Serializable] 
	public class ListReference
	{
		public MultipleNumberContainer MultNumberRef;
		public int WriteValueID;
	}

	[SerializeField] ListReference[] WriteToFields;

	protected override void ValueChanged(){
		foreach (ListReference Ref in WriteToFields) 
		{
			Ref.MultNumberRef.WriteNumber(_floatValue,Ref.WriteValueID);
		}
	}
}
