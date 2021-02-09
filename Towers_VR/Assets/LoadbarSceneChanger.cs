using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadbarSceneChanger : SceneChanger
{
	[SerializeField] CanvasGroup BaseCanvas;
	[SerializeField] Slider LoadBar;
	[SerializeField] float FadeSpeed;
	[SerializeField] TextMeshProUGUI ContinueTextPrompt;
	[SerializeField] string TextPrompt = "Press any key to continue";
	[Header("ReloaderSettings")]
	[SerializeField] Object Camera;
	bool LoadingScene;
	void Start(){
		LoadingScene = false;
		BaseCanvas.blocksRaycasts = false;
		BaseCanvas.alpha = 1;
		StartCoroutine(DelayedFadeout());
	}
	IEnumerator DelayedFadeout(){
		yield return new WaitForSeconds(0.3f);
		StartCoroutine(FadeCanvas(0));
	}
    protected override void SceneLoadRequest(int SceneId){
    	if(!LoadingScene){
    		StartCoroutine(AsyncLoad(SceneId));	
    	}
    }
    protected override void SceneReloadRequest(){
    	if(!LoadingScene){
    		StartCoroutine(AsyncReload());	
    	}
    }
    IEnumerator FadeCanvas(float Target){
    	while(Mathf.Abs(BaseCanvas.alpha - Target) > 0.01f){
    		BaseCanvas.alpha = Mathf.Lerp(BaseCanvas.alpha, Target, Time.deltaTime*FadeSpeed);
    		yield return null;
    	}
    	BaseCanvas.alpha = Target;
    }
    IEnumerator AsyncLoad(int LoadSceneIndex){ // Method takes in an int as the scene does not exist yet
    	PreSceneLoad();
    	Scene CurrentScene = gameObject.scene; // Getting the current scene
    	LoadBar.value = 0;
    	ContinueTextPrompt.text = "";
    	BaseCanvas.blocksRaycasts = true;
    	LoadingScene = true;
    	yield return StartCoroutine(FadeCanvas(1)); // Fades the canvas in
    	AsyncOperation LoadOperation = SceneManager.LoadSceneAsync(LoadSceneIndex, LoadSceneMode.Additive); // The operation of loading the new scene
    	
    	LoadOperation.allowSceneActivation = false;
    	while (!LoadOperation.isDone)
    	{
    		LoadBar.value = LoadOperation.progress;
    		if(LoadOperation.progress >= 0.9f && !LoadOperation.allowSceneActivation){
    			ContinueTextPrompt.text = TextPrompt;
    			while (!Input.anyKey) 
    			{
    				yield return null;
    			}
    			ContinueTextPrompt.text = "";
	    		LoadOperation.allowSceneActivation = true;
    		}
    		yield return null;
    	}
	   	SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(LoadSceneIndex));
	   	yield return StartCoroutine(UnloadAsync(CurrentScene)); // Unloads the current scene
		LoadingScene = false;
    }
    IEnumerator AsyncReload(){
    	PreSceneLoad();
    	Scene CurrentScene = gameObject.scene; // Getting the current scene index for later reloading it by build index
        int ReloadSceneIndex = CurrentScene.buildIndex; // Saving the build index of the current scene before it's unloaded to load it up later

    	Scene InBetweenScene = SceneManager.CreateScene("InBetweenScene"); // Creates an empty inbetween scene for loading
    	SceneManager.SetActiveScene(InBetweenScene);
    	SceneManager.MoveGameObjectToScene(gameObject, InBetweenScene);
    	Object.Instantiate(Camera, transform); // Creates a camera to avoid flicker

	   	yield return StartCoroutine(UnloadAsync(CurrentScene)); // Unloads the scene the object was previously in
	   	LoadScene(ReloadSceneIndex); // loads the scene the object was previously in by the saved index
    }
    IEnumerator UnloadAsync(Scene UnloadScene){
    	AsyncOperation UnloadOperation = SceneManager.UnloadSceneAsync(UnloadScene);
    	while (!UnloadOperation.isDone)
    	{
    		yield return null;
    	}	
    }
    protected virtual void PreSceneLoad(){}
}


