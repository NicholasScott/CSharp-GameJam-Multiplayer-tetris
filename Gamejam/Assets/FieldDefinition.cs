using UnityEngine;
using System.Collections;

public class FieldDefinition : MonoBehaviour {

	public static FieldDefinition instance = null;

	public int height = 8;

	public int width = 8;

	public TetrisField player1Field;
	public TetrisField player2Field;

	public GameObject[] blocksPrefabs;

	// Use this for initialization
	void Awake () {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
