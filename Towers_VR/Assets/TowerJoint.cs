using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerJoint : MonoBehaviour
{
	[Header("Joint Settings")]

	[Tooltip("The pointer towards the direction which the joint should form towards")]
	[SerializeField] Transform Pointer;

	[Tooltip("Enables collision between connected objects")]
	[SerializeField] bool EnableObjectCollision;

	[Tooltip("The force after which the joint should break")]
	[SerializeField] float BreakForce;

	[Tooltip("The torque after which the joint should break")]
	[SerializeField] float  BreakTorque;

	[SerializeField] float MaxDistance = 0.75f;


	[Header("Gizmo Settings")]
	[SerializeField] bool DrawGizmos;
	[SerializeField] float GizmoSize;
	[SerializeField] Color MainColor;
	[SerializeField] Color PointerColor;
    
    void Start()
    {
        GameObject Followed = transform.parent.gameObject;
        Rigidbody Follower = null;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Pointer.position-transform.position, out hit, MaxDistance)){
            if(hit.collider.gameObject != Followed){
                Follower = hit.collider.attachedRigidbody;
            }
        }
        if(Followed && Follower){ // Checks if the conncetion objects exist
        	Rigidbody followedRB = Followed.GetComponent<Rigidbody>();
        	if(followedRB && Follower){
	        	ConfigurableJoint CJ = Followed.AddComponent<ConfigurableJoint>();
	            CJ.connectedBody = Follower;
	            CJ.autoConfigureConnectedAnchor = false;
	            CJ.anchor = Followed.transform.InverseTransformPoint(transform.position);
	            CJ.connectedAnchor = Follower.transform.InverseTransformPoint(transform.position);


	            CJ.xMotion = ConfigurableJointMotion.Limited;
	            CJ.yMotion = ConfigurableJointMotion.Limited;
	            CJ.zMotion = ConfigurableJointMotion.Limited;
	            CJ.angularXMotion = ConfigurableJointMotion.Limited;
	            CJ.angularYMotion = ConfigurableJointMotion.Limited;
	            CJ.angularZMotion = ConfigurableJointMotion.Limited;


	            CJ.breakForce = BreakForce;
	            CJ.breakTorque = BreakTorque;
	            CJ.enableCollision = EnableObjectCollision;
        	}
        }
        Destroy(gameObject);
    }
    void OnDrawGizmos()
    {
    	if(DrawGizmos){
    		Gizmos.color = MainColor;
    		Gizmos.DrawSphere(transform.position, GizmoSize);
    		Gizmos.color = PointerColor;
            Gizmos.DrawSphere(Pointer.position, GizmoSize/2f);
    	}
    }
}
