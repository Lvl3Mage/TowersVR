using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalControllerTracker : MonoBehaviour
{
	public enum ControllerType{ Left, Right}

	[SerializeField] ControllerType Controller;
	[SerializeField] GameObject VrRig;
	[SerializeField] float RotationSpeed = 5, MovementSpeed = 5;
	[SerializeField] LayerMask Grabable, Carriable, Interactable, Collectable;
	[SerializeField] bool GrabTriggers;
	[SerializeField] float Spheresize, InteractRadius,InteractDistance, InventoryCollectRadius;
	Rigidbody VRRB, selfRB;
	Vector3 PastVRRigPos;
	SpringJoint GrabJoint;
	FixedJoint CarryJoint;
    Interactable InteractedObject;
    GameObject CollectedItem;
    Transform CollectedParent;
    [Header("Controlls")]
    [SerializeField] OVRInput.RawButton GrabInput;
    [SerializeField] OVRInput.RawButton CollectInput;

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
        if(OVRInput.GetDown(CollectInput)){
            CollectAction();
        }
        if(OVRInput.GetDown(GrabInput)){
        	// GrabRaycast
            Interaction();
        }
        if(OVRInput.GetUp(GrabInput)){
        	// GrabRaycast
            if(InteractedObject){
                InteractedObject.Interact(false);
            }
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
    Collider GetClosestObj(Collider[] GrabObjects){
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
            return ClosestCol;
        }
        else{
            return null;
        }
    }
    void CollectAction(){
        if(CollectedItem){
            ThrowItem();
        }
        else{
            CollectItem();
        }
    }   
    void CollectItem(){
        Collider[] CollectObjects = Physics.OverlapSphere(transform.position, InventoryCollectRadius, Collectable);
        if(CollectObjects.Length>0){
            Collider CollectedCol = GetClosestObj(CollectObjects);
            Rigidbody CollectedRigidbody = CollectedCol.attachedRigidbody;
            if(CollectedRigidbody){
                CollectedItem = CollectedRigidbody.gameObject;
                /*CollectedItem = CollectedRigidbody.gameObject;
                CollectedRigidbody.detectCollisions = false;
                CollectedRigidbody.isKinematic = true;*/
            }
            else{
                CollectedItem = CollectedCol.gameObject;
            }
            CollectedParent = CollectedItem.transform.parent;
            CollectedItem.transform.parent = transform;
            CollectedItem.SetActive(false);            
        }


    }
    void ThrowItem(){
        CollectedItem.transform.parent = CollectedParent;
        CollectedItem.SetActive(true);
        CollectedItem = null;
        CollectedParent = null;
    }
    void Interaction(){
        Collider[] GrabObjects = Physics.OverlapCapsule(transform.position, transform.TransformPoint(0, 0, InteractDistance), InteractRadius, Interactable);
        GameObject Obj = null;
        if(GrabObjects.Length > 0){
            Obj = GetClosestObj(GrabObjects).gameObject;  
        }
        Interactable Inter = null;
        if(Obj){
            Inter = Obj.GetComponent<Interactable>();
        }
        if(Inter){
            Inter.Interact(true);
            InteractedObject = Inter;
            //Debug.Log(Inter.gameObject.name);
        }
        else{
            PhysicsInteraction();
        }
    }
    void PhysicsInteraction(){
        Collider[] GrabObjects = Physics.OverlapSphere(transform.position, Spheresize, Grabable);
        Collider ClosestCol = GetClosestObj(GrabObjects);
        if(ClosestCol){
            if(ClosestCol.attachedRigidbody){
                if((Carriable == (Carriable | (1 << ClosestCol.attachedRigidbody.gameObject.layer)))){
                    CreateCarryJoint(ClosestCol.attachedRigidbody);
                }  
                else{
                    CreateGrabJoint(ClosestCol.ClosestPoint(transform.position),ClosestCol.attachedRigidbody);
                }         
            }
        }
    }
    void CreateGrabJoint(Vector3 GrabPos, Rigidbody GrabedRB){
    	VRRB.drag = 10f;
    	Vector3 LocalGrabPos;
    	GrabJoint = VrRig.AddComponent<SpringJoint>();
        if(GrabedRB){
            LocalGrabPos = GrabedRB.gameObject.transform.InverseTransformPoint(GrabPos); // Grab position in local coordinates of the grabed object
            GrabJoint.connectedBody = GrabedRB;
        }
        else{
            LocalGrabPos = GrabPos;
        }
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
    /*bool GetTriggerDown(){
    	bool trigger = false;
    	if(Controller == ControllerType.Left){
    		trigger = OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger, OVRInput.Controller.LTouch);
    	}
    	else if(Controller == ControllerType.Right){
    		trigger = OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger, OVRInput.Controller.RTouch);
    	}
    	return OVRInput.GetDown(GrabInput);   	
    }
    bool GetTriggerUp(){
    	bool trigger = false;
    	if(Controller == ControllerType.Left){
    		trigger = OVRInput.GetUp(OVRInput.RawButton.LIndexTrigger, OVRInput.Controller.LTouch);
    	}
    	else if(Controller == ControllerType.Right){
    		trigger = OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger, OVRInput.Controller.RTouch);
    	}
    	return OVRInput.GetUp(GrabInput);   	
    }*/
    void TrackJoint(SpringJoint TrackedJoint){
    	Vector3 controllerPos = LocalControllerPos();
    	TrackedJoint.anchor = controllerPos;
    }
}
