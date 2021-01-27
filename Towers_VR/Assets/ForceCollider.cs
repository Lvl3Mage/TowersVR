using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceCollider : MonoBehaviour
{
    [Header("Required References")]
    [Tooltip("The point where the collision vector is calculated from")]
	[SerializeField] Transform MinimumClimbPoint;
    [Tooltip("The player center. Determines the actual force of the collision")]
    [SerializeField] Transform Center;
    [Tooltip("The player rigidbody")]
    [SerializeField] Rigidbody RB;
    [Header("Per Collision Settings")]
    [Tooltip("The strength of each collision")]
	[SerializeField] float Strength;
    [Tooltip("Minimum horizontal force. Values below this one will not be applied")]
    [SerializeField] float MinimumSidePushback;
    [Tooltip("Horizontal and vertical force limitations. Values above these will be clamped")] 
    [SerializeField] Vector2 ImpulseLimits;
    [Tooltip("Horizontal and vertical velocity limitations. Values, application of which will result in a velocity higher than specified, will not be applied")]  
    [SerializeField] Vector2 VelLimits;
    [Tooltip("Specifies how closely does the current colision has to match the total collision vecotor to be ignored. 1 means it has to be identical where 0.5 means it can be up to 45 degrees different")]
    [SerializeField] [Range(1f,0.5f)] float SimillarCollisionCutoff;
	[Tooltip("The layers the player will collide with")] 
    [SerializeField] LayerMask Collidable;
    [Header("Total Impulse Settings")]
    [Tooltip("Horizontal and vertical force limitations. Values above these will be clamped")] 
    [SerializeField] Vector2 FrameImpulseLimits;
    Vector3 TotalForce = Vector3.zero; //the total force applied per frame
    void Start(){
        StartCoroutine(FrameImpulseTimer());
    }
    IEnumerator FrameImpulseTimer(){
        while (true) 
        {
            yield return new WaitForFixedUpdate(); // Will wait until after all the collisions have been calculated for this frame 
            ApplyFrameForce(); // applies the force for this       
        }
    }
    void ApplyFrameForce(){ // applies force calculated per physics update
        if(TotalForce != Vector3.zero){
            TotalForce = LimitImpulse(TotalForce, FrameImpulseLimits);
            RB.velocity += TotalForce;
            TotalForce = Vector3.zero;
        }
    }
    void OnTriggerStay(Collider other){
        if(other.isTrigger){// won't collide with triggers
            return;
        }
        if(!(Collidable == (Collidable | (1 << other.gameObject.layer)))){ // won't collide with layers marked as non collidable
            return;
        }
    	Vector3 CollisionPoint;
    	Vector3 CollisionVector;
    	if(other.attachedRigidbody){
    		CollisionPoint = other.ClosestPoint(MinimumClimbPoint.position);
    		CollisionVector = CollisionPoint - Center.position;
    	}
    	else{
            RaycastHit hit;
    		if(Physics.Raycast(Center.position, Vector3.down, out hit,Mathf.Infinity, Collidable,QueryTriggerInteraction.Ignore)){
    			CollisionPoint = hit.point;
    			Vector3 DirVectorDown = CollisionPoint - Center.position;
    			CollisionVector = (DirVectorDown.normalized - hit.normal)/2 * DirVectorDown.magnitude;
    			
    		}
    		else{ //The ClosestPoint function can't be used cause it doesn't work with mesh coliders so right now i'm just checking the distance downwards
    			CollisionPoint = Vector3.zero;
    			CollisionVector = Vector3.zero;
    			Debug.LogError("Fix this later : can't apply force because the object colliding has a mesh colider and isn't underneath the player");
    		}
    		
    	}
        CalculateCollisionForce(CollisionPoint,CollisionVector);
    }
    void CalculateCollisionForce(Vector3 CollisionPoint, Vector3 CollisionVector){ // Calculates the force for the specified collision
        //Ignores the side pushback force if it is lower than the mimimum value
        if(Mathf.Abs(CollisionVector.x)<MinimumSidePushback){
            CollisionVector.x = 0;
        }
        if(Mathf.Abs(CollisionVector.z)<MinimumSidePushback){
            CollisionVector.z = 0;
        }

        Debug.DrawRay(CollisionPoint, -CollisionVector.normalized, Color.green);

        Vector3 impulse = -(CollisionVector.normalized/CollisionVector.magnitude)*Strength; // the directional vector of a collision is devided by the distance of a collision and then scaled with the strength

        impulse = LimitImpulseByVelocity(impulse);
        impulse = LimitImpulse(impulse, ImpulseLimits);

        //Eliminates collisions that are identical
        float difference = Vector3.Dot(impulse.normalized, TotalForce.normalized);
        if(SimillarCollisionCutoff<difference){ // Checks if the current collision has to be cut off
            if(impulse.magnitude>TotalForce.magnitude){
                TotalForce = impulse; // If the active vector is the strongest the total force will be reset
                //Debug.Log("Reset Impulse");
            }
            else{
                impulse = Vector3.zero;
                //Debug.Log("Not Collided");
            }
        }
        else{
            TotalForce += impulse;
        }
        
    }

    Vector3 LimitImpulseByVelocity(Vector3 impulse){
        if(Mathf.Abs(RB.velocity.x+impulse.x)>Mathf.Abs(RB.velocity.x)){
            if(Mathf.Abs(RB.velocity.x)>VelLimits.x){
                impulse.x = 0;
            }   
        }
        if(Mathf.Abs(RB.velocity.y+impulse.y)>Mathf.Abs(RB.velocity.y)){
            if(Mathf.Abs(RB.velocity.y)>VelLimits.y){
                impulse.y = 0;
            }
        }
        if(Mathf.Abs(RB.velocity.z+impulse.z)>Mathf.Abs(RB.velocity.z)){
            if(Mathf.Abs(RB.velocity.z)>VelLimits.x){ // Vel limits is a vector 2 because the 3 component is unnecesary (1 & 3 are the same)
                impulse.z = 0;
            }
        }
        return impulse;
    }
    Vector3 LimitImpulse(Vector3 impulse, Vector2 Limiter){
        impulse.x = Limit(impulse.x,Limiter.x);
        impulse.y = Limit(impulse.y,Limiter.y);
        impulse.z = Limit(impulse.z,Limiter.x);
        return impulse;
    }
    float Limit(float value,float limiter){
    	if(value == 0){
    		return 0;
    	}
    	float sign = value/Mathf.Abs(value);
    	value = Mathf.Min(value*sign, limiter);
    	value *= sign;
    	return value;
    }
}
