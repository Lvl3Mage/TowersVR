using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GaugeArrow{
	[HideInInspector] public float value;

	public Transform transform;
	public MeshRenderer meshRenderer;
	[HideInInspector] public bool enabled;
	[Tooltip("The id of the material in the materials array that should be recolored")]
	[SerializeField] int recolorMaterialID;
	[Tooltip("The index of the other arrow this one will be attracted to (set to -1 if none)")]
	[SerializeField] int attractedTo;
	public void ReEvaluateColor(Gradient ColorGradient, GaugeArrow[] Arrows, float valRadius, bool DisableNegative){
		if(enabled){
			if(attractedTo > 0){
				GaugeArrow AtractedToArrow = Arrows[attractedTo];
				float EvaluationValue;
				if(AtractedToArrow.enabled){
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

	[SerializeField] bool DisableNegativeArrows;
    protected override void OnListChange(int id){
    	GaugeArrow ChangedArrow = Arrows[id];

    	float newValue = NumberList[id].value; // the value of the base arrow

    	ChangedArrow.value = newValue; // updates the value of the arrow no matter what

    	if((DisableNegativeArrows && (newValue < 0)) || (newValue != newValue)){ // if the value is null or if it is negative and disable negative is set to true
			//Disables the mesh renderer if needed
	    	if(ChangedArrow.enabled || ChangedArrow.meshRenderer.enabled){
	    		ChangedArrow.enabled = false;
	    		ChangedArrow.meshRenderer.enabled = false;
	    		
	    	}
    	}
    	else{
    		if(!ChangedArrow.enabled || !ChangedArrow.meshRenderer.enabled){
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
    		Arrow.ReEvaluateColor(ArrowAtractionGradient, Arrows, attractionValueRadius, DisableNegativeArrows);
    	}
    }
    float ChangeRange(float value, Vector2 OldRange, Vector2 NewRange){
		float NewValue = (((value - OldRange.x) * (NewRange.y - NewRange.x)) / (OldRange.y - OldRange.x)) + NewRange.x;
		return NewValue;
	}
}
