using UnityEngine;
using System.Collections;

public class PositionTween : MonoBehaviour {

	private Vector3 startPosition;
	public Vector3 targetPosition;

	public float transitionTime;
	public float timeLeft;

	public delegate void TweenEndCallback();

	public TweenEndCallback tweenEndCallback;

	public void TweenTo(Vector3 targetPosition, float time) {
		this.startPosition = transform.position;
		this.targetPosition = targetPosition;
		this.transitionTime = time;
		this.timeLeft = time;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (timeLeft > 0) {
			timeLeft -= Time.deltaTime;
			if (timeLeft <= 0) {
				transform.position = targetPosition;
				if (tweenEndCallback != null) {
					tweenEndCallback();
				}
				return;
			}
			transform.position = Vector3.Lerp(startPosition, targetPosition, 1.0f-timeLeft/transitionTime);
		}
	}
}
