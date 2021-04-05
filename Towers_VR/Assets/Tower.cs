using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
	[SerializeField] protected Transform[] _KeyPoints;
	public Transform[] KeyPoints { 
		get{
			return _KeyPoints;
		} 
		private set{
			_KeyPoints = value;
		} 
	}
    
}
