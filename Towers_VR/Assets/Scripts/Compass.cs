using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
	Wind Wind;
	[SerializeField] NumberContainer WindVelocity;
    [Tooltip("The object that will indicate the local north direction of the compass (Should be placed flat and rotate with the tower)")]
    [SerializeField] Transform RotationDirectioner;
    [SerializeField] Transform Arrow, CompassPlane;
    //[SerializeField] Transform one,two,three;
    // Start is called before the first frame update
    void Start()
    {
        Wind = GameObject.FindGameObjectWithTag("Wind").GetComponent<Wind>();
        if(!Wind){
            Debug.LogError("No Wind found in scene");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //RotationDirectioner.eulerAngles -= new Vector3(RotationDirectioner.eulerAngles.x,0,RotationDirectioner.eulerAngles.z); // This object will serve as a 0 x and z rotation reference to correctly transform vectors to local coordinates
        Vector3 NorthVector = new Vector3(0,0,1);
        Vector3 LocalNorth = TransformVectorToLocal(NorthVector);
        Vector3 LocalWind = TransformVectorToLocal(new Vector3(Wind.WindVector.x,0,Wind.WindVector.y)); // transforming the wind to local coordinates which will be independant of the x and z rotation of the sensor
        CompassPlane.localRotation = Quaternion.LookRotation(LocalNorth, Vector3.up);
        Arrow.localRotation = Quaternion.LookRotation(LocalWind, Vector3.up); // rotating the arrow local rotation toward the local wind vector
        WindVelocity.floatValue = Wind.WindVector.magnitude;
    }
    Vector3 TransformVectorToLocal(Vector3 Input){
    	return RotationDirectioner.InverseTransformDirection(Input); // transforming the Vector to local coordinates which will be independant of the x and z rotation of the sensor
    }
}
