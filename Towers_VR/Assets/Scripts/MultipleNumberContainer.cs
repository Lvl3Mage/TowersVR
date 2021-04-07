using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleNumberContainer : MonoBehaviour
{
	[System.Serializable]
	public class NumberField
	{
		[HideInInspector] public float value;
		[SerializeField] string ValueDescription;
	}
	[SerializeField] protected NumberField[] NumberList;
	public void WriteNumber(float val, int id){
		NumberList[id].value = val;
		OnListChange(id);
	}
	protected virtual void OnListChange(int id){

	}
	
}
