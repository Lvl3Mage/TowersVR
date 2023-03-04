using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightDistanceCalculator : FloatContainer
{
	//[SerializeField] float SingleCameraOffset; //Half of both cameras
	[SerializeField] DataContainer[] TargetsForDistance, TargetsForHeight;
    // Start is called before the first frame update
    float VerticalRotation;
    float LinearDistance;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void SetFloat(DataType dataType, float value)
    {
        switch(dataType){
            case DataType.VerticalRotation:
                VerticalRotation = value;
                break;
            case DataType.LinearDistance:
                LinearDistance = value;
                break;
            default:
                Debug.LogError("WTF is this => " + dataType + "?", gameObject);
                break;
        }
    	float xangle = 90-VerticalRotation; // converting the angle to triangle angles
    	float distancex = Mathf.Sin(xangle*Mathf.Deg2Rad)*LinearDistance; // calculating the horizontal distance to the point using the formula sin(alfa) = o/h
    	float relheight = Mathf.Cos(xangle*Mathf.Deg2Rad)*LinearDistance;// calculating the relative height to the point using the formula cos(alfa) = a/h
        foreach (DataContainer Container in TargetsForDistance) 
    	{
    		Container.SetValue(DataType.HorizontalDistance, distancex); // outputing the horizontal distance
    	}
    	foreach (DataContainer Container in TargetsForHeight) 
    	{
    		Container.SetValue(DataType.Height, relheight); // outputing the relative height
    	}
    	//}

    }
}
