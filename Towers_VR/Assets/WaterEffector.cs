using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEffector : MonoBehaviour
{
	[SerializeField] LayerMask AffectedLayers;
    [Tooltip("The number iterations after which the lowest point is recalculated")]
    [SerializeField] int LowestPointSkipIterations;
    [SerializeField] float UnderwaterDrag;
    [SerializeField] float UnderwaterAngularDrag;
    [SerializeField] [Range(0f,1f)] float rotationSmoothness;
    [Tooltip("The buoyancy force on the surface")]
    [SerializeField] float SurfaceBuoyancyForce;
    [Tooltip("The local surface height in local coordinates")]
    [SerializeField] float LocalSurfaceHeight;
    [Tooltip("The maximum buoyancy force that can be applied")]
    [SerializeField] float MaximumBuoyancyForce;
    [Tooltip("The depth at which the maximum buoyancy force will be reached")]
    [SerializeField] float MaximumForceDepth;

	List<AffectedBody> affectedBodies = new List<AffectedBody>();

    void OnTriggerEnter(Collider other){
    	Rigidbody affectedRigidbody = other.attachedRigidbody;
		if(affectedRigidbody){ // if collided with a rigidbody
			if(AffectedLayers == (AffectedLayers | (1 << affectedRigidbody.gameObject.layer))){ // if it's in the affected layermask

				AddCollider(other);
			}
		}
    }
    void OnTriggerExit(Collider other){
        Debug.Log("Exit");
    	RemoveCollider(other);
    }
    void AddCollider(Collider col){
    	Rigidbody affectedRigidbody = col.attachedRigidbody;
    	AffectedBody collidedBody = null;
    	for(int i = 0; i < affectedBodies.Count; i++){ // tries to find the collider's body
    		if(affectedRigidbody == affectedBodies[i].body){
    			collidedBody = affectedBodies[i];
    			break;
    		}
    	}
    	if(collidedBody != null){ // if the rigidbody has been found
    		collidedBody.affectedColliders.Add(col); // simply adds the collider to it
    	}
    	else{ // if it hasn't
    		AffectedBody newBody = new AffectedBody(affectedRigidbody,affectedRigidbody.drag,affectedRigidbody.angularDrag,LowestPointSkipIterations); // creates a new one and remembers the drag of the rigidbody
    		SetUpDrag(newBody); // sets the bodies' drag properties
            newBody.affectedColliders.Add(col); // adds the collider to it
    		affectedBodies.Add(newBody); // adds it to the affected bodies list
    	}
    }
    void RemoveCollider(Collider col){
    	Rigidbody affectedRigidbody = col.attachedRigidbody;
    	for(int i = 0; i < affectedBodies.Count; i++){ // runs through every affected body
    		if(affectedRigidbody == affectedBodies[i].body){ // if the needed one has been found
    			AffectedBody body = affectedBodies[i];
    			for(int j = 0; j < body.affectedColliders.Count; j++){ // runs through every collider in the found rigidbody
    				if(body.affectedColliders[j] == col){ // if the needed one has been found
    					body.affectedColliders.RemoveAt(j); // removes it
    					break;
    				}
    			}
    			if(body.affectedColliders.Count == 0){ // if no colliders are left
                    RestoreDrag(body); // restores the bodies drag to it's original state
    				affectedBodies.RemoveAt(i); // removes the body 
    			}
    			break; // the needed rigidbody has been found so we can break
    		}
    	}
    }
    void FixedUpdate()
    {
        float surfaceHeight = transform.position.y + LocalSurfaceHeight;
        for(int i = 0; i < affectedBodies.Count; i++){
            Vector3 LowestPoint = affectedBodies[i].GetLowestPoint(); // calculates relative force point
            Rigidbody AffectedRigidbody = affectedBodies[i].body;
            Vector3 AffectedBodyPosition = AffectedRigidbody.gameObject.transform.position;
            LowestPoint = Vector3.Lerp(LowestPoint,AffectedBodyPosition,rotationSmoothness);
            float surfaceDistance = surfaceHeight-AffectedBodyPosition.y;
            float AppliedForce = CalculateBuoyancyForce(surfaceDistance,SurfaceBuoyancyForce,MaximumBuoyancyForce,MaximumForceDepth)*Time.deltaTime; // calculates teh force
            AffectedRigidbody.AddForceAtPosition(new Vector3(0,AppliedForce,0),LowestPoint,ForceMode.VelocityChange); // applies the force
        }
    }
    float CalculateBuoyancyForce(float relativeHeight, float baseForce, float maxForce, float maxForceDepth){
        float force = relativeHeight*(maxForce-baseForce)/maxForceDepth + baseForce; // resolves a linear equation for the force increment
        if(force < baseForce){
            force = 0;
        }
        force = Mathf.Clamp(force,0,maxForce); // clamps the force between 0 and the maximum force
        return force;
    }

    void SetUpDrag(AffectedBody affectedBody){
        Rigidbody body = affectedBody.body;
        body.drag = UnderwaterDrag;
        body.angularDrag = UnderwaterAngularDrag;
    }
    void RestoreDrag(AffectedBody affectedBody){
        Rigidbody body = affectedBody.body;
        body.drag = affectedBody.originalDrag;
        body.angularDrag = affectedBody.originalAngularDrag;
    }
}
[System.Serializable]
class AffectedBody{
    int LowestPointSkipIterations;
    int skippedIterations;
    Vector3 LowestPoint;
    public float originalDrag;
    public float originalAngularDrag;
	public Rigidbody body;
	public List<Collider> affectedColliders;
	public AffectedBody(Rigidbody _body,float drag, float angDrag, int skipIterations){
        LowestPointSkipIterations = skipIterations;
        skippedIterations = skipIterations; // sets the affected body to update the lowest point on the first iteration
        originalDrag = drag;
        originalAngularDrag = angDrag;
		body =_body;
		affectedColliders = new List<Collider>();
	}
    public Vector3 GetLowestPoint(){
        if(skippedIterations >= LowestPointSkipIterations){ // in case the update time has been reached
            skippedIterations = 0;
            LowestPoint = FindLowestPoint(); // updates the lowest point
        }
        else{
            skippedIterations ++;
        }
        return LowestPoint;
    }
    Vector3 FindLowestPoint(){
        List<Collider> colliders = affectedColliders;
        Vector3 LowestPoint = new Vector3(0,Mathf.Infinity,0);
        for(int i = 0; i < colliders.Count; i++){
            Vector3 colliderPos = colliders[i].gameObject.transform.position;
            colliderPos -= new Vector3(0,20,0); // lowering the collider position to -20 (the closest the point to the collider would be th lowest one form here)
            Vector3 LowestColliderPoint = colliders[i].ClosestPoint(colliderPos);
            if(LowestColliderPoint.y <= LowestPoint.y){
                LowestPoint = LowestColliderPoint;
            }
        }
        return LowestPoint;
    }
}
