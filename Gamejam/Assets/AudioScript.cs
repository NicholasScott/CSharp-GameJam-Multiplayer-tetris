using UnityEngine;
using System.Collections;

public class AudioScript : MonoBehaviour {
	AudioSource audio;
	public GameObject lineClip, lineClip2, sendClip, moveClip;
	// Use this for initialization
	void Start () {
	
	}
	public void PlayMoveClip(){
		moveClip.audio.Play ();
		}
	public void PlaySendClip(){
		sendClip.audio.Play ();
		}
	public void PlayLineClip(){
		int clipChosen = Random.Range (0, 1);
		if (clipChosen == 0) {
			lineClip.audio.Play();
				}
		if (clipChosen == 1) {
			lineClip2.audio.Play();
				}
	
	
		}
	// Update is called once per frame
	void Update () {
	
	}
}
