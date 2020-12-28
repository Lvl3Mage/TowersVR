using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{

	public Object DestroyedObj;
	public float health, damagemultipier = 0.05f;
    public bool DestroyOnTerrain;
    public GameObject Terrain;
    bool Spawned = false;
    void OnCollisionEnter(Collision other){
        if(DestroyOnTerrain){
            if(other.gameObject == Terrain){
                health = 0;
            }
        }
        float mult;
        if(other.gameObject.GetComponent<Rigidbody>() != null){
            mult = other.gameObject.GetComponent<Rigidbody>().mass;
        }
        else{
            mult = 1/damagemultipier;
        }
        if(other.relativeVelocity.magnitude * mult * damagemultipier > 40){
	        health -= other.relativeVelocity.magnitude * mult * damagemultipier;
        }
        if(health<=0){
            if(!Spawned){
                Spawned = true;
                GameObject Broken = Object.Instantiate(DestroyedObj, transform.position, transform.rotation) as GameObject;
                Rigidbody[] RBs = Broken.GetComponentsInChildren<Rigidbody>();
                Rigidbody MYRB = GetComponent<Rigidbody>();
                foreach (Rigidbody RB in RBs) 
                {
                    RB.velocity = MYRB.velocity;
                }
                gameObject.SetActive(false);
            }
            
        } 

    }
}
