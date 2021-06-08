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
        GameObject Followed = transform.parent.gameObject; //Main Object
        RaycastHit RaycastHit;
        bool Hit;
        if(Physics.Raycast(transform.position, Pointer.position-transform.position, out RaycastHit, MaxDistance)){
        	Rigidbody Follower = RaycastHit.collider.attachedRigidbody;

		    if(Followed){ // Checks if the base conncetion object exist
		    	Rigidbody followedRB = Followed.GetComponent<Rigidbody>();
		    	if(followedRB){ // checks if it has a rigidbody
					ConfigurableJoint CJ = Followed.AddComponent<ConfigurableJoint>();
					CJ.autoConfigureConnectedAnchor = false;

					if(Follower){
						CJ.connectedBody = Follower;
						CJ.connectedAnchor = Follower.transform.InverseTransformPoint(transform.position);
					}
					else{
						CJ.connectedAnchor = transform.position;
					}

					CJ.anchor = Followed.transform.InverseTransformPoint(transform.position);
		            


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
