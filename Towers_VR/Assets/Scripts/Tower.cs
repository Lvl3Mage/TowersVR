using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
	[Tooltip("Keypoints that indicate structuraly important pounts of the tower")]
	[SerializeField] protected TowerKeypoint[] StructuralKeypoints;
	[Tooltip("Keypoints that indicate the integrity of the tower")]
	[SerializeField] protected TowerKeypoint[] IntactKeypoints;
	[Tooltip("The minimum percentage of keypoints that should remain intact for the tower to be considered as intact aswell")]
	[SerializeField] [Range(0f, 1f)] float MinIntactPercentage;
	protected bool intact = true;
	public void RecalculateIntegrity(){
		int intactPoints = RecalculateKeypoints(IntactKeypoints);

		float intactPercent = ((float)intactPoints)/((float)IntactKeypoints.Length);
		intact = intactPercent>MinIntactPercentage;
	}
	public void RecalculateStructure(){
		RecalculateKeypoints(StructuralKeypoints);
	}
	int RecalculateKeypoints(TowerKeypoint[] points){ // returns how many active there are
		int intactkeypoints = 0;
		foreach(TowerKeypoint keypoint in points){
			keypoint.RecalculateKeypoint();
			if(keypoint.intact){
				intactkeypoints ++;
			}
		}
		return intactkeypoints;
	}
	public bool TowerIntact(){
		return intact;
	}    
}
