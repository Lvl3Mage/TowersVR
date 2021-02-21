using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OnMapSpawnPicker : MonoBehaviour
{
	[SerializeField] Camera MapViewCamera;
	[SerializeField] Transform[] SpawnPoints;
	[SerializeField] RectTransform Map;
	[SerializeField] Object ExampleButton;                                                   
    // Start is called before the first frame update
    void Start()
    {
		for (int i = 0; i < SpawnPoints.Length; i++) 
		{
			Vector3 RelativePos = MapViewCamera.WorldToViewportPoint(SpawnPoints[i].position);
			CreateButton(ExampleButton,new Vector2(RelativePos.x,RelativePos.y));
		}
    }
    Button CreateButton(Object Example, Vector2 Position){
    	GameObject ButtonInstance = Object.Instantiate(Example, Map) as GameObject;
    	RectTransform ButtonTrns = ButtonInstance.GetComponent<RectTransform>();

    	//Setting the position in using the persentage and the map size
        ButtonTrns.anchoredPosition = Position*Map.rect.width;

        //Scaling the anchor to match the button (this also scales the button but we adjust for it later)
        Vector2 offset = new Vector2((ButtonTrns.rect.width/Map.rect.width),(ButtonTrns.rect.height/Map.rect.height))/2;
        ButtonTrns.anchorMax = new Vector2(Position.x+offset.x,Position.y+offset.y);
        ButtonTrns.anchorMin = new Vector2(Position.x-offset.x,Position.y-offset.y);

        // Normalizing button size after the anchor scale (it changes the actual size aswell so we have to adjust for that after the scale happens)
        ButtonTrns.offsetMax = Vector2.zero; 
        ButtonTrns.offsetMin = Vector2.zero;
        return ButtonInstance.GetComponent<Button>();
    }
    // Update is called once per frame
    void Update()
    {
    }
}
