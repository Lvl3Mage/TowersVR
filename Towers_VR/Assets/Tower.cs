using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
	[SerializeField] protected TowerKeypoint[] _KeyPoints;
	public TowerKeypoint[] KeyPoints { 
		get{
			return _KeyPoints;
		} 
		private set{
			_KeyPoints = value;
		} 
	}
    
}
