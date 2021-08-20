using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomRelay : ReferenceContainer
{
    DataContainer Tower;
    public void SetTower(DataContainer newTower){
        Tower = newTower;
        foreach(KeyValuePair<string, DataContainer[]> entry in ReferenceData){
            if(entry.Value == null){
                ReferenceData[entry.Key] = new DataContainer[] {newTower};
            }
        }
    }
    protected virtual void ChangeValue(string varName, float value){ // reroutes all calls for value change to references
        InvokeReference(varName,value);
    }
    public bool ContainsReference(string varName, DataContainer self){
        DataContainer[] value = ReferenceData[varName];
        int index = System.Array.IndexOf(value,self);
        return value != null && index != -1; // if the value exists for this data container and it contains a reference for an object other than the one who is asking
        //this is done to prevent the object which is asking (the tower) to reference the object who referenced the tower in the first place
    }
}
