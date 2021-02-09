using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
	public enum CursorState{Enabled, Disabled, Untouched}
	public CursorState CursorLockState = CursorState.Enabled;

	void Awake(){
		LoadSceneCongfiguration();
	}
	void LoadSceneCongfiguration(){
		if(CursorLockState == CursorState.Enabled){
			Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
		}
		else if(CursorLockState == CursorState.Disabled) {
			Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
		}
	}
    public void LoadScene(int id){
    	SceneLoadRequest(id);
    }
    public void ReloadScene(){
    	SceneReloadRequest();
    }
    protected virtual void SceneLoadRequest(int SceneId){}
    protected virtual void SceneReloadRequest(){

    }
}
