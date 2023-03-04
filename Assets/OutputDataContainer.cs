using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OutputDataContainer
{
    public DataContainer[] OutputDataContainers;
    public DataType OutputType;
    public void InvokeOutput<T>(T data){
        foreach(DataContainer container in OutputDataContainers){
            container.SetValue(OutputType, data);
        }
    }
}
