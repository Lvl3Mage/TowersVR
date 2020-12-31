using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayerMovement : MonoBehaviour
{
	[SerializeField] Rigidbody VrRig;
	[SerializeField] Transform CameraTracker;
	[SerializeField] LayerMask Walkable;
	[SerializeField] Transform MovementCenter,RaycastCenter;
	[Header("Normal Movement Settings")]
	[SerializeField] float AxisSpeed;
    [Range(0f, 10f)]  [SerializeField] float Acceleration;
    [Range(0f, 10f)]  [SerializeField] float AirAccelerate;
	[SerializeField] float AirSpeed;
    [Range(2f, 10f)]  [SerializeField] float WalkDifficulty;
	[SerializeField] float JumpSpeed; 
    [Range(0f, 1f)]  [SerializeField] float DirectionalJump;
    [Range(0f, 1f)]  [SerializeField] float WalkSteepnessMax;
	Vector3 Axis;
	List<Collider> CurrentCollisions = new List<Collider>();
    bool Jump, JumpReady = true;
	void Update(){
		ApplyMovement();

	}
	int SelectStoodOnObject(){ // Selects the collider with the smallest normal vector difference
    	float MaxAngle = Mathf.Infinity;
    	int Saved_i = 0;
    	for (int i = 0; i<CurrentCollisions.Count; i++) 
    	{
    		Vector3 StandVector = (MovementCenter.position - CurrentCollisions[0].ClosestPoint(MovementCenter.position));
            float Angle = Vector3.Angle(Vector3.up, StandVector.normalized);
            Debug.DrawRay(MovementCenter.position, -StandVector, Color.yellow);
            if(Angle<MaxAngle){
            	MaxAngle = Angle;
            	Saved_i = i;
            }
    	}
    	return Saved_i;
    }
    void ApplyMovement(){
    	SetAxis();
        ClearArrayGarbage();

    	if(Input.GetKeyDown(KeyCode.Space) && JumpReady){
            Jump = true;
            JumpReady = false;
        }

        if(CurrentCollisions.Count>0){
        	Collider StoodOnCol = CurrentCollisions[SelectStoodOnObject()];
            Rigidbody StoodOnRB = StoodOnCol.attachedRigidbody;
            float WalkEase;
            Vector3 StandVector;
            if(StoodOnRB && !StoodOnRB.isKinematic){
                StandVector = (MovementCenter.position - StoodOnCol.ClosestPoint(MovementCenter.position));
                WalkEase = Mathf.Pow(Vector3.Dot(Vector3.up, StandVector.normalized),WalkDifficulty);

                Debug.DrawRay(MovementCenter.position, -StandVector, Color.red);
                //VrRig.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
                //VrRig.angularVelocity = StoodOnRB.angularVelocity;
            }
            else{
                RaycastHit hit;
                WalkEase = 1;
                StandVector = Vector3.up;
                if(Physics.Raycast(RaycastCenter.position, Vector3.down, out hit,Mathf.Infinity, Walkable, QueryTriggerInteraction.Ignore)){
                    StandVector = hit.normal;
                    WalkEase = Mathf.Pow(Vector3.Dot(Vector3.up, hit.normal),WalkDifficulty);
                }
                //VrRig.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
            }
            if(WalkEase>WalkSteepnessMax){
				ApplyHorizontalVel(WalkEase,StoodOnRB);
            }
            if(Jump){ // Essentialy happens on the frame the jump occurs
                VrRig.velocity += (StandVector.normalized * DirectionalJump + Vector3.up * (1-DirectionalJump))*JumpSpeed; //Makes a blend between the standing vector and the up vector
                Jump = false;
                StartCoroutine(JumpReset());
            }
            
        }
        else{
            VrRig.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
            VrRig.angularVelocity = Vector3.zero;
            Vector3 Acceleration = Axis*AxisSpeed*AirAccelerate*Time.deltaTime;
            Vector2 FlatAcceleration = new Vector2(Acceleration.x,Acceleration.z);
            Vector2 FlatSpeed = new Vector2(VrRig.velocity.x,VrRig.velocity.z);
            //Checks if the acceleration will result in movement slower than the maximum or at least slow down the VrRig. if so then it accelerates.
            if((FlatSpeed + FlatAcceleration).magnitude < AirSpeed || (FlatSpeed + FlatAcceleration).magnitude < FlatSpeed.magnitude){
            	VrRig.velocity += Acceleration;
            }    
        }
    }
    void ApplyHorizontalVel(float WalkEase, Rigidbody StoodOnRB){
        float YVel = VrRig.velocity.y;
        Vector3 BaseSpeed = Vector3.zero;
        if(StoodOnRB){
        	BaseSpeed = StoodOnRB.velocity;
        }
        VrRig.velocity = Vector3.Lerp(VrRig.velocity, BaseSpeed + Axis*AxisSpeed, (Acceleration*Time.deltaTime*WalkEase));
        if(!JumpReady){
            VrRig.velocity = new Vector3(VrRig.velocity.x, YVel, VrRig.velocity.z); // A bit of a crude fix but what I essentialy do is ovveride the lerp of the y velocity if the VrRig is jumping (could be done better)
        }
    }

    IEnumerator JumpReset(){
        yield return new WaitForSeconds(0.1f); // this stops the VrRig sticking to surfaces for 0.1s as well as not letting the VrRig jump without stoping
        JumpReady = true;
    }
	void ClearArrayGarbage(){
        
        if(CurrentCollisions.Count>0){
            for(int i = 0;i<CurrentCollisions.Count;i++)
            {
                if(!CurrentCollisions[i].gameObject.activeSelf){
                    Debug.Log("Removed " + CurrentCollisions[i].gameObject.name);
                    CurrentCollisions.RemoveAt(i);
                }
            }
        }
          
    }
    void SetAxis(){
        Vector2 LocAxis = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
        Axis = CameraTracker.TransformDirection(LocAxis.x,0,LocAxis.y);
    }
    void OnTriggerEnter(Collider other){
    	if(!(Walkable == (Walkable | (1 << other.gameObject.layer)))){
    		return;
    	}
    	if(other.isTrigger){
    		return;
    	}

        bool HasCollided = false; // Checks if the object has colided with this collider previously
        for (int i = 0; i < CurrentCollisions.Count && !HasCollided; i++) 
        {
            if(CurrentCollisions[i] == other){
                HasCollided = true;
            }
        }
        if(!HasCollided){
            CurrentCollisions.Add(other);
            Debug.Log("Added " + other.gameObject.name);
        }
    }
    void OnTriggerExit(Collider other){
        for (int i = 0; i < CurrentCollisions.Count; i++) 
        {
            if(CurrentCollisions[i] == other){
                CurrentCollisions.RemoveAt(i);
                Debug.Log("Removed " + other.gameObject.name);
            }
        }        	
    }
}
