using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayerMovement : Movement
{
    [Header("Reference Requirements")]
    [SerializeField] Transform CamDirection;
    [SerializeField] Transform Camera;
    [SerializeField] Transform CameraOffseter;
    [Header("Controlls")]
    [SerializeField] OVRInput.RawAxis2D MovementAxis;
    [SerializeField] OVRInput.RawButton Jump;
    [SerializeField] OVRInput.RawButton SnapLeft;
    [SerializeField] OVRInput.RawButton SnapRight;
    [SerializeField] float SnapAngle;
    [SerializeField] OVRInput.RawButton Reset;
    void Update(){
        SnapCamera();
        if(OVRInput.GetDown(Reset)){
            Player.gameObject.transform.position = PlayerSpawnPoint.position;
        }
        ApplyMovement(OVRInput.Get(Jump), GetAxis(),false);   

    }
    void SnapCamera(){
        if(OVRInput.GetDown(SnapLeft)){
            CameraOffseter.localEulerAngles -= new Vector3(0, SnapAngle, 0);
        }
        if(OVRInput.GetDown(SnapRight)){
            CameraOffseter.localEulerAngles += new Vector3(0, SnapAngle, 0);
        }
    }
    Vector3 GetAxis(){
        Vector2 LocAxis = OVRInput.Get(MovementAxis);
        return CamDirection.TransformDirection(LocAxis.x,0,LocAxis.y);
    }
    void OnTriggerEnter(Collider other){
        if(!(Walkable == (Walkable | (1 << other.gameObject.layer)))){ // if the collided object is not in the walkable layermask
            return;
        }
        if(other.isTrigger){ // if it is a trigger
            return;
        }

        AddToArray(other);
    }
    void GetOffLadderPush(){
        Player.velocity += Camera.TransformDirection(Vector3.forward*4);
    }
    void OnTriggerExit(Collider other){
        RemoveFromArray(other);
    }
}
