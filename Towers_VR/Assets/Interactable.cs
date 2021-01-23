using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
	
	public void Interact(bool ToggleValue){
		Activate(ToggleValue);
	}
	/*void OnCollisionEnter(Collision collisionInfo){
    	Activate(true);
    }
    void OnCollisionExit(Collision collisionInfo){
    	Activate(false);
    }*/
    /*void OnTriggerEnter(Collider collisionInfo){
        Activate(true);
    }
    void OnTriggerExit(Collider collisionInfo){
        Activate(false);
    }*/
    protected virtual void Activate(bool ToggleVal) {}
}
