using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : Relay
{
    [Tooltip("Write the names of variables to which the tower reference should be added")] 
    [SerializeField] List<string> TowerReference = new List<string>();
    protected PlayableTower ParentTower;
    public void SetTower(PlayableTower newTower){
        ParentTower = newTower;
        for(int i = 0; i < TowerReference.Count; i++){
            if(ReferenceData[TowerReference[i]] == null){
                ReferenceData.Add(TowerReference[i],new DataContainer[]{ParentTower});
            }
            else{ // this is gross but it will do for now (until and if I rewrite the dictionary to use a list)
                List<DataContainer> newData = new List<DataContainer>(ReferenceData[TowerReference[i]]);
                newData.Add(ParentTower);
                ReferenceData[TowerReference[i]] = newData.ToArray();

            }
            
        }
    }
    public Dictionary<string, DataContainer[]> GetReferenceData(){
        return ReferenceData;
    }
}
