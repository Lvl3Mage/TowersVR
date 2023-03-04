using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntContainer : DataContainer
{
    public override void SetValue<T>(DataType dataType, T value)
    {
        if(value is int){
            SetInt(dataType, (int)(object)value); // can't use the "as" operator here. float is a non nullable type :P
        }
        else{
            Debug.LogWarning("Unexpected type recieved. Expected type: int", gameObject);
        }
    }
    protected virtual void SetInt(DataType dataType, int value){

    }
}
