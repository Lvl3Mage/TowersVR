using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedGrabable : MonoBehaviour
{
	[SerializeField] protected Transform _ViewOffset;
	[HideInInspector] public Transform ViewOffset {

		get {
			return _ViewOffset;
		}

		protected set {
			_ViewOffset = value;
		}
	}
	[SerializeField] protected Transform _HoldOffset;
	[HideInInspector] public Transform HoldOffset {

		get {
			return _HoldOffset;
		}

		protected set {
			_HoldOffset = value;
		}
	}
}
