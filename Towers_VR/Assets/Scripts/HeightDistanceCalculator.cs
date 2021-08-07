using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightDistanceCalculator : DataContainer
{
    Dictionary<string,float> NamedData = new Dictionary<string,float>(){
        {"CannonAngle",0},
        {"DirectDistance",0}
    };
    [Tooltip("A callback array which identifies the objects that receive the jorizontal distance")]
    [SerializeField] protected DataContainer[] CallBackHorizDistance;
    [SerializeField] string HorizDistanceVarName;
    [Tooltip("A callback array which identifies the objects that receive the height")]
    [SerializeField] protected DataContainer[] CallBackHeight;
    [SerializeField] string HeightVarName; 

    protected override void ChangeValue(string varName, float value){
        NamedData[varName] = value;
    }
    protected override void OnValueChange(string varName) // the first angle is the x rotation while the second is the y rotation
    { // the input id is discarded for now. It can be used to not perform the calculations of the distance if it wasn't changed
    	//if(NumberList[1].value != 0){
	    	/*float yangle = 90-NumberList[1].value; // converting the angle to triangle angles
	    	float distanceH = Mathf.Tan(yangle*Mathf.Deg2Rad)*SingleCameraOffset; // Calculating the hipetenuse distance using the formula tan(alfa) = o/a*/

    	float xangle = 90-NamedData["CannonAngle"]; // converting the angle to triangle angles
    	float distancex = Mathf.Sin(xangle*Mathf.Deg2Rad)*NamedData["DirectDistance"]; // calculating the horizontal distance to the point using the formula sin(alfa) = o/h
    	float relheight = Mathf.Cos(xangle*Mathf.Deg2Rad)*NamedData["DirectDistance"];// calculating the relative height to the point using the formula cos(alfa) = a/h

        //invoking callback
        foreach(DataContainer cont in CallBackHorizDistance){
            cont.SetValue(HorizDistanceVarName,distancex);
        }
        foreach(DataContainer cont in CallBackHeight){
            cont.SetValue(HeightVarName,relheight);
        }

    }
}
