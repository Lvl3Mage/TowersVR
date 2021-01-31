using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightDistanceCalculator : MultipleNumberContainer
{
	//[SerializeField] float SingleCameraOffset; //Half of both cameras
	[SerializeField] NumberContainer[] TargetsForDistance, TargetsForHeight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void OnListChange(int id) // the first angle is the x rotation while the second is the y rotation
    { // the input id is discarded for now. It can be used to not perform the calculations of the distance if it wasn't changed
    	//if(NumberList[1].value != 0){
	    	/*float yangle = 90-NumberList[1].value; // converting the angle to triangle angles
	    	float distanceH = Mathf.Tan(yangle*Mathf.Deg2Rad)*SingleCameraOffset; // Calculating the hipetenuse distance using the formula tan(alfa) = o/a*/

    	float xangle = 90-NumberList[0].value; // converting the angle to triangle angles
    	float distancex = Mathf.Sin(xangle*Mathf.Deg2Rad)*NumberList[1].value; // calculating the horizontal distance to the point using the formula sin(alfa) = o/h
    	float relheight = Mathf.Cos(xangle*Mathf.Deg2Rad)*NumberList[1].value;// calculating the relative height to the point using the formula cos(alfa) = a/h
    	foreach (NumberContainer Container in TargetsForDistance) 
    	{
    		Container.floatValue = distancex; // outputing the horizontal distance
    	}
    	foreach (NumberContainer Container in TargetsForHeight) 
    	{
    		Container.floatValue = relheight; // outputing the relative height
    	}
    	//}

    }
}
