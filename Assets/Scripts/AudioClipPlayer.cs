using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipPlayer : MonoBehaviour
{
	[SerializeField] AudioClip[] Clips;
	[SerializeField] AudioSource AudioSource;

	void PlayClip(int id){
		if(id<Clips.Length){
			AudioSource.clip = Clips[id];
			AudioSource.Play();
		}
		else{
			Debug.LogError("Selected clip is out of range");
		}
	}
}
