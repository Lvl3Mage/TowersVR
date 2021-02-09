using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PCMenuManger : MonoBehaviour
{
	[SerializeField] CanvasGroup BaseCanvas;
	[SerializeField] float FadeSpeed;
	[SerializeField] PCLookAround PlayerCamera;
	[SerializeField] KeyCode MenuOpenKey;
	bool MenuState = false, Fading = false;
    Coroutine FadeAction;
	void Update(){
		if(Input.GetKeyDown(MenuOpenKey)){
			SetMenuState(!MenuState);
		}
	}
    IEnumerator FadeCanvas(float Target){ // Fades the canvas to a specified value
    	while(Mathf.Abs(BaseCanvas.alpha - Target) > 0.01f){
    		BaseCanvas.alpha = Mathf.Lerp(BaseCanvas.alpha, Target, Time.deltaTime*FadeSpeed);
    		yield return null;
    	}
    	BaseCanvas.alpha = Target;
    	Fading = false;
    }
    public void SetMenuState(bool SetValue){// Function to either enable or disable the munu
        if(FadeAction != null){ // if a fade coroutine is already running it will be stopped before the new one is initiated
            StopCoroutine(FadeAction);
        }
        SetMenu(SetValue);
    }
    void SetMenu(bool SetValue){ //Don't call this method directly, call SetMenuState instead
    	MenuState = SetValue;
    	if(SetValue){
    		PlayerCamera.SetLookAround(false); // Disabling player camera rotation
    		BaseCanvas.interactable = true; // Enabling menu interaction
	    	BaseCanvas.blocksRaycasts = true;

	    	FadeAction = StartCoroutine(FadeCanvas(1)); // Fading the menu in
    	}
    	else{
    		PlayerCamera.SetLookAround(true); // Enabling player camera rotation
    		BaseCanvas.interactable = false; // Disabling menu interaction
	    	BaseCanvas.blocksRaycasts = false;
	    	FadeAction = StartCoroutine(FadeCanvas(0)); // Fading the menu out
    	}
    }
}
