using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAnimator : MonoBehaviour
{
	[SerializeField] Activatable[] ActivatedObjs;
    [SerializeField] GameObject[] EnableObjects;
    public bool Intercept;
	[SerializeField] float TimeScale;
    Joint Joint;
	float startingTimescale, startingTimestep;
	void Awake(){
		startingTimestep = Time.fixedDeltaTime;
		startingTimescale = Time.timeScale;
	}
    void Update()
    {
    	if(Time.timeScale != TimeScale){
	    	UpdateFixedStep(TimeScale);
	        Time.timeScale = TimeScale;    		
    	}

    }
    void UpdateFixedStep(float newTimescale){
    	if(newTimescale<=0.001f){
    		Time.fixedDeltaTime = 0.02f;
    	}
    	Time.fixedDeltaTime = startingTimestep*(newTimescale/startingTimescale); // multiplying the fixed timestep by the timescale change
    }
    public void ActivateObj(int id){
    	ActivatedObjs[id].Activate(true);
    }
    public void RemoveTrackedObject(){
        GameObject TrackedObj = Joint.connectedBody.gameObject;
        Destroy(Joint);
        TrackedObj.SetActive(false);
    }
    public void EnableObj(int id){
        EnableObjects[id].SetActive(true);
    }
    void OnTriggerEnter(Collider other){
        if(Intercept && ! Joint){
            Joint = gameObject.AddComponent<FixedJoint>();
            Joint.connectedBody = other.attachedRigidbody;            
        }

    }
}
