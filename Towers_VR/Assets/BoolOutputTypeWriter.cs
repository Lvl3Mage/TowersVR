using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolOutputTypeWriter : BoolContainer
{
	[SerializeField] CannonAngleCalculator AngularCalculator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void ValueChanged(){
    	AngularCalculator.WriteOutputType(_boolValue);
	}
}
