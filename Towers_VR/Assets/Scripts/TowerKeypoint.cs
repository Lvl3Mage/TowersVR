using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerKeypoint : MonoBehaviour
{
	[Header("Structure defenition")]
	[Tooltip("The radius of Rigidbody structures that define the keypoint (only rigidbody structures that are in this radius will be accounted for)")]
	[SerializeField] float StructureRadius;
	[SerializeField] LayerMask StructureLayerMask;

	[Header("Destroyed Conditions")]
	[Tooltip("Defines how much of the related structures have to be connected for the keypoint to be considered active")]
	[SerializeField] [Range(0f, 1f)] float StructureDestructionPercentage;
	[Tooltip("Defines the maximum distance the structure can be away from the keypoint for it to be considered to be too far away")]
	[SerializeField] float MaxStructureDistance;
	[Tooltip("Defines the maximum distance the keypoint can move from its starting position for it to be considered to be destroyed")]
	[SerializeField] float MaxKeypointMoveDistance;
	[Tooltip("If marked, once the keypoint has been deactivated it will remain inactive until the end of the game")]
	[SerializeField] bool InactiveOnce;

	[Header("Position recalculation")]
	[Tooltip("Defines the influence a structure has on the position of the keypoint where x is the fraction of MaxInfluenceDistance/structure's distance from the origin from of the keypoint and y is the influence")]
	[SerializeField] AnimationCurve InfluenceCurve;
	[Tooltip("The max distance that represents 1 on the x axis of the influence curve")]
	[SerializeField] float MaxInfluenceDistance;
	[Header("Gizmo settings")]
	[SerializeField] bool DrawGizmos;
	[SerializeField] float FirstGizmoTransparency;
	[SerializeField] float SecondGizmoTransparency;
	[SerializeField] float ThirdGizmoTransparency;
	[Header("Debug settings")]
	[SerializeField] bool DebugMode;
	[SerializeField] float DebugTransparency;
	[SerializeField] float DebugRadius;

	Transform[] relatedStructures;
	Vector3 Origin;

	private bool _intact;

	public bool intact{ // determines whether the keypoint is still intact (if it should be targeted)
		get { 
			return _intact;;
		}
		private set { 
			_intact = value; 
		}
	}
	void Awake (){
		intact = true;
	}
	void Start(){
		Origin = transform.position;
		//Gets all physics objects in radius
		Collider[] RelatedColliders = GetObjectsInRadius(StructureRadius, StructureLayerMask);

		//Saves them as array of transforms discarding ones that are thurther than the maximum distance
		relatedStructures = RecalculateColliders(RelatedColliders);
	}
	void Update(){
		if(DebugMode){
			RecalculateKeypoint();
		}
	}
	Transform[] RecalculateColliders(Collider[] Colliders){ // transforms the given array of colliders into an array of transforms discarding the ones that might be thurther than the structure radius
		List<Transform> Transforms = new List<Transform>();
		for (int i = 0; i < Colliders.Length; i++){
			Transform curTransform = Colliders[i].gameObject.transform;
			float Distance = DistanceToTransform(curTransform);
			if(Distance < StructureRadius){
				Transforms.Add(curTransform);
			}
		}
		return Transforms.ToArray();
	}

	Collider[] GetObjectsInRadius(float r, LayerMask Mask){
    	Collider[] ColliderArray = Physics.OverlapSphere(transform.position, r, Mask, QueryTriggerInteraction.Ignore);
    	return ColliderArray;
    }
    float DistanceToTransform(Transform otherTransform){ // returns the distance from the object to the desired transform
    	Vector3 otherPos = otherTransform.position;
    	Vector3 myPos = transform.position;
    	return Vector3.Distance(myPos, otherPos);
    }
    bool CalculateStructureIntegrity(){ // returns whether enough structures have been destroyed for the keypoint to be considered inactive (Call CalculateKeypointState() to determine whether the keypoint is active)
    	int connectedStructures = 0;
    	foreach(Transform relatedStructure in relatedStructures){ // at the end of this loop the new position will be the sum of all the structures' positions
    		if(relatedStructure.gameObject.activeSelf){ // in case the structure hasn't been destroyed
    			float Distance = DistanceToTransform(relatedStructure); 
    			if(Distance<MaxStructureDistance){ // if the structure is close enough
    				connectedStructures++;
    			}
    		}
    	}
    	float connectedStructuresPercent = ((float)connectedStructures)/((float)relatedStructures.Length);

    	return (connectedStructuresPercent >= StructureDestructionPercentage); // returns true only if enough structures remain intact
    }
    bool CalculateKeypointState(){ // returns whether the keypoint is still active
    	bool StructureIntegrity = CalculateStructureIntegrity();

    	float distanceFromOrigin = Vector3.Distance(Origin, transform.position);
    	bool KeypointMovedIntegrity = distanceFromOrigin <= MaxKeypointMoveDistance;
    	return StructureIntegrity && KeypointMovedIntegrity; // only returns true if the keypoint stays within range of its origin and if more than enough structures are connected
    }
    void RecalculateKeypointPosition(){ // recalculates the keypoint position to be between all the structures
    	float totalInfluence = 0;
    	Vector3 newPosition = Vector3.zero;
    	foreach(Transform relatedStructure in relatedStructures){ // at the end of this loop the new position will be the sum of all the structures' positions
    		float distance =  Vector3.Distance(Origin, relatedStructure.position); //the distance of the structure form the origin of the keypoint
    		float influence = InfluenceCurve.Evaluate(distance/MaxInfluenceDistance);
    		newPosition += relatedStructure.position*influence;
    		totalInfluence += influence;
    	}
    	newPosition /= totalInfluence;

    	transform.position = newPosition; // sets the new position
    }
    [ContextMenu("Recalculate Keypoint")]
    public void RecalculateKeypoint(){
    	RecalculateKeypointPosition(); // first recalculates the keypoint position

    	intact = CalculateKeypointState();//then checks if the keypoint is still active
    }
    void OnDrawGizmos(){
    	if(DrawGizmos){
    		Color Color;
    		//Draws the inner circle
    		if(intact){
    			Color = Color.green;
    		}
    		else{
    			Color = Color.red;
    		}
    		
    		Color.a = FirstGizmoTransparency;
	    	Gizmos.color = Color;
	    	Gizmos.DrawSphere(transform.position, StructureRadius);

	    	//Draws the outer circle
	    	Color = Color.yellow;
    		Color.a = SecondGizmoTransparency;
	    	Gizmos.color = Color;
	    	Gizmos.DrawSphere(transform.position, MaxStructureDistance);

	    	//Draws the 3rd circle
	    	Color = Color.blue;
    		Color.a = ThirdGizmoTransparency;
	    	Gizmos.color = Color;
	    	Gizmos.DrawSphere(transform.position, MaxKeypointMoveDistance);
    	}
    	if(DebugMode){
    		Color Color;
    		if(intact){
    			Color = Color.green;
    		}
    		else{
    			Color = Color.red;
    		}
    		Color.a = DebugTransparency;
	    	Gizmos.color = Color;
	    	Gizmos.DrawSphere(transform.position, DebugRadius);
    	}
    }
}
