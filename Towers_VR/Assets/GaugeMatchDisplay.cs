using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaugeMatchDisplay : MultipleNumberContainer
{
	[SerializeField] Transform baseArrow;
	[SerializeField] Transform matchingArrow;
	[SerializeField] Vector2 valueRange;
	[SerializeField] Vector2 positionRange;
    protected override void OnListChange(int id){
    	float newBaseValue = NumberList[0].value; // the value of the base arrow

    	Vector3 newBasePosition = baseArrow.transform.localPosition; // sets the new position to the old one
    	newBasePosition.x = ChangeRange(newBaseValue, valueRange, positionRange); // modifies the x coordinate with the new range
    	newBasePosition.x = Mathf.Clamp(newBasePosition.x, positionRange.x, positionRange.y); // clams it to the new range
    	Debug.Log( NumberList[0].value);
    	baseArrow.transform.localPosition = newBasePosition;


    	float newMatchingValue = NumberList[1].value; // the value of the matching arrow

    	Vector3 newMatchingPosition = matchingArrow.transform.localPosition; // sets the new position to the old one
    	newMatchingPosition.x = ChangeRange(newMatchingValue, valueRange, positionRange); // modifies the x coordinate with the new range
    	newMatchingPosition.x = Mathf.Clamp(newMatchingPosition.x, positionRange.x, positionRange.y); // clams it to the new range
    	matchingArrow.transform.localPosition = newMatchingPosition;

    }
    float ChangeRange(float value, Vector2 OldRange, Vector2 NewRange){
		float NewValue = (((value - OldRange.x) * (NewRange.y - NewRange.x)) / (OldRange.y - OldRange.x)) + NewRange.x;
		return NewValue;
	}
}
