using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberContainer : MonoBehaviour
{
	protected float _floatValue = 0;
	public float floatValue {

		get {
			return _floatValue;
		}

		set {
			_floatValue = value;
			ValueChanged();
		}
	}
	protected virtual void ValueChanged(){

	}
}
