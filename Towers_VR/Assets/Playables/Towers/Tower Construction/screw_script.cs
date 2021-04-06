using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screw_script : MonoBehaviour
{
	[SerializeField] bool DrawGizmos, EnableCol;
    [SerializeField] Transform Pointer;
	GameObject Followed, Follower;
	[SerializeField] float BreakForce, BreakTorque, GSize;
    
    void Start()
    {
        Followed = transform.parent.gameObject;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Pointer.position-transform.position, out hit, 0.75f)){
            if(hit.collider.gameObject != Followed){
                Follower = hit.collider.gameObject;
            }
        }

        if (!(Followed == null || Follower == null || Followed.GetComponent<Rigidbody>() == null || Follower.GetComponent<Rigidbody>() == null)){
            ConfigurableJoint CJ = Followed.AddComponent<ConfigurableJoint>();
            CJ.connectedBody = Follower.GetComponent<Rigidbody>();
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
            CJ.enableCollision = EnableCol;
        }
        Destroy(gameObject);
    }
    void OnDrawGizmos()
    {
    	if (Followed == null || Follower == null||Followed.GetComponent<Rigidbody>() == null || Follower.GetComponent<Rigidbody>() == null){
    		Gizmos.color = Color.red;
            if (Pointer){
                Gizmos.color = Color.yellow;
            }
    	}
    	else {
    		Gizmos.color = Color.green;
    	}
    	
    	if(DrawGizmos){
    		Gizmos.DrawSphere(transform.position, GSize);
            Gizmos.DrawSphere(Pointer.position, GSize/2f);
    	}
    }
}
