using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
	int InventorySize;
	PCPickedUpObject[] PickedUpObjects;
	int CurrentInventorySlot;
	int Break;
	public Inventory(int _InventorySize){
		InventorySize = _InventorySize;

		PickedUpObjects = new PCPickedUpObject[InventorySize];
		CurrentInventorySlot = 0;
	}
	public void SwitchTo(int val){
		CurrentInventorySlot = val;
    	CheckOverflow();
	}
    public void SwitchBy(int change){
    	CurrentInventorySlot += change;
    	CheckOverflow();
	}
	void CheckOverflow(){
		int PrevSlot = CurrentInventorySlot;
		if(CurrentInventorySlot<0){
    		CurrentInventorySlot = InventorySize - 1 /*+ -PrevSlot*/;
    		if(CurrentInventorySlot<0){
    			Break ++;
    			if (Break<100){
    				CheckOverflow();
    			}
    			else{
    				Debug.LogError("Yup");
    			}
    			
    		}
    	}
    	else if (CurrentInventorySlot >= InventorySize) {
    		CurrentInventorySlot = 0 /*+ (PrevSlot - (InventorySize-1))*/;
    		if(CurrentInventorySlot >= InventorySize){
    			Break ++;
    			if (Break<100){
    				CheckOverflow();
    			}
    			else{
    				Debug.LogError("Yup");
    			}
    		}
    	}
	}
	public void Grab(GameObject Obj){

		Rigidbody RB = Obj.GetComponent<Rigidbody>();
		AdvancedGrabable advGrab = Obj.GetComponent<AdvancedGrabable>();
		Transform Offset = null;
		if(advGrab){
			Offset = advGrab.ViewOffset;
		}
		if(RB){

			PickedUpObjects[CurrentInventorySlot] = new PCPickedUpObject(Obj.transform,Offset,Obj.transform.parent,RB.mass,RB.drag,RB.angularDrag);
		}
		else{
			Debug.LogError("Can't grab an object with no Rigidbody");
			//PickedUpObjects[CurrentInventorySlot] = new PCPickedUpObject(Obj.transform,Obj.transform.parent,0,0,0); If you want to grab obj with no RB
		}
		/*if(PickedUpObjects[CurrentInventorySlot]){
			Debug.Log(PickedUpObjects[CurrentInventorySlot].Obj.gameObject);
		}
		else{
			Debug.Log(new PCPickedUpObject(Obj.transform,Obj.transform.parent,RB.mass,RB.drag,RB.angularDrag));
		}*/
		
	}
	public PCPickedUpObject CurrentSlotObj(){
		PCPickedUpObject PickedObj = PickedUpObjects[CurrentInventorySlot];
		return PickedObj;
	}
	public int CurrentSlot(){
		return CurrentInventorySlot;
	}
	public void Remove(){
		PickedUpObjects[CurrentInventorySlot] = null;

	}
}
