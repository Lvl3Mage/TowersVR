using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activatable : MonoBehaviour
{
	public void Activate(bool toggleValue){
		OnActivate(toggleValue);
	}
	protected virtual void OnActivate(bool toggleValue){

	}
}
