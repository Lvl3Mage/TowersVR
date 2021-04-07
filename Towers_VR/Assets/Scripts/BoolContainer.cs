using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolContainer : MonoBehaviour
{
	protected bool _boolValue = false;
	public bool boolValue {

		get {
			return _boolValue;
		}

		set {
			_boolValue = value;
			ValueChanged();
		}
	}
	protected virtual void ValueChanged(){

	}
}
