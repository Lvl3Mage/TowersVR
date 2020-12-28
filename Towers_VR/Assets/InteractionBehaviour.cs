using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBehaviour : MonoBehaviour
{
	[SerializeField] float maxDist, ThrowForce, RotSmoothing, PosSmoothing;
	[SerializeField] LayerMask InteractableLayers, AbleToPickUp;
	[SerializeField] Transform GrabPoint, OutInventoryPointL, OutInventoryPointR, StoredInventoryPoint, LoadPoint;
	[SerializeField] PCPlayerMovement PlayerMovement;
	Transform HeldObject, PuttingAwayObject, CurrentINInv, CurrentOUTInv;
	Transform HeldObjectOffset, PuttingAwayObjectOffset;
	WeaponReloader LoadTarget;
	Inventory Inventory = new Inventory(5);
	Interactable Interacted;

	void Update(){
		if(Input.GetKeyDown(KeyCode.F)){
			GameObject AimedObject = RayForw();
			Action(AimedObject);
		}
		if(Input.GetKeyUp(KeyCode.F)){
			
			if(Interacted){
				Interacted.Interact(false);
				Interacted = null;
			}
		}
		if(Input.mouseScrollDelta.y>0){
			Switch(1);
		}
		else if (Input.mouseScrollDelta.y<0) {
			Switch(-1);
		}
		if(HeldObject){ // lerp the held object if exists
			Transform ObjPos;
			if(HeldObjectOffset){
				ObjPos = HeldObjectOffset;
			}
			else{
				ObjPos = null;
			}
			Lerp(HeldObject, ObjPos, GrabPoint, PosSmoothing, RotSmoothing);
		}
		if(PuttingAwayObject){ // lerp the putting away object if exists
			Transform ObjPos;
			if(PuttingAwayObjectOffset){
				ObjPos = PuttingAwayObjectOffset;
			}
			else{
				ObjPos = null;
			}
			Lerp(PuttingAwayObject, ObjPos, CurrentINInv, PosSmoothing, RotSmoothing);

			if((PuttingAwayObject.position-CurrentINInv.position).magnitude < 0.2f){ // Check if the object putting away animation has ended
				StoreObject();
			}
		}
	}
	void Lerp(Transform Moved,Transform Offset, Transform B, float SpeedPos, float SpeedRot){
		if(Offset){
			Vector3 PosOffset = Moved.TransformDirection(Offset.localPosition);
			Moved.position = Vector3.Lerp(Moved.position, B.position - PosOffset, SpeedPos*Time.deltaTime);
			Moved.rotation = Quaternion.Lerp(Moved.rotation, B.rotation*Quaternion.Inverse(Offset.localRotation), SpeedPos*Time.deltaTime);
		}
		else{
			Moved.position = Vector3.Lerp(Moved.position, B.position, SpeedPos*Time.deltaTime);
			Moved.rotation = Quaternion.Lerp(Moved.rotation, B.rotation, SpeedPos*Time.deltaTime);

		}
	}
	GameObject RayForw(){
		RaycastHit hit;
		Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward*maxDist), Color.green);
		if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, maxDist, InteractableLayers)){
			
			return hit.collider.gameObject;
		}
		else{
			return null;
		}
	}
	void Action(GameObject Obj){
		
		if(Obj){
			
			Interactable Inter = Obj.GetComponent<Interactable>();
			if(Inter){
				Inter.Interact(true);
				Interacted = Inter;
				//Debug.Log(Inter.gameObject.name);
			}
			else{
				WeaponReloader Reload = Obj.GetComponent<WeaponReloader>();
				AmmoIdentifier Ammo = null;
				if(HeldObject){
					Ammo = HeldObject.GetComponent<AmmoIdentifier>();
				}
				if(Reload && Ammo){ // Checks if the target is an ammo reciever and the player is holding ammo 
					if(Reload.LoadShell(Ammo)){ // Initiates player loading sequence
						HeldObject = null;
						HeldObjectOffset = null;
						Inventory.Remove();
					}
				}
				else{
					if(HeldObject){
						ThrowObject();
					}
					else if (AbleToPickUp == (AbleToPickUp | (1 << Obj.layer))){
						PickUp(Obj);
					}
					else{
						if(Obj.layer == 14){// Checks if object is a ladder
							Collider LadderCol = Obj.GetComponent<Collider>();
							if(LadderCol){
								PlayerMovement.UseLadder(LadderCol);
							}
							
						} 
					}				
				}

			}
		}
		else{
			if(HeldObject){
				ThrowObject();
			}
		}
	}
	void Switch(int change){
		Inventory.SwitchBy(change);
		if(change>0){
			CurrentINInv = OutInventoryPointL;
			CurrentOUTInv = OutInventoryPointR;
		}
		else if (change<0){
			CurrentINInv = OutInventoryPointR;
			CurrentOUTInv = OutInventoryPointL;
		}
		if(PuttingAwayObject){ // In case the previews animation hasn't finished yet I put the object away 
			StoreObject();
		}
		PuttingAwayObject = HeldObject;
		PuttingAwayObjectOffset = HeldObjectOffset;
		PCPickedUpObject SwitchObject = Inventory.CurrentSlotObj();
		if(SwitchObject != null){
			HeldObject = SwitchObject.Obj;
			HeldObjectOffset = SwitchObject.Offset;
			HeldObject.position = CurrentOUTInv.position;
			HeldObject.rotation = CurrentOUTInv.rotation;
			HeldObject.parent = transform.parent; // parenting the object that I'm getting out to the player
		}
		else{
			HeldObject = null;
		}
		
	}
	void StoreObject(){ // a function that stores the object that was beign animated out of the screen in a safe place (behind the player)
		PuttingAwayObject.parent = StoredInventoryPoint; // remember that stored objects are parented unter StoredInventoryPoint to prevent the player from seeing them
		PuttingAwayObject.localPosition = Vector3.zero; // Both rotation and position are 0 due to the object beign parented
		PuttingAwayObject.localRotation = Quaternion.identity;
		PuttingAwayObject = null;
	}
	void PickUp(GameObject Obj){
		Inventory.Grab(Obj);
		Obj.GetComponent<Collider>().enabled = false;
		Rigidbody RB = Obj.GetComponent<Rigidbody>();
		Destroy(RB);
		PCPickedUpObject SlotObj = Inventory.CurrentSlotObj();
		HeldObject = SlotObj.Obj;
		HeldObjectOffset = SlotObj.Offset;
		HeldObject.parent = transform.parent;

	}
	void ThrowObject(){
		PCPickedUpObject SlotObj = Inventory.CurrentSlotObj();
		HeldObject.parent = SlotObj.Parent;
		HeldObject.gameObject.GetComponent<Collider>().enabled = true;
		Rigidbody RB = HeldObject.gameObject.AddComponent<Rigidbody>();

		RB.mass = SlotObj.Mass;
		RB.drag = SlotObj.Drag;
		RB.angularDrag = SlotObj.AngDrag;
		RB.velocity += transform.TransformDirection(Vector3.forward*ThrowForce);
		RB.angularVelocity = new Vector3(Random.Range(-1f, 1)*ThrowForce,Random.Range(-1f, 1)*ThrowForce,Random.Range(-1f, 1)*ThrowForce);
		HeldObject = null;
		Inventory.Remove();
	}
}
