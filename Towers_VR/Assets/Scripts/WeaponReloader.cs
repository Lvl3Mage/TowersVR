using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReloader : MonoBehaviour
{
	protected Weapon weapon;
	[SerializeField] DataContainer[] LoadstateCallback; // Calls back to an int container (The states of the said container are 1 for loaded, 2 for loading, 3 for unloaded)
	[SerializeField] AmmoCaliber WeaponCaliber;
	protected bool Loading = false; // determines when the weapon can be loaded

    void OnTriggerEnter(Collider other)
    {
    	AmmoObjectIdentifier Ammo = other.gameObject.GetComponent<AmmoObjectIdentifier>();
    	if(Ammo != null){
    		if(Loadable(Ammo.caliber)){
				LoadAmmunitionObject(Ammo);
	    	}
    	}
    }
    public bool LoadAmmo(AmmoObjectIdentifier Ammo){ // Call to load an ammo object
    	bool _Loadable = Loadable(Ammo.caliber);
    	if(_Loadable){
    		LoadAmmunitionObject(Ammo);
    	}
    	return _Loadable;
    }
    public bool LoadAmmo(Ammunition Ammo){ // Call to load ammo
        bool _Loadable = Loadable(Ammo.caliber);
        if(_Loadable){
            LoadAmmunition(Ammo);
        }
        return _Loadable;
    }
    bool Loadable(AmmoCaliber Caliber){
    	return !weapon.Loaded() && weapon.Loadable(Caliber) && !Loading;
    }
    protected virtual void LoadAmmunitionObject(AmmoObjectIdentifier Ammo){} // Called when weapon is being reloaded
    protected void LoadAmmunition(Ammunition Ammo){
        weapon.Reload(Ammo);
    } // Loads an Ammunition class object into the cannon

    protected void LoadingCallback(int val){
    	foreach (DataContainer Cont in LoadstateCallback) 
    	{
    		Cont.SetValue(DataType.CannonLoadState, val);
    	}
    }
}

