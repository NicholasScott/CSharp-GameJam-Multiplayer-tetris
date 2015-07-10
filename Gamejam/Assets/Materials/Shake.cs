using UnityEngine;
using System.Collections;

public class Shake : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	float val = 0;
	// Update is called once per frame
	void Update () {
		val += Time.deltaTime;
		var pos = transform.localPosition;
		pos.x = Mathf.Sin(val*10) * 3;
		pos.y = 5+Mathf.Cos(2+val*6) * 3;

		float v = 5*(0.8f + Mathf.Sin(val*10 + 4) * 0.5f);
		transform.localScale = new Vector3(v,v,v);
		transform.localPosition = pos;
	}
}
