﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCPlayerMovement : MonoBehaviour
{
	public class CollidedObject
	{
		public Collider Collider;
		public GameObject Object;
		public Rigidbody Rigidbody;
		/*public CollidedObject(Collider _Collider, GameObject _Object, Rigidbody _Rigidbody){ // a separate class is needed
		Collider = _Collider;
		Rigidbody = _Rigidbody;
		}*/
	}
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
    
    [Header("Ladder Movement Settings")]
    [SerializeField] float LadderMoveSpeed;
    [SerializeField] float LadderLerpSpeed;

    [Header("Reference Requirements")]
    [SerializeField] Rigidbody Player;
    [SerializeField] Transform CamDirection, MovementCenter,RaycastCenter,Camera;
    [SerializeField] LayerMask Walkable;
    [SerializeField] Collider PlayerPhysicalCollider;
    Collider Ladder;
    Vector3 Axis;
    bool Jump, JumpReady = true;
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
    void Update(){

        if(Ladder){ // Checks if the ladder collider is empty, if it isn't then it applies ladder motion, otherwise it applies normal motion
        	ApplyLadderMovement(Ladder); 
        }
        else{
        	ApplyMovement();	
        }

    }
    public void UseLadder(Collider LadderCol){ // a function that makes the player use a specified ladder or stops the player from using one
    	if(LadderCol == Ladder){// checks if the interaction script has passed the same value meaning that you have to stop using the ladder
    		Ladder = null;
    		ToggleRigidbody(true);
    	}
    	else{
	    	Ladder = LadderCol;
	    	ToggleRigidbody(false);    		
    	}

    }
    void ApplyLadderMovement(Collider Ladder){
    	if(Input.GetKeyDown(KeyCode.Space)){
    		ToggleRigidbody(true);
		    Player.velocity += Camera.TransformDirection(Vector3.forward*5); // Jumps from ladder
		    Ladder = null; // nulifies the ladder 
    	}
    	else{
		    
	    	float MovementAxis = 0;
	        if(Input.GetKey(KeyCode.W)){
	            MovementAxis = 1;
	        }
	        else if (Input.GetKey(KeyCode.S)) {
	            MovementAxis = -1;
	        }
	        Debug.Log(Player.velocity);
	        Transform LadderTransform = Ladder.gameObject.transform;
	        Transform PlayerTransform = Player.gameObject.transform;
	        Vector3 LerpTarget = LadderTransform.TransformPoint(Vector3.down/3); // sets the horizontal position relative to the ladder
	        LerpTarget.y = PlayerTransform.position.y + MovementAxis*LadderMoveSpeed;
	        PlayerTransform.position = Vector3.Lerp(PlayerTransform.position,LerpTarget,Time.deltaTime * LadderLerpSpeed); // actual position set
    	}
    	
    }
    void ToggleRigidbody(bool val){
    	Player.isKinematic = !val;
    	PlayerPhysicalCollider.enabled = val;
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
                Player.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
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
                Player.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
            }
            if(WalkEase>WalkSteepnessMax){
				ApplyHorizontalVel(WalkEase,StoodOnRB);
            }
            if(Jump){ // Essentialy happens on the frame the jump occurs
                Player.velocity += (StandVector.normalized * DirectionalJump + Vector3.up * (1-DirectionalJump))*JumpSpeed; //Makes a blend between the standing vector and the up vector
                Jump = false;
                StartCoroutine(JumpReset());
            }
            
        }
        else{
            Player.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
            Player.angularVelocity = Vector3.zero;
            Vector3 Acceleration = Axis*AxisSpeed*AirAccelerate*Time.deltaTime;
            Vector2 FlatAcceleration = new Vector2(Acceleration.x,Acceleration.z);
            Vector2 FlatSpeed = new Vector2(Player.velocity.x,Player.velocity.z);
            //Checks if the acceleration will result in movement slower than the maximum or at least slow down the player. if so then it accelerates.
            if((FlatSpeed + FlatAcceleration).magnitude < AirSpeed || (FlatSpeed + FlatAcceleration).magnitude < FlatSpeed.magnitude){
            	Player.velocity += Acceleration;
            }    
        }
    }
    void ApplyHorizontalVel(float WalkEase, Rigidbody StoodOnRB){
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
                    Debug.Log("Removed " + CurrentCollisions[i].gameObject.name);
                    CurrentCollisions.RemoveAt(i);
                }
            }
        }
          
    }
    void SetAxis(){
        Vector2 LocAxis;
        if(Input.GetKey(KeyCode.W)){
            LocAxis.y = 1;
        }
        else if (Input.GetKey(KeyCode.S)) {
            LocAxis.y = -1;
        }
        else{
            LocAxis.y = 0;
        }

        if(Input.GetKey(KeyCode.D)){
            LocAxis.x = 1;
        }
        else if (Input.GetKey(KeyCode.A)) {
            LocAxis.x = -1;
        }
        else{
            LocAxis.x = 0;
        }
        Axis = CamDirection.TransformDirection(LocAxis.x,0,LocAxis.y);
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
        if(other == Ladder){ // in case the player exited the ladder we enable the rigidbody and remove the ladder collider
        	ToggleRigidbody(true);
        	Ladder = null;
        }
        else{
	        for (int i = 0; i < CurrentCollisions.Count; i++) 
	        {
	            if(CurrentCollisions[i] == other){
	                CurrentCollisions.RemoveAt(i);
	                Debug.Log("Removed " + other.gameObject.name);
	            }
	        }        	
        }

    }
}



    /*Rigidbody RB;
    public float Walkspeed, Runspeed, JumpSpeed;
    public Transform CamDirection;
    int ColCount;
    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 Axis = GetAxis();
        if(Input.GetKey(KeyCode.LeftShift)){
            Axis*=Runspeed;
        }
        else{
            Axis*=Walkspeed;
        }
        
        Vector3 Axis3D = new Vector3(Axis.x,RB.velocity.y,Axis.y);
        Axis3D = CamDirection.TransformDirection(Axis3D);
        if(Input.GetKeyDown(KeyCode.Space) && ColCount>0){
            Axis3D.y += JumpSpeed;
        }
        RB.velocity = Axis3D;
    }
    Vector2 GetAxis(){
        Vector2 Axis = new Vector2(0,0);
        if(Input.GetKey(KeyCode.W)){
            Axis.y = 1;
        }
        else if (Input.GetKey(KeyCode.S)){
            Axis.y = -1;
        }
        if(Input.GetKey(KeyCode.A)){
            Axis.x = -1;
        }
        else if (Input.GetKey(KeyCode.D)){
            Axis.x = 1;
        }
        return Axis;
    }
    void OnCollisionEnter(Collision collisionInfo){
        ColCount ++;
    }
    void OnCollisionExit(Collision collisionInfo){
        ColCount --;
    }*/