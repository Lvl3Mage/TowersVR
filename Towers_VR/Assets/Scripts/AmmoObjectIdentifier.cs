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
    [Tooltip("Defined in percentage of the velocity (multiply the velocity by the percentage to get the max meters a projectile can be off after traveling for 1 second)")]
    [Range(0f,0.05f)] public float spread; // percentage of the velocity
}
