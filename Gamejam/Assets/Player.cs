using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public string axisName = "Vertical";
	public string fire1 = "Fire1";
	public string fire2 = "Fire2";
	public string rotate1 = "Rotate1";

	public int position;

	public GameObject currentBlock;

	public TetrisField field;
	public TetrisField enemyField;

	public Material material;

	private float timeToMove = 0;
	private float timeToMove2 = 0;

	public float delay = 2f;
	private float timeToFire;

	public float xOffset;

	public int Lines = 0;
	public GUIText guiLines;

	// Use this for initialization
	void Start () {
		Random.seed = 10;
		CreateNewRandomBlock();
	}
	
	// Update is called once per frame
	void Update () {
		float yAxis = Input.GetAxis(axisName);
		if (timeToMove > 0) {
			timeToMove -= Time.deltaTime;
			if (timeToMove < 0) {
				timeToMove = 0;
			}
		}
		if (yAxis != 0) {
			if (timeToMove <= 0) {
				position += (yAxis > 0) ? 1 : -1;
				timeToMove+= 0.2f;
				GameObject.FindGameObjectWithTag("Players").GetComponent<AudioScript>().PlayMoveClip();
			}
		} else {
			timeToMove = 0;
		}

		if (position < 0) {
			position = 0;
		}
		int max = FieldDefinition.instance.width - currentBlock.GetComponent<TetrisBlock>().GetBlockWidth();
		if (position > max) {
			position = max;
		}

		SetNewBlockPosition();

		timeToFire -= Time.deltaTime;
		if(Input.GetButtonDown(fire1)){
			Fire(field, true);
		}
		if(Input.GetButtonDown(fire2)) {
			if (Lines > 0) {
				if(Fire (enemyField, false)) {
					removeLines();
				}
			}
		}
		if(Input.GetButtonDown(rotate1)) {
			var n = currentBlock.GetComponent<TetrisBlock>().rotateRightPrefab;
			if (n != null) {
				Destroy(currentBlock);
				CreateNewBlock(n);
			}
		}
	}

	bool Fire(TetrisField field, bool canWin) {
		if (timeToFire <= 0) {
			if (!field.Fire(currentBlock.GetComponent<TetrisBlock>(), position, canWin)) {
				return false;
			}
			CreateNewRandomBlock();
			GameObject.FindGameObjectWithTag("Players").GetComponent<AudioScript>().PlaySendClip();
			timeToFire = delay;
			return true;
		}
		return false;
	}

	
	void SetNewBlockPosition() {
		if (currentBlock != null) {
			var pos = currentBlock.transform.position;
			pos.y = position;
			pos.x = xOffset;
			currentBlock.transform.position = pos;
		}
	}

	public void addLines() {
		Lines++;
		GameObject.FindGameObjectWithTag("Players").GetComponent<AudioScript>().PlayLineClip();
		guiLines.text = "Lines: " + Lines  + "";
	}

	public bool removeLines() {
		if (Lines > 0) {
			Lines--;
			guiLines.text = "Lines: " + Lines  + "";
			return true;
		}
		return false;
	}


	void CreateNewRandomBlock() {
		var newBlock = FieldDefinition.instance.blocksPrefabs[
			Random.Range(0, FieldDefinition.instance.blocksPrefabs.Length)
		                                                      ];
		CreateNewBlock(newBlock);
	}
	
	void CreateNewBlock(GameObject prefab) {
		GameObject newBlock = Instantiate(prefab) 
			as GameObject;

		var Mat = new Material(this.material);
		Mat.color = new Color(
			Mat.color.r + Random.value * 0.2f,
			Mat.color.g + Random.value * 0.2f,
			Mat.color.b + Random.value * 0.2f);

		foreach (Transform child in newBlock.transform) {
			child.renderer.material = Mat;
		}

		currentBlock = newBlock;
		SetNewBlockPosition();
	}

	public void Fail() {
		transform.FindChild("Win").gameObject.SetActive(true);
	}
}
