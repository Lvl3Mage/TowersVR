using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
    HELP
    To use this class you will need to:

        >> create an object that will serve as a loading point (where the loading will start) and give its reference to the loader

        >> create an animator on this object that will have:

            >>a loading animation called "Load" (that will move a loading point along the desired loading path) and that calls 
            the "LoadAnimationFinished" function at the end

            >>a reset animation (that will have a transition from the Load animation with a trigger called "Reloaded") which will reset 
            the loading point to its original position

*/
public class CannonReloader : WeaponReloader
{
	[SerializeField] Transform ReloadPoint;
	[SerializeField] float LerpRotationSpeed, LerpMovementSpeed;
	Animator Animator;
    AmmoObjectIdentifier LoadingObject;
	Transform ShellOffset;
    Coroutine AmmoObjectLerp;
    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator>();
    }
    protected override void LoadAmmunitionObject(AmmoObjectIdentifier Ammo){
        Loading = true;
        LoadingObject = Ammo;
    	LoadingCallback(1);
    	RemovePhysics(Ammo.gameObject);
        if(AmmoObjectLerp != null){ // for now just stopping the coroutine but in the future I should delete the actual object aswell (btw this situation is not possible cause it won't load an object while loading)
            StopCoroutine(AmmoObjectLerp);
        }

    	AmmoObjectLerp = StartCoroutine(AmmoLerp(Ammo.gameObject));
    }
    void RemovePhysics(GameObject obj){
    	Collider[] Colliders = obj.GetComponentsInChildren<Collider>();
        foreach (Collider Col in Colliders) 
        {
            Destroy(Col);
        }
    	Rigidbody RB = obj.GetComponent<Rigidbody>();
    	if(RB){
    		Destroy(RB);
    	}
    }
    IEnumerator AmmoLerp(GameObject AmmoObject){
        AdvancedGrabable LoadSettings = AmmoObject.GetComponent<AdvancedGrabable>();
        Transform ShellOffset;
        if(LoadSettings){
            ShellOffset = LoadSettings.HoldOffset;
        }
        else{
            ShellOffset = null;
        }
        AmmoObject.transform.parent = ReloadPoint;

        bool Finished = false;
        while(!Finished){
            Finished = Lerp(AmmoObject.transform, ShellOffset,ReloadPoint,LerpMovementSpeed,LerpRotationSpeed);
            yield return null;
        }

        Vector3 PosOffset = Vector3.zero;
        Quaternion RotationOffset = Quaternion.identity;
        if(ShellOffset){
            PosOffset = AmmoObject.transform.TransformDirection(ShellOffset.localPosition);
            RotationOffset = Quaternion.Inverse(ShellOffset.localRotation);
        }
        AmmoObject.transform.localPosition = PosOffset;
        AmmoObject.transform.localRotation = RotationOffset;
        LerpFinished();
        
    }
    void LerpFinished(){
        Animator.Play("Load");
    }
    void LoadAnimationFinished(){ //called by the animator
        weapon.Reload(new Ammunition(LoadingObject)); // loading the cannon with a new ammunition object (that does not derrive from monobehavior)
        Clear(); // clears the loading process and resets it
        Animator.SetTrigger("Reloaded"); // sends a signal to the animator for it to be restarted
    }
    void Clear(){
        Destroy(LoadingObject.gameObject);
        LoadingObject = null;
    }
    void AnimationReset(){ // called by the animator
        Loading = false;
    }
    bool Lerp(Transform Moved,Transform Offset, Transform B, float SpeedPos, float SpeedRot){ // Lerps an object with a given offset to a target and returns true if the lerp is complete
    	bool PositionLerped, RotationLerped;
    	Vector3 PosOffset;
    	Quaternion TargetRotation;
		if(Offset){
			PosOffset = Moved.TransformDirection(Offset.localPosition);
			TargetRotation = B.rotation*Quaternion.Inverse(Offset.localRotation);
			Moved.rotation = Quaternion.Lerp(Moved.rotation, TargetRotation, SpeedPos*Time.deltaTime);

		}
		else{
			PosOffset = Vector3.zero;
			Moved.rotation = Quaternion.Lerp(Moved.rotation, B.rotation, SpeedPos*Time.deltaTime);
			TargetRotation = B.rotation;
		}
		Moved.position = Vector3.Lerp(Moved.position, B.position - PosOffset, SpeedPos*Time.deltaTime);

		RotationLerped = Mathf.Abs(Quaternion.Angle(TargetRotation,Moved.rotation))<1f;
		PositionLerped = Mathf.Abs((Moved.position-(B.position-PosOffset)).magnitude)<0.1f;
		return PositionLerped && RotationLerped;

	}
    
}
