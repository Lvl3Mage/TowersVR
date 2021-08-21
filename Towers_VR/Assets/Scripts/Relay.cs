using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relay : DataContainer
{
    [System.Serializable]
    public class TypeRelay
    {
        public DataType Type;
        public DataContainer[] Destinations;
        public bool RelayUp;
    }
    [SerializeField] TypeRelay[] TypeRelayList;
    Dictionary<DataType, DataContainer[]> TypeRelays = new Dictionary<DataType, DataContainer[]>();//Dict of all input destinations
    HashSet<DataType> RelayUp = new HashSet<DataType>(); // all data types that will be relayed up
    DataContainer UpperRelay;

    void Awake(){
        foreach(TypeRelay typeRelay in TypeRelayList){
            TypeRelays.Add(typeRelay.Type, typeRelay.Destinations);

            // in case the relay up is marked
            if(typeRelay.RelayUp){
                RelayUp.Add(typeRelay.Type); // add this type to relay up
            }
        }
    }
    public override void SetValue<T>(DataType dataType, T value){
        if(TypeRelays.ContainsKey(dataType)){ // if this relay contains the demanded key
            DataContainer[] Destinations = TypeRelays[dataType];
            foreach(DataContainer dest in Destinations){
                dest.SetValue(dataType, value);
            }
            if(RelayUp.Contains(dataType)){ // if this particular datatype is marked to relay up
                UpperRelay.SetValue(dataType, value);
            }
        }
        else{ // in case this data type can't be found
            Debug.Log("Sending up");
            UpperRelay.SetValue(dataType, value);
        }
    }
    public HashSet<DataType> GetRelayInputs(){
        HashSet<DataType> Inputs = new HashSet<DataType>();
        foreach(KeyValuePair<DataType, DataContainer[]> entry in TypeRelays) // for each relay
        {
            if(!RelayUp.Contains(entry.Key)){ // if the entry is not set to relay up
                Inputs.Add(entry.Key); // then it is an input
            }
        }
        return Inputs;
    }
    protected void SetUpperRelay(DataContainer upperRelay){
        UpperRelay = upperRelay;
    }
    

}
