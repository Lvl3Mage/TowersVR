using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingRoom : Room
{
    public Weapon GetCannon(){
        return ParentTower.GetCannon();
    }
}
