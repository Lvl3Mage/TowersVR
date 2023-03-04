using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolContainer : DataContainer
{
    public override void SetValue<T>(DataType dataType, T value)
    {
        if(value is bool){
            SetBool(dataType, (bool)(object)value); // can't use the "as" operator here. float is a non nullable type :P
        }
        else{
            Debug.LogWarning("Unexpected type recieved. Expected type: bool", gameObject);
        }
    }
    protected virtual void SetBool(DataType dataType, bool value){

    }
}
