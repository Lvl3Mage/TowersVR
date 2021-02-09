using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LoadInputManager : MonoBehaviour
{
	SceneChanger SceneChanger;
	void Start(){
		GameObject[] FoundObjects = GameObject.FindGameObjectsWithTag("SceneChanger");
		foreach (GameObject FoundObject in FoundObjects) 
		{
			if(FoundObject.scene == gameObject.scene){
				SceneChanger = FoundObject.GetComponent<SceneChanger>();
				break;
			}
		}
		if(!SceneChanger){
			Debug.LogError("No SceneChanger found in the scene");
		}
	}
    public void LoadScene(int id){
    	SceneChanger.LoadScene(id);
    }
    public void RestartLevel(){
    	SceneChanger.ReloadScene();
    }
}
