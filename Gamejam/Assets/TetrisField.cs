using UnityEngine;
using System.Collections;

public class TetrisField : MonoBehaviour {

	public GameObject[,] blocks;

	public enum Direction {
		RIGHT,
		LEFT
	};

	public float xOffset;

	public Direction direction;

	public Player player;
	
	// Use this for initialization
	void Start () {
		CreateField();
	}

	void CreateField() {
		blocks = new GameObject[FieldDefinition.instance.height, FieldDefinition.instance.width];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool IsBlocking(TetrisBlock block, int x, int y) {
		for (int ix = 0; ix < block.GetBlockWidth(); ix++) {
			for (int iy = 0; iy < block.GetBlockHeight(); iy++) {
				if (direction == Direction.RIGHT) {
					if (block.blocks[iy,ix] == null) { 
						continue;
					}
				} else {
					if (block.blocks[block.GetBlockHeight()-1-iy,ix] == null) { 
						continue;
					}
				}
				int ny = iy+y;
				int nx = ix+x;
				if (ny < 0 || nx < 0 || nx >= blocks.GetLength(1) || ny >= blocks.GetLength(0)) {
					continue;
				}
				if (blocks[iy+y, ix+x] != null) {
					return true;
				}
				
			}
		}
		return false;
	}

	public void MoveDown(int from) {
		for (int ix = 0; ix < FieldDefinition.instance.width; ix++) {
			var blo = blocks[from, ix];
			if (blo != null) {
				Destroy(blo);
			}
		}

		for (int iy = from; iy < FieldDefinition.instance.height-1; iy++) {
			for (int ix = 0; ix < FieldDefinition.instance.width; ix++) {
				blocks[iy, ix] = blocks[iy+1, ix];
				if (blocks[iy, ix] != null) {
					var blockGO = blocks[iy, ix].gameObject;
					if (blockGO.GetComponent<PositionTween>() == null) {
						blockGO.AddComponent<PositionTween>().targetPosition = blockGO.transform.position;
					}

					var pos = blockGO.GetComponent<PositionTween>().targetPosition;
					pos.x += (direction == Direction.RIGHT ? 1 : -1);
					blockGO.GetComponent<PositionTween>().TweenTo(pos, 0.1f);
				}
			}
		}
		for (int ix = 0; ix < FieldDefinition.instance.width; ix++) {
			blocks[FieldDefinition.instance.height-1, ix] = null;
		}
	}

	public void Fail() {
		player.Fail();
	}

	public bool PutBlock(TetrisBlock block, int x, int y, bool canWin) {
		bool b = false;
		for (int ix = 0; ix < block.GetBlockWidth(); ix++) {
			for (int iy = 0; iy < block.GetBlockHeight(); iy++) {
				int magicY = iy ;
				if (direction == Direction.LEFT) {
					magicY = block.GetBlockHeight()-1-iy;
				}

				if (block.blocks[magicY, ix] != null) {
					int ny = iy+y;
					int nx = ix+x;

					if (ny < 0 || nx < 0 || nx >= blocks.GetLength(1) || ny >= blocks.GetLength(0)) {
						if (canWin) {
							b = true;
							Fail();
							break;
						} else {
							return false;
						}
					}
					blocks[iy+y, ix+x] = 
						block.blocks[magicY, ix];
				}
			}
			if (b) {
				break;
			}
		}

		float xVal = FieldDefinition.instance.height-y-block.GetBlockHeight()+1;
		if (direction == Direction.LEFT) {
			xVal = -(FieldDefinition.instance.height-y);
		}

		if (block.GetComponent<PositionTween>() == null) {
			block.gameObject.AddComponent<PositionTween>();
		}
		block.GetComponent<PositionTween>().TweenTo(
		new Vector3(
			xOffset + xVal,
			x, 0), 0.2f);

		block.GetComponent<PositionTween>().tweenEndCallback = ScanForLines;
		return true;
	}

	public void ScanForLines() {
		for (int y = FieldDefinition.instance.height-1; y >= 0; y--) {
			bool line = true;
			for (int ix = 0; ix < FieldDefinition.instance.width; ix++) {
				if (blocks[y, ix] == null) {
					line = false;
					break;
				}
			}

			if (line) {
				player.addLines();
				MoveDown(y);
				ScanForLines();
				return;
			}
		}
	}

	public bool Fire(TetrisBlock block, int xPosition, bool canWin) {

		for (int y = FieldDefinition.instance.height-1; y >= 0; y--) {
			if (IsBlocking(block, xPosition, y)) {
				if (!PutBlock(block, xPosition, y+1, canWin)) {
					return false;
				}
				return true;
			}
		}
		PutBlock(block, xPosition, 0, canWin);
		return true;
	}
}
