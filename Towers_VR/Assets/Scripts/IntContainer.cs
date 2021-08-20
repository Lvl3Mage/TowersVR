using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntContainer : MonoBehaviour
{
	protected int _intValue;
	public int intValue {

		get {
			return _intValue;
		}

		set {
			_intValue = value;
			ValueChanged();
		}
	}
	protected virtual void ValueChanged(){

	}
}
