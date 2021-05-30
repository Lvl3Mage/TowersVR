using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoConditionsSpawnpicker : OnMapSpawnPicker
{
    protected override bool CheckStartingConditions(){
        return true;
    }
}
