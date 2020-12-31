using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalControllerTracker : MonoBehaviour
{
	public enum ControllerType{ Left, Right}

	[SerializeField] ControllerType Controller;
	[SerializeField] GameObject VrRig;
	[SerializeField] float RotationSpeed = 5, MovementSpeed = 5;
	[SerializeField] LayerMask Grabable, Carriable;
	[SerializeField] bool GrabTriggers;
	[SerializeField] float GrabableDistance, Spheresize;
	Rigidbody VRRB, selfRB;
	Vector3 PastVRRigPos;
	SpringJoint GrabJoint;
	FixedJoint CarryJoint;
    // Start is called before the first frame update
    void Start()
    {
    	selfRB = GetComponent<Rigidbody>();
    	VRRB = VrRig.GetComponent<Rigidbody>();
        PastVRRigPos = VrRig.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CompensatePlayerMovement();
        TrackController();
        if(GetTriggerDown()){
        	// GrabRaycast
        	Collider[] GrabObjects = Physics.OverlapSphere(transform.position, Spheresize, Grabable);
        	if(GrabObjects.Length > 0){
	        	float ClosestGrabPointDistance = Mathf.Infinity;
	        	Vector3 ClosestGrabPoint = Vector3.zero;
	        	Collider ClosestCol = null;
	        	foreach (Collider col in GrabObjects) 
	        	{
	        		Vector3 point = col.ClosestPoint(transform.position);
	        		if(transform.InverseTransformPoint(point).magnitude<ClosestGrabPointDistance){
	        			ClosestGrabPointDistance = transform.InverseTransformPoint(point).magnitude;
	        			ClosestGrabPoint = point;
	        			ClosestCol = col;
	        		}
	        	}
	        	if((Carriable == (Carriable | (1 << ClosestCol.attachedRigidbody.gameObject.layer)))){
	        		CreateCarryJoint(ClosestCol.attachedRigidbody);
	        	}
	        	else{
		        	CreateGrabJoint(ClosestGrabPoint,ClosestCol.attachedRigidbody);
	        	}
        	}
        }
        if(GetTriggerUp()){
        	// GrabRaycast
			if(GrabJoint){
				Destroy(GrabJoint);
				if(VrRig.GetComponents<SpringJoint>().Length <= 1){
					VRRB.drag = 0;
				}
			}
			if(CarryJoint){
				Destroy(CarryJoint);
			}
        }
        if(CarryJoint){
        	if(!CarryJoint.connectedBody){
				Destroy(CarryJoint);
        	}
		}
        if(GrabJoint){
        	TrackJoint(GrabJoint);
        }
    }
    void CreateGrabJoint(Vector3 GrabPos, Rigidbody GrabedRB){
    	VRRB.drag = 10f;
    	Vector3 LocalGrabPos;
    	if(GrabedRB){
	    	LocalGrabPos = GrabedRB.gameObject.transform.InverseTransformPoint(GrabPos); // Grab position in local coordinates of the grabed object
    	}
    	else{
    		LocalGrabPos = GrabPos;
    	}
    	GrabJoint = VrRig.AddComponent<SpringJoint>();
    	GrabJoint.connectedBody = GrabedRB;
    	GrabJoint.autoConfigureConnectedAnchor = false;
    	GrabJoint.connectedAnchor = LocalGrabPos;
    	GrabJoint.spring = 10000f;
    	GrabJoint.tolerance = 0.00001f;
    	GrabJoint.damper = 100f;
    	TrackJoint(GrabJoint);
    }
    void CreateCarryJoint(Rigidbody GrabedRB){
    	CarryJoint = selfRB.gameObject.AddComponent<FixedJoint>();
    	CarryJoint.connectedBody = GrabedRB;
    }
    void CompensatePlayerMovement(){
    	Vector3 Delta = VrRig.transform.position - PastVRRigPos;
    	transform.position += Delta;
    	PastVRRigPos = VrRig.transform.position;
    }
    void TrackController(){
    	if(!GrabJoint){
	    	TrackRotation();		
    	}
    	TrackPosition();
    }
    void TrackPosition(){
    	Vector3 PosVector = LocalControllerPos() - VrRig.transform.InverseTransformPoint(transform.position);
    	selfRB.velocity = VrRig.transform.TransformDirection(PosVector)*MovementSpeed;
    	//transform.localPosition = Vector3.Lerp(transform.localPosition, LocalControllerPos(), Time.deltaTime * MovementSpeed);
    }
    void TrackRotation(){
		transform.localRotation = Quaternion.Lerp(transform.localRotation, LocalControllerRot(), Time.deltaTime * RotationSpeed);
    }
    Vector3 LocalControllerPos(){ // GetControllerOrientationTracked could help with the jiggling issue
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
    bool GetTriggerDown(){
    	bool trigger = false;
    	if(Controller == ControllerType.Left){
    		trigger = OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger, OVRInput.Controller.LTouch);
    	}
    	else if(Controller == ControllerType.Right){
    		trigger = OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger, OVRInput.Controller.RTouch);
    	}
    	return trigger;   	
    }
    bool GetTriggerUp(){
    	bool trigger = false;
    	if(Controller == ControllerType.Left){
    		trigger = OVRInput.GetUp(OVRInput.RawButton.LIndexTrigger, OVRInput.Controller.LTouch);
    	}
    	else if(Controller == ControllerType.Right){
    		trigger = OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger, OVRInput.Controller.RTouch);
    	}
    	return trigger;   	
    }
    void TrackJoint(SpringJoint TrackedJoint){
    	Vector3 controllerPos = LocalControllerPos();
    	TrackedJoint.anchor = controllerPos;
    }
}
