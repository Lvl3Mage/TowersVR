using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relay : ReferenceContainer
{
    protected override void ChangeValue(string varName, float value){ // reroutes all calls for value change to references
        InvokeReference(varName,value);
    }
    public bool ContainsReference(string varName){
        DataContainer[] value = ReferenceData[varName];
        return value != null; // if the value exists for this data container
    }
    public bool ContainsReferenceTo(string varName, DataContainer self){
        DataContainer[] value = ReferenceData[varName];
        if(value != null){
            int index = System.Array.IndexOf(value,self);
            return index != -1;
        }
        else{
            return false;
        }
        
    }
}
