using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loadbar_SL_UnloadSettings : LoadbarSceneChanger
{
	[SerializeField] bool EnableCursor;
    protected override void PreSceneLoad(){
    	if(EnableCursor){
    		Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
    	}
    }
}
