using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalControllerTracker : MonoBehaviour
{
	public enum ControllerType{ Left, Right}

	[SerializeField] ControllerType Controller;
	[SerializeField] GameObject VrRig;
	[SerializeField] float RotationSpeed = 5, MovementSpeed = 5;
	Rigidbody VRRB;
	Vector3 PastVRRigPos;
	SpringJoint GrabJoint;
    // Start is called before the first frame update
    void Start()
    {
    	VRRB = VrRig.GetComponent<Rigidbody>();
        PastVRRigPos = VrRig.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CompensatePlayerMovement();
        TrackController();
        if(GrabJoint){
        	TrackJoint(GrabJoint);
        }
    }
    void CompensatePlayerMovement(){
    	Vector3 Delta = VrRig.transform.position - PastVRRigPos;
    	transform.position += Delta;
    	PastVRRigPos = VrRig.transform.position;
    }
    void TrackController(){
    	TrackRotation();
    	TrackPosition();
    }
    void TrackPosition(){
    	transform.localPosition = Vector3.Lerp(transform.localPosition, LocalControllerPos(), Time.deltaTime * MovementSpeed);
    }
    void TrackRotation(){
		transform.localRotation = Quaternion.Lerp(transform.localRotation, LocalControllerRot(), Time.deltaTime * RotationSpeed);
    }
    Vector3 LocalControllerPos(){
    	Vector3 controllerPos = Vector3.zero;;
    	if(Controller == ControllerType.Left){
    		controllerPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
    	}
    	else if(Controller == ControllerType.Right){
    		controllerPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
    	}
    	return controllerPos;
    }
    Quaternion LocalControllerRot(){
    	Quaternion controllerRot = Quaternion.identity;
    	if(Controller == ControllerType.Left){
    		controllerRot = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
    	}
    	else if(Controller == ControllerType.Right){
    		controllerRot = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
    	}
    	return controllerRot;
    }
    void TrackJoint(SpringJoint TrackedJoint){
    	Vector3 controllerPos = LocalControllerPos();
    	TrackedJoint.anchor = controllerPos;
    }
}
