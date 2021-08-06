using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceContainer : DataContainer
{
    [SerializeField] List<DictVariable> CallbackReferences = new List<DictVariable>(); // the serialized list that can be configured in the editor
    protected Dictionary<string,DataContainer[]> ReferenceData; // the dictionary that will be used for referencing other objects
    void Awake(){
        for(int i = 0; i < CallbackReferences.Count; i++){
            ReferenceData.Add(CallbackReferences[i].name, CallbackReferences[i].reference);
        }
        PostAwake();
    }
    protected virtual void PostAwake(){}
    protected void InvokeReference(string varName, float value){ // invokes a variable from the reference data dict
        DataContainer[] containers = ReferenceData[varName];
        foreach(DataContainer container in containers){
            container.SetValue(varName, value);
        }
        
    }
    protected void InvokeAllReferences(float value){ // invokes all references
        foreach(KeyValuePair<string, DataContainer[]> entry in ReferenceData){
            DataContainer[] containers = entry.Value;
            foreach(DataContainer container in containers){
                container.SetValue(entry.Key, value);
            }
        }
    }

}

