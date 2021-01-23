using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	public class CollidedObject
	{
		public Collider Collider;
		public GameObject Object;
		public Rigidbody Rigidbody;
	}
	bool JumpReady = true;
    List<Collider> CurrentCollisions = new List<Collider>();
    [Header("Normal Movement Settings")]
	[SerializeField] float AxisSpeed;
    [Range(0f, 10f)]  [SerializeField] float Acceleration;
    [Range(0f, 10f)]  [SerializeField] float AirAccelerate;
	[SerializeField] float AirSpeed;
    [Range(2f, 10f)]  [SerializeField] float WalkDifficulty;
	[SerializeField] float JumpSpeed; 
    [Range(0f, 1f)]  [SerializeField] float DirectionalJump;
    [Range(0f, 1f)]  [SerializeField] float WalkSteepnessMax;
    [Header("Reference Requirements")]
    [SerializeField] protected Rigidbody Player;
    [SerializeField] Transform MovementCenter,RaycastCenter;
    [SerializeField] protected LayerMask Walkable;
	int SelectStoodOnObject(){ // Selects the collider with the smallest normal vector difference
    	float MaxAngle = -Mathf.Infinity;
    	int Saved_i = 0;
    	for (int i = 0; i<CurrentCollisions.Count; i++) 
    	{
    		Vector3 StandVector = (MovementCenter.position - CurrentCollisions[i].ClosestPoint(MovementCenter.position));
            float Angle = Vector3.Dot(Vector3.up, StandVector.normalized);
            Debug.DrawRay(MovementCenter.position, -StandVector, Color.yellow);
            if(Angle>MaxAngle){
            	MaxAngle = Angle;
            	Saved_i = i;
            }
    	}
    	return Saved_i;
    }
    protected void ApplyMovement(bool JumpInput,Vector3 Axis, bool ModifyConstraints){
        ClearArrayGarbage();
        if(CurrentCollisions.Count>0){
        	bool JumpAction = false;
	    	if(JumpInput && JumpReady){
	            JumpAction = true;
	            JumpReady = false;
	        }
        	Collider StoodOnCol = CurrentCollisions[SelectStoodOnObject()];
            Rigidbody StoodOnRB = StoodOnCol.attachedRigidbody;
            float WalkEase;
            Vector3 StandVector;
            if(StoodOnRB && !StoodOnRB.isKinematic){
                StandVector = (MovementCenter.position - StoodOnCol.ClosestPoint(MovementCenter.position));
                WalkEase = Mathf.Pow(Vector3.Dot(Vector3.up, StandVector.normalized),WalkDifficulty);

                Debug.DrawRay(MovementCenter.position, -StandVector, Color.red);
                if(ModifyConstraints){
                	Player.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;         	
                }
                Player.angularVelocity = StoodOnRB.angularVelocity;
            }
            else{
                RaycastHit hit;
                WalkEase = 1;
                StandVector = Vector3.up;
                if(Physics.Raycast(RaycastCenter.position, Vector3.down, out hit,Mathf.Infinity, Walkable, QueryTriggerInteraction.Ignore)){
                    StandVector = hit.normal;
                    WalkEase = Mathf.Pow(Vector3.Dot(Vector3.up, hit.normal),WalkDifficulty);
                }
                if(ModifyConstraints){
	                Player.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
                }
            }
            if(WalkEase>WalkSteepnessMax){
				ApplyHorizontalVel(WalkEase,StoodOnRB,Axis);
            }
            if(JumpAction){ // Essentialy happens on the frame the jump occurs
                Player.velocity += (StandVector.normalized * DirectionalJump + Vector3.up * (1-DirectionalJump))*JumpSpeed; //Makes a blend between the standing vector and the up vector
                JumpAction = false;
                StartCoroutine(JumpReset());
            }
            
        }
        else{
        	if(ModifyConstraints){
	            Player.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
	            Player.angularVelocity = Vector3.zero;        		
        	}
            Vector3 Acceleration = Axis*AxisSpeed*AirAccelerate*Time.deltaTime;
            Vector2 FlatAcceleration = new Vector2(Acceleration.x,Acceleration.z);
            Vector2 FlatSpeed = new Vector2(Player.velocity.x,Player.velocity.z);
            //Checks if the acceleration will result in movement slower than the maximum or at least slow down the player. if so then it accelerates.
            if((FlatSpeed + FlatAcceleration).magnitude < AirSpeed || (FlatSpeed + FlatAcceleration).magnitude < FlatSpeed.magnitude){
            	Player.velocity += Acceleration;
            }    
        }
    }
    void ApplyHorizontalVel(float WalkEase, Rigidbody StoodOnRB, Vector3 Axis){
        float YVel = Player.velocity.y;
        Vector3 BaseSpeed = Vector3.zero;
        if(StoodOnRB){
        	BaseSpeed = StoodOnRB.velocity;
        }
        Player.velocity = Vector3.Lerp(Player.velocity, BaseSpeed + Axis*AxisSpeed, (Acceleration*Time.deltaTime*WalkEase));
        if(!JumpReady){
            Player.velocity = new Vector3(Player.velocity.x, YVel, Player.velocity.z); // A bit of a crude fix but what I essentialy do is ovveride the lerp of the y velocity if the player is jumping (could be done better)
        }
    }
    IEnumerator JumpReset(){
        yield return new WaitForSeconds(0.1f); // this stops the player sticking to surfaces for 0.1s as well as not letting the player jump without stoping
        JumpReady = true;
    }
    void ClearArrayGarbage(){
        if(CurrentCollisions.Count>0){
            for(int i = 0;i<CurrentCollisions.Count;i++)
            {
                if(!CurrentCollisions[i].gameObject.activeSelf){
                    //Debug.Log("Removed " + CurrentCollisions[i].gameObject.name);
                    CurrentCollisions.RemoveAt(i);
                }
            }
        }
    }
    protected void AddToArray(Collider other){
    	bool HasCollided = false; // Checks if the object has colided with this collider previously
        for (int i = 0; i < CurrentCollisions.Count && !HasCollided; i++) 
        {
            if(CurrentCollisions[i] == other){
                HasCollided = true;
            }
        }
        if(!HasCollided){
            CurrentCollisions.Add(other);
            //Debug.Log("Added " + other.gameObject.name);
        }
    }
    protected void RemoveFromArray(Collider other){
    	for (int i = 0; i < CurrentCollisions.Count; i++) 
	    {
            if(CurrentCollisions[i] == other){
                CurrentCollisions.RemoveAt(i);
                //Debug.Log("Removed " + other.gameObject.name);
            }
	    } 
    }
}
