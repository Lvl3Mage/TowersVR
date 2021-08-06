using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonRoom : RoomRelay
{
    [SerializeField] Activatable Cannon;
    public Activatable GetCannon(){
        return Cannon;
    }
}
