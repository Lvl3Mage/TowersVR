using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ParticipantConfigurationCard : MonoBehaviour
{
    BaseParticipant AssignedParticipant;
    [SerializeField] Sprite[] PlayerTypeSprites;
    [SerializeField] Image PlayerTypeImage;
    [SerializeField] TMP_Dropdown WeaponDropdown;
    public void SetParticipant(BaseParticipant newparticipant){
        AssignedParticipant = newparticipant;
        PlayerTypeImage.sprite = PlayerTypeSprites[(int)AssignedParticipant.playerType];

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach(WeaponConfigType val in WeaponConfigType.GetValues(typeof(WeaponConfigType))) {
            options.Add(new TMP_Dropdown.OptionData(val.ToString()));
        }
        WeaponDropdown.ClearOptions();
        WeaponDropdown.AddOptions(options);
        WeaponDropdown.value = (int)AssignedParticipant.participantConfiguration.WeaponConfiguration;
    }
    public BaseParticipant GetParticipant(){
        return AssignedParticipant;
    }
    public void SetWeaponType(int Type){
        Debug.Log("Changed to " + Type);
        AssignedParticipant.participantConfiguration.WeaponConfiguration = (WeaponConfigType)Type;
    }
}
