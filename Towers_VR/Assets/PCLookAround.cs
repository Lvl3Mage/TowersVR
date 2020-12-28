using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCLookAround : MonoBehaviour
{
	[SerializeField] float speed, zoomspeed, animzoomspeed, BaseZoom, ZoomedIn;
	float x,y;
    Camera Camera;
    bool zoomed;
    // Start is called before the first frame update
    void Start()
    {
        Camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
    	if(Input.GetKeyDown(KeyCode.Q)){
    		Cursor.visible = !Cursor.visible;
    		if(Cursor.lockState == CursorLockMode.Locked){
    			Cursor.lockState = CursorLockMode.None;
    		}
    		else{
    			Cursor.lockState = CursorLockMode.Locked;
    		}
    	}
        float rspeed = speed*Camera.fieldOfView/BaseZoom;
    	x += rspeed*Input.GetAxis("Mouse Y");
    	y += rspeed*Input.GetAxis("Mouse X");
        if(!Cursor.visible){
            transform.localEulerAngles = new Vector3(-x, y, 0);
        }
        if(Input.GetKey(KeyCode.X)){
            if(!zoomed){
                Camera.fieldOfView = Interpolate(ZoomedIn);
                if(Camera.fieldOfView == ZoomedIn){
                    zoomed = true;
                }
            }
            else{
                float zoomchange = Input.GetAxis("Mouse ScrollWheel")*zoomspeed;
                float view = Camera.fieldOfView;
                if((view-zoomchange)<1){
                    Camera.fieldOfView = 1;
                }
                else if((view-zoomchange)>BaseZoom){
                    Camera.fieldOfView = BaseZoom;
                }
                else{
                    Camera.fieldOfView -= zoomchange; 
                }
            }
        }
        
        if(!Input.GetKey(KeyCode.X)){
        	if(Camera.fieldOfView != BaseZoom){
        		Camera.fieldOfView = Interpolate(BaseZoom);
        	}
            
            else{
            	zoomed = false;
            }
        }
    }
    float Interpolate(float value){
        if(Mathf.Abs(value-Camera.fieldOfView)>0.5f){
            return Mathf.Lerp(Camera.fieldOfView, value, Time.deltaTime * animzoomspeed);
        }
        else{
            return value;
        }
        
    }
}
