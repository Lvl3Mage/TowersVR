using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoIdentifier : MonoBehaviour
{

	[SerializeField] protected Object _self;
    public Object self {

        get {
            return _self;
        }

        private set {
            _self = value;
        }
    }
    [SerializeField] protected Object _bullet;
    public Object bullet {

        get {
            return _bullet;
        }

        private set {
            _bullet = value;
        }
    }
    [SerializeField] protected float _velocity;
    public float velocity {

        get {
            return _velocity;
        }

        private set {
            _velocity = value;
        }
    }
    [SerializeField] protected int _type;
    public int type {

        get {
            return _type;
        }

        private set {
            _type = value;
        }
    }
    [SerializeField] protected int _count;
    public int count {

        get {
            return _count;
        }

        set {
            _count = value;
        }
    }
    
	/*[SerializeField] Object AmmoType;
	[SerializeField] float velocity;
	[SerializeField] int Type, Count;
	public int GetAmmoType(){
    	return Type;
    }
    public Object GetShell(){
    	return AmmoType;
    }
    public float GetVelocity(){
    	return velocity;
    }
    public int GetCount(){
    	return Count;
    }
    public int SetCount(){
    	return Count;
    }*/
}
