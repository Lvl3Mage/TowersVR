using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReloader : MonoBehaviour
{
	[SerializeField] protected Weapon weapon;
	[SerializeField] IntContainer[] Callback; // Calls back to an int container (The states of the said container are 1 for loaded, 2 for loading, 3 for unloaded)
	[SerializeField] int AmmoType; // 0 for cannon 
	protected bool Loading = false; // determines when the weapon can be loaded

    void OnTriggerEnter(Collider other)
    {
    	AmmoIdentifier Shell = other.gameObject.GetComponent<AmmoIdentifier>();
    	if(Shell != null){
    		if(Loadable(Shell)){
				LoadWeapon(Shell);
	    	}
    	}
    }
    public bool LoadShell(AmmoIdentifier Shell){ // Should load a shell
    	bool _Loadable = Loadable(Shell);
    	if(_Loadable){
    		LoadWeapon(Shell);
    	}
    	return _Loadable;
    }
    bool Loadable(AmmoIdentifier Shell){
    	return !weapon.Loaded() && (Shell.type == AmmoType) && !Loading;
    }
    protected virtual void LoadWeapon(AmmoIdentifier Shell){} // Called when weapon is being reloaded
    
    protected void LoadingCallback(int val){
    	foreach (IntContainer Cont in Callback) 
    	{
    		Cont.intValue = val;
    	}
    }
}

