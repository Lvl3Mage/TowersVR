using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	Rigidbody RB;
	[SerializeField] bool RotateToVelocity;
    [SerializeField] [Range(0f,10f)] protected float _WindFactor;
    public float WindFactor{
        get{
            return _WindFactor;
        }
    }
    public bool HasCollided(){
        return Impact;
    }
    bool Impact = false;
    // Start is called before the first frame update
    void Start()
    {
    	RB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    	if(RotateToVelocity){
    		transform.rotation = Quaternion.LookRotation(RB.velocity, Vector3.up);
    	}   
    }
    void OnCollisionEnter(Collision collisionInfo){
        Activate(collisionInfo);
        Impact = true;
    }
    protected virtual void Activate(Collision collisionInfo){

    }
}
