using System.Collections;
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
    [SerializeField] List<Collider> CurrentCollisions = new List<Collider>();
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
    [SerializeField] Transform PlayerSpawnPoint;
    Collider Ladder;
    Vector3 Axis, PastLadderPos;
    bool Jump, JumpReady = true, RigidbodyToggled;
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
        if(Input.GetKeyDown(KeyCode.H)){
            if(!Ladder){
                Player.gameObject.transform.position = PlayerSpawnPoint.position;
            }
        }
        if(Ladder){ // Checks if the ladder collider is empty, if it isn't then it applies ladder motion, otherwise it applies normal motion
        	ApplyLadderMovement(Ladder); 
        }
        else{
        	ApplyMovement();	
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
    	if(Input.GetKeyDown(KeyCode.Space)){
            Debug.Log("Player chose to get off " + Ladder.gameObject.name + " by pressing space");
    		ToggleRigidbody(true);
		    Player.velocity += Camera.TransformDirection(Vector3.forward*5); // Jumps from ladder
		    ClearLadder();
    	}
    	else{
		    
	    	float MovementAxis = 0;
	        if(Input.GetKey(KeyCode.W)){
	            MovementAxis = 1;
	        }
	        else if (Input.GetKey(KeyCode.S)) {
	            MovementAxis = -1;
	        }
	        Transform LadderTransform = Ladder.gameObject.transform;
	        Transform PlayerTransform = Player.gameObject.transform;
            Vector3 RelativeLerpTarget = Vector3.down/3 + Vector3.forward * (LadderTransform.InverseTransformPoint(PlayerTransform.position).z + MovementAxis*LadderMoveSpeed);
	        Vector3 LerpTarget = LadderTransform.TransformPoint(RelativeLerpTarget); // sets the horizontal position relative to the ladder
            Vector3 LadderDelta = PastLadderPos - LadderTransform.position; // A vector that defines ladder movement (it is then added to the player position)
	        PlayerTransform.position = Vector3.Lerp(PlayerTransform.position,LerpTarget,Time.deltaTime * LadderLerpSpeed) - LadderDelta ; // actual position set
            
            PastLadderPos = LadderTransform.position; // Sets the ladder past position for calculating next frame delta
        }
    	
    }
    void SetLadder(Collider LadderCol){
        Debug.Log("New Ladder Set - " + LadderCol.gameObject.name);
        Ladder = LadderCol;
        PastLadderPos = Ladder.attachedRigidbody.transform.position;
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
        StartCoroutine(ScipTrigger());
    }
    IEnumerator ScipTrigger(){
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame(); // waits twice for the end of frame essentially skiping the frame 
        RigidbodyToggled = false;
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
                    //Debug.Log("Removed " + CurrentCollisions[i].gameObject.name);
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
        if(RigidbodyToggled && other.gameObject.layer == 14){
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
            //Debug.Log("Added " + other.gameObject.name);
        }
    }
    void OnTriggerExit(Collider other){
        if(other == Ladder && !RigidbodyToggled){ // in case the player exited the ladder we enable the rigidbody and remove the ladder collider
            Debug.Log("Player Physically got off the ladder");
        	ToggleRigidbody(true);
        	ClearLadder();
        }
        else{
	        for (int i = 0; i < CurrentCollisions.Count; i++) 
	        {
	            if(CurrentCollisions[i] == other){
	                CurrentCollisions.RemoveAt(i);
	                //Debug.Log("Removed " + other.gameObject.name);
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