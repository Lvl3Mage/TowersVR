using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FrameRateCounter : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI TextField;
	[SerializeField] [Range(1,20)] int FrameSample = 1;
	[SerializeField] [Range(0,10)] float MinFrameDifference = 0;
	[SerializeField] List<float> FrameTimes = new List<float>();
	float PastFrame = 0;
    // Update is called once per frame
    void Update()
    {
    	if(Input.GetKeyDown(KeyCode.P)){
    		TextField.enabled = !TextField.enabled;
    	}
    	if(TextField.enabled){
	    	float frameTime;
	    	if(FrameTimes.Count<FrameSample){
	    		FrameTimes.Add(Time.deltaTime);
	    	}
	    	else if (FrameTimes.Count>FrameSample) {
	    		int dif = FrameTimes.Count-FrameSample;
	    		for (int i = 0; i < dif; i++) 
	    		{
	    			FrameTimes.RemoveAt(i);
	    		}
	    		FrameTimes[FrameTimes.Count-1] = Time.deltaTime;
	    	}
	    	else{
	    		FrameTimes.RemoveAt(0);
	    		FrameTimes.Add(Time.deltaTime);
	    	}
	    	float CurFrame = 1/CalculateAvarage(FrameTimes);
	    	
	    	float FrameDif = Mathf.Abs(CurFrame-PastFrame);
	    	if(FrameDif>MinFrameDifference){
		        TextField.text = CurFrame.ToString("F0");
	    	}
	    	PastFrame = CurFrame;
    	}
    }
    float CalculateAvarage(List<float> List){
    	float frameTime = 0;
    	for (int i = 0; i < List.Count; i++) 
    	{
    		frameTime += List[i]/List.Count;
    	}
    	return frameTime;
    }
}
