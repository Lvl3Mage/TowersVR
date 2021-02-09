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
	void Update(){
		if(Input.GetKeyDown(MenuOpenKey) && !Fading){
			SetMenu(!MenuState);
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
    public void SetMenu(bool SetValue){ // Function to either enable or disable the munu
    	Fading = true;
    	MenuState = SetValue;
    	if(SetValue){
    		PlayerCamera.SetLookAround(false); // Disabling player camera rotation
    		BaseCanvas.interactable = true; // Enabling menu interaction
	    	BaseCanvas.blocksRaycasts = true;

	    	StartCoroutine(FadeCanvas(1)); // Fading the menu in
    	}
    	else{
    		PlayerCamera.SetLookAround(true); // Enabling player camera rotation
    		BaseCanvas.interactable = false; // Disabling menu interaction
	    	BaseCanvas.blocksRaycasts = false;
	    	StartCoroutine(FadeCanvas(0)); // Fading the menu out
    	}
    }
}
