using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCPlayerMovement : Movement
{
    [Header("Ladder Movement Settings")]
    [SerializeField] float LadderMoveSpeed;
    [SerializeField] float LadderLerpSpeed;
    [Tooltip("The downwards oriented cone angle where the player has to point to invert the ladder movement")]
    [SerializeField] [Range(0f,90f)] float InverseMovementConeAngle;

    [Header("Reference Requirements")]
    [SerializeField] Transform CamDirection;
    [SerializeField] Transform Camera;
    [SerializeField] Collider PlayerPhysicalCollider;
    [Header("Controlls")]
    [SerializeField] KeyCode Front;
    [SerializeField] KeyCode Back;
    [SerializeField] KeyCode Right;
    [SerializeField] KeyCode Left;
    [SerializeField] KeyCode Jump;
    [SerializeField] KeyCode Reset;
    Collider Ladder;
    Vector3 PastLadderPos;
    bool RigidbodyToggled;
    void Update(){
        if(Input.GetKeyDown(Reset)){
            if(!Ladder){
                Player.gameObject.transform.position = PlayerSpawnPoint.position;
            }
        }
        if(Ladder){ // Checks if the ladder collider is empty, if it isn't then it applies ladder motion, otherwise it applies normal motion
        	ApplyLadderMovement(Ladder); 
        }
        else{
        	ApplyMovement(Input.GetKeyDown(Jump), GetAxis(),true);	
        }

    }
    public void UseLadder(Collider LadderCol){ // a function that makes the player use a specified ladder or stops the player from using one
    	if(LadderCol == Ladder){// checks if the interaction script has passed the same value meaning that you have to stop using the ladder
    		Debug.Log("Player chose to get off " + LadderCol.gameObject.name + " by pressing f");
            ClearLadder();
    		ToggleRigidbody(true);

    	}
    	else{
	    	SetLadder(LadderCol);
	    	ToggleRigidbody(false);    		
    	}

    }
    void ApplyLadderMovement(Collider Ladder){
    	if(Input.GetKeyDown(Jump)){
            Debug.Log("Player chose to get off " + Ladder.gameObject.name + " by pressing space");
    		ToggleRigidbody(true);
		    Player.velocity += Camera.TransformDirection(Vector3.forward*5); // Jumps from ladder
		    ClearLadder();
    	}
    	else{
		    
	    	float MovementAxis = 0;
	        if(Input.GetKey(Front)){
	            MovementAxis = 1;
	        }
	        else if (Input.GetKey(Back)) {
	            MovementAxis = -1;
	        }
	        Transform LadderTransform = Ladder.gameObject.transform; // this is supposed to be the climb area transform and not the ladder transform
	        Transform PlayerTransform = Player.gameObject.transform;

            float LadderAngle = GetAngleToLadderClimbDirection(LadderTransform);
            if(LadderAngle>(180-InverseMovementConeAngle)){ // if the camera is pointing against the ladder climb direction
                MovementAxis *= -1; // inverts movement
            }


            // Use Vector3.Up to add an offset to the ladder. I chose to use the position of the climb area for this
            Vector3 RelativeLerpTarget = Vector3.forward * (LadderTransform.InverseTransformPoint(PlayerTransform.position).z + MovementAxis*LadderMoveSpeed);
	        Vector3 LerpTarget = LadderTransform.TransformPoint(RelativeLerpTarget); // sets the horizontal position relative to the ladder
            Vector3 LadderDelta = PastLadderPos - LadderTransform.position; // A vector that defines ladder movement (it is then added to the player position)
	        PlayerTransform.position = Vector3.Lerp(PlayerTransform.position,LerpTarget,Time.deltaTime * LadderLerpSpeed) - LadderDelta ; // actual position set
            
            PastLadderPos = LadderTransform.position; // Sets the ladder past position for calculating next frame delta
        }
    	
    }
    float GetAngleToLadderClimbDirection(Transform Ladder){
        Vector3 PlayerLookDirection = Camera.forward; // Camera rotation vector

        Vector3 LadderLookDirection = Ladder.TransformDirection(Vector3.forward); // The climb direction of the ladder
        float LookAngle = Vector3.Angle(PlayerLookDirection, LadderLookDirection);// teh angle between the climb direction of the ladder and the player look direction
        return LookAngle;
    }
    void SetLadder(Collider LadderCol){
        Debug.Log("New Ladder Set - " + LadderCol.gameObject.name);
        Ladder = LadderCol;
        PastLadderPos = Ladder.gameObject.transform.position;
    }
    void ClearLadder(){  // Clears The ladder 
        Debug.Log("Ladder Cleared");
        Ladder = null;
        PastLadderPos = Vector3.zero; // clears the past ladder position so that it doesn't infuence next ladder delta 
    }
    void ToggleRigidbody(bool val){
    	Player.isKinematic = !val;
    	PlayerPhysicalCollider.enabled = val;
        RigidbodyToggled = true; // the OnTriggerExit is called after kinematic is changed so I try to ignore those calls with this
        StartCoroutine(SkipTrigger());
    }
    IEnumerator SkipTrigger(){
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame(); // waits twice for the end of frame essentially skiping the frame 
        RigidbodyToggled = false;
    }
    Vector3 GetAxis(){
        Vector2 LocAxis;
        if(Input.GetKey(Front)){
            LocAxis.y = 1;
        }
        else if (Input.GetKey(Back)) {
            LocAxis.y = -1;
        }
        else{
            LocAxis.y = 0;
        }

        if(Input.GetKey(Right)){
            LocAxis.x = 1;
        }
        else if (Input.GetKey(Left)) {
            LocAxis.x = -1;
        }
        else{
            LocAxis.x = 0;
        }
        return CamDirection.TransformDirection(LocAxis.x,0,LocAxis.y);
    }
    void OnTriggerEnter(Collider other){
    	if(!(Walkable == (Walkable | (1 << other.gameObject.layer)))){ // if the collided object is not in the walkable layermask
    		return;
    	}
    	if(other.isTrigger){ // if it is a trigger
    		return;
    	}
        if(RigidbodyToggled && other.gameObject.layer == 14){ // checks if the collided object is a ladder and if the rigidbody was recently toggled. if both are true no collision will be detected to prevent the player from exiting the ladder immediately
            return;
        }

        AddToArray(other);
    }
    void GetOffLadderPush(){
        Player.velocity += Camera.TransformDirection(Vector3.forward*4);
    }
    void OnTriggerExit(Collider other){
        if(other == Ladder && !RigidbodyToggled){ // in case the player exited the ladder we enable the rigidbody and remove the ladder collider we also check that this hasn't happened 
            Debug.Log("Player Physically got off the ladder");
        	ToggleRigidbody(true);
        	ClearLadder();
            GetOffLadderPush();

        }
        else{
            RemoveFromArray(other);       	
        }
    }
}