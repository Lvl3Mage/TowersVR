using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITowerController : TowerController
{
	
	[SerializeField] AITower SelfTower;
	[SerializeField] Transform gunPoint;
	[SerializeField] Ammunition[] AIExampleAmmo;
	[Header("Aim Settings")]
	[Tooltip("The time ")]
	[SerializeField] float PerRotationDegreeWait;
	[SerializeField] float PerRotationWait;
	[SerializeField] Vector2 AmmoLoadTimeRange;
	[Tooltip("The cannon innacuracy during the first shot. The x value represents the vertical innacuracy and the y represents the horizontal innacuracy")]
	[SerializeField] Vector2 FirstShotInnacuracy;
	[SerializeField] CorrectionVariation[] CorrectionVariations; 
    void Start()
    {
    	StartCoroutine(AICycle());	
    }
    IEnumerator AICycle(){
    	yield return null; // waits till initialization
    	while(SelfTower.TowerIntact() || SelfTower.GameRunning()){
    		Tower TargetTower = GetClosestTower();
    		TowerKeypoint TargetKeypoint = SelectTowerKeypoint(TargetTower);
    		yield return StartCoroutine(ShootKeypoint(TargetKeypoint,TargetTower));

    	}
    }
    IEnumerator ShootKeypoint(TowerKeypoint keypoint, Tower TargetTower){
    	// First unmodified aiming
    	Transform keypointTransform = keypoint.gameObject.transform;
    	//Loading ammo
		yield return StartCoroutine(LoadAmmunition(0));

    	Vector2 Aim;
    	Aim.x = CalculateXAim(keypointTransform.position);
    	Aim.y = CalculateYAim(keypointTransform.position);

    	//Adding first shot innacuracy
    	Vector2 innacuracy;
    	innacuracy.x = Random.Range(-FirstShotInnacuracy.x, FirstShotInnacuracy.x);
    	innacuracy.y = Random.Range(-FirstShotInnacuracy.y, FirstShotInnacuracy.y);
    	Aim += innacuracy;

		yield return StartCoroutine(AimAtAngles(Aim)); // waits for the tower to aim
		
		//Following adjustment aiming
		while(SelfTower.TowerIntact() && TargetTower.TowerIntact() && keypoint.intact){ // will stop if the tower breaks / enemy tower breaks / keypoint breaks
			FireCannon();
			GameObject ProjectileObject = GetLastProjectile();
			Projectile Projectile = ProjectileObject.GetComponent<Projectile>();

			Vector3 PastProjectilePosition = ProjectileObject.transform.position; 
			float minimumDistance = Mathf.Infinity; // the minimum distance the projectile has been from the target so far
			while(!Projectile.HasCollided()){ // waits till the projectile has collided or started moving away from the keypoint

				float newDistance = Vector3.Distance(ProjectileObject.transform.position,keypointTransform.position); // calculates the distance form projectile to the transform
				if(minimumDistance>=newDistance){
					minimumDistance = newDistance;
					PastProjectilePosition = ProjectileObject.transform.position;
				}
				else{
					break; // if the projectile has moved away from the keypoint then break 
				}

				yield return null;
			}

			Vector3 impactPoint;
			if(Projectile.HasCollided()){
				impactPoint = ProjectileObject.transform.position; // the impact point will be the position of the projectile if it has collided

			}
			else{
				impactPoint = PastProjectilePosition; // if it didn't collide it means it passed by, so the impact point is the past projectile position
			}
			
			yield return StartCoroutine(LoadAmmunition(0));

			keypoint.RecalculateKeypoint();

			Vector2 AngleCorrection;
			float RealYAngle = CalculateYAim(impactPoint); // calculates the angle that the projectile really flew at (theoretically)
			float OriginalYAngle = CalculateYAim(keypointTransform.position); // the angle that the projectile should fly at
			AngleCorrection.y = OriginalYAngle - RealYAngle; // the y angle correction that is neccesary to hit the target 
			

			//Vector3 localImpactPoint = impactPoint - gunPoint.position;
			float RealXAngle = CalculateXAim(impactPoint);
			float OriginalXAngle = CalculateXAim(keypointTransform.position);
			AngleCorrection.x = OriginalXAngle - RealXAngle;

			CorrectionVariation correction = ChooseCorrection();
			if(correction != null){
				Vector2 VarRange = correction.GetRange();
				float variation = Random.Range(VarRange.x,VarRange.y);
				//Debug.Log("the chosen range is " + VarRange + " and the variation is " + variation);
				AngleCorrection *= variation;
			}
			Aim += AngleCorrection;
			//float distance = new Vector2(localImpactPoint.x, localImpactPoint.z).magnitude;
			/*float TheoreticalCollisionHeight = ProjectileMotion.ResolveParabolaForY(Aim.x,GetExitVelocity(),distance); // calculates how high the projectile really should have been when it collided
			float HeightCorrection = localImpactPoint.y - TheoreticalCollisionHeight;*/
			//Vector3 CorrectedTarget = keypointTransform.position + (Vector3.up*HeightCorrection); // will aim higher or lower depending on whether the projectile flew lower or higher than needed
			//Aim.x = CalculateXAim(CorrectedTarget);
			yield return StartCoroutine(AimAtAngles(Aim));
			yield return new WaitForSeconds(PerRotationWait);
    		
    	}
    }
    IEnumerator AimAtAngles(Vector2 Aim){
    	Vector2 PastAim = GetCurrentRotation();

    	if(float.IsNaN(Aim.x)){
			Debug.LogError("The selected tower is unreachable", gameObject);
		}
		else{
			SetXAngle(Aim.x);
			SetYAngle(Aim.y);
		}
		float rotationDelta = Mathf.Abs(PastAim.x-Aim.x) + Mathf.Abs(PastAim.y-Aim.y);//Should actually wait till the tower aims in the correct direction but for now this works
		yield return new WaitForSeconds(rotationDelta*PerRotationDegreeWait); // waits directly proportional to the delta of the turn
    }
    IEnumerator LoadAmmunition(int id){
    	Ammunition ExampleAmmo = AIExampleAmmo[0];
		LoadCannon(new Ammunition(ExampleAmmo.bullet, ExampleAmmo.velocity, ExampleAmmo.caliber, ExampleAmmo.ammoCount));
		yield return new WaitForSeconds(Random.Range(AmmoLoadTimeRange.x, AmmoLoadTimeRange.y)); // loading wait
    }
    CorrectionVariation ChooseCorrection(){
    	int totalChances = 0;
    	foreach(CorrectionVariation correction in CorrectionVariations){
    		totalChances += correction.GetChance();
    	}
    	int selection = Random.Range(0, totalChances);
    	int maxSelection = 0;
    	foreach(CorrectionVariation correction in CorrectionVariations){
    		maxSelection += correction.GetChance();
    		if(maxSelection>=selection){ // we have passed the selection so this is the chosen correction
    			return correction;
    		}
    	}
    	return null;
    }
    Tower GetClosestTower(){ // returns the closest tower (by angle to turn towards it)
    	TeamInstance[] EnemyTeams = SelfTower.GetEnemyTeams();
		float smallestAngle = 181f;
		Tower ClosestTower = null;
		foreach(TeamInstance EnemyTeam in EnemyTeams){
			Tower[] enemyTowers = EnemyTeam.towers;
			foreach(Tower enemyTower in enemyTowers){
				if(enemyTower.TowerIntact()){
					float angle = RotationAngleTo(enemyTower.gameObject.transform);
					if(angle <= smallestAngle){
						ClosestTower = enemyTower;
						smallestAngle = angle;
					}
				}
			}
		}
		return ClosestTower;

    }
    float RotationAngleTo(Transform aimObj){ // calculates the angle that the tower has to move to reach a target
			Vector3 AimDirection = aimObj.position - SelfTower.gameObject.transform.position;
			Vector3 CurrentAimDirection = gunPoint.forward;
			float angle = Vector3.Angle(AimDirection, CurrentAimDirection);
			return angle;
    }
    TowerKeypoint SelectTowerKeypoint(Tower Tower){// will select the lowest ones for now
    	float Heightgroupdistance = 1f; // the distance that a keypoint can be from the smallest height but still be considered viable for aiming
    	
    	TowerKeypoint[] keypoints = Tower.GetStructureKeypoints();
    	float lowestHeight = Mathf.Infinity;
    	float smallestDistance = Mathf.Infinity;
    	TowerKeypoint selectedkeypoint = null;
    	foreach(TowerKeypoint keypoint in keypoints){
    		Transform keypointTransform = keypoint.gameObject.transform;
    		if(Mathf.Abs(keypointTransform.position.y-lowestHeight)<Heightgroupdistance || keypointTransform.position.y<lowestHeight){ // if the keypoint y is smaller or close enough to the current lowest height
    			float distance = Vector3.Distance(transform.position, keypointTransform.position);
    			if(distance<smallestDistance){ // if it's the closest one yet
    				smallestDistance = distance;
    				selectedkeypoint = keypoint;
    				if(keypointTransform.position.y<lowestHeight){
    					lowestHeight = keypointTransform.position.y;
    				}
    			}
    		}
    	}
    	Debug.DrawRay(transform.position, selectedkeypoint.gameObject.transform.position-transform.position, Color.red, 0.5f);
    	return selectedkeypoint;

    }
    float CalculateXAim(Vector3 target){
    	float velocity = GetExitVelocity();

		Vector3 relTargetPos = target - gunPoint.position;
		Vector2 HrelPos = new Vector2(relTargetPos.x, relTargetPos.z);
		Vector2 DimensionalTargetPos = new Vector2(HrelPos.magnitude, relTargetPos.y); // creating a vector that would contain the distance to the target on the x and z axis as the x coordinate and the relative height as the y coordinate
		Debug.DrawRay(gunPoint.position,relTargetPos,Color.red,2f);
		float angle = ProjectileMotion.CalculateLaunchAngle(DimensionalTargetPos,velocity);

    	return angle;
    }
    float CalculateYAim(Vector3 target){
    	Vector3 relTargetPos = SelfTower.gameObject.transform.InverseTransformPoint(target);
		float angle = Mathf.Atan2(relTargetPos.x,relTargetPos.z);
		return angle * Mathf.Rad2Deg;
    }
}
[System.Serializable]
public class CorrectionVariation{
	[SerializeField] int CorrectionChance;
	public int GetChance(){
		return CorrectionChance;
	}
	[SerializeField] Vector2 Range;
	public Vector2 GetRange(){
		return Range;
	}
}
