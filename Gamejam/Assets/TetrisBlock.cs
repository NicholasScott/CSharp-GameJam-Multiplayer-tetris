using UnityEngine;
using System.Collections;

public class TetrisBlock : MonoBehaviour {
	public GameObject[,] blocks;
	
	public GameObject rotateRightPrefab;

	// Use this for initialization
	void Start () {
		DiscoverBlocks();
	}

	void DiscoverBlocks() {
		int maxX = 0;
		int maxY = 0;
		foreach (Transform child in transform) {
			maxX = Mathf.Max( Mathf.RoundToInt(child.localPosition.y), maxX);
			maxY = Mathf.Max( Mathf.RoundToInt(child.localPosition.x), maxY);
		}
		blocks = new GameObject[maxY+1,maxX+1];

		foreach (Transform child in transform) {
			int x = Mathf.RoundToInt(child.localPosition.y);
			int y = Mathf.RoundToInt(child.localPosition.x);
			blocks[blocks.GetLength(0)-1-y, x] = child.gameObject;
		}
		print(blocks);
	}

	public int GetBlockHeight() {
		return blocks.GetLength(0);
	}

	public int GetBlockWidth() {
		return blocks.GetLength(1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
