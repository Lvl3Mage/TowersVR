using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FrameRateCounter : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI TextField;

    // Update is called once per frame
    void Update()
    {
    	if(Input.GetKeyDown(KeyCode.P)){
    		TextField.enabled = !TextField.enabled;
    	}
        TextField.text = (1/Time.deltaTime).ToString("F1");
    }
}
