using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataContainer : MonoBehaviour
{
    public void SetValue(string varName, float value){
        ChangeValue(varName,value);
        OnValueChange(varName);
    }
    protected virtual void ChangeValue(string varName, float value){}
    protected virtual void OnValueChange(string varName){

    }
}
