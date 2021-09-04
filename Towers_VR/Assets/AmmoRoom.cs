using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoRoom : Room
{
    [System.Serializable]
    public class AmmoGroup
    {
        public enum AmmoGroupType
        {
            Explosive,
            ArmorPiercing
        }
        public AmmoGroupType Type;
        public List<AmmoObjectIdentifier> ammo;
    }
    [SerializeField] List<AmmoGroup> AmmoGroups;
    public List<AmmoGroup> GetAmmo(){
        return AmmoGroups;
    }
}
