using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Relay // manages integrity and structure
{
	[Tooltip("Keypoints that indicate structuraly important pounts of the tower")]
	[SerializeField] protected TowerKeypoint[] StructuralKeypoints;
	[Tooltip("Keypoints that indicate the integrity of the tower")]
	[SerializeField] protected TowerKeypoint[] IntactKeypoints;
	[Tooltip("The minimum percentage of keypoints that should remain intact for the tower to be considered as intact aswell")]
	[SerializeField] [Range(0f, 1f)] float MinIntactPercentage;
	protected GameManager GameManager;
	protected bool intact = true;

	void Start(){
		GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}
	public void RecalculateIntegrity(){
		int intactPoints = RecalculateKeypoints(IntactKeypoints);

		float intactPercent = ((float)intactPoints)/((float)IntactKeypoints.Length);

		intact = intactPercent>MinIntactPercentage;
		if(!intact){

		}
	}
	protected virtual void OnDestroy(){ // called when the tower is destroyed

	}
	public TowerKeypoint[] GetStructureKeypoints(){
		RecalculateKeypoints(StructuralKeypoints);
		List<TowerKeypoint> ActiveKeypoints = new List<TowerKeypoint>();
		for(int i = 0; i<StructuralKeypoints.Length; i++){
			if(StructuralKeypoints[i].intact){
				ActiveKeypoints.Add(StructuralKeypoints[i]);
			}
		}
		return ActiveKeypoints.ToArray();
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
