using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : Relay
{
    protected TowerRelay towerRelay;
    public void SetTowerRelay(TowerRelay newTowerRelay){
        towerRelay = newTowerRelay;
        SetUpperRelay(newTowerRelay);
    }
}
