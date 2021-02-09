using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{

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
