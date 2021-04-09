using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonReloader : WeaponReloader
{
	[SerializeField] Transform ReloadPoint;
	[SerializeField] float LerpRotationSpeed, LerpMovementSpeed;
	bool LerpShell;
	Animator Animator;
	GameObject LoadingObj;
	Transform ShellOffset;
    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(LerpShell){
        	bool Finished = Lerp(LoadingObj.transform, ShellOffset,ReloadPoint,LerpMovementSpeed,LerpRotationSpeed);
        	if(Finished){
        		FinishLerp();
        	}
        }
    }
    protected override void LoadWeapon(AmmoIdentifier Shell){
    	LoadingCallback(1);
    	LoadingObj = Shell.gameObject;
    	Loading = true;
    	RemovePhysics();
    	StartLerp();
    }
    void RemovePhysics(){
    	Collider[] Colliders = LoadingObj.GetComponentsInChildren<Collider>();
        foreach (Collider Col in Colliders) 
        {
            Destroy(Col);
        }
    	Rigidbody RB = LoadingObj.GetComponent<Rigidbody>();
    	if(RB){
    		Destroy(RB);
    	}
    }
    void StartLerp(){
    	AdvancedGrabable LoadSettings = LoadingObj.GetComponent<AdvancedGrabable>();
    	if(LoadSettings){
    		ShellOffset = LoadSettings.HoldOffset;
    	}
    	else{
    		ShellOffset = null;
    	}
    	LoadingObj.transform.parent = ReloadPoint;
    	LerpShell = true;

    }
    bool Lerp(Transform Moved,Transform Offset, Transform B, float SpeedPos, float SpeedRot){
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
    void FinishLerp(){
    	LerpShell = false;
    	Vector3 PosOffset = Vector3.zero;
    	Quaternion RotationOffset = Quaternion.identity;
    	if(ShellOffset){
			PosOffset = LoadingObj.transform.TransformDirection(ShellOffset.localPosition);
			RotationOffset = Quaternion.Inverse(ShellOffset.localRotation);
		}
		LoadingObj.transform.localPosition = PosOffset;
		LoadingObj.transform.localRotation = RotationOffset;
		Animate();
		

    }
    void Animate(){
    	Animator.Play("Load");
    }
    void LoadAnimationFinished(){
    	weapon.Reload(LoadingObj.GetComponent<AmmoIdentifier>());
    	Clear();
        Animator.SetTrigger("Reloaded");
    }
    void Clear(){
    	ShellOffset = null;
    	Destroy(LoadingObj);
    }
    void AnimationReset(){
    	Loading = false;
    }
}
