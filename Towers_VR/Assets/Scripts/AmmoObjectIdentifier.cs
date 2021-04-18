using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoObjectIdentifier : MonoBehaviour
{
    [SerializeField] protected Object _bullet;
    public Object bullet {

        get {
            return _bullet;
        }
    }
    [SerializeField] protected float _velocity;
    public float velocity {

        get {
            return _velocity;
        }
    }
    [SerializeField] protected AmmoCaliber _caliber;
    public AmmoCaliber caliber {

        get {
            return _caliber;
        }
    }
    [SerializeField] protected int _ammoCount;
    public int ammoCount {

        get {
            return _ammoCount;
        }
    }
}
