using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoRoom : Room
{
    [System.Serializable]
    public class FiringGroup
    {
        [System.Serializable]
        public class FiringStage
        {
            [SerializeField] string stageName = "FiringStage";
            public int stageLength;
            public AmmoGroup.AmmoGroupType type;
        }
        [System.Serializable]
        public class AmmoGroup
        {
            public enum AmmoGroupType
            {
                Explosive,
                ArmorPiercing
            }
            [SerializeField] string groupName = "GroupName";
            public AmmoGroupType type;
            public List<AmmoObjectIdentifier> ammo;
        }


        int RoundsFired = 0;

        [SerializeField] FiringStage[] firingCycle;
        [SerializeField] List<AmmoGroup> ammoGroups;
        

        public AmmoObjectIdentifier GetNextRound(){
            //selecting the firing stage using the number of shells fired (RoundsFired)
            FiringStage currentStage = GetCurrentStage();
            //selecting the first ammo group that matches the type of the (currentStage). We also delete groups that have no more ammo in them here
            AmmoGroup currentAmmoGroup = GetAmmoGroupFromStage(currentStage);
            // getting the first non null ammo in the (currentAmmoGroup). we also delete this ammo from the group
            AmmoObjectIdentifier ammunition = GetRoundFromGroup(currentAmmoGroup);
            //incrementing the rounds fired
            RoundsFired ++;
            return ammunition;
            
            
        }
        AmmoObjectIdentifier GetRoundFromGroup(AmmoGroup group){
            AmmoObjectIdentifier ammunition = null;
            for(int i = group.ammo.Count-1; i >= 0; i--){
                //saving the ammo
                ammunition = group.ammo[i];

                //removing the selected ammo after remembering it (it has to be removed either way)
                group.ammo.RemoveAt(i); // removing the ammo works because the group is passed by REFERENCE

                if(ammunition != null){ // if the ammo is valid
                    break; // we have found the needed ammo;
                    
                }
            }
            if(ammunition == null){
            }
            return ammunition;
        }
        FiringStage GetCurrentStage(){ // this runs through every stage to find the current one (a less flexible approach would be to save the last stage and cound the ammo from there)
            int stageEnd = 0;
            for(int i = 0; i < firingCycle.Length; i++){
                //add the next amount of rounds to the end
                stageEnd += firingCycle[i].stageLength;

                //checks if that exceeds the current round id 
                if(RoundsFired<stageEnd){
                    //if that is the case then we have found the current stage
                    return firingCycle[i];
                }
            }
            //if we have gotten this far that means that the amount of rounds fired has exceded the firing cycle length
            //in this case we reset that amount to 0 and return the first stage (essentially looping around the cycle)
            RoundsFired = 0;
            return firingCycle[0];
        }
        AmmoGroup GetAmmoGroupFromStage(FiringStage stage){
            for(int i = 0; i < ammoGroups.Count; i++){
                if(ammoGroups[i].ammo.Count > 0){ // if the group isn't empty
                    if(ammoGroups[i].type == stage.type){ // check if it's type matches the given stage
                        return ammoGroups[i]; // if so then we have found the needed group
                    }
                }
                else{
                    ammoGroups.RemoveAt(i); // if the group has no ammo left in it it should be removed from the list 
                }
            }
            // if no groups are left then we have run out of ammo
            return null;
        }
        
    }
    
    [SerializeField] FiringGroup firingGroup;
    public FiringGroup GetAmmo(){
        return firingGroup;
    }
}
