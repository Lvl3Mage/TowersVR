using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatContainer : DataContainer
{
    public override void SetValue<T>(DataType dataType, T value)
    {
        if(value is float){
            SetFloat(dataType, (float)(object)value); // can't use the "as" operator here. float is a non nullable type :P
        }
        else{
            Debug.LogWarning("Unexpected type recieved. Expected type: float", gameObject);
        }
    }
    protected virtual void SetFloat(DataType dataType, float value){

    }
}
