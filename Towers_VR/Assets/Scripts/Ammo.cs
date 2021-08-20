using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AmmoCaliber
{
	Flak,
	Medium,
	Heavy,
    Extreme
}


[System.Serializable]
public class Ammunition
{
	public Ammunition(AmmoObjectIdentifier ammoObject){
		_bullet = ammoObject.bullet;
		_velocity = ammoObject.velocity;
		_caliber = ammoObject.caliber;
		_ammoCount = ammoObject.ammoCount;
	}
	public Ammunition(Object newBullet, float newVelocity, AmmoCaliber newCaliber, int newAmmoCount){
		_bullet = newBullet;
		_velocity = newVelocity;
		_caliber = newCaliber;
		_ammoCount = newAmmoCount;
	}
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

        set {
            _ammoCount = value;
        }
    }
}
/*
*/
