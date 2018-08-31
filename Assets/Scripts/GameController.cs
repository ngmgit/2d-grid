using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public PlayerStats mPlayerStats;
	public GridLayoutGroup gridGrpBg;
	public SetSpawnPositions playerSpawn;

	int columns = 6;
	int rows = 8;

	void Awake()
	{
		float Hz = Screen.width;
		float Vt = Screen.height;

		float gridSizeHz = Hz / columns;
		float gridSizeVt = Vt / rows;

		mPlayerStats.grid.sizeInPixel = new Vector2(gridSizeHz, gridSizeVt);
		mPlayerStats.grid.rows = rows;
		mPlayerStats.grid.columns = columns;

		gridGrpBg.cellSize = mPlayerStats.grid.sizeInPixel;

		playerSpawn.spawnPosInGrid = new Vector2 (
			mPlayerStats.grid.sizeInPixel.x * 3,
			mPlayerStats.grid.sizeInPixel.y * 4);

		mPlayerStats.mbounds.lower = Camera.main.ScreenToWorldPoint (new Vector2(0,0));
		mPlayerStats.mbounds.upper = Camera.main.ScreenToWorldPoint (new Vector2(Screen.width, Screen.height));
	}
}
