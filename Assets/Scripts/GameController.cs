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

	public static PlayerStats.MyBounds GetScreenToWorld(float incrementInGridUnit, PlayerStats playerStats) {

		Vector2 incrementinScreenUnits = new Vector2(playerStats.grid.sizeInPixel.x * incrementInGridUnit,
											playerStats.grid.sizeInPixel.y * incrementInGridUnit);

		Vector2 uppperBoundsInScreenUnits = new Vector2(Screen.width, Screen.height);

		Vector2 newLowerBounds = Camera.main.ScreenToWorldPoint (new Vector2(- incrementinScreenUnits.x, -incrementinScreenUnits.y));
		Vector2 newUpperBounds = Camera.main.ScreenToWorldPoint (new Vector2(uppperBoundsInScreenUnits.x + incrementinScreenUnits.x,
									uppperBoundsInScreenUnits.y + incrementinScreenUnits.y));

		PlayerStats.MyBounds newBounds;
		newBounds.lower = newLowerBounds;
		newBounds.upper = newUpperBounds;

		return newBounds;
	}
}
