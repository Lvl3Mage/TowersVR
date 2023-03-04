using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangableValueContainer : MonoBehaviour
{
    [SerializeField] OutputDataContainer[] Outputs;
    [SerializeField] bool Clamp;
    [SerializeField] Vector2 ClampValue;
    [SerializeField] float DefaultValue;
    float Value;
    void Awake(){
        Value = DefaultValue;
    }
    public void ChangeValueBy(float delta){
        Value += delta;
        if(Clamp){
            Value = Mathf.Clamp(Value, ClampValue.x, ClampValue.y);             
        }
        foreach (OutputDataContainer Output in Outputs) 
        {
            Output.InvokeOutput(Value);
        }
    }
}
