using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GaugeArrow{
	public float value;

	public Transform transform;
	public MeshRenderer meshRenderer;
	[HideInInspector] public bool enabled;
	[Tooltip("The id of the material in the materials array that should be recolored")]
	[SerializeField] int recolorMaterialID;
	[Tooltip("The index of the other arrow this one will be attracted to (set to -1 if none)")]
	[SerializeField] int attractedTo;
	public bool DisableOutOfRange;
	public bool FloorNullValues;
	public void ReEvaluateColor(Gradient ColorGradient, GaugeArrow[] Arrows, float valRadius){
		if(enabled){ // only if this arrow is enabled
			if(attractedTo > 0){
				GaugeArrow AtractedToArrow = Arrows[attractedTo];
				float EvaluationValue;
				if(AtractedToArrow.enabled){ // chack if the other arrow is enabled
					float AttractedValue = AtractedToArrow.value;
					float difference = Mathf.Abs(value - AttractedValue);
					EvaluationValue = difference/valRadius;
				}
				else{
					EvaluationValue = 1;
				}
				
				SetColor(ColorGradient.Evaluate(EvaluationValue));
			}
			else{
				SetColor(ColorGradient.Evaluate(0));
			}
		}
		
	}
	void SetColor(Color newColor){
		Material Material = meshRenderer.materials[recolorMaterialID];
		Material.SetColor("_UnlitColor",newColor);
	}
}
public class GaugeDisplay : MultipleNumberContainer
{
	[Tooltip("An array of the gauge controlled arrows (tip: each input element should have its corresponding arrow in this array)")]
	[SerializeField] GaugeArrow[] Arrows;

	[Tooltip("The maximum and minimum input values this display will recieve")]
	[SerializeField] Vector2 valueRange;
	[Tooltip("The lowest and highest positions of the arrows")]
	[SerializeField] Vector2 positionRange;
	[Tooltip("The color of the arrows based on how close they are to their attracted to arrow")]
	[SerializeField] Gradient ArrowAtractionGradient;
	[Tooltip("Defines the outer radius of the attraction gradient")]
	[SerializeField] float attractionValueRadius;
    protected override void OnListChange(int id){
    	GaugeArrow ChangedArrow = Arrows[id];
    	bool DisableOutOfRange = ChangedArrow.DisableOutOfRange;
    	bool FloorNullValues = ChangedArrow.FloorNullValues;
    	

    	float newValue = NumberList[id].value; // the value of the base arrow
    	ChangedArrow.value = newValue;

    	if(newValue != newValue && !FloorNullValues){
			
    		ChangedArrow.enabled = false;
    		ChangedArrow.meshRenderer.enabled = false;
    	}
    	else if(!CheckRange(newValue, valueRange) && DisableOutOfRange){
    		
			ChangedArrow.enabled = false;
			ChangedArrow.meshRenderer.enabled = false;
	    		
    	}
    	else{
    		if(newValue != newValue){ // if the value is null then it means that we should floor it (because otherwise it would have been disabled)
    			newValue = valueRange.x;
    			ChangedArrow.enabled = false; // we also disable the arrow
    			ChangedArrow.meshRenderer.enabled = true; //but we enable it's mesh renderer because it needs to be shown in this case
    		}
    		else if(!ChangedArrow.enabled || !ChangedArrow.meshRenderer.enabled){ // in case the value isn't null then that means the arrow is ready to be used so we enable it if needed
	    		ChangedArrow.enabled = true;
	    		ChangedArrow.meshRenderer.enabled = true;
	    		
	    	}

	    	//Updates arrow's position
	    	Vector3 newPosition = ChangedArrow.transform.localPosition; // sets the new position to the old one
		    newPosition.x = ChangeRange(newValue, valueRange, positionRange); // modifies the x coordinate with the new range
		    newPosition.x = Mathf.Clamp(newPosition.x, positionRange.x, positionRange.y); // clams it to the new range
		    ChangedArrow.transform.localPosition = newPosition;
    	}

	    	


    	ReEvaluateAllArrowColor();
    }
    void ReEvaluateAllArrowColor(){ // ReEvaluates all the arrow's color
    	foreach(GaugeArrow Arrow in Arrows){
    		Arrow.ReEvaluateColor(ArrowAtractionGradient, Arrows, attractionValueRadius);
    	}
    }
    float ChangeRange(float value, Vector2 OldRange, Vector2 NewRange){
		float NewValue = (((value - OldRange.x) * (NewRange.y - NewRange.x)) / (OldRange.y - OldRange.x)) + NewRange.x;
		return NewValue;
	}
	bool CheckRange(float value, Vector2 Range){
		return (value>=Range.x && value<=Range.y);
	}
}
