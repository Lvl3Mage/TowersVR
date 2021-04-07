using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRForceCollider : MonoBehaviour
{
	[SerializeField] Transform MinimumClimbPoint, Center;
	[SerializeField] float Strength, MinimumSidePushback;
	[SerializeField] Vector2 VelLimits,ImpulseLimits;
	[SerializeField] Rigidbody RB;
	[SerializeField] LayerMask Collidable;
	void Update(){

	}
    void OnTriggerStay(Collider other){
        if(other.isTrigger){
            return;
        }
        if(!(Collidable == (Collidable | (1 << other.gameObject.layer)))){
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
		ApplyCollisionForce(CollisionPoint,CollisionVector);    		

    }
    void ApplyCollisionForce(Vector3 CollisionPoint, Vector3 CollisionVector){
        if(Mathf.Abs(CollisionVector.x)<MinimumSidePushback){
            CollisionVector.x = 0;
        }
        if(Mathf.Abs(CollisionVector.z)<MinimumSidePushback){
            CollisionVector.z = 0;
        }
        Vector3 CollisionDir = CollisionVector.normalized;
        float CollisionMag = CollisionVector.magnitude;
        //Debug.Log(CollisionMag);
        Debug.DrawRay(CollisionPoint, -CollisionDir, Color.green);
        Vector3 impulse = -(CollisionDir/CollisionMag)*Strength; // the directional vector of a collision is devided by the distance of a clooision and then scaled with strength

        impulse = LimitImpulseByVelocity(impulse);

        impulse.x = Limit(impulse.x,ImpulseLimits.x);
        impulse.y = Limit(impulse.y,ImpulseLimits.y);
        impulse.z = Limit(impulse.z,ImpulseLimits.x);
        RB.velocity += impulse;
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
