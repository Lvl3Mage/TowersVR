using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
	[Header("Explosion Quality Settings")] 
	[Tooltip("Casts rays towards objects to determine whether they are reachable")] 
	[SerializeField] bool RaycastObjects;
	[Tooltip("Layers that will block the raycast")]
	[SerializeField] LayerMask Blocking;
	[Tooltip("Calculates the contact point based on the closest point to the object collider instead of the object position")] 
	[SerializeField] bool SmartExplosion;
	[Tooltip("Damage is applied before the forces")] 
	[SerializeField] bool DamageFirst;


	[Header("Explosion Parameters")]	
	[Tooltip("The layers affected by the explosion")]
	[SerializeField] LayerMask AffectedLayers;
	[Tooltip("Explodes the object on creation")]
	[SerializeField] bool ExplodeOnStart;
	[Tooltip("The radius in which the objects will be affected by the explosion")]
	[SerializeField] float ExplosionRadius;
	[Tooltip("The Force Falloff curve where time 0 is in the immediate position of the explosion and time 1 at the end range of the explosion")]
	[SerializeField] AnimationCurve ForceCurve;
	[Tooltip("The Damage Falloff curve where time 0 is in the immediate position of the explosion and time 1 at the end range of the explosion")]
	[SerializeField] AnimationCurve DamageCurve;
	[Tooltip("The force of the explosion applied to an object in the immediate position of the explosion (Further away forces will only get closer to 0)")]	
	[SerializeField] float BaseExplosionForce;
	[Tooltip("The damage applied to an object in the immediate position of the explosion (Further away damage will only get closer to 0)")]
	[SerializeField] float BaseDamage;

	[Header("Gizmo Settings")]
	[SerializeField] bool DrawGizmos;
	[Range(0f,1f)] [SerializeField] float Transparency;

    void Start()
    {
        if(ExplodeOnStart){
        	Explode();
        }
    }

    [ContextMenu("Explode")]
    public void Explode(){
    	OnExplosion();
    }
    protected virtual void OnExplosion(){ // this can supposadly be overriden to delay the explosion or something alike
    	ApplyExplosion();
    }

    protected void ApplyExplosion(){ // this method could be set to virtual for future overriding in theory but it is written for maximum performance so re writing it would be wierd + you would be using a whole bunch of unncecesary methods that can't be overriden cause thay have very specific parameters. I think that for now it's best to have a separate OnExpode() method that can be overriden but not much else

    	Vector3 ExplosionPoint = transform.position;
    	Collider[] AffectedObjects = GetObjectsInRadius(ExplosionRadius,AffectedLayers);
    	foreach (Collider Col in AffectedObjects) 
    	{
    		bool Reachable;
    		if(RaycastObjects){ // if raycast are enabled then it will check, if they are not then it assumes that all objects are available
    			Reachable = CheckAvailabuility(Col);
    		}
    		else{
    			Reachable = true;
    		}
    		if(Reachable){
	    		Rigidbody AffectedRB = Col.attachedRigidbody;
	    		GameObject Object;
	    		if(AffectedRB){
	    			Object = AffectedRB.gameObject;
	    		}
	    		else{
	    			Object = Col.gameObject;
	    		}
	    		DestructableObject Destructable = Object.GetComponent<DestructableObject>(); //Couldn't get around this one here because the DestructableObject isn't referenced anywhere (a possible optimization would be to place every Destructable Object inside a destruct layer so you wouldn't have to check every object for the script. It is useless tho cause most object affected would be destructable)

			    // this all could be placed in another if (Rigidbody || Destructable) but doesn't seem neccesary 
			    
			    //Calculating vector and contact point of the explosion
			    Vector3 ContactPoint; // the point on the object where the force and damage is applied
				if(SmartExplosion){
					ContactPoint = Col.ClosestPoint(ExplosionPoint); // this should be blended with the center point of an object with distance
				}
				else{
					ContactPoint = Object.transform.position;
				}
				Vector3 ContactVector = ContactPoint - ExplosionPoint;
	    		
	    		// Applying the force to the affected object if it has a rigidbody
	    		if(!DamageFirst){
		    		if(AffectedRB){
						if(BaseExplosionForce != 0){//Checking if the explosion Force isn't 0 to prevent unnecesary calculations and forces
		    				float Force = SampleCurve(ContactVector.magnitude, ExplosionRadius,BaseExplosionForce,ForceCurve);
		    				ApplyForce(AffectedRB, Force * ContactVector.normalized, ContactPoint); // Applying the force
		    			}
		    		}
	    		}

	    		if(Destructable){
					if(BaseDamage != 0){//Checking if the damage isn't 0 to prevent unnecesary calculations and damage application
	    				float Damage = SampleCurve(ContactVector.magnitude, ExplosionRadius, BaseDamage,DamageCurve);
	    				ApplyDamage(Destructable, Damage); // Applying the Damage
	    			}
	    		}
    		}
    	}

    	if(DamageFirst){
    		AffectedObjects = GetObjectsInRadius(ExplosionRadius,AffectedLayers);
	    	foreach (Collider Col in AffectedObjects) 
	    	{
	    		bool Reachable;
	    		if(RaycastObjects){ // if raycast are enabled then it will check, if they are not then it assumes that all objects are available
	    			Reachable = CheckAvailabuility(Col);
	    		}
	    		else{
	    			Reachable = true;
	    		}
	    		if(Reachable){
		    		Rigidbody AffectedRB = Col.attachedRigidbody;
		    		GameObject Object;
		    		if(AffectedRB){
		    			Object = AffectedRB.gameObject;
		    		}
		    		else{
		    			Object = Col.gameObject;
		    		}
				    
				    //Calculating vector and contact point of the explosion
				    Vector3 ContactPoint; // the point on the object where the force and damage is applied
					if(SmartExplosion){
						ContactPoint = Col.ClosestPoint(ExplosionPoint); // this should be blended with the center point of an object with distance
					}
					else{
						ContactPoint = Object.transform.position;
					}
					Vector3 ContactVector = ContactPoint - ExplosionPoint;
		    		
		    		// Applying the force to the affected object if it has a rigidbody
		    		if(AffectedRB){
						if(BaseExplosionForce != 0){//Checking if the explosion Force isn't 0 to prevent unnecesary calculations and forces
		    				float Force = SampleCurve(ContactVector.magnitude, ExplosionRadius,BaseExplosionForce,ForceCurve);
		    				ApplyForce(AffectedRB, Force * ContactVector.normalized, ContactPoint); // Applying the force
		    			}
		    		}
	    		}
	    	}
    	}
    }
    bool CheckAvailabuility(Collider col){ // Checks if an object is available through raycast
    	RaycastHit hit;
    	Vector3 Dir = col.bounds.center - transform.position; // The directional Vector is calculated using the collider's position as that would make more sence (not rigidbody's cause it might be out of reach)
    	Debug.DrawRay(transform.position, Dir,Color.green,0.1f);
    	if(Physics.Raycast(transform.position, Dir, out hit, ExplosionRadius, Blocking, QueryTriggerInteraction.Ignore)){ // Checks if the hit occured - if it has then it checks if with the right object, if it didn't that means that the object was simply not on the blocking layer
	    	return hit.collider == col; //returns whether the collided object is equal to the one we were checking for	
    	}
    	else{
    		return true;
    	}
    }
    void ApplyDamage(DestructableObject destrct, float Damage){
    	destrct.DamageObject(Damage);
    }
    void ApplyForce(Rigidbody RB, Vector3 Force, Vector3 Point){
    	RB.velocity = Force;
    	//RB.AddForceAtPosition(Force, Point, ForceMode.Impulse);
    }
    float SampleCurve(float Distance, float MaxDistance, float Base, AnimationCurve Curve){
    	if(MaxDistance != 0){
    		float Value = Curve.Evaluate(Distance/MaxDistance);
    		return Base*Value;
    	}
    	else{
    		return 0;
    	}
    	
    }

    Collider[] GetObjectsInRadius(float r, LayerMask Mask){
    	Collider[] ColliderArray = Physics.OverlapSphere(transform.position, ExplosionRadius, Mask, QueryTriggerInteraction.Ignore);
    	return ColliderArray;
    }
    void OnDrawGizmos(){
    	if(DrawGizmos){
    		Color Color = Color.red;
    		Color.a = Transparency;
	    	Gizmos.color = Color;
	    	Gizmos.DrawSphere(transform.position, ExplosionRadius);	
    	}
    }
}
